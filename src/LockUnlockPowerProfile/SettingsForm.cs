using LockUnlockPowerProfile.Interfaces;
using LockUnlockPowerProfile.Models;
using LockUnlockPowerProfile.Services;
using LockUnlockPowerProfile.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LockUnlockPowerProfile
{
	internal partial class SettingsForm : Form
	{
		private readonly SettingsService _settingsService;
		private readonly PluginService _pluginService;

		private readonly SettingsModel _settingsModel;

		internal SettingsForm(
			SettingsService settingsService, PluginService pluginService)
		{
			InitializeComponent();
			_settingsModel = settingsService.CurrentSettings;

			_settingsService = settingsService;
			_pluginService = pluginService;
			LoggerService.Instance.AddObserver(UpdateLogger);

			IEnumerable<Guid> guidPlans = PowerFunctions.GetAll();

			foreach (Guid guidPlan in guidPlans)
			{
				PowerPlanModel p = new PowerPlanModel(guidPlan);
				lockedProfileComboBox.Items.Add(p);
				unlockProfileComboBox.Items.Add(p);
				if (_settingsModel.LockPowerPlan.Equals(p))
					lockedProfileComboBox.SelectedIndex = lockedProfileComboBox.Items.IndexOf(p);
				if (_settingsModel.UnlockPowerPlan.Equals(p))
					unlockProfileComboBox.SelectedIndex = unlockProfileComboBox.Items.IndexOf(p);
			}

			pluginListBox.DisplayMember = "Name";

			foreach (IPlugin plugin in _pluginService.GetAllPlugins())
			{
				pluginListBox.Items.Add(plugin, _settingsModel.Plugins.ContainsKey(plugin) && _settingsModel.Plugins[plugin]);
			}

			pluginListBox.SelectedValueChanged += PluginListBoxOnSelectedValueChanged;
			pluginDescription.Text = string.Empty;
		}

		private void PluginListBoxOnSelectedValueChanged(object sender, EventArgs e)
		{
			if (pluginListBox.SelectedItem is IPlugin plugin)
			{
				pluginDescription.Text = plugin.Description;
			}
		}

		private void SaveButtonClick(object sender, EventArgs e)
		{
			_settingsModel.UnlockPowerPlan = (PowerPlanModel)unlockProfileComboBox.SelectedItem;
			_settingsModel.LockPowerPlan = (PowerPlanModel)lockedProfileComboBox.SelectedItem;


			foreach (IPlugin checkedPlugin in pluginListBox.Items)
			{
				KeyValuePair<IPlugin, bool> kvPair =
					_settingsModel.Plugins.SingleOrDefault(x => x.Key.Name == checkedPlugin.Name);
				if (pluginListBox.CheckedItems.Contains(checkedPlugin))
				{
					_settingsModel.Plugins[kvPair.Key] = true;
					_pluginService.EnablePlugin(kvPair.Key);
				}
				else
				{
					_settingsModel.Plugins[kvPair.Key] = false;
					_pluginService.DisablePlugin(kvPair.Key);
				}
			}

			_settingsService.SaveSettings(_settingsModel);
		}

		private void UpdateLogger()
		{
			richTextBox1.Text = string.Empty;
			foreach (string log in LoggerService.Instance.GetLogs())
			{
				richTextBox1.Text += log + @"
";
			}
		}
	}
}