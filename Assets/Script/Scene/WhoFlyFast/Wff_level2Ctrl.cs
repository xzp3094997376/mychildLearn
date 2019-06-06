using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Wff_level2Ctrl : MonoBehaviour
{

    private WhoFlyFastCtrl mCtrl;
    AudioClip button_down;
    AudioClip button_up;
    bool bOpenClick = false;

    private const int nToCount = 3;
    public int nCount = 0;

    private List<int> mQueIDList = new List<int>();
    private List<float> mMoveOutTimeList = new List<float>();

    public ParticleSystem clickFx;

    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as WhoFlyFastCtrl;

        button_down = Resources.Load<AudioClip>("sound/button_down");
        button_up = Resources.Load<AudioClip>("sound/button_up");

        float findex = 3f;
        for (int i = 0; i < 7; i++)
        {
            float getT = findex + 0.5f * i;
            mMoveOutTimeList.Add(getT);
        }
        mMoveOutTimeList = Common.BreakRank(mMoveOutTimeList);

        ParticleSystem loadFX = Resources.Load<ParticleSystem>("prefab/clickFX/clickFX");
        clickFx = GameObject.Instantiate(loadFX) as ParticleSystem;
        clickFx.transform.SetParent(transform);
        clickFx.transform.localPosition = mCtrl.mBtnClick.transform.localPosition;
        clickFx.transform.localScale = Vector3.one;
        clickFx.gameObject.SetActive(false);
        clickFx.loop = true;
        clickFx.transform.GetChild(0).GetComponent<ParticleSystem>().loop = true;
    }

    public void ResetInfos()
    {
        bOpenClick = false;
        nCount = 0;
        mQueIDList.Clear();
        bPlaySucSound = false;
    }

    public void SetData()
    {
        ResetInfos();
        //checkBtn
        EventTriggerListener.Get(mCtrl.mBtnClick.gameObject).onDown = ClickDown;
        EventTriggerListener.Get(mCtrl.mBtnClick.gameObject).onUp = ClickUp;

        mQueIDList = Common.GetIDList(1, 5, 3, -1);

        //显示按钮
        mCtrl.mBtnClick.transform.localScale = Vector3.one * 0.001f;
        mCtrl.mBtnClick.gameObject.SetActive(true);
        mCtrl.mBtnClick.transform.DOScale(Vector3.one, 0.2f).OnComplete(()=> 
        {
            clickFx.gameObject.SetActive(true);
            clickFx.Play();
        });
        mCtrl.PlayTipSound();
    }

    //出现火箭
    private void ShowOutRockets()
    {
        mCtrl.StartCoroutine(ieWaiteSet());
    }
    IEnumerator ieWaiteSet()
    {
        for (int i = 0; i < mCtrl.mSceneRockets.Length; i++)
        {
            mCtrl.mScenePanels[i].SetState(true);
            mCtrl.mSceneRockets[i].SetNumberActive(false);
            mCtrl.mSceneRockets[i].SetNumBox2DActive(false);
            mCtrl.mSceneRockets[i].DoPingpongMove();
            mCtrl.mSceneRockets[i].SetState(true);
        }
        yield return new WaitForSeconds(6f);

        for (int i = 0; i < 5; i++)
        {          
            mCtrl.mSceneRockets[i].SetState(true);
            mCtrl.mScenePanels[i].SetState(false);
            mCtrl.mSceneRockets[i].StopPingpong();
        }
        yield return new WaitForSeconds(0.5f);
        mCtrl.SetDirffientFast();

        yield return new WaitForSeconds(1.5f);
 
        //play tip sound
        mCtrl.PlayTipSound();
        //开启碰撞
        for (int i = 0; i < 5; i++)
        {
            mCtrl.mSceneRockets[i].SetBox2DActive(true);
        }
    }


    private void ClickDown(GameObject _go)
    {
        if (mCtrl.nLevel != 2)
            return;
        if (mCtrl.bLvPass)
            return;
        if (bOpenClick)
            return;
        mCtrl.SoundCtrl.PlaySortSound(button_down);
        mCtrl.mBtnClick.sprite = ResManager.GetSprite("whoflyfast_sprite", "play2");
    }
    private void ClickUp(GameObject _go)
    {
        if (mCtrl.nLevel != 2)
            return;
        if (mCtrl.bLvPass)
            return;
        if (bOpenClick)
            return;
        bOpenClick = true;
        ShowOutRockets();
        mCtrl.mBtnClick.transform.DOScale(Vector3.one * 0.001f, 0.3f).SetDelay(0.5f).OnComplete(() =>
        {
            mCtrl.mBtnClick.gameObject.SetActive(false);
        });
        clickFx.gameObject.SetActive(false);

    }



    #region//Play sound
    public void PlayTipSound()
    {
        mCtrl.SoundCtrl.StopTipSound();
        if (!bOpenClick)
        {
            mCtrl.SoundCtrl.PlaySound(ResManager.GetClip("whoflyfast_sound", "点点开始按钮启动这些火箭吧"), 1f);
        }
        else
        {
            //根据问题列表提问
            if (mQueIDList.Count > 0)
            {
                mCtrl.SoundCtrl.PlayTipSound(iePlaySound());
            }
        }
    }
    IEnumerator iePlaySound()
    {
        int queid = mQueIDList[0];
        string strCp = GetSoundNameByQueID(queid);
        yield return new WaitForSeconds(0.1f);
        List<AudioClip> cpList = new List<AudioClip>();
        cpList.Add(ResManager.GetClip("whoflyfast_sound", strCp));
        cpList.Add(ResManager.GetClip("whoflyfast_sound", "点一点吧"));
        for (int i = 0; i < cpList.Count; i++)
        {
            mCtrl.SoundCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }
    }
    public void PlayWrongSound()
    {
        mCtrl.SoundCtrl.StopTipSound();
        mCtrl.SoundCtrl.PlaySound(ResManager.GetClip("whoflyfast_sound", "不对哦要仔细观察呢"), 1f);
    }

    private string GetSoundNameByQueID(int queID)
    {
        string getname = "";
        switch (queID)
        {
            case 1:
                getname = "排在最后的火箭是哪个";
                break;
            case 2:
                getname = "排在第4的火箭是哪个";
                break;
            case 3:
                getname = "排在第3的火箭是哪个";
                break;
            case 4:
                getname = "排在第2的火箭是哪个"; 
                break;
            case 5:
                getname = "哪个火箭最快";
                break;
            default:
                break;
        }
        return getname;
    }
    #endregion


    public void ClickCall(WFF_RocketObj _clickObj)
    {
        if (bPlaySucSound)
            return;
        if (mQueIDList.Count <= 0)
            return;
        int nowID = mQueIDList[0];

        mCtrl.SetOrder();
        WFF_RocketObj answerRocket = mCtrl.mSceneRockets[nowID - 1];
        if (_clickObj == answerRocket)
        {
            mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("whoflyfast_sound", "setok"));
            _clickObj.SetNumber(6 - nowID, false);
            _clickObj.SetBox2DActive(false);
            _clickObj.PlayMoveStart();
            _clickObj.PlayAnimation("face_sayyes", true);
            mCtrl.StartCoroutine(ieSetNextQue());          
        }
        else
        {
            mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("whoflyfast_sound", "setwrong"));
            _clickObj.ShakeObj();
            PlayWrongSound();
        }
    }
    bool bPlaySucSound = false;
    IEnumerator ieSetNextQue()
    {
        mCtrl.bPlayOtherTip = true;
        bPlaySucSound = true;

        mCtrl.SoundCtrl.StopTipSound();
        string strCp = "哇哦做得很好";
        if (UnityEngine.Random.value > 0.5f)
            strCp = "真是火眼金睛";
        AudioClip cp = ResManager.GetClip("whoflyfast_sound", strCp);
        mCtrl.SoundCtrl.PlaySound(cp, 1f);

        yield return new WaitForSeconds(cp.length);
        mCtrl.bPlayOtherTip = false;
        bPlaySucSound = false;

        //下一个问题
        mQueIDList.RemoveAt(0);
        if (mQueIDList.Count <= 0)
        {
            mCtrl.LevelCheckNext();
        }
        else
        {           
            PlayTipSound();
        }
    }


    WFF_RocketObj mSelect = null;
    void Update()
    {
        if (bPlaySucSound)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            #region//one
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                WFF_RocketObj com = hit.collider.gameObject.GetComponent<WFF_RocketObj>();
                if (com != null)
                {
                    if (mSelect == null)
                    {
                        mSelect = com;
                    }
                }
            }
            #endregion
        }
        else if (Input.GetMouseButtonUp(0))
        {
            #region//two
            if (mSelect != null)
            {
                mSelect.transform.SetSiblingIndex(10);
                ClickCall(mSelect);
                mSelect = null;
            }
            #endregion
        }
    }


}
