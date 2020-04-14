using LockUnlockPowerProfile.Utils;
using System;

namespace LockUnlockPowerProfile.Models
{
	internal class PowerPlanModel
	{
		public Guid Guid { get; }
		public string Name { get; }

		public PowerPlanModel(Guid guid)
		{
			Guid = guid;
			// Set the name based on the given guid
			Name = PowerFunctions.ReadFriendlyName(guid);
		}

		public override string ToString()
		{
			return Name;
		}

		protected bool Equals(PowerPlanModel other)
		{
			return Guid.Equals(other.Guid);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((PowerPlanModel)obj);
		}

		public override int GetHashCode()
		{
			return Guid.GetHashCode();
		}
	}
}