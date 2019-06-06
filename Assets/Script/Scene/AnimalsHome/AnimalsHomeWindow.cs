using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class AnimalsHomeWindow : MonoBehaviour
{
    /// <summary>
    /// 坐标 层
    /// </summary>
    public int nPosY = 0;
    /// <summary>
    /// 坐标 号
    /// </summary>
    public int nPosX = 0;
    /// <summary>
    /// 列表索引编号
    /// </summary>
    public int nIndex = 0;

    /// <summary>
    /// 0关着 1半开 2开着
    /// </summary>
    public int nWinOpenState = 0;

    public bool bPassOK = false;

    #region
    private Image bg1;
    private Image bg0;

    private Transform door;
    private AnimalsHomeDoor door0;
    private AnimalsHomeDoor door1;
    private Image door2;
    private Image door3;
    private Image door4;
    private Image door5;

    private GameObject mBirdPos;
    private GameObject mBirdStartFlyPos;
    public GameObject BirdPos { get { return mBirdPos; } }   
    public GameObject BirdStartFlyPos { get { return mBirdStartFlyPos; } }

    bool bInit = false;
    private AnimalsHomeCtrl mCtrl;
    #endregion

    private AnimalsHomeBird mBird;
    public AnimalsHomeBird Bird { set { mBird = value; } get { return mBird; } }

    //init
    public void InitAwake(int _x,int _y)
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as AnimalsHomeCtrl;
        nPosX = _x;
        nPosY = _y;

        #region//doors
        bg0 = transform.Find("bg0").GetComponent<Image>();
        bg1 = transform.Find("bg1").GetComponent<Image>();

        door = transform.Find("door");
        door0 = door.Find("door0").gameObject.AddComponent<AnimalsHomeDoor>();
        door1 = door.Find("door1").gameObject.AddComponent<AnimalsHomeDoor>();
        door0.InitAwake(nPosY, true, this);
        door1.InitAwake(nPosX, false, this);

        door2 = door.Find("door2").GetComponent<Image>();
        door3 = door.Find("door3").GetComponent<Image>();
        door4 = door.Find("door4").GetComponent<Image>();
        door5 = door.Find("door5").GetComponent<Image>();

        bg0.sprite = ResManager.GetSprite("animalshome_sprite", "window0");
        bg1.sprite = ResManager.GetSprite("animalshome_sprite", "window1");
        door2.sprite = ResManager.GetSprite("animalshome_sprite", "winyellow1");
        door3.sprite = ResManager.GetSprite("animalshome_sprite", "winred1");
        door4.enabled = false;
        door5.enabled = false;

        ResetDoor(0);
        #endregion

        mBirdPos = UguiMaker.newGameObject("birdPos", transform);
        mBirdPos.transform.localPosition = new Vector3(35f, 51f);
        mBirdStartFlyPos = UguiMaker.newGameObject("birdStartFlyPos", transform);
        mBirdStartFlyPos.transform.localPosition = new Vector3(0f, -35f);

        bInit = true;
    }

    /// <summary>
    /// 获取门 0左 1右
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public AnimalsHomeDoor GetDoor(int _id)
    {
        if (_id == 0)
        { return door0; }
        else if (_id == 1)
        { return door1; }
        else
            return null;
    }

    #region//open/close window
    private IEnumerator theIEWindow = null;
    public void OpenWindow()
    {
        if (nWinOpenState == 2)
            return;
        if (theIEWindow != null)
            StopCoroutine(theIEWindow);

        theIEWindow = ieOpenWindow();
        StartCoroutine(theIEWindow);
    }
    IEnumerator ieOpenWindow()
    {
        AudioClip cp = ResManager.GetClip("animalshome_sound", "window_open");
        mCtrl.MSoundCtrl.PlaySortSound(cp);
        ResetDoor(0);
        yield return new WaitForSeconds(0.1f);
        ResetDoor(1);
        yield return new WaitForSeconds(0.1f);
        ResetDoor(2);
    }
    public void CloseWindow()
    {
        if (nWinOpenState == 0)
            return;
        if (theIEWindow != null)
            StopCoroutine(theIEWindow);

        theIEWindow = ieCloseWindow();
        StartCoroutine(theIEWindow);
    }
    IEnumerator ieCloseWindow()
    {
        AudioClip cp = ResManager.GetClip("animalshome_sound", "window_close");
        mCtrl.MSoundCtrl.PlaySortSound(cp);
        ResetDoor(2);
        yield return new WaitForSeconds(0.1f);
        ResetDoor(1);
        yield return new WaitForSeconds(0.1f);
        ResetDoor(0);
    }
    #endregion

    #region//动物头像
    public int nAnimalID = 1;
    private GameObject mAnimalHead;
    /// <summary>
    /// 创建动物头像
    /// </summary>
    /// <param name="_id"></param>
    public void CreateAnimalHead(int _id)
    {
        nAnimalID = _id;
        if (mAnimalHead == null)
        {
            string strname = MDefine.GetAnimalHeadResNameByID(_id);
            GameObject mgo = UguiMaker.newGameObject("animalhead", transform);
            GameObject spine = ResManager.GetPrefab("animalhead_prefab", strname);
            spine.transform.SetParent(mgo.transform);
            spine.transform.localPosition = new Vector3(0f, -30f, 0f);
            spine.transform.localScale = Vector3.one * 0.25f;
            mAnimalHead = mgo;
            mAnimalHead.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 显示动物头像
    /// </summary>
    public void ShowAnimalHead(bool _show)
    {
        if (mAnimalHead != null)
        {
            if (_show)
            {
                mAnimalHead.gameObject.SetActive(true);
                mAnimalHead.transform.localScale = Vector3.one * 0.001f;
                mAnimalHead.transform.DOScale(Vector3.one, 0.2f);
            }
            else
            {
                mAnimalHead.transform.DOScale(Vector3.one * 0.001f, 0.2f).OnComplete(() =>
                { mAnimalHead.gameObject.SetActive(false); });
            }
        }
    }
    #endregion

    /// <summary>
    /// 开门状态 0关着 1半开 2全开
    /// </summary>
    /// <param name="_index"></param>
    public void ResetDoor(int _index)
    {
        nWinOpenState = _index;
        door0.gameObject.SetActive(false);
        door1.gameObject.SetActive(false);
        door2.enabled = false;
        door3.enabled = false;
        if (_index == 0)
        {
            door0.gameObject.SetActive(true);
            door1.gameObject.SetActive(true);
            door0.ResetStartPos();
            door1.ResetStartPos();
        }
        else if (_index == 1)
        {
            door2.enabled = true;
            door3.enabled = true;
        }
        else if (_index == 2)
        {
            door0.gameObject.SetActive(true);
            door1.gameObject.SetActive(true);
            door0.transform.localPosition = door4.transform.localPosition;
            door1.transform.localPosition = door5.transform.localPosition;
        }
    }
    /// <summary>
    /// 数据清除
    /// </summary>
    public void ResetInfos()
    {
        if (mAnimalHead != null)
        {
            GameObject.Destroy(mAnimalHead);
            mAnimalHead = null;
        }
        ResetDoor(0);
        mBird = null;
        bPassOK = false;
        door0.ResetInfos();
        door1.ResetInfos();
    }
    /// <summary>
    /// 预设置好点数
    /// </summary>
    public void PreSetPoint()
    {
        door0.PreSetPoint(nPosY);
        door1.PreSetPoint(nPosX);
    }
    /// <summary>
    /// 门按钮 开/关
    /// </summary>
    /// <param name="_active"></param>
    public void DoorBtnActive(bool _active)
    {
        door0.DoorBtnActive(_active);
        door1.DoorBtnActive(_active);
    }


    /// <summary>
    /// 检测window是否OK
    /// </summary>
    public void CheckWindowIsOK()
    {
        if (door0.bOK && door1.bOK)
        {
            if (bPassOK)
                return;
            bPassOK = true;
            mCtrl.RemoveFinishWindowIndex(nIndex);
            mCtrl.nCount++;
            if (mCtrl.nCount >= 3)
                mCtrl.mClock.StopTimeRun();
            StartCoroutine(ieCheckToNext());
        }
    }
    IEnumerator ieCheckToNext()
    {
        yield return new WaitForSeconds(1f);
        OpenWindow();
        ShowAnimalHead(true);
        door0.CheckBtnShow(false);
        door1.CheckBtnShow(false);
        yield return new WaitForSeconds(0.6f);
        //动物叫声音效
        string strCp = MDefine.GetAnimalNameByID_CH(nAnimalID) + "0";
        AudioClip cp = ResManager.GetClip("aa_animal_sound", strCp);
        if (cp != null)
            mCtrl.MSoundCtrl.PlaySortSound(cp);
        yield return new WaitForSeconds(0.5f);
        //住在几层几号语音
        mCtrl.PlayWindowFinishSound(this);

        if (mBird != null)
        {
            int ndir = 1;
            if (Random.value > 0.5f)
            { ndir = -1; }
            Vector3 vposout = mBird.transform.localPosition + new Vector3(400f * ndir, 700f, 0f);
            if (ndir > 0)
            { mBird.transform.localScale = new Vector3(-0.8f, 0.8f, 0.8f); }
            else
            { mBird.transform.localScale = Vector3.one * 0.8f; }
            mBird.MoveToPosLocal(vposout, 2f);
        }
        mCtrl.LevelCheckNext();
    }

}
