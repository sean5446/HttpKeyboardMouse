using HttpKeyboardMouse;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using static WinFormsApp1.ConfigReader;

namespace WinFormsApp1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // Visual Studio ToolBox is not working... can't make a Menu
            // and InitializeComponent is auto-generated when controls are changed in the designer
            this.HttpKeyboardMouse.ContextMenuStrip = new ContextMenuStrip()
            {
                Items =
                    {
                        new ToolStripMenuItem("Show", null, new EventHandler(Open), "Show"),
                        new ToolStripMenuItem("Exit", null, new EventHandler(Exit), "Exit")
                    }
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Trace.Listeners.Add(new TextFileTraceListener("http.log"));
            Trace.Listeners.Add(new TextBoxTraceListener(TextLog));
            Trace.AutoFlush = true;

            string defaultConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "config.yaml");
            Config config = LoadFile(defaultConfigPath);

            // Trace statements make it to release build, whereas debug to not
            Trace.WriteLine("Starting on port: " + config.Server.Port);

            string? hostname = Environment.GetEnvironmentVariable("COMPUTERNAME");
            LinkLabelComputerName.Text = $"http://{hostname?.ToLower()}.local:{config.Server.Port}/";

            string? hostName = Dns.GetHostName() ?? Environment.GetEnvironmentVariable("COMPUTERNAME");
            // TODO figure out a way to get 'correct' IPv4
            string ipAddr = "";
            foreach (var ip in Dns.GetHostEntry(hostName).AddressList)
            {
                if (ip.ToString().Contains('.')) ipAddr = ip.ToString();
            }
            LinkLabelIP.Text = $"http://{ipAddr?.ToLower()}:{config.Server.Port}/";

            HttpServer httpServer = new();
            httpServer.Start(config.Server?.Port);
            //Trace.Flush();
        }

        private static void Open(object? sender, EventArgs e)
        {
            if (sender != null)
            {
                ((Control)sender).Show();
            }
        }

        private static void Exit(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnClearLog_Click(object sender, EventArgs e)
        {
            TextLog.Text = "";
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private void LinkLabelComputerName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = LinkLabelComputerName.Text, UseShellExecute = true });
        }
    }
}