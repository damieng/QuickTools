namespace QuickReminder.Forms
{
	partial class ReminderForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReminderForm));
			this.nameCombo = new System.Windows.Forms.ComboBox();
			this.atTimePicker = new System.Windows.Forms.DateTimePicker();
			this.happensGroupBox = new System.Windows.Forms.GroupBox();
			this.minutesLabel = new System.Windows.Forms.Label();
			this.inMinutesNud = new System.Windows.Forms.NumericUpDown();
			this.inRadio = new System.Windows.Forms.RadioButton();
			this.atRadio = new System.Windows.Forms.RadioButton();
			this.setButton = new System.Windows.Forms.Button();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.clearButton = new System.Windows.Forms.Button();
			this.hideButton = new System.Windows.Forms.Button();
			this.happensGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.inMinutesNud)).BeginInit();
			this.SuspendLayout();
			// 
			// nameCombo
			// 
			this.nameCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.nameCombo.FormattingEnabled = true;
			this.nameCombo.Location = new System.Drawing.Point(12, 6);
			this.nameCombo.Name = "nameCombo";
			this.nameCombo.Size = new System.Drawing.Size(174, 21);
			this.nameCombo.TabIndex = 0;
			this.nameCombo.SelectedIndexChanged += new System.EventHandler(this.NameCombo_SelectedIndexChanged);
			// 
			// atTimePicker
			// 
			this.atTimePicker.CustomFormat = "HH:MM";
			this.atTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.atTimePicker.Location = new System.Drawing.Point(49, 48);
			this.atTimePicker.Name = "atTimePicker";
			this.atTimePicker.ShowUpDown = true;
			this.atTimePicker.Size = new System.Drawing.Size(70, 21);
			this.atTimePicker.TabIndex = 6;
			// 
			// happensGroupBox
			// 
			this.happensGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.happensGroupBox.Controls.Add(this.minutesLabel);
			this.happensGroupBox.Controls.Add(this.inMinutesNud);
			this.happensGroupBox.Controls.Add(this.inRadio);
			this.happensGroupBox.Controls.Add(this.atRadio);
			this.happensGroupBox.Controls.Add(this.atTimePicker);
			this.happensGroupBox.Location = new System.Drawing.Point(12, 33);
			this.happensGroupBox.Name = "happensGroupBox";
			this.happensGroupBox.Size = new System.Drawing.Size(174, 75);
			this.happensGroupBox.TabIndex = 1;
			this.happensGroupBox.TabStop = false;
			this.happensGroupBox.Text = "Happens";
			// 
			// minutesLabel
			// 
			this.minutesLabel.AutoSize = true;
			this.minutesLabel.Location = new System.Drawing.Point(124, 24);
			this.minutesLabel.MinimumSize = new System.Drawing.Size(44, 13);
			this.minutesLabel.Name = "minutesLabel";
			this.minutesLabel.Size = new System.Drawing.Size(44, 13);
			this.minutesLabel.TabIndex = 4;
			this.minutesLabel.Text = "minutes";
			// 
			// inMinutesNud
			// 
			this.inMinutesNud.Location = new System.Drawing.Point(49, 20);
			this.inMinutesNud.Name = "inMinutesNud";
			this.inMinutesNud.Size = new System.Drawing.Size(70, 21);
			this.inMinutesNud.TabIndex = 3;
			this.inMinutesNud.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.inMinutesNud.ThousandsSeparator = true;
			this.inMinutesNud.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// inRadio
			// 
			this.inRadio.AutoSize = true;
			this.inRadio.Checked = true;
			this.inRadio.Location = new System.Drawing.Point(7, 20);
			this.inRadio.Name = "inRadio";
			this.inRadio.Size = new System.Drawing.Size(36, 17);
			this.inRadio.TabIndex = 2;
			this.inRadio.TabStop = true;
			this.inRadio.Text = "&In";
			this.inRadio.UseVisualStyleBackColor = true;
			this.inRadio.CheckedChanged += new System.EventHandler(this.ReflectInterface);
			// 
			// atRadio
			// 
			this.atRadio.AutoSize = true;
			this.atRadio.Location = new System.Drawing.Point(6, 50);
			this.atRadio.Name = "atRadio";
			this.atRadio.Size = new System.Drawing.Size(37, 17);
			this.atRadio.TabIndex = 5;
			this.atRadio.Text = "&At";
			this.atRadio.UseVisualStyleBackColor = true;
			this.atRadio.CheckedChanged += new System.EventHandler(this.ReflectInterface);
			// 
			// setButton
			// 
			this.setButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.setButton.Location = new System.Drawing.Point(12, 119);
			this.setButton.Name = "setButton";
			this.setButton.Size = new System.Drawing.Size(54, 23);
			this.setButton.TabIndex = 7;
			this.setButton.Text = "Set";
			this.setButton.UseVisualStyleBackColor = true;
			this.setButton.Click += new System.EventHandler(this.SetButton_Click);
			// 
			// notifyIcon
			// 
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Visible = true;
			this.notifyIcon.BalloonTipClosed += new System.EventHandler(this.ClearButton_Click);
			this.notifyIcon.Click += new System.EventHandler(this.NotifyIcon_Click);
			this.notifyIcon.BalloonTipClicked += new System.EventHandler(this.ClearButton_Click);
			// 
			// timer
			// 
			this.timer.Interval = 1000;
			this.timer.Tick += new System.EventHandler(this.Timer_Tick);
			// 
			// clearButton
			// 
			this.clearButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.clearButton.Location = new System.Drawing.Point(72, 119);
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size(54, 23);
			this.clearButton.TabIndex = 8;
			this.clearButton.Text = "Clear";
			this.clearButton.UseVisualStyleBackColor = true;
			this.clearButton.Click += new System.EventHandler(this.ClearButton_Click);
			// 
			// hideButton
			// 
			this.hideButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.hideButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.hideButton.Location = new System.Drawing.Point(132, 119);
			this.hideButton.Name = "hideButton";
			this.hideButton.Size = new System.Drawing.Size(54, 23);
			this.hideButton.TabIndex = 9;
			this.hideButton.Text = "Hide";
			this.hideButton.UseVisualStyleBackColor = true;
			this.hideButton.Click += new System.EventHandler(this.HideButton_Click);
			// 
			// ReminderForm
			// 
			this.AcceptButton = this.setButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.hideButton;
			this.ClientSize = new System.Drawing.Size(198, 154);
			this.Controls.Add(this.hideButton);
			this.Controls.Add(this.clearButton);
			this.Controls.Add(this.setButton);
			this.Controls.Add(this.happensGroupBox);
			this.Controls.Add(this.nameCombo);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(206, 183);
			this.Name = "ReminderForm";
			this.Text = "Reminder";
			this.happensGroupBox.ResumeLayout(false);
			this.happensGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.inMinutesNud)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox nameCombo;
		private System.Windows.Forms.DateTimePicker atTimePicker;
		private System.Windows.Forms.GroupBox happensGroupBox;
		private System.Windows.Forms.NumericUpDown inMinutesNud;
		private System.Windows.Forms.RadioButton inRadio;
		private System.Windows.Forms.RadioButton atRadio;
		private System.Windows.Forms.Label minutesLabel;
		private System.Windows.Forms.Button setButton;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.Button clearButton;
		private System.Windows.Forms.Button hideButton;
	}
}