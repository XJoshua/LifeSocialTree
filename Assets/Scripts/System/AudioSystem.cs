﻿using QFramework;

namespace GameSystem
{
    public interface IAudioSystem
    {
    }

    public class AudioSystem : BaseSystem, IAudioSystem
    {
        public override void RegisterEvents()
        {
            PlayBgm(GameConfig.DefaultBgm);
        }

        public void PlaySound(string sound)
        {
            AudioKit.PlaySound(sound);
        }

        public void PlayBgm(string bgm)
        {
            AudioKit.PlayMusic(bgm);
        }
    }
}