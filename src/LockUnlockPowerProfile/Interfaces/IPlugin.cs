namespace LockUnlockPowerProfile.Interfaces
{
	public interface IPlugin
	{
		string Name { get; }
		string Description { get; }
		void OnLock();
		void OnUnlock();
		void OnStart();
		void OnEnd();
	}
}