using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 全局有一个SoundManager，也可以创建游戏自己的SoundManager
/// </summary>
public class SoundManager : MonoBehaviour {
    //全局的SoundManager
    public static SoundManager instance = null;
    string mAssetbundleName = "";

    void Awake()
    {
        if (null == instance)
        {
            //Global全局的SoundManager
            instance = this;
            Debug.Log("初始化全局SoundManager");
        }
        else
        {
            
        }
    }
    void Start ()
    {
	
	}
    public void SetAbName(string ab_name)
    {
        mAssetbundleName = ab_name;
    }


    AudioSource mComBg { get; set; }
    public void PlayBg(string sound_name, float volume = 1)
    {
        if(null == mComBg)
        {
            mComBg = gameObject.AddComponent<AudioSource>();
            mComBg.loop = true;
        }
        mComBg.clip = ResManager.GetClipInResources(sound_name);
        mComBg.volume = volume;
        mComBg.Play();
    }
    public void PlayBg(string ab_name, string sound_name, float volume = 1)
    {
        if (null == mComBg)
        {
            mComBg = gameObject.AddComponent<AudioSource>();
            mComBg.loop = true;
        }
        mComBg.clip = ResManager.GetClip(ab_name, sound_name);
        mComBg.volume = volume;
        mComBg.Play();
    }
    public void PlayBg(AudioClip _cp, float volume = 1)
    {
        if (null == mComBg)
        {
            mComBg = gameObject.AddComponent<AudioSource>();
            mComBg.loop = true;
        }
        mComBg.clip = _cp;
        mComBg.volume = volume;
        mComBg.Play();
    }
    public void PlayBgAsync(string ab_name, string sound_name, float volume = 1)
    {
        if (null == mComBg)
        {
            mComBg = gameObject.AddComponent<AudioSource>();
            mComBg.loop = true;
        }ResManager.GetClipAsync(ab_name, sound_name, PlayBgAsync_Callback);
        mComBg.volume = volume;
    }
    public void PlayBgAsync_Callback(AudioClip clip)
    {
        mComBg.clip = clip;
        mComBg.Play();
    }
    public void PlayBg()
    {
        if (null != mComBg && !mComBg.isPlaying)
            mComBg.Play();
    }
    public void StopBg()
    {
        if (null != mComBg)
            mComBg.Stop();
    }


    AudioSource mComBg2 { get; set; }
    public void PlayBg2(string sound_name, float volume = 1)
    {
        if (null == mComBg2)
        {
            mComBg2 = gameObject.AddComponent<AudioSource>();
            mComBg2.loop = true;
        }
        mComBg2.clip = ResManager.GetClipInResources(sound_name);
        mComBg2.volume = volume;
        mComBg2.Play();
    }
    public void PlayBg2(string ab_name, string sound_name, float volume = 1)
    {
        if (null == mComBg2)
        {
            mComBg2 = gameObject.AddComponent<AudioSource>();
            mComBg2.loop = true;
        }
        mComBg2.clip = ResManager.GetClip(ab_name, sound_name);
        mComBg2.volume = volume;
        mComBg2.Play();
    }
    public void PlayBg2Async(string ab_name, string sound_name, float volume = 1)
    {
        if (null == mComBg2)
        {
            mComBg2 = gameObject.AddComponent<AudioSource>();
            mComBg2.loop = true;
        }
        ResManager.GetClipAsync(ab_name, sound_name, PlayBg2Async_Callback);
        mComBg2.volume = volume;
    }
    public void PlayBg2Async_Callback(AudioClip clip)
    {
        mComBg2.clip = clip;
        mComBg2.Play();
    }
    public void PlayBg2()
    {
        if (null != mComBg2 && !mComBg2.isPlaying)
            mComBg2.Play();
    }
    public void StopBg2()
    {
        if (null != mComBg2)
            mComBg2.Stop();
    }



    //唯一播放控件，播放下一个会终止上一个
    AudioSource mComOnly { get; set; }
    public void PlayOnly(string ab_name, string sound_name, float volume = 1, bool loop = false)
    {
        //Debug.Log("PlayOnly " + sound_name);
        if (null == mComOnly)
        {
            mComOnly = gameObject.AddComponent<AudioSource>();
            mComOnly.loop = false;
        }
        mComOnly.clip = ResManager.GetClip(ab_name, sound_name);
        mComOnly.volume = volume;
        mComOnly.loop = loop;
        mComOnly.Play();
    }
    public void PlayOnly(string sound_name, float volume = 1)
    {
        Debug.Log("PlayOnly " + sound_name);
        if (null == mComOnly)
        {
            mComOnly = gameObject.AddComponent<AudioSource>();
            mComOnly.loop = false;
        }
        mComOnly.clip = ResManager.GetClipInResources(sound_name);
        mComOnly.volume = volume;
        mComOnly.Play();
    }
    public void PlayOnlyDefaultAb(string sound_name, float volume = 1, bool loop = false)
    {
        //Debug.Log("PlayOnly " + sound_name);
        if (null == mComOnly)
        {
            mComOnly = gameObject.AddComponent<AudioSource>();
            mComOnly.loop = false;
        }
        mComOnly.clip = ResManager.GetClip(mAssetbundleName, sound_name);
        mComOnly.volume = volume;
        mComOnly.loop = loop;
        mComOnly.Play();
    }
    public void StopOnly()
    {
        if (null == mComOnly)
        {
            mComOnly = gameObject.AddComponent<AudioSource>();
            mComOnly.loop = false;
        }
        mComOnly.Stop();
        //Debug.LogError("StopOnly()");
    }
    public bool IsPlayingOnly()
    {
        if (null == mComOnly)
            return false;
        return mComOnly.isPlaying;
    }
         


    List<AudioSource> mComShorts = new List<AudioSource>();
    /*
    public void PlayShortTip(string ab_name, string sound_name, float volume = 1)
    {
        TopTitleCtl.instance.mSoundTipData.SetData(this, "short", ab_name, sound_name);
        PlayShort(ab_name, sound_name, volume);
    }
    */
    public void PlayShort(string ab_name, string sound_name, float volume = 1)
    {
        AudioSource com = null;
        for(int i = 0; i < mComShorts.Count; i++)
        {
            if(!mComShorts[i].isPlaying)
            {
                com = mComShorts[i];
                break;
            }
        }
        if(null == com)
        {
            com = gameObject.AddComponent<AudioSource>();
            mComShorts.Add(com);
        }
        com.clip = ResManager.GetClip(ab_name, sound_name);
        com.volume = volume;
        com.Play();
    }
    public void PlayShort(string sound_name, float volume = 1)
    {
        AudioSource com = null;
        for (int i = 0; i < mComShorts.Count; i++)
        {
            if (!mComShorts[i].isPlaying)
            {
                com = mComShorts[i];
                break;
            }
        }
        if (null == com)
        {
            com = gameObject.AddComponent<AudioSource>();
            mComShorts.Add(com);
        }
        com.clip = ResManager.GetClipInResources(sound_name);
        com.volume = volume;
        com.Play();
    }
    public void PlayShortDefaultAb( string sound_name, float volume = 1)
    {
        AudioSource com = null;
        for (int i = 0; i < mComShorts.Count; i++)
        {
            if (!mComShorts[i].isPlaying)
            {
                com = mComShorts[i];
                break;
            }
        }
        if (null == com)
        {
            com = gameObject.AddComponent<AudioSource>();
            mComShorts.Add(com);
        }
        com.clip = ResManager.GetClip(mAssetbundleName, sound_name);
        com.volume = volume;
        com.Play();
    }


    //流水播放
    AudioSource mComSoundList { get; set; }
    List<string> soundlist_ab_names = null;
    List<string> soundlist_sound_names;
    List<System.Action> soundlist_funcs = null;
    public void PlaySoundList(List<string> ab_names, List<string> sound_names, List<System.Action> callback_funcs = null)
    {
        if (null == ab_names || null == sound_names || 0 == sound_names.Count)
            return;
        soundlist_ab_names = ab_names;
        soundlist_funcs = null;

        if (null == mComSoundList)
        {
            mComSoundList = gameObject.AddComponent<AudioSource>();
            mComSoundList.loop = false;
        }
        soundlist_funcs = callback_funcs;
        soundlist_sound_names = sound_names;
        //soundlist_sound_names = Common.CopyList<string>(sound_names);

        StopCoroutine("TSoundList");
        StartCoroutine("TSoundList");

    }
    public void PlaySoundList(string ab_name, List<string> sound_names, List<System.Action> callback_funcs = null)
    {
        if (string.IsNullOrEmpty(ab_name) || null == sound_names || 0 == sound_names.Count)
            return;
        soundlist_funcs = null;

        if (null == mComSoundList)
        {
            mComSoundList = gameObject.AddComponent<AudioSource>();
            mComSoundList.loop = false;
        }
        soundlist_funcs = callback_funcs;
        soundlist_ab_names = new List<string>() { ab_name };
        soundlist_sound_names = sound_names;
        //soundlist_sound_names = Common.CopyList<string>(sound_names);

        StopCoroutine("TSoundList");
        StartCoroutine("TSoundList");

    }
    public void PlaySoundList(List<string> sound_names, List<System.Action> callback_funcs = null)
    {
        if (null == sound_names || 0 == sound_names.Count)
            return;
        soundlist_funcs = null;

        if (null == mComSoundList)
        {
            mComSoundList = gameObject.AddComponent<AudioSource>();
            mComSoundList.loop = false;
        }
        soundlist_funcs = callback_funcs;
        soundlist_ab_names = null;
        soundlist_sound_names = sound_names;
        //soundlist_sound_names = Common.CopyList<string>(sound_names);

        StopCoroutine("TSoundList");
        StartCoroutine("TSoundList");

    }
    public void StopSoundList()
    {
        StopCoroutine("TSoundList");

        if (null != soundlist_funcs)
        {
            soundlist_funcs.Clear();
            soundlist_funcs = null;
        }
        if(null != soundlist_sound_names)
        {
            soundlist_sound_names.Clear();
            soundlist_sound_names = null;
        }

    }
    IEnumerator TSoundList()
    {
        int callback_count = 0;
        if(null == soundlist_ab_names)
        {
            for (int i = 0; i < soundlist_sound_names.Count; i++)
            {
                AudioClip c = ResManager.GetClipInResources(soundlist_sound_names[i]);
                mComSoundList.clip = c;
                mComSoundList.Play();
                if (null != c)
                    yield return new WaitForSeconds(c.length);
                if (null != soundlist_funcs && callback_count < soundlist_funcs.Count && null != soundlist_funcs[callback_count])
                    soundlist_funcs[callback_count]();
                callback_count++;
            }

        }
        else
        {
            for (int i = 0; i < soundlist_sound_names.Count; i++)
            {
                string ab_name = soundlist_ab_names[i < soundlist_ab_names.Count ? i : soundlist_ab_names.Count - 1];
                AudioClip c = ResManager.GetClip(ab_name, soundlist_sound_names[i]);
                mComSoundList.clip = c;
                mComSoundList.Play();
                if(null != c)
                    yield return new WaitForSeconds(c.length);
                if (null != soundlist_funcs && callback_count < soundlist_funcs.Count && null != soundlist_funcs[callback_count])
                    soundlist_funcs[callback_count]();
                callback_count++;
            }

        }

        if (null != soundlist_funcs && callback_count < soundlist_funcs.Count && null != soundlist_funcs[callback_count])
            soundlist_funcs[callback_count]();
        callback_count++;

        if(null != soundlist_funcs)
        {
            soundlist_funcs.Clear();
            soundlist_funcs = null;
        }
        if(null != soundlist_sound_names)
        {
            soundlist_sound_names.Clear();
            soundlist_sound_names = null;
        }

    }


    //播放语音专用组件
    public bool is_playing_tip = false;
    AudioSource mComSoundTip { get; set; }
    List<string> tip_ab_names = new List<string>();
    List<string> tip_sound_names = new List<string>();
    List<float> tip_sound_volume = new List<float>();
    IEnumerator TPlayTipList()
    {
        is_playing_tip = true;
        for (int i = 0; i < tip_sound_names.Count; i++)
        {
            string ab_name = tip_ab_names[i];
            AudioClip c = ResManager.GetClip(ab_name, tip_sound_names[i]);
            mComSoundTip.clip = c;
            if (null != tip_sound_volume)
                mComSoundTip.volume = tip_sound_volume[i];
            else
                mComSoundTip.volume = 1;

            mComSoundTip.Play();
            if (null != c)
                yield return new WaitForSeconds(c.length);
        }
        is_playing_tip = false;
    }
    public void PlayTip(string ab_name, string sound_name, float volume = 1, bool can_top_title_btn_play = false)//播放提示语音专用，会记录在TopTitle中
    {
        //Debug.LogError("--" + sound_name);
        if(null == mComSoundTip)
        {
            mComSoundTip = gameObject.AddComponent<AudioSource>();
        }

        if (can_top_title_btn_play)
            TopTitleCtl.instance.mSoundTipData.SetData(this, ab_name, sound_name);

        mComSoundTip.clip = ResManager.GetClip(ab_name, sound_name);
        mComSoundTip.volume = volume;
        mComSoundTip.Play();

    }
    public void PlayTipList(List<string> ab_names, List<string> sound_names, bool can_top_title_btn_play = false)
    {
        //Debug.LogError("no tip_sound_names");
        if (null == mComSoundTip)
        {
            mComSoundTip = gameObject.AddComponent<AudioSource>();
        }

        if (can_top_title_btn_play)
        {
            TopTitleCtl.instance.mSoundTipData.SetData(this, ab_names, sound_names);
        }

        tip_ab_names = ab_names;
        tip_sound_names = sound_names;
        tip_sound_volume = null;

        StopCoroutine("TPlayTipList");
        StartCoroutine("TPlayTipList");

    }
    public void PlayTipList(List<string> ab_names, List<string> sound_names, List<float> volumes, bool can_top_title_btn_play = false)
    {
        //Debug.LogError("tip_sound_names");
        if (null == mComSoundTip)
        {
            mComSoundTip = gameObject.AddComponent<AudioSource>();
        }

        if (can_top_title_btn_play)
        {
            TopTitleCtl.instance.mSoundTipData.SetData(this, ab_names, sound_names);
        }

        tip_ab_names = ab_names;
        tip_sound_names = sound_names;
        if(null != volumes)
        {
            tip_sound_volume = volumes;
        }

        StopCoroutine("TPlayTipList");
        StartCoroutine("TPlayTipList");

    }
    public void PlayTipDefaultAb( string sound_name, float volume = 1, bool can_top_title_btn_play = false)//播放提示语音专用，会记录在TopTitle中
    {
        //Debug.LogError("--" + sound_name);
        if (null == mComSoundTip)
        {
            mComSoundTip = gameObject.AddComponent<AudioSource>();
        }

        if (can_top_title_btn_play)
            TopTitleCtl.instance.mSoundTipData.SetData(this, mAssetbundleName, sound_name);

        StopTip();
        mComSoundTip.clip = ResManager.GetClip(mAssetbundleName, sound_name);
        mComSoundTip.volume = volume;
        mComSoundTip.Play();

    }
    public void PlayTipListDefaultAb(List<string> sound_names, List<float> volumes, bool can_top_title_btn_play = false)
    {
        List<string> ab_names = new List<string>();
        for(int i = 0; i < sound_names.Count; i++)
        {
            //ab_names.Add(sound_names[i]);
            ab_names.Add(mAssetbundleName);
        }
        PlayTipList(ab_names, sound_names, volumes, can_top_title_btn_play);
    }
    public void StopTip()
    {
        StopCoroutine("TPlayTipList");
        if(null != mComSoundTip)
            mComSoundTip.Stop();
    }

}
