using System;

namespace QuickReminder
{
	public enum ReminderTypes { Time, Interval }
	
	public class Reminder
	{
		private DateTime setAt;
		private TimeSpan time;

		public bool Active;
		public string Name;
		public ReminderTypes ReminderType;

		public TimeSpan Time
		{
			get { return time; }
			set {
				time = value;
				setAt = DateTime.Now;
			}
		}

		public DateTime NextOccurence
		{
			get {
				if (ReminderType == ReminderTypes.Interval)
					return setAt.Add(time);

				var next = setAt.Subtract(setAt.TimeOfDay).Add(time);
				return setAt.TimeOfDay > time ? next.AddDays(1) : next;
			}
		}

		public override string ToString()
		{
			return Name;
		}

		public Reminder Clone()
		{
			return (Reminder)MemberwiseClone();
		}
	}
}