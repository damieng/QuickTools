using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

namespace QuickRes.Forms
{
    public partial class MainForm : Form
    {
        private const string StartupRegistry = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string AppStartName = "QuickRes";
        private readonly Display display = new Display();

        public MainForm()
        {
            InitializeComponent();
            SetIcons();
        }

        private void SetIcons()
        {
            notifyIcon.Icon = Icon = Properties.Resources.Display;
            settingsToolStripMenuItem.Image = Icon.ToBitmap();
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private void UpdateDisplayResolutions(Object sender, EventArgs eventArgs)
        {
            var getMenuLabel = Properties.Settings.Default.ShowRefreshRate
                ? (Func<Tuple<int, int, int>, string>) (r => $"{r.Item1} × {r.Item2} @ {r.Item3}Hz")
                : r => $"{r.Item1} × {r.Item2}";

            while (contextMenuStrip.Items.Count > 0 && !(contextMenuStrip.Items[0] is ToolStripSeparator))
            {
                var item = contextMenuStrip.Items[0];
                item.Click -= SetDisplayResolution;
                contextMenuStrip.Items.Remove(item);
            }

            foreach (var r in display.GetResolutions())
            {
                var item = new ToolStripMenuItem(getMenuLabel(r)) { Tag = r };
                item.Click += SetDisplayResolution;
                contextMenuStrip.Items.Insert(0, item);
            }
        }

        private void SetDisplayResolution(Object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                var r = (Tuple<int, int, int>)((ToolStripMenuItem)sender).Tag;
                display.SetResolution(r.Item1, r.Item2, r.Item3);
            }
        }

        private static void SetStartup(bool startup)
        {
            var key = Registry.CurrentUser.OpenSubKey(StartupRegistry, true);
            if (key == null) return;

            if ((key.GetValue(AppStartName) == null) && startup)
                key.SetValue(AppStartName, Application.ExecutablePath, RegistryValueKind.String);

            if ((key.GetValue(AppStartName) != null) && !startup)
                key.DeleteValue(AppStartName);

            key.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateDisplayResolutions(sender, e);
            contextMenuStrip.Click += SetDisplayResolution;

            var key = Registry.CurrentUser.CreateSubKey(StartupRegistry);
            if (key == null) return;

            launchStartupCheckbox.Checked = key.GetValue(AppStartName) != null;
            key.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Hide();
                e.Cancel = true;
            }

            SetStartup(launchStartupCheckbox.Checked);
            Properties.Settings.Default.Save();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Show();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void VersionLabel_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.damieng.com");
        }

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var current = Screen.PrimaryScreen;

            foreach (var r in contextMenuStrip.Items.OfType<ToolStripMenuItem>().Where(i => i.Tag is Tuple<int, int, int>))
            {
                var tag = (Tuple<int, int, int>)r.Tag;
                r.Checked = current.Bounds.Width == tag.Item1 && current.Bounds.Height == tag.Item2;
            }
        }
    }
}