using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

namespace QuickReminder.Forms
{
	public partial class MainForm : Form
	{
		private const string StartupRegistry = @"Software\Microsoft\Windows\CurrentVersion\Run";
		private const string AppStartName = "QuickReminder";

		public MainForm()
		{
			InitializeComponent();
			SetIcons();
		}

		private void SetIcons()
		{
			notifyIcon.Icon = Icon = Properties.Resources.Settings;
			newReminderToolStripMenuItem.Image = Properties.Resources.Reminder.ToBitmap();
			settingsToolStripMenuItem.Image = Icon.ToBitmap();
		}

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}

		public static readonly List<Reminder> AllReminders = new List<Reminder>();

		public static int PendingRemindersCount
		{
			get
			{
				return AllReminders.Count(reminder => reminder.Active);
			}
		}

		private static void SetStartup(bool startup)
		{
			var key = Registry.CurrentUser.OpenSubKey(StartupRegistry, true);

			if ((key.GetValue(AppStartName) == null) && (startup))
				key.SetValue(AppStartName, Application.ExecutablePath, RegistryValueKind.String);

			if ((key.GetValue(AppStartName) != null) && (!startup))
				key.DeleteValue(AppStartName);

			key.Close();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			var key = Registry.CurrentUser.CreateSubKey(StartupRegistry);
			launchStartupCheckbox.Checked = (key.GetValue(AppStartName) != null);
			key.Close();
			warnPendingCheckBox.Checked = Properties.Settings.Default.WarnPendingAlarmsOnClose;

			if (Properties.Settings.Default.FirstRun) {
				notifyIcon.ShowBalloonTip(5000);
				Properties.Settings.Default.FirstRun = false;
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing) {
				Hide();
				e.Cancel = true;
			}

			SetStartup(launchStartupCheckbox.Checked);
			Properties.Settings.Default.WarnPendingAlarmsOnClose = warnPendingCheckBox.Checked;
			Properties.Settings.Default.Save();
		}

		private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (PendingRemindersCount == 0 || !warnPendingCheckBox.Checked || MessageBox.Show(string.Format("You have {0} pending reminders. Are you sure you wish to exit?", PendingRemindersCount),
				"Exit application", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				Application.Exit();
		}

		private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Normal;
			Show();
		}

		private void NewReminderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var reminderForm = new ReminderForm();
			reminderForm.Show();
			reminderForm.Focus();
		}

		private void OkButton_Click(object sender, EventArgs e)
		{
			Hide();
		}

		private void VersionLabel_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.damieng.com");
		}
	}
}