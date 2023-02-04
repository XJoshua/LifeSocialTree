using QFramework;
using UnityEngine;

namespace Game
{
    public enum Language
    {
        English,
        Chinese,
    }
    
    /// <summary>
    /// 玩家设置
    /// </summary>
    public class SettingData : Singleton<SettingData>
    {
        private SettingData() {}
        
        public Language Language = Language.English;


        // 只有第一次调用，之后就用保存的数据
        public void Init()
        {
            if (Application.systemLanguage == SystemLanguage.Chinese 
                || Application.systemLanguage == SystemLanguage.ChineseSimplified)
            {
                Language = Language.Chinese;
            }
            else
            {
                Language = Language.English;
            }
        }

    }
}