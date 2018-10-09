﻿using System;
using System.Collections.Generic;
using UnityEngine;

public enum MusicType
{
    NormalBg,
    Click,
    ClickRight,
    CountDown,
}

/// <summary>
/// 音乐种类
/// </summary>
public enum SoundType
{
    /// <summary>背景音</summary>
    Bg,
    /// <summary>音效</summary>
    SoundFx,
}

namespace UICore
{
    public class AudioManager : UnitySingleton<AudioManager>
    {

        public Dictionary<MusicType, string> dic = new Dictionary<MusicType, string>
        {
            { MusicType.NormalBg,"柔和的风_爱给网_aigei_com"},
            { MusicType.Click,"点击音效"},
            { MusicType.ClickRight,"点击正确"},
            { MusicType.CountDown,"倒计时"},
        };

        public Action OnCompleteAction;
        private float time = 0;

        private AudioSource _audioSource;
        private AudioSource _bgSource;
        private AudioSource[] sources;

        public AudioSource BgSource
        {
            get { return _bgSource; }
        }

        public AudioSource AudioSource(int index)
        {
            return audioSources[index];
        }

        private AudioSource[] audioSources;

        void Awake()
        {
            for (int i = 0; i < 5; i++)
                transform.gameObject.AddComponent<AudioSource>();
            sources = transform.GetComponents<AudioSource>();
            _bgSource = sources[0];
            audioSources = (AudioSource[])LinqUtil.CustomWhere(sources,item => item != _bgSource);
        }

        public void Play_Bg(MusicType type)
        {
            if (_bgSource != null)
            {
                _bgSource.Stop();
                _bgSource.clip = null;
            }
            AudioClip audioClip = Resources.Load<AudioClip>("Music" + "/" + dic[type]);
            _bgSource.clip = audioClip;
            _bgSource.Play();
            _bgSource.loop = true;
        }

        public void Play_Audio(MusicType type = MusicType.Click, int audioIndex = 0, bool isLoop = false)
        {
            time = 0;
            //切换场景时先关闭所有非背景音效
            foreach (var item in audioSources)
            {
                if (item != null)
                {
                    item.Stop();
                    item.clip = null;
                }
            }
            _audioSource = audioSources[audioIndex];
            AudioClip audioClip = Resources.Load<AudioClip>("Music" + "/" + dic[type]);
            _audioSource.clip = audioClip;
            _audioSource.Play();
            _audioSource.loop = isLoop;
        }

        protected override void Update()
        {
            if (_audioSource != null && _audioSource.clip != null)
            {
                time += Time.deltaTime;
                if (time >= _audioSource.clip.length && OnCompleteAction != null)
                {
                    OnCompleteAction();
                    OnCompleteAction = null;
                }
            }
        }
    }

    #region Audio链式拓展
    public static class AudioExtension
    {
        public static AudioManager PlayBg(this AudioManager gm, MusicType musicType)
        {
            gm.Play_Bg(musicType);
            return gm;
        }

        /// <summary>播放</summary>
        public static AudioManager PlayAudio(this AudioManager gm, MusicType musicType = MusicType.Click, int index = 0, bool isLoop = false)
        {
            gm.Play_Audio(musicType, index, isLoop);
            return gm;
        }

        /// <summary>完成回调</summary>
        public static AudioManager OnComplete(this AudioManager gm, Action action)
        {
            gm.OnCompleteAction = action;
            return gm;
        }

        /// <summary>设置音量</summary>
        public static AudioManager SetVolume(this AudioManager gm, float volume, int index = 0, SoundType soundType = SoundType.SoundFx)
        {
            if (soundType == SoundType.Bg)
                gm.BgSource.volume = volume;
            else
                gm.AudioSource(index).volume = volume;
            return gm;
        }
    }
    #endregion
}

