using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Management;
using System.Net.Http;
using Newtonsoft.Json;


namespace rdp.Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            //ActivationCodeTextBox.TextChanged += ActivationCodeTextBox_TextChanged;

            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            memAvailableCounter = new PerformanceCounter("Memory", "Available Bytes");
            using (var memMC = new ManagementClass("Win32_PhysicalMemory"))
            {
                using var memMOC = memMC.GetInstances();
                foreach (var memMO in memMOC)
                {
                    memTotal += (ulong)memMO.GetPropertyValue("Capacity");
                }
            }

            DriveInfo drive = new DriveInfo("C");
            totalDiskSpaceAvailable = drive.TotalSize / (1024 * 1024 * 1024);

            networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(ni => ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback);


            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

        }


        private DispatcherTimer timer;

        private PerformanceCounter cpuCounter;

        private readonly PerformanceCounter memAvailableCounter;
        private readonly ulong memTotal = 0;

        private double totalDiskSpaceAvailable;
        private double diskSpaceConsumed;
        private double diskSpaceAvailable;

        private NetworkInterface networkInterface;
        private long previousDownloadBytes;
        private long previousUploadBytes;

        private bool isDataFetched = true;
        /*// Class variables (add these to your existing variables)
        private TextBlock DownloadSpeedText; // Add to XAML
        private TextBlock UploadSpeedText;   // Add to XAML
        private TextBlock SystemHealthText;  // Add to XAML
        private TextBlock StartupTimeText;   // Add to XAML
        private TextBlock BackgroundProcessesText; // Add to XAML*/

        private void Timer_Tick(object sender, EventArgs e)
        {
            // CPU
            float currentCpuUsage = cpuCounter.NextValue();
            int roundedCpuUsage = (int)Math.Round(currentCpuUsage);
            UpdateProgressCPU(roundedCpuUsage);

            // RAM
            var memUsed = memTotal - memAvailableCounter.NextValue();
            int roundedRamUsage = (int)Math.Round((memUsed / memTotal) * 100);
            UpdateProgressRAM(roundedRamUsage);

            // Disk
            DriveInfo drive = new DriveInfo("C");
            diskSpaceConsumed = (drive.TotalSize - drive.AvailableFreeSpace) / (1024 * 1024 * 1024);
            double usedDiskSpacePercentage = (diskSpaceConsumed / totalDiskSpaceAvailable) * 100;
            int roundedDiskUsage = (int)Math.Round(usedDiskSpacePercentage);
            diskSpaceAvailable = drive.AvailableFreeSpace / (1024 * 1024 * 1024);
            UpdateProgressDisk(roundedDiskUsage);

            // Network speed
            if (networkInterface != null)
            {
                IPv4InterfaceStatistics stats = networkInterface.GetIPv4Statistics();
                long currentDownloadBytes = stats.BytesReceived;
                long currentUploadBytes = stats.BytesSent;

                double downloadSpeedMbps = (currentDownloadBytes - previousDownloadBytes) / 1024.0 / 1024.0 * 8;
                double uploadSpeedMbps = (currentUploadBytes - previousUploadBytes) / 1024.0 / 1024.0 * 8;

                previousDownloadBytes = currentDownloadBytes;
                previousUploadBytes = currentUploadBytes;

                UpdateNetworkSpeed(downloadSpeedMbps, uploadSpeedMbps);
            }

            // System health
            //UpdateSystemHealthScore();

            // Startup time
            TimeSpan startupTime = GetSystemUptime();
            int startupTimeInSeconds = (int)startupTime.TotalSeconds;
            UpdateStartupTimeText(startupTimeInSeconds);

            // Background processes
            int backgroundProcessesCount = GetBackgroundProcessesCount();
            UpdateBackgroundProcessesText(backgroundProcessesCount);
        }
        private TimeSpan GetSystemUptime()
        {
            TimeSpan uptime = TimeSpan.FromMilliseconds(Environment.TickCount);
            return uptime;
        }
        private int GetBackgroundProcessesCount()
        {
            var processes = Process.GetProcesses();
            var backgroundProcesses = processes.Where(p => string.IsNullOrEmpty(p.MainWindowTitle));
            return backgroundProcesses.Count();
        }

        private void Grid_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }

        private void RAM_Gauge_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void UpdateProgressCPU(double percentage)
        {
            if (percentage == 100)
            {
                percentage = 99;
            }

            // Update the gauge value
            CPU_Gauge.Value = percentage;

            // Update the percentage text if needed (assuming you want to display it somewhere)
            // If you have a TextBlock to show the percentage like in the old UI, you would do:
            // ProgressTextCPU.Text = $"{percentage}%";

            // Note: In the new UI, the gauge itself shows the value, so you might not need
            // a separate text display unless you want it elsewhere
        }

        
        private void CPU_Gauge_Loaded(object sender, RoutedEventArgs e)
        {

        }
        //private void Gauge_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    if (sender is LiveCharts.Wpf.Gauge gauge)
        //    {
        //        // Calculate the smaller dimension (width or height)
        //        double smallerDimension = Math.Min(gauge.ActualWidth, gauge.ActualHeight);

        //        // Set base InnerRadius proportionally
        //        double proportionalRadius = smallerDimension / 4; // Adjust divisor for responsiveness

        //        // Apply constraints for minimum and maximum InnerRadius
        //        double minRadius = 50;  // Prevents the arc from becoming too thin on small sizes
        //        double maxRadius = smallerDimension / 2; // Keeps the arc balanced at larger sizes

        //        // Dynamically set the InnerRadius with constraints
        //        //gauge.InnerRadius = Math.Max(maxRadius, Math.Min(minRadius, proportionalRadius));
        //        gauge.InnerRadius = 200;

        //    }
        //}


        private void UpdateProgressDisk(double percentage)
        {
            if (percentage == 100)
            {
                percentage = 99;
            }

            // Update the gauge value
            Disk_Gauge.Value = percentage;

            // Update the disk usage text (GB/TB display)
            // Format the total disk space to show TB if over 1000GB
           /* string totalSpaceText = totalDiskSpaceAvailable >= 1000 ?
                $"{(totalDiskSpaceAvailable / 1000):F1}TB" :
                $"{totalDiskSpaceAvailable:F0}GB";

            // Format the consumed space similarly
            string consumedSpaceText = diskSpaceConsumed >= 1000 ?
                $"{(diskSpaceConsumed / 1000):F1}TB" :
                $"{diskSpaceConsumed:F0}GB";*/

            // If you have a TextBlock to show the disk usage (like ProgressTextDisk in old UI)
            // ProgressTextDisk.Text = $"{consumedSpaceText}/{totalSpaceText}";
        }
        private void UpdateProgressRAM(double percentage)
        {
            if (percentage == 100)
            {
                percentage = 99;
            }

            // Update the gauge value
            RAM_Gauge.Value = percentage;

            // Update the percentage text display
            // If you have a TextBlock to show the percentage (like ProgressTextRAM in old UI)
            // ProgressTextRAM.Text = $"{percentage}%";
        }
        private void UpdateNetworkSpeed(double downloadSpeed, double uploadSpeed)
        {
            // Update separate download/upload text blocks
            DownloadSpeedText.Text = $"{downloadSpeed:F1} Mbps";
            UploadSpeedText.Text = $"{uploadSpeed:F1} Mbps";
        }

        /*private void UpdateSystemHealthScore()
        {
            int healthScore = CalculateSystemHealthScore();
            Performance_Gauge.Value = healthScore;
            SystemHealthText.Text = $"{healthScore}%";
        }*/

        private void UpdateStartupTimeText(int seconds)
        {
            int hours = seconds / 3600;
            int minutes = (seconds % 3600) / 60;
            int remainingSeconds = seconds % 60;
           // StartupTimeText.Text = $"{hours}:{minutes:D2}:{remainingSeconds:D2}";
        }

        private void UpdateBackgroundProcessesText(int count)
        {
           // BackgroundProcessesText.Text = $"{count} running";
        }
       /* private int CalculateSystemHealthScore()
        {
            var score = ((100 * Properties.Settings.Default.Total_ToggleButtons_Checked) / 180.0); // [ total_tweaks = (x/100) * 185 ] => we need x (185 total, 5 duplicate, now 180)
            return (int)Math.Round(score);
        }*/
    }

}
