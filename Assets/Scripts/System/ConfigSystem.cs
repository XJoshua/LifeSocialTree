using System;
using System.Collections.Generic;
using Game;
using SimpleJSON;
using UnityEngine;
using QFramework;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace GameSystem
{
    public interface IConfigSystem
    {
        
    }
    
    public class ConfigSystem : BaseSystem, IConfigSystem
    {
        private ResLoader mResLoader = ResLoader.Allocate();
        private DevelopConfig developConfig;
        
        private cfg.Tables tables;
        public cfg.Tables Tables => tables;
        
        private cfg.BaseConfig baseConfig;
        public cfg.BaseConfig BaseConfig => baseConfig;
        
        public override void RegisterEvents()
        {
            tables = new cfg.Tables(file => 
                JSON.Parse(Resources.Load<TextAsset>("TextAsset/JsonConfig/" + file).text));
            
            baseConfig = Tables.TbBaseConfig.Get(GameConfig.BaseConfigId);
        }
        
        public cfg.CharacterConfig GetCfgCharacter(string id)
        {
            cfg.CharacterConfig result;
            try
            {
                result = Tables.TbCharacter.Get(id);
            }
            catch (Exception e)
            {
                Debug.LogError($"GetCfgWeather id: {id} {e}");
                throw;
            }
        
            return result;
        }
        
        public cfg.EffectConfig GetCfgEffect(string id)
        {
            cfg.EffectConfig result;
            try
            {
                result = Tables.TbEffect.Get(id);
            }
            catch (Exception e)
            {
                Debug.LogError($"GetCfgEffect id: {id} {e}");
                throw;
            }
        
            return result;
        }
        
        public List<cfg.CharacterConfig> GetAllCfgCharacters()
        {
            return Tables.TbCharacter.DataList;
        }
        
        public Sprite GetCharacterIcon(string id)
        {
            var prop = GetCfgCharacter(id);
            var spriteAtlas = mResLoader.LoadSync<SpriteAtlas>(GameConfig.IconAtlasPath);
            var icon = spriteAtlas.GetSprite(prop.Image);
            mResLoader.AddObjectForDestroyWhenRecycle2Cache(icon);
            return icon;
        }

        public Sprite GetIconSprite(string iconPath)
        {
            var spriteAtlas = mResLoader.LoadSync<SpriteAtlas>(GameConfig.IconAtlasPath);
            var icon = spriteAtlas.GetSprite(iconPath);
            mResLoader.AddObjectForDestroyWhenRecycle2Cache(icon);
            return icon;
        }
    }
}