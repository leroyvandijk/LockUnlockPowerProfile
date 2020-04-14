using LockUnlockPowerProfile.Properties;

namespace LockUnlockPowerProfile
{
	internal partial class SettingsForm
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
            if (disposing && (components != null))
            {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
			this.saveButton = new System.Windows.Forms.Button();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.lockedProfileComboBox = new System.Windows.Forms.ComboBox();
			this.lockProfileLabel = new System.Windows.Forms.Label();
			this.unlockProfileComboBox = new System.Windows.Forms.ComboBox();
			this.unlockProfileLabel = new System.Windows.Forms.Label();
			this.pluginListBox = new System.Windows.Forms.CheckedListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.pluginDescription = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// saveButton
			// 
			this.saveButton.Location = new System.Drawing.Point(142, 286);
			this.saveButton.Margin = new System.Windows.Forms.Padding(2);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(155, 32);
			this.saveButton.TabIndex = 0;
			this.saveButton.Text = "Save";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.SaveButtonClick);
			// 
			// richTextBox1
			// 
			this.richTextBox1.Location = new System.Drawing.Point(516, 2);
			this.richTextBox1.Margin = new System.Windows.Forms.Padding(2);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.Size = new System.Drawing.Size(453, 317);
			this.richTextBox1.TabIndex = 1;
			this.richTextBox1.Text = "";
			// 
			// lockedProfileComboBox
			// 
			this.lockedProfileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.lockedProfileComboBox.FormattingEnabled = true;
			this.lockedProfileComboBox.Location = new System.Drawing.Point(142, 8);
			this.lockedProfileComboBox.Margin = new System.Windows.Forms.Padding(2);
			this.lockedProfileComboBox.Name = "lockedProfileComboBox";
			this.lockedProfileComboBox.Size = new System.Drawing.Size(311, 21);
			this.lockedProfileComboBox.TabIndex = 2;
			// 
			// lockProfileLabel
			// 
			this.lockProfileLabel.AutoSize = true;
			this.lockProfileLabel.Location = new System.Drawing.Point(15, 10);
			this.lockProfileLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lockProfileLabel.Name = "lockProfileLabel";
			this.lockProfileLabel.Size = new System.Drawing.Size(103, 13);
			this.lockProfileLabel.TabIndex = 3;
			this.lockProfileLabel.Text = "Profile when locked:";
			// 
			// unlockProfileComboBox
			// 
			this.unlockProfileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.unlockProfileComboBox.FormattingEnabled = true;
			this.unlockProfileComboBox.Location = new System.Drawing.Point(142, 38);
			this.unlockProfileComboBox.Margin = new System.Windows.Forms.Padding(2);
			this.unlockProfileComboBox.Name = "unlockProfileComboBox";
			this.unlockProfileComboBox.Size = new System.Drawing.Size(311, 21);
			this.unlockProfileComboBox.TabIndex = 4;
			// 
			// unlockProfileLabel
			// 
			this.unlockProfileLabel.AutoSize = true;
			this.unlockProfileLabel.Location = new System.Drawing.Point(15, 40);
			this.unlockProfileLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.unlockProfileLabel.Name = "unlockProfileLabel";
			this.unlockProfileLabel.Size = new System.Drawing.Size(115, 13);
			this.unlockProfileLabel.TabIndex = 5;
			this.unlockProfileLabel.Text = "Profile when unlocked:";
			// 
			// pluginListBox
			// 
			this.pluginListBox.FormattingEnabled = true;
			this.pluginListBox.Location = new System.Drawing.Point(142, 75);
			this.pluginListBox.Name = "pluginListBox";
			this.pluginListBox.Size = new System.Drawing.Size(311, 94);
			this.pluginListBox.TabIndex = 7;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(15, 75);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(85, 13);
			this.label1.TabIndex = 8;
			this.label1.Text = "Enabled plugins:";
			// 
			// pluginDescription
			// 
			this.pluginDescription.AutoSize = true;
			this.pluginDescription.Location = new System.Drawing.Point(139, 176);
			this.pluginDescription.MaximumSize = new System.Drawing.Size(310, 100);
			this.pluginDescription.Name = "pluginDescription";
			this.pluginDescription.Size = new System.Drawing.Size(310, 100);
			this.pluginDescription.TabIndex = 9;
			this.pluginDescription.Text = resources.GetString("pluginDescription.Text");
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(967, 329);
			this.Controls.Add(this.pluginDescription);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pluginListBox);
			this.Controls.Add(this.unlockProfileLabel);
			this.Controls.Add(this.unlockProfileComboBox);
			this.Controls.Add(this.lockProfileLabel);
			this.Controls.Add(this.lockedProfileComboBox);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.saveButton);
			this.Icon = global::LockUnlockPowerProfile.Properties.Resources.logo;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(987, 368);
			this.MinimumSize = new System.Drawing.Size(515, 368);
			this.Name = "SettingsForm";
			this.Text = "Settings";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ComboBox lockedProfileComboBox;
        private System.Windows.Forms.Label lockProfileLabel;
        private System.Windows.Forms.ComboBox unlockProfileComboBox;
        private System.Windows.Forms.Label unlockProfileLabel;
		private System.Windows.Forms.CheckedListBox pluginListBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label pluginDescription;
	}
}