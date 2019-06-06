using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class AnimalStatistics_panel4 : MonoBehaviour
{
    public int nChooseType = 0;
    public List<AnimalStatistics_dataObj4> mDataObj = new List<AnimalStatistics_dataObj4>();

    private Image mpanel;
    private Image mflower;
    private Text mtextTip;
    public Image checkButton;
    private bool bfirstOK = false;

    private Vector3 vStart;
    private Vector3 vBase;

    AnimalStatisticsCtrl mctrl;

    public void InitAwake()
    {
        gameObject.SetActive(true);
        mctrl = SceneMgr.Instance.GetNowScene() as AnimalStatisticsCtrl;

        vBase = transform.localPosition;
        transform.localPosition = vBase + new Vector3(1500f, 0f, 0f);

        mpanel = transform.GetComponent<Image>();
        mpanel.enabled = false;
        //mpanel.sprite = ResManager.GetSprite("animalstatistics_sprite", "mpanelbg");

        mflower = transform.Find("tipFlower").GetComponent<Image>();
        mtextTip = mflower.transform.Find("tipText").GetComponent<Text>();
        mflower.transform.localScale = Vector3.one;

        checkButton = transform.Find("checkButton").GetComponent<Image>();
        checkButton.sprite = ResManager.GetSprite("animalstatistics_sprite", "checkbtnup");
        checkButton.SetNativeSize();
        EventTriggerListener.Get(checkButton.gameObject).onUp = CheckBtnUp;
        EventTriggerListener.Get(checkButton.gameObject).onDown = CheckBtnDown;
    }


    public void ResetInfos()
    {
        for (int i = 0; i < mDataObj.Count; i++)
        {
            if (mDataObj[i].gameObject != null)
                GameObject.Destroy(mDataObj[i].gameObject);
        }
        mDataObj.Clear();
        checkButton.gameObject.SetActive(false);
        bfirstOK = false;
        checkButton.sprite = ResManager.GetSprite("animalstatistics_sprite", "checkbtnup");

        bOpenQuea2 = false;
        nQuea2Count = 0;
        isSingleCheck = true;
        mmCount = 0;
        mmToCount = 0;
        bqueFinish = false;
    }


    public void SetData(List<int> _typeList, List<int> _numList)
    {
        ResetInfos();

        nChooseType = Random.Range(3, 5);
        
        mflower.sprite = ResManager.GetSprite("animalstatistics_sprite", "mflower" + nChooseType);
        mtextTip.text = "3";

        List<float> indexXList = Common.GetOrderList(5, 180f);
        for (int i = 0; i < 5; i++)
        {
            GameObject gogo = mctrl.CreateResObj("dataobj4", transform);
            gogo.name = "dataobj" + i;
            gogo.transform.localPosition = new Vector3(indexXList[i], 0f, 0f);
            AnimalStatistics_dataObj4 data4 = gogo.AddComponent<AnimalStatistics_dataObj4>();
            data4.InitAwake();
            data4.SetData(_typeList[i], _numList[i]);
            mDataObj.Add(data4);         
        }

        mctrl.StartCoroutine(IEMove(true));
    }
    IEnumerator IEMove(bool _in)
    {
        for (int i = 0; i < 5; i++)
        {
            mDataObj[i].SceneMove(_in);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void CheckBtnDown(GameObject _go)
    {
        if (mctrl.bLevelPass)
            return;
        if (bfirstOK)
            return;
        checkButton.sprite = ResManager.GetSprite("animalstatistics_sprite", "checkbtndown");
        AudioClip cp = ResManager.GetClip("checkgamebtn_sound", "checkgamebtn_down");
        if (cp != null)
            AudioSource.PlayClipAtPoint(cp, Camera.main.transform.position);
    }
    private void CheckBtnUp(GameObject _go)
    {
        if (mctrl.bLevelPass)
            return;
        if (bfirstOK)
            return;
        int count = 0;
        for (int i = 0; i < 5; i++)
        {
            if (mDataObj[i].CheckNumIsOK())
            {
                count++;
            }
            else
            {
                mDataObj[i].SetRedColor();
            }
        }
        if (count >= 5)
        {
            mctrl.nCount = 4;
            bfirstOK = true;
            for (int i = 0; i < mDataObj.Count; i++)
            {
                mDataObj[i].CloseFlowerClickAndHeadClick();
            }
            mctrl.StartCoroutine(iePlayCheckSucEffect4());
        }
        else
        {
            checkButton.sprite = ResManager.GetSprite("animalstatistics_sprite", "checkbtnup");
            AudioClip cp = ResManager.GetClip("checkgamebtn_sound", "checkgamebtn_up");
            if (cp != null)
                AudioSource.PlayClipAtPoint(cp, Camera.main.transform.position);
            mctrl.PlaySortSound("sound_checkfail");
        }      
    }

    IEnumerator iePlayCheckSucEffect4()
    {
        mctrl.PlayFlowerOK_fx(checkButton.transform.position);
        mctrl.PlaySortSound("sound_checksuc");
        yield return new WaitForSeconds(3.3f);
        checkButton.gameObject.SetActive(false);
        bOpenQuea2 = true;
        mctrl.MLevelPass();
    }


    /// <summary>
    /// 按钮显示
    /// </summary>
    public void CheckBtnShow()
    {
        if (nQuea2Count > 0)
            return;

        bool bshow = true;
        for (int i = 0; i < mDataObj.Count; i++)
        {
            if (mDataObj[i].nCountClick <= 0)
            {
                bshow = false;
                break;
            }
        }
        if (bshow)
        {
            if (!checkButton.gameObject.activeSelf)
            {
                checkButton.gameObject.SetActive(true);
                checkButton.transform.localScale = Vector3.one * 0.001f;
                checkButton.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);           
            }
        }
        else
        { checkButton.gameObject.SetActive(false); }
    }

    public void PanelMove(bool _in)
    {
        if (_in)
        {
            transform.localPosition = vStart + new Vector3(1500f, 0f, 0f);
            transform.DOLocalMove(vStart, 1f);
        }
        else
        {
            transform.DOLocalMove(vStart + new Vector3(0f, -800f, 0f), 1f);
        }
    }


    #region//奇数偶数提问-----
    bool bOpenQuea2 = false;
    public int nQuea2Count = 0;//提问完成累计
    bool isSingleCheck = true;//奇数or偶数
    public int mmToCount = 0;//要点击完成的数量
    public int mmCount = 0;//点击完成的数量统计
    /// <summary>
    /// 设置第4关最后提问
    /// </summary>
    public void SetQuea2()
    {
        isSingleCheck = true;
        if (UnityEngine.Random.value > 0.5f)
        { isSingleCheck = false; }

        for (int i = 0; i < mDataObj.Count; i++)
        {
            mDataObj[i].SetQuea2ClickCall(ClickCheckCall);
            mDataObj[i].ShowQuea2Btn(true);
            if (isSingleCheck)
            {
                if (mDataObj[i].nNum % 2 == 1)
                {
                    mmToCount++;
                }
            }
            else
            {
                if (mDataObj[i].nNum % 2 == 0)
                {
                    mmToCount++;
                }
            }
        }

        mctrl.PlayTitleSound();
    }
    bool bqueFinish = false;
    private void ClickCheckCall(AnimalStatistics_dataObj4 _obj)
    {
        if (mctrl.bLevelPass)
            return;
        if (bqueFinish)
            return;

        _obj.SetAnimalHeadBigEffect();

        bool bClickOK = false;
        if (isSingleCheck)
        {
            if (_obj.nNum % 2 == 1)
            { bClickOK = true; }
        }
        else
        {
            if (_obj.nNum % 2 == 0)
            { bClickOK = true; }
        }

        if (bClickOK)
        {
            mmCount++;
            _obj.ShowQuea2Btn(false);
            _obj.SetOKColor();
            mctrl.PlaySortSound("choose_ok");

            if (mmCount >= mmToCount)
            {
                nQuea2Count++;
                for (int i = 0; i < mDataObj.Count; i++)
                {
                    mDataObj[i].SetClickQuea2Active(false);
                }
                mctrl.bLevelPass = true;
                bqueFinish = true;
                mctrl.StartCoroutine(ieToCheckZuiShao());
            }
        }
        else
        {
            _obj.SetRedColor();
            mctrl.PlaySortSound("choose_error");
        }
    }
    IEnumerator ieToCheckZuiShao()
    {
        //ok sound
        mctrl.SoundCtrl.StopTipSound();
        string strcp0 = "sound_guli1";
        if (Random.value > 0.5f)
        { strcp0 = "sound_guli3"; }
        AudioClip cp0 = mctrl.GetClip(strcp0);
        mctrl.SoundCtrl.PlaySound(cp0, 1f);
        yield return new WaitForSeconds(cp0.length);
        mctrl.MLevelPass();
    }
    #endregion

    public IEnumerator ieTipSoundLv4()
    {
        List<AudioClip> cpList = new List<AudioClip>();
        if (!bOpenQuea2)
        {
            cpList.Add(mctrl.GetClip("z_additional_huabiaoshi" + mctrl.nBeiShu));
            cpList.Add(mctrl.GetClip("sound_gametip4"));
        }
        else
        {
            cpList.Add(mctrl.GetClip("z哪个动物的数字是"));
            if (isSingleCheck)
            {
                cpList.Add(mctrl.GetClip("z奇数"));
            }
            else
            {
                cpList.Add(mctrl.GetClip("z偶数"));
            }
        }
        for (int i = 0; i < cpList.Count; i++)
        {
            mctrl.SoundCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }
    }

}
