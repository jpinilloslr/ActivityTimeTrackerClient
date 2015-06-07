using System;
using System.Drawing;
using System.Windows.Forms;
using ActivityMonitor.Infrastructure;
using ActivityMonitor.Interfaces;
using ActivityMonitor.Services;

namespace ActivityMonitor.View
{
    public partial class MainForm : Form
    {
        public bool CanClose { get; set; }
        private readonly IActivityMonitor _activityMonitor;

        private void RefreshLogs()
        {
            var logs = LogManager.GetInstance().GetLogs();
            LogManager.GetInstance().Clear();

            foreach (var log in logs)
            {
                var strItem = new string[2];
                strItem[0] = log.Time.ToString();
                strItem[1] = log.Text;

                var lvi = new ListViewItem(strItem) {ImageIndex = (int) log.LogLevel};
                listView1.Items.Add(lvi);
                listView1.EnsureVisible(listView1.Items.Count-1);
            }
            
        }

        private void Start()
        {
            _activityMonitor.Start();
            startToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Enabled = true;
            Visible = false;
            ShowInTaskbar = false;
        }

        private void Stop()
        {
            _activityMonitor.Stop();
            startToolStripMenuItem.Enabled = true;
            stopToolStripMenuItem.Enabled = false;
        }

        public MainForm()
        {
            _activityMonitor = MonitorService.GetInstance().GetActivityMonitor();
            InitializeComponent();           
            CanClose = false;

            if (_activityMonitor.Started())
            {
                startToolStripMenuItem.Enabled = false;
                stopToolStripMenuItem.Enabled = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Visible = true;
            ShowInTaskbar = true;
            timerRefreshLogs.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                CanClose = true;
            }

            if (!CanClose)
            {
                Visible = false;
                ShowInTaskbar = false;
                timerRefreshLogs.Enabled = false;
            }

            if (CanClose && _activityMonitor.Started())
                Stop();
            
            e.Cancel = !CanClose;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Start();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CanClose = true;
            Close();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void timerRefreshLogs_Tick(object sender, EventArgs e)
        {
            RefreshLogs();
        }
    }
}
