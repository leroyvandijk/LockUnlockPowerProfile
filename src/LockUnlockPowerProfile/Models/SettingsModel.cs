using LockUnlockPowerProfile.Interfaces;
using System.Collections.Generic;

namespace LockUnlockPowerProfile.Models
{
	internal class SettingsModel
	{
		public PowerPlanModel UnlockPowerPlan { get; set; }
		public PowerPlanModel LockPowerPlan { get; set; }
		public Dictionary<IPlugin, bool> Plugins { get; set; }
	}
}