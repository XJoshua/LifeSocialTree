using System;
using System.Collections.Generic;
// using Game.UI;
using GameSystem;
using QFramework;
using Sirenix.Utilities;

namespace Game
{
    public static class Service
    {
        private static readonly Dictionary<Type, BaseSystem> Systems = new Dictionary<Type, BaseSystem>();
        
        public static IUISystem UI;
        public static ConfigSystem Cfg;
        public static AudioSystem Audio;
        
        // public static IEasySaveSystem EasySave;
        // public static BattleSystem BattleSys;
        // public static IAnalysisSystem analysisSys;
        // public static IGameControllerSystem Ctrl;
        // public static PoolSystem Pool;
        
        public static async void StartUp()
        {
            ResKit.Init();
            
            Cfg = AddSystem<ConfigSystem>();
            // Ctrl = AddSystem<GameControllerSystem>();
            //UI = AddSystem<NullUISystem>();
            UI = AddSystem<UISystem>();
            Audio = AddSystem<AudioSystem>();
            // Pool = AddSystem<PoolSystem>();
            
            // await UniTask.NextFrame();
            // EasySave = AddSystem<EasySaveSystem>();
            // analysisSys = AddSystem<AnalysisSystem>();
            // AddSystem<GamePlaySystem>();
            // await UniTask.NextFrame();
            // BattleSys = AddSystem<BattleSystem>();
            // AddSystem<BagSystem>();
            // AddSystem<GeneSystem>();

            //TheGame.Get().OnSingletonInit();
            
            EnterInitScene();
        }

        static void EnterInitScene()
        {
            UI.OnAppStart();
        }

        static T AddSystem<T>(bool isRegisterEvent = true) where T : BaseSystem, new()
        {
            var sys = new T();
            if (isRegisterEvent)
                sys.RegisterEvents();
            sys.StartSystem();
            // sys.StartSystemAsync().Forget();
            Systems[typeof(T)] = sys;
            return sys;
        }

        public static T GetSystem<T>() where T : BaseSystem
        {
            if (Systems.TryGetValue(typeof(T), out var sys))
            {
                return (T)sys;
            }
            return null;
        }

        public static void OnAppQuit()
        {
            if (null != Systems && 0 < Systems.Count)
            {
                LinqExtensions.ForEach(Systems, (sys) =>
                {
                    sys.Value.OnAppQuit();
                });
            }
        }

        public static void OnApplicationPause(bool isPause)
        {
            if (null != Systems && 0 < Systems.Count)
            {
                LinqExtensions.ForEach(Systems, (sys) =>
                {
                    sys.Value.OnApplicationPause(isPause);
                });
            }
        }

        #region 扩展


        #endregion   
    }
}
