using System;
using System.Collections.Generic;
using UnityEngine;

public enum MusicType
{
    NormalBg,
    Click,
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
        { MusicType.NormalBg,"《龙谣》的4张专辑-02-道情(CCTV酒业广告曾用)_爱给网_aigei_com"},
        { MusicType.Click,"click"},
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

        public AudioSource AudioSource
        {
            get { return _audioSource; }
        }

        void Awake()
        {
            for (int i = 0; i < 2; i++)
                transform.gameObject.AddComponent<AudioSource>();
            sources = transform.GetComponents<AudioSource>();
            _audioSource = sources[0];
            _bgSource = sources[1];

            PlayBg(MusicType.NormalBg);
        }

        public void PlayBg(MusicType type)
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

        public void PlayAudio(MusicType type = MusicType.Click)
        {
            time = 0;
            if (_audioSource != null)
            {
                _audioSource.Stop();
                _audioSource.clip = null;
            }
            AudioClip audioClip = Resources.Load<AudioClip>("Music" + "/" + dic[type]);
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }

        private void Update()
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
        /// <summary>播放</summary>
        public static AudioManager Play(this AudioManager gm, MusicType musicType = MusicType.Click, SoundType soundType = SoundType.SoundFx)
        {
            if (soundType == SoundType.Bg)
                gm.PlayBg(musicType);
            else
                gm.PlayAudio(musicType);
            return gm;
        }

        /// <summary>完成回调</summary>
        public static AudioManager OnComplete(this AudioManager gm, Action action)
        {
            gm.OnCompleteAction = action;
            return gm;
        }

        /// <summary>设置音量</summary>
        public static AudioManager SetVolume(this AudioManager gm, float volume, SoundType soundType = SoundType.SoundFx)
        {
            if (soundType == SoundType.Bg)
                gm.BgSource.volume = volume;
            else
                gm.AudioSource.volume = volume;
            return gm;
        }
    }
    #endregion
}


