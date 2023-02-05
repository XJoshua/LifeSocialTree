using System;
using System.Collections.Generic;
using Game;
// using Game.UI;
using QFramework;
using QFramework.Example;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace GameSystem
{

    public interface IUISystem
    {
        void OpenPanel(string panelName);
        
        #region 流程函数

        void OnAppStart();
        
        void OnGameStart();

        void OnGameFailed();
        
        #endregion

        Camera GetUICamera();
    }
    
    // 不显示UI
    public class NullUISystem : BaseSystem, IUISystem
    {
        public void OpenPanel(string panelName) {}
        
        public void OnAppStart() {}
        
        public void OnGameStart() {}

        public void OnGameFailed() {}
        
        public Camera GetUICamera()
        {
            return null;
        }
    }
    
    /// <summary>
    /// use QFramework UIKit
    /// </summary>
    public class UISystem : BaseSystem, IUISystem
    {
        private EasyEvent enterGameEvent = new EasyEvent();
        
        private ResLoader mResLoader = ResLoader.Allocate();
        
        public UISystem()
        {
            RegisterUIEvent();
            UIKitInit();
        }
        
        private void RegisterUIEvent()
        {
            //TypeEventSystem.Global.Register<GameStartEvent>(OnGameStart);
        }

        private void UIKitInit()
        {
            UIKit.Root.SetResolution(1920,1080,0);
        }
        
        public void OpenPanel(string panelName)
        {
            throw new System.NotImplementedException();
        }

        public void OnAppStart()
        {
            UIKit.Root.SetResolution(1920,1080,0);
            //UIKit.OpenPanel<UIHomePanel>();
            TheGame.Get().NewGame();

            UIKit.OpenPanel<UIGamePanel>();
        }
        
        public void OnGameStart()
        {
            //UIKit.OpenPanel<UIGamePanel>();
        }

        public void OnGameFailed()
        {
            //UIKit.OpenPanel<UIGameEndDialog>();
        }
        
        public Camera GetUICamera()
        {
            return UIRoot.Instance.Camera;
        }
    }
}