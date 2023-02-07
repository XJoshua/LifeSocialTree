using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UIGamePanelData : UIPanelData
	{
	}
	public partial class UIGamePanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIGamePanelData ?? new UIGamePanelData();
			// please add init code here
			
			BtnMenu.onClick.AddListener(OnClickMenu);
			
			MenuBg.onClick.AddListener(CloseMenu);
			BtnResume.onClick.AddListener(CloseMenu);
			
			BtnRestart.onClick.AddListener(OnClickRestart);
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}

		protected void Update()
		{
			if (TheGame.Get() != null && TheGame.Get().TreeMng != null)
			{
				TxtLifeTime.text = $"Life Time: {(int)TheGame.Get().TreeMng.TotalTime}";
			}
			else
			{
				TxtLifeTime.text = "Life Time: 0";
			}
		}

		public void OnClickMenu()
		{
			MenuPanel.gameObject.SetActive(true);
		}

		public void CloseMenu()
		{
			MenuPanel.gameObject.SetActive(false);
		}

		public void OnClickRestart()
		{
			MenuPanel.gameObject.SetActive(false);
			TheGame.Get().NewGame();
		}
		
	}
}
