using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Drawing;

namespace QuickRes.Forms
{
    public partial class MainForm : Form
    {
        const string StartupRegistry = @"Software\Microsoft\Windows\CurrentVersion\Run";
        const string AppStartName = "QuickRes";
        readonly Display display = new Display();
        readonly Res initialRes;

        public MainForm()
        {
            initialRes = display.GetCurrentResolution();
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
            while (contextMenuStrip.Items.Count > 0 && !(contextMenuStrip.Items[0] is ToolStripSeparator))
            {
                var item = contextMenuStrip.Items[0];
                item.Click -= SetDisplayResolution;
                contextMenuStrip.Items.Remove(item);
            }

            doubleClickCombo.Items.Clear();
            doubleClickCombo.Items.Add("does nothing");
            doubleClickCombo.Items.Add("set to initial res");

            var currentRes = display.GetCurrentResolution();

            foreach (var r in display.GetResolutions())
            {
                var item = new ToolStripMenuItem(r.ToString()) { Tag = r };
                item.Click += SetDisplayResolution;
                if (r.Equals(currentRes))
                    item.Checked = true;
                contextMenuStrip.Items.Insert(0, item);

                doubleClickCombo.Items.Add(r);
                if (r.ToString() == Properties.Settings.Default.DoubleClickAction)
                    doubleClickCombo.SelectedItem = r;
            }
        }

        private void SetDisplayResolution(Object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                display.SetResolution((Res)((ToolStripMenuItem)sender).Tag);
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
            SystemEvents.DisplaySettingsChanged += SystemEventsOnDisplaySettingsChanged;
            UpdateDisplayResolutions(sender, e);
            contextMenuStrip.Click += SetDisplayResolution;
        }

        private void SystemEventsOnDisplaySettingsChanged(object sender, EventArgs e)
        {
            UpdateDisplayResolutions(sender, e);
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
            Focus();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DoubleClickAction = doubleClickCombo.Text;
            Hide();
        }

        private void VersionLabel_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.damieng.com");
        }

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var current = Screen.PrimaryScreen;

            foreach (var r in contextMenuStrip.Items.OfType<ToolStripMenuItem>().Where(i => i.Tag is Res))
            {
                var tag = (Res)r.Tag;
                r.Checked = current.Bounds.Width == tag.Width && current.Bounds.Height == tag.Height;

                if (r.Text == Properties.Settings.Default.DoubleClickAction)
                {
                    r.Font = new Font(contextMenuStrip.Font, FontStyle.Bold);
                }
                else
                {
                    r.Font = contextMenuStrip.Font;
                }
            }
        }

        StringFormat comboRightAlignment = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };

        private void doubleClickCombo_DrawItem(object sender, DrawItemEventArgs e)
        {
            var comboBox = (ComboBox) sender;
            e.DrawBackground();

            if (e.Index >= 0)
            {
                e.Graphics.DrawString(comboBox.Items[e.Index].ToString(), comboBox.Font, 
                    (e.State & DrawItemState.Selected) == DrawItemState.Selected ? SystemBrushes.HighlightText : new SolidBrush(comboBox.ForeColor),
                    e.Bounds, comboRightAlignment);
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            var action = Properties.Settings.Default.DoubleClickAction;
            switch (action)
            {
                case "":
                case "does nothing":
                    return;
                case "set to initial res":
                    display.SetResolution(initialRes);
                    return;
                default:
                    display.SetResolution(display.GetResolutions().FirstOrDefault(r => r.ToString() == action));
                    return;
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            var key = Registry.CurrentUser.CreateSubKey(StartupRegistry);
            if (key == null) return;

            launchStartupCheckbox.Checked = key.GetValue(AppStartName) != null;
            key.Close();
        }
    }
}