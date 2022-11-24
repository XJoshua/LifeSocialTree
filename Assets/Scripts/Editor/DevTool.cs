using System.Collections.Generic;
using cfg;
using QFramework;
using SimpleJSON;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class DevTool
    {
        [MenuItem("Tools/DevTool/Check Symbol Table")]
        public static void CheckSymbolTable()
        {
            // ResLoader mResLoader = ResLoader.Allocate();
            var tables = new cfg.Tables(file => 
                JSON.Parse(Resources.Load<TextAsset>("TextAsset/JsonConfig/" + file).text));
            var list = tables.TbSymbol.DataList;
            var nameDic = new Dictionary<string, string>();
            var iconDic = new Dictionary<string, string>();
            string result = string.Empty;
            for (var i = 0; i < list.Count; i++)
            {
                var name = GameUtil.GetLocalize("sbl/" + list[i].Name);
                if (nameDic.ContainsKey(name))
                {
                    result += $"\nDuplication name in {nameDic[name]};{list[i].Id}";
                    nameDic[name] = nameDic[name] + ";" + list[i].Id;
                }
                else
                {
                    nameDic.Add(name, list[i].Id);
                }
                
                var icon = list[i].Image;
                if (iconDic.ContainsKey(icon))
                {
                    result += $"\nDuplication icon in {iconDic[icon]};{list[i].Id}";
                    iconDic[icon] = iconDic[icon] + ";" + list[i].Id;
                }
                else
                {
                    iconDic.Add(icon, list[i].Image);
                }
            }

            if (result.IsNullOrEmpty())
            {
                Debug.Log("Check Symbol Table Success!");
            }
            else
            {
                Debug.LogError("Check Symbol Table Error! \n" + result);
            }
        }
    }
}