using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace QuickReminder.Forms
{
	public partial class ReminderForm : Form
	{
		private const int BalloonTimeout = 60 * 60 * 1000;
		private readonly Icon[] animationFrames = { Properties.Resources.Alarm1, Properties.Resources.Reminder, Properties.Resources.Alarm2 };

		private int alarmAnimationFrame;
		private AlarmStates alarmState;
		private Reminder reminder;

		public enum AlarmStates { Pending, Alarming, Off }

		public ReminderForm()
		{
			InitializeComponent();
			Icon = Properties.Resources.Reminder;
			AlarmState = AlarmStates.Off;
		}

		public AlarmStates AlarmState
		{
			get { return alarmState; }
			set
			{
				alarmState = value;
				switch (alarmState)
				{
					case AlarmStates.Alarming:
						{
							timer.Interval = 250;
							notifyIcon.ShowBalloonTip(BalloonTimeout, reminder.NextOccurence.ToString(CultureInfo.CurrentUICulture), reminder.Name, ToolTipIcon.None);
							break;
						}
					case AlarmStates.Pending:
						{
							Icon = Properties.Resources.Reminder;
							timer.Interval = 1000;
							break;
						}
					case AlarmStates.Off:
						{
							Icon = Properties.Resources.Reminder;
							if (reminder != null)
								reminder.Active = false;
							break;
						}
				}

				timer.Enabled = alarmState != AlarmStates.Off;
			}
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			if (reminder != null)
				ReminderToUserInterface();

			nameCombo.Items.Clear();
			foreach (var availableReminder in MainForm.AllReminders)
				nameCombo.Items.Add(availableReminder);
			nameCombo.SelectedItem = reminder;

			ReflectInterface(this, e);
		}

		private void ReflectInterface(object sender, EventArgs e)
		{
			atTimePicker.Visible = atRadio.Checked;
			inMinutesNud.Visible = inRadio.Checked;
			minutesLabel.Visible = inRadio.Checked;
		}

		private void NotifyIcon_Click(object sender, EventArgs e)
		{
			Show();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			if ((alarmState == AlarmStates.Pending) && (DateTime.Now >= reminder.NextOccurence))
				AlarmState = AlarmStates.Alarming;

			if (alarmState == AlarmStates.Alarming)
			{
				alarmAnimationFrame = (alarmAnimationFrame + 1) % animationFrames.Length;
				notifyIcon.Icon = animationFrames[alarmAnimationFrame];
			}
		}

		private void ClearButton_Click(object sender, EventArgs e)
		{
			AlarmState = AlarmStates.Off;
			Close();
		}

		private void SetButton_Click(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(nameCombo.Text))
			{
				MessageBox.Show("Name can not be blank");
				return;
			}

			if (reminder == null)
			{
				reminder = new Reminder();
				MainForm.AllReminders.Add(reminder);
			}

			reminder.Name = nameCombo.Text;
			reminder.ReminderType = atRadio.Checked ? ReminderTypes.Time : ReminderTypes.Interval;
			reminder.Time = reminder.ReminderType == ReminderTypes.Time ? TimeSpan.Parse(atTimePicker.Text) : new TimeSpan(0, (int)inMinutesNud.Value, 0);

			notifyIcon.Text = $"{reminder.Name} at {reminder.NextOccurence}";
			timer.Enabled = true;

			AlarmState = AlarmStates.Pending;

			Hide();
		}

		private void NameCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (nameCombo.SelectedItem != null)
			{
				var potentialReminder = (Reminder)nameCombo.SelectedItem;
				if (potentialReminder.Active)
				{
					reminder = potentialReminder.Clone();
					MainForm.AllReminders.Add(reminder);
				}
				else
					reminder = potentialReminder;

				ReminderToUserInterface();
			}
		}

		private void ReminderToUserInterface()
		{
			nameCombo.Text = reminder.Name;
			atRadio.Checked = (reminder.ReminderType == ReminderTypes.Time);
			inRadio.Checked = (reminder.ReminderType == ReminderTypes.Interval);
			atTimePicker.Value = (reminder.ReminderType == ReminderTypes.Time) ? reminder.NextOccurence : DateTime.Now;
			inMinutesNud.Value = (reminder.ReminderType == ReminderTypes.Interval) ? reminder.Time.Minutes : 0;
		}

		private void HideButton_Click(object sender, EventArgs e)
		{
			Hide();
		}
	}
}