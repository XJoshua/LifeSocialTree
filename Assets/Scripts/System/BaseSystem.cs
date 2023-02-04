namespace GameSystem
{
    public class BaseSystem
    {
        protected bool IsSystemRunning { get; private set; }

        public virtual void RegisterEvents()
        {
        }

        public virtual void OnAppQuit()
        {
        }

        public virtual void StartSystem()
        {
            IsSystemRunning = true;
        }

        // public virtual async UniTaskVoid StartSystemAsync()
        // {
        // }

        public virtual void StopSystem()
        {
            IsSystemRunning = false;
        }

        public virtual void OnApplicationPause(bool isPause)
        {
        }
    }
}