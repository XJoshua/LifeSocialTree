// using Game.GameModel;
using QFramework;
using UnityEngine;

namespace Game
{
    public class App : MonoSingleton<App>
    {
        public static App Ins { get; private set; }
    
        void Start()
        {
            Ins = this;
            
            // DataMgr.OnGameStartup();
            // Observable.Delay(60, DataMgr.Serialize2Disk); //一分钟保存一次数据到本地
            
            Service.StartUp();
            SettingData.Get().Init();
            DontDestroyOnLoad(gameObject);
            
            Service.UI.OnAppStart();
            // Service.Audio.PlayBgm(GameConfig.DefaultBgm);
        }

        private void OnApplicationQuit()
        {
            // DataMgr.OnAppQuit();
            Service.OnAppQuit();
        }

        private void OnApplicationPause(bool isPause)
        {
            //当程序暂停时，执行一次，传入参数true
            //当程序继续时，再执行一次，传入参数false。
            //用于当按Home键，或者其他意外情况：接到短信，电话，手机休眠等，
            //检测到游戏暂停，调用OnApplicationPause，弹出暂停界面

            // if (isPause)
            //     DataMgr.Serialize2Disk();
            Service.OnApplicationPause(isPause);
        }
    }
}