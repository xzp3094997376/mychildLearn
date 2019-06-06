using UnityEngine;
using System.Collections;

/// <summary>
/// 播放声音控制器
/// </summary>
public class PlaySoundController : MonoBehaviour
{
    private AssetBundle bgSoundAB = null;
    public string strClipName = "";
    public float fVlue = 0.1f;
    /// <summary>
    /// 一般y用来播放提示音
    /// </summary>
    public AudioSource mKimiAudioSource { set; get; }
    

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    public void PlayBGSound(string _alats, string _clipName, float _vlue = 0.1f)
    {
        InitAwake();

        strClipName = _clipName;
        fVlue = _vlue;
        PlayBGSoundAsync(_alats, _clipName);
        //SoundManager.instance.PlayBg(_alats, _clipName, fVlue);
    }

    public void PlayBGSound1(string _alats, string _clipName, float _vlue = 0.1f)
    {
        strClipName = _clipName;
        fVlue = _vlue;
        PlayBGSoundAsync(_alats, _clipName);
    }
    bool bplayed = false;

    public void InitAwake()
    {
        mKimiAudioSource = gameObject.AddComponent<AudioSource>();
        mKimiAudioSource.loop = false;
    }

    //先load AB文件
    private void PlayBGSoundAsync(string _alats, string _clipName)
    {
        StartCoroutine(iePlayBGSoundAsync(_alats, _clipName));
    }
    IEnumerator iePlayBGSoundAsync(string _alats, string _clipName)
    {
        if (ApkInfo.g_load_res_type == LoadResourcesEnum.ResCopy)
        {
            string strResPath = "ResCopy/sound/" + _clipName + "/" + _clipName;
            ResourceRequest resReq = Resources.LoadAsync(strResPath);
            yield return resReq;
            AudioClip clip = resReq.asset as AudioClip;
            if (clip != null)
            {
                SoundManager.instance.PlayBg(clip, fVlue);
            }
        }
        else if (ApkInfo.g_load_res_type == LoadResourcesEnum.Ab)
        {
            string path = PathTool.gStreamingAssets + AbEnum.sound.ToString() + "/" + _alats;
            AssetBundleCreateRequest reque = AssetBundle.LoadFromFileAsync(path);
            yield return reque;
            bgSoundAB = reque.assetBundle;
            if (bgSoundAB != null)
            {
                MPlayBGSoundAsync(_clipName);
            }
        }
    }
    //再load clicp文件
    private void MPlayBGSoundAsync(string _clipName)
    {
        StartCoroutine(ieMPlayBGSoundAsync(_clipName));
    }
    IEnumerator ieMPlayBGSoundAsync(string _clipName)
    {
        //5.3.6版本 LoadAssetAsync 只要load过程中有第二个协程就会卡!!!
        yield return new WaitForSeconds(fDelayLoadClip);
        //Debug.LogError("look look");
        AssetBundleRequest request = bgSoundAB.LoadAssetAsync(_clipName);
        yield return request;
        AudioClip clip = request.asset as AudioClip;
        if (clip != null)
        {
            SoundManager.instance.PlayBg(clip, fVlue);
        }
    }

    float fDelayLoadClip = 1f;
    public void SetDelayLoadBGClip(float _time)
    {
        fDelayLoadClip = _time;
    }

    /// <summary>
    /// 播放声音片段
    /// </summary>
    /// <param name="_adSource"></param>
    /// <param name="_clip"></param>
    /// <param name="_volume"></param>
    public void PlaySound(AudioSource _adSource, AudioClip _clip, float _volume)
    {
        _adSource.volume = _volume;
        _adSource.Stop();
        _adSource.clip = _clip;
        _adSource.Play();
    }
    /// <summary>
    /// 播放kimi提示语音
    /// </summary>
    /// <param name="_clip"></param>
    /// <param name="_volume"></param>
    public void PlaySound(AudioClip _clip, float _volume)
    {
        mKimiAudioSource.volume = _volume;
        PlaySound(mKimiAudioSource, _clip, _volume);
    }

    /// <summary>
    /// 取得声音片段
    /// </summary>
    /// <param name="_clipName"></param>
    /// <returns></returns>
    public AudioClip GetClip(string _alats, string _clipName)
    {
        AudioClip clip1 = ResManager.GetClip(_alats, _clipName);
        return clip1;
    }
    /// <summary>
    /// 取得数字声音片段 1-100
    /// </summary>
    /// <param name="_num"></param>
    /// <returns></returns>
    public AudioClip GetNumClip(int _num)
    {
        AudioClip clip1 = GetClip("number_sound", _num.ToString());
        return clip1;
    }

    /// <summary>
    /// 播放短暂的声音
    /// </summary>
    /// <param name="_clipname"></param>
    public void PlaySortSound(string _alats, string _clipname)
    {
        AudioClip aclip = GetClip(_alats, _clipname);
        PlaySortSound(aclip);
    }
    public void PlaySortSound(string _alats, string _clipname, float _volume)
    {
        AudioClip aclip = GetClip(_alats, _clipname);
        PlaySortSound(aclip, _volume);
    }
    /// <summary>
    /// 播放短暂的声音
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySortSound(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }
    public void PlaySortSound(AudioClip clip,float _volume)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, _volume);
    }

    /// <summary>
    /// 停止语音
    /// </summary>
    public void StopSound()
    {
        mKimiAudioSource.Stop();
    }


    IEnumerator ieTipSound = null;
    /// <summary>
    /// 设置播放语音协程
    /// </summary>
    /// <param name="_ieTipSound"></param>
    public void SetTipSound(IEnumerator _ieTipSound)
    {
        ieTipSound = _ieTipSound;
    }
    /// <summary>
    /// 停止播放语音
    /// </summary>
    public void StopTipSound()
    {
        StopSound();
        if (ieTipSound != null)
            StopCoroutine(ieTipSound);
    }
    /// <summary>
    /// 开启协程播放语音
    /// </summary>
    public void StartTipSound()
    {
        if (ieTipSound != null)
            StartCoroutine(ieTipSound);
    }

    /// <summary>
    /// 直接播放语音
    /// </summary>
    /// <param name="_ieTipSound"></param>
    public void PlayTipSound(IEnumerator _ieTipSound)
    {
        StopTipSound();
        SetTipSound(_ieTipSound);
        StartTipSound();
    }
}
