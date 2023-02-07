using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:9480f5cf-f9bb-4836-9a29-cfe3a45dfaba
	public partial class UIGamePanel
	{
		public const string Name = "UIGamePanel";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnMenu;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtLifeTime;
		[SerializeField]
		public TMPro.TextMeshProUGUI Tips;
		[SerializeField]
		public RectTransform MenuPanel;
		[SerializeField]
		public UnityEngine.UI.Button MenuBg;
		[SerializeField]
		public UnityEngine.UI.Button BtnRestart;
		[SerializeField]
		public UnityEngine.UI.Button BtnResume;
		
		private UIGamePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnMenu = null;
			TxtLifeTime = null;
			Tips = null;
			MenuPanel = null;
			MenuBg = null;
			BtnRestart = null;
			BtnResume = null;
			
			mData = null;
		}
		
		public UIGamePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIGamePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIGamePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
