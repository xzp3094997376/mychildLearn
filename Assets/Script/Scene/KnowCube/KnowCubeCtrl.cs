using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KnowCubeCtrl : BaseScene
{

    public int nLevel = 1;
    public const int nLevels = 3;

    private RawImage sceneBG;
    [HideInInspector]
    public KnowCubeLv1 mLevel1Ctrl;
    private KnowCubeLv2 mLevel2Ctrl;
    private KnowCubeLv3 mLevel3Ctrl;

    public bool bLvPass = false;

    void Awake()
    {
        mSceneType = SceneEnum.KnowCube;
        CallLoadFinishEvent();

        sceneBG = transform.Find("bg").GetComponent<RawImage>();
        sceneBG.texture = ResManager.GetTexture("knowcube_texture", "knowcubebg");
        sceneBG.rectTransform.sizeDelta = new Vector2(1280f, 800f);
    }
    // Use this for initialization
    void Start ()
    {
        mLevel1Ctrl = transform.Find("panel1").gameObject.AddComponent<KnowCubeLv1>();
        mLevel2Ctrl = transform.Find("panel2").gameObject.AddComponent<KnowCubeLv2>();
        mLevel3Ctrl = transform.Find("panel3").gameObject.AddComponent<KnowCubeLv3>();
        mLevel1Ctrl.gameObject.SetActive(false);
        mLevel2Ctrl.gameObject.SetActive(false);
        mLevel3Ctrl.gameObject.SetActive(false);

        StartCoroutine(ieMMStart());
    }
    IEnumerator ieMMStart()
    {
        yield return new WaitForSeconds(0.1f);
        PlayBGSound();

        mLevel1Ctrl.gameObject.SetActive(true);
        mLevel2Ctrl.gameObject.SetActive(true);
        mLevel3Ctrl.gameObject.SetActive(true);
        mLevel1Ctrl.InitAwake();
        mLevel2Ctrl.InitAwake();
        mLevel3Ctrl.InitAwake();

        TopTitleCtl.instance.Reset();
        TopTitleCtl.instance.mSoundTipData.SetData(PlayTipSound);
        nLevel = 1;
        InitLevelData();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InitLevelData();
        }
    }

    public void ResetInfos()
    {
        mLevel1Ctrl.gameObject.SetActive(false);
        mLevel2Ctrl.gameObject.SetActive(false);
        mLevel3Ctrl.gameObject.SetActive(false);
        bLvPass = false;
    }

    public void InitLevelData()
    {
        ResetInfos();
        if (nLevel == 1)
        {
            mLevel1Ctrl.gameObject.SetActive(true);
            mLevel1Ctrl.SetData();
        }
        else if (nLevel == 2)
        {
            mLevel2Ctrl.gameObject.SetActive(true);
            mLevel2Ctrl.SetData();
        }
        else
        {
            mLevel3Ctrl.gameObject.SetActive(true);
            mLevel3Ctrl.SetData();
        }
    }


    public void LevelPass()
    {        
        StartCoroutine(IEToNextLevel());
    }
    IEnumerator IEToNextLevel()
    {
        TopTitleCtl.instance.AddStar();
        nLevel++;
        if (nLevel > nLevels)
        {
            //结算
            //Debug.LogError("Game Run Over!");
            GameOverCtl.GetInstance().Show(3, () =>
            {
                nLevel = 1;
                TopTitleCtl.instance.Reset();
                InitLevelData();
            });
        }
        else
        {
            yield return new WaitForSeconds(1f);
            SceneMoveOut();
            yield return new WaitForSeconds(1.1f);
            InitLevelData();
        }
    }

    void SceneMoveOut()
    {
        if (mLevel1Ctrl.gameObject.activeSelf)
            mLevel1Ctrl.SceneMove(false);
        if (mLevel2Ctrl.gameObject.activeSelf)
            mLevel2Ctrl.SceneMove(false);
        if (mLevel3Ctrl.gameObject.activeSelf)
            mLevel3Ctrl.SceneMove(false);
    }


    #region//sound
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    public void PlayBGSound()
    {
        SoundManager.instance.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.1f);

        mKimiAudioSource = gameObject.AddComponent<AudioSource>();
        mKimiAudioSource.loop = false;
    }
    public AudioSource mKimiAudioSource { set; get; }
    public void PlaySound(AudioSource _adSource, AudioClip _clip, float _volume)
    {
        _adSource.volume = _volume;
        _adSource.Stop();
        _adSource.clip = _clip;
        _adSource.Play();
    }
    public AudioClip GetClip(string _clipName)
    {
        AudioClip clip1 = ResManager.GetClip("knowcube_sound", _clipName);
        return clip1;
    }
    public AudioClip GetNumClip(int _num)
    {
        AudioClip clip1 = ResManager.GetClip("number_sound", _num.ToString());
        return clip1;
    }
    public void PlayTheSortSound(string _clipname)
    {
        AudioClip aclip = GetClip(_clipname);
        AudioSource.PlayClipAtPoint(aclip, Camera.main.transform.position,0.3f);
    }
    public void PlayTheSortSound(AudioClip aclip)
    {
        AudioSource.PlayClipAtPoint(aclip, Camera.main.transform.position);
    }
    #endregion


    IEnumerator ieTipSound = null;
    public void PlayTipSound()
    {
        if (bLvPass)
            return;
        if (ieTipSound != null)
            StopCoroutine(ieTipSound);

        if (nLevel == 1)
        {
            SetTipSound(mLevel1Ctrl.IEPlayTipSound1());
        }
        else if (nLevel == 2)
        {
            SetTipSound(mLevel2Ctrl.IEPlayTipSound2());
        }
        else
        { SetTipSound(mLevel3Ctrl.IEPlayTipSound3()); }

        if (ieTipSound != null)
            StartCoroutine(ieTipSound);
    }
    public void SetTipSound(IEnumerator _ieTipSound)
    {
        ieTipSound = _ieTipSound;
    }

}
