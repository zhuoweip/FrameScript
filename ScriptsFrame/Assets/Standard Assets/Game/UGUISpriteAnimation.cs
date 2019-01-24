using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
 
[RequireComponent(typeof(Image))]
public class UGUISpriteAnimation : MonoBehaviour
{
    private Image ImageSource;
    private int mCurFrame = 0;
    private float mDelta = 0;
 
    public float FPS = 5;
    public List<Sprite> SpriteFrames;
    public bool IsPlaying = false;
    public bool Foward = true;
    public bool AutoPlay = false;
    public bool Loop = false;
	public bool SetNatve = true;
	public int LoopStart = 0;
    public int LoopEnd = 0;
	public string SpritePath;

 
    public int FrameCount
    {
        get{return SpriteFrames.Count;}
    }
 
    void Awake()
    {
        ImageSource = GetComponent<Image>();
    }
 
    void Start()
    {
        if (AutoPlay)
            Play();
        else
            IsPlaying = false;
    }

    private void SetSprite(int idx)
    {
        ImageSource.sprite = SpriteFrames[idx];
		if(SetNatve)
        	ImageSource.SetNativeSize();   //设置精灵图片用原始大小
    }
 
    public void Play()
    {
        IsPlaying = true;
        Foward = true;
    }
 
    public void PlayReverse()
    {
        IsPlaying = true;
        Foward = false;
    }

    //用这个来判断在第几张跟第几张之间循环
    public bool IsLoopEnd;
 
    void Update()
    {
        if (!IsPlaying || 0 == FrameCount)
            return;
 
        mDelta += Time.deltaTime;
        if (mDelta > 1 / FPS)
        {
            mDelta = 0;
            if(Foward)
                mCurFrame++;
            else
                mCurFrame--;

            if (mCurFrame >= (IsLoopEnd ? LoopEnd : FrameCount)) 
            {
                if (Loop)
					mCurFrame = LoopStart;
                else
                {
                    IsPlaying = false;
                    return;
                }
            }
            else if (mCurFrame<0)
            {
                if (Loop)
                    mCurFrame = FrameCount-1;
                else
                {
                    IsPlaying = false;
                    return;
                }          
            }
             
            SetSprite(mCurFrame);
        }
    }
 
    public void Pause()
    {
        IsPlaying = false;
    }
 
    public void Resume()
    {
        if (!IsPlaying)
            IsPlaying = true;
    }
 
    public void Stop()
    {
        mCurFrame = 0;
        SetSprite(mCurFrame);
        IsPlaying = false;
    }
 
    public void Rewind()
    {
        mCurFrame = 0;
        SetSprite(mCurFrame);
        Play();
    }
}