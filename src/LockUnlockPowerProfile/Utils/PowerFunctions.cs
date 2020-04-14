using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LockUnlockPowerProfile.Utils
{
	internal class PowerFunctions
	{
		public enum AccessFlags : uint
		{
			AccessScheme = 16,
			AccessSubgroup = 17,
			AccessIndividualSetting = 18
		}

		public static Guid GetActiveGuid()
		{
			Guid activeScheme = Guid.Empty;
			IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
			if (PowerGetActiveScheme((IntPtr)null, out ptr) == 0)
			{
				activeScheme = (Guid)Marshal.PtrToStructure(ptr, typeof(Guid));
				Marshal.FreeHGlobal(ptr);
			}

			return activeScheme;
		}

		public static string ReadFriendlyName(Guid schemeGuid)
		{
			uint sizeName = 1024;
			IntPtr pSizeName = Marshal.AllocHGlobal((int)sizeName);

			string friendlyName;

			try
			{
				PowerReadFriendlyName(IntPtr.Zero, ref schemeGuid, IntPtr.Zero, IntPtr.Zero, pSizeName,
					ref sizeName);
				friendlyName = Marshal.PtrToStringUni(pSizeName);
			}
			finally
			{
				Marshal.FreeHGlobal(pSizeName);
			}

			return friendlyName;
		}

		public static IEnumerable<Guid> GetAll()
		{
			Guid schemeGuid = Guid.Empty;
			uint sizeSchemeGuid = (uint)Marshal.SizeOf(typeof(Guid));
			uint schemeIndex = 0;

			while (PowerEnumerate(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, (uint)AccessFlags.AccessScheme,
				schemeIndex, ref schemeGuid, ref sizeSchemeGuid) == 0)
			{
				yield return schemeGuid;
				schemeIndex++;
			}
		}

		[DllImport("powrprof.dll", EntryPoint = "PowerSetActiveScheme")]
		public static extern uint PowerSetActiveScheme(IntPtr userPowerKey, ref Guid activePolicyGuid);

		[DllImport("powrprof.dll", EntryPoint = "PowerGetActiveScheme")]
		public static extern uint PowerGetActiveScheme(IntPtr userPowerKey, out IntPtr activePolicyGuid);

		[DllImport("powrprof.dll", EntryPoint = "PowerReadFriendlyName")]
		public static extern uint PowerReadFriendlyName(IntPtr rootPowerKey, ref Guid schemeGuid,
			IntPtr subGroupOfPowerSettingsGuid, IntPtr powerSettingGuid, IntPtr buffer, ref uint bufferSize);

		[DllImport("powrprof.dll", EntryPoint = "PowerEnumerate")]
		public static extern uint PowerEnumerate(IntPtr rootPowerKey, IntPtr schemeGuid,
			IntPtr subGroupOfPowerSettingGuid, uint acessFlags, uint index, ref Guid buffer, ref uint bufferSize);
	}
}