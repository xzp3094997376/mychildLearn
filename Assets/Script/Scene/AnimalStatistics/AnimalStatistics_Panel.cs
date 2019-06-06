using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class AnimalStatistics_Panel : MonoBehaviour
{

    private Transform lines;
    private Image[] liness = new Image[11];
    private Text[] texts = new Text[11];

    public AnimalStatistics_DataObj[] mDataObj = new AnimalStatistics_DataObj[5];

    private Image mBlockTip;
    private Image mflower;
    private Image mpanel;
    private Text mtextTip;

    private Vector3 vStart;
    private Vector3 vBase;

    public Image checkButton;

    AnimalStatisticsCtrl mctrl;

    private GameObject mmPos;
    private List<Transform> mPosList = new List<Transform>();

    public void InitAwake()
    {
        mctrl = SceneMgr.Instance.GetNowScene() as AnimalStatisticsCtrl;

        gameObject.SetActive(true);

        vBase = transform.localPosition;
        transform.localPosition = vBase + new Vector3(1500f, 0f, 0f);

        mpanel = transform.GetComponent<Image>();
        mpanel.sprite = ResManager.GetSprite("animalstatistics_sprite", "mpanelbg");

        mBlockTip = transform.Find("mBlockTip").GetComponent<Image>();
        mBlockTip.sprite = ResManager.GetSprite("animalstatistics_sprite", "block_yellow");
        mBlockTip.rectTransform.localScale = Vector3.one * 1.5f;
        mBlockTip.rectTransform.anchoredPosition = new Vector2(432f, 284f);
        mflower = mBlockTip.transform.Find("flower").GetComponent<Image>();
        mflower.sprite = ResManager.GetSprite("animalstatistics_sprite", "flower");
        mflower.enabled = false;
        mtextTip = mBlockTip.transform.Find("Text").GetComponent<Text>();

        lines = transform.Find("lines");
        for (int i=0;i<11;i++)
        {
            liness[i] = lines.Find("line" + i).GetComponent<Image>();
            liness[i].sprite = ResManager.GetSprite("animalstatistics_sprite", "xuxian");

            texts[i] = liness[i].transform.Find("Text").GetComponent<Text>();
        }

        List<float> indexXList = Common.GetOrderList(5, 180f);
        for (int i = 0; i < 5; i++)
        {
            GameObject gogo = mctrl.CreateResObj("dataobj",transform);
            gogo.transform.localPosition = new Vector3(indexXList[i], -180f, 0f);
            mDataObj[i] = gogo.AddComponent<AnimalStatistics_DataObj>();          
            mDataObj[i].InitAwake();
        }

        checkButton = transform.Find("checkButton").GetComponent<Image>();
        checkButton.sprite = ResManager.GetSprite("animalstatistics_sprite", "checkbtnup");
        checkButton.SetNativeSize();
        checkButton.GetComponent<Button>().transition = Selectable.Transition.None;
        EventTriggerListener.Get(checkButton.gameObject).onUp = CheckButtonClick;
        EventTriggerListener.Get(checkButton.gameObject).onDown = CheckBtnDown;

        mmPos = ResManager.GetPrefab("animalstatistics_prefab", "mposs", transform);
        for (int i = 0; i < 14; i++)
        {
            mPosList.Add(mmPos.transform.Find("Image" + i));        
        }
    }

    /// <summary>
    /// 1,2关设置
    /// </summary>
    public void SetDataLv1To2(List<int> _typeList, List<int> _numList)
    {
        bNextQua = false;
        nNextQuaID = 0;
        bNextQuaOK = false;
        nQuea2Count = 0;
        Quea2ResetInfos();

        string strblock = "block_yellow";
        if (mctrl.bLittleFlower)
        { mflower.gameObject.SetActive(true);
            mtextTip.rectTransform.anchoredPosition = new Vector2(30f, 0f);
            strblock = "block_flower";
        }
        else
        { mflower.gameObject.SetActive(false);
            mtextTip.rectTransform.anchoredPosition = Vector2.zero;
        }

        mBlockTip.sprite = ResManager.GetSprite("animalstatistics_sprite", strblock);

        vStart = vBase;
        checkButton.gameObject.SetActive(false);
        for (int i=0;i<5;i++)
        {
            mDataObj[i].bQuaOK = false;
            mDataObj[i].SetBlockSprite(strblock);
            mDataObj[i].SetDataLv1To2(_typeList[i], _numList[i]);
        }
    }
    /// <summary>
    /// 3关设置
    /// </summary>
    public void SetDataLv3(List<int> _typeList, List<int> _numList)
    {
        Quea2ResetInfos();

        string strblock = "block_yellow";
        if (mctrl.bLittleFlower)
        { mflower.gameObject.SetActive(true);
            mtextTip.rectTransform.anchoredPosition = new Vector2(30f, 0f);
            strblock = "block_flower";
        }
        else
        { mflower.gameObject.SetActive(false);
            mtextTip.rectTransform.anchoredPosition = Vector2.zero;
        }

        mBlockTip.sprite = ResManager.GetSprite("animalstatistics_sprite", strblock);

        vStart = vBase + new Vector3(-70f, 0f, 0f);
        checkButton.gameObject.SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            mDataObj[i].SetBlockSprite(strblock);
            mDataObj[i].SetDataLv3To4(_typeList[i], _numList[i]);
        }
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

    public void DataObjMove(bool _in)
    {
        mctrl.StartCoroutine(IEMove(_in));
    }
    IEnumerator IEMove(bool _in)
    {
        for (int i = 0; i < 5; i++)
        {
            mDataObj[i].SceneMove(_in);
            yield return new WaitForSeconds(0.5f);
        }
        if (mctrl.nLevel == 3)
        {
            AnimalStatistics_DataObj gdata = mctrl.mPanel.mDataObj[0];
            mctrl.GuideHandShowLv3(gdata.Animalhead.transform.position);
        }
        else if (mctrl.nLevel == 1)
        {
            yield return new WaitForSeconds(0.5f);
            //第一关指引显示
            mctrl.GuideHandShowLv1();
        }
    }

    /// <summary>
    /// 检测按钮显示/隐藏
    /// </summary>
    public void CheckBtnActive()
    {
        bool bshow = true;
        for (int i = 0; i < mDataObj.Length; i++)
        {
            if (mDataObj[i].mblockList.Count <=0 )
            {
                bshow = false;
                break;
            }
        }
        if (bshow)
        {         
            if (!checkButton.gameObject.activeSelf)
            {
                checkButton.transform.localScale = Vector3.one * 0.001f;
                checkButton.gameObject.SetActive(true);
                checkButton.transform.DOScale(Vector3.one, 0.3f);
            }
        }
        else
        {
            if (checkButton.gameObject.activeSelf)
            {           
                checkButton.transform.DOScale(Vector3.one * 0.001f, 0.3f).OnComplete(()=> 
                { checkButton.gameObject.SetActive(false); });
            }
        }
    }
    private void CheckBtnDown(GameObject _go)
    {
        if (!mctrl.bContrlFinish)
            return;
        if (mctrl.bLevelPass)
            return;
        checkButton.sprite = ResManager.GetSprite("animalstatistics_sprite", "checkbtndown");

        AudioClip cp = ResManager.GetClip("checkgamebtn_sound", "checkgamebtn_down");
        if (cp != null)
            AudioSource.PlayClipAtPoint(cp, Camera.main.transform.position);
    }
    private void CheckButtonClick(GameObject _go)
    {
        if (!mctrl.bContrlFinish)
            return;
        if (mctrl.bLevelPass)
            return;
        
        int count = 0;
        for (int i=0;i<5;i++)
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
            mctrl.bLevelPass = true;
            mctrl.nCount = 4;           
            for (int i = 0; i < 5; i++)
            {
                mDataObj[i].SetOKColor();
            }
            mctrl.StartCoroutine(iePlayCheckSucEffect0());
        }
        else
        {
            AudioClip cp = ResManager.GetClip("checkgamebtn_sound", "checkgamebtn_up");
            if (cp != null)
                AudioSource.PlayClipAtPoint(cp, Camera.main.transform.position);
            checkButton.sprite = ResManager.GetSprite("animalstatistics_sprite", "checkbtnup");
            mctrl.PlaySortSound("sound_checkfail");
        }     
    }

    IEnumerator iePlayCheckSucEffect0()
    {
        mctrl.PlayFlowerOK_fx(checkButton.transform.position);
        mctrl.PlaySortSound("sound_checksuc");
        yield return new WaitForSeconds(3.3f);
        mctrl.MLevelPass();
    }


    /// <summary>
    /// 表文字 隐藏/显示
    /// </summary>
    /// <param name="_active"></param>
    public void TextActive(bool _active)
    {
        for (int i = 0; i < 11; i++)
        {
            texts[i].enabled = _active;
        }
    }
    public AnimalStatistics_DataObj GetDataObjByNum(int _num)
    {
        AnimalStatistics_DataObj theobj = null;
        for (int i = 0; i < 5; i++)
        {
            if (mDataObj[i].nNum == _num)
                theobj = mDataObj[i];
        }
        return theobj;
    }
    /// <summary>
    /// 方格表示数量
    /// </summary>
    /// <param name="_num"></param>
    public void SetTipText(int _num)
    {
        mtextTip.text = _num.ToString();
    }
    /// <summary>
    /// 设置表刻
    /// </summary>
    public void SetLineText()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            float findex = i * mctrl.nBeiShu;
            texts[i].text = findex.ToString();
        }
    }


    #region//提问谁最多/最少--------------
    /// <summary>
    /// 是否在提问
    /// </summary>
    public bool bNextQua = false;
    public int nNextQuaID = 0;
    public bool bNextQuaOK = false;
    /// <summary>
    /// 提问谁最多/最少
    /// </summary>
    public void ToNextQua()
    {
        //设置点击回调
        for (int i = 0; i < mDataObj.Length; i++)
        {
            mDataObj[i].SetCheckEndCall(CheckTheNum);
        }
        mctrl.StartCoroutine(ieToNexQua());
    }
    IEnumerator ieToNexQua()
    {
        yield return new WaitForSeconds(0.5f);
        bNextQua = true;
        nNextQuaID++;
        bNextQuaOK = false;
        //播放提问语音
        mctrl.PlayLvStartSound();
    }
    /// <summary>
    /// 检测大小
    /// </summary>
    /// <param name="_dateobj"></param>
    public void CheckTheNum(AnimalStatistics_DataObj _dateobj)
    {
        if (mctrl.nCount <= 0)
            return;
        if (bNextQuaOK || _dateobj.bQuaOK)
            return;
        _dateobj.SetAnimalHeadBigEffect();
        bool bcheckok = false;
        if (nNextQuaID == 1)
        {
            if (_dateobj == GetTheMaxNumData())
            {
                //Debug.Log("ok is max");
                bNextQuaOK = true;
                bcheckok = true;
                _dateobj.bQuaOK = true;
            }
            else
            {
                //Debug.Log("not ok is not max");
                bcheckok = false;             
            }
        }
        else
        {
            if (_dateobj == GetTheMinNumData())
            {
                //Debug.Log("ok is min");
                bNextQuaOK = true;
                bcheckok = true;
                _dateobj.bQuaOK = true;
            }
            else
            {
                //Debug.Log("not ok is not min");
                bcheckok = false;
            }
        }
        if (bcheckok)
        {
            _dateobj.SetOKColor();
            mctrl.StartCoroutine(ieToCheckZuiShao());           
        }
        else
        {
            _dateobj.SetRedColor();
            //faile sound
            mctrl.SoundCtrl.StopTipSound();
            string strcp0 = "sound_fail_" + Random.Range(1,6);
            AudioClip mcp = mctrl.GetClip(strcp0);
            mctrl.SoundCtrl.PlaySound(mcp, 1f);
        }
    }
    //检测最少
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

    /// <summary>
    /// 取最大的数
    /// </summary>
    /// <returns></returns>
    public AnimalStatistics_DataObj GetTheMaxNumData()
    {
        AnimalStatistics_DataObj thedata = mDataObj[0];
        for (int i=1;i< mDataObj.Length;i++)
        {
            if (mDataObj[i].nNum > thedata.nNum)
            {
                thedata = mDataObj[i];
            }
        }
        return thedata;
    }
    /// <summary>
    /// 取最小的数
    /// </summary>
    /// <returns></returns>
    public AnimalStatistics_DataObj GetTheMinNumData()
    {
        AnimalStatistics_DataObj thedata = mDataObj[0];
        for (int i = 1; i < mDataObj.Length; i++)
        {
            if (mDataObj[i].nNum < thedata.nNum)
            {
                thedata = mDataObj[i];
            }
        }
        return thedata;
    }
    #endregion





    #region//提问xx比xx多几只,xx比xx少几只-----
    public int nQuea2Count = 0;//第二关提问完成累计
    public List<AnimalStatistics_DataObj> mQuea2DataObjList = new List<AnimalStatistics_DataObj>();//数字从小到大排序的object
    public AnimalStatistics_DataObj targetA = null;
    public AnimalStatistics_DataObj targetB = null;
    bool bQuea2OK = false;
    /// <summary>
    /// 设置第二关最后提问
    /// </summary>
    public void SetQuea2()
    {
        bQuea2OK = false;
        //设置点击回调
        for (int i = 0; i < mDataObj.Length; i++)
        {
            mDataObj[i].SetCheckEndCall(Quea2ClickCall);
        }

        mQuea2DataObjList = GetOrderDataObjList();

        if (targetA == null)
        {
            Debug.Log("only do once");
            List<int> getlistID = Common.GetIDList(1, 3, 2, -1);
            targetA = mQuea2DataObjList[getlistID[0]];
            if (targetB == null)
            {
                targetB = mQuea2DataObjList[getlistID[1]];
            }
        }
        
        mctrl.PlayTitleSound();
        //mctrl.StartCoroutine(ieSetQuea2());
    }

    private void Quea2ResetInfos()
    {
        mQuea2DataObjList.Clear();
        targetA = null;
        targetB = null;
        bQuea2OK = false;
    }

    private void Quea2ClickCall(AnimalStatistics_DataObj _dateobj)
    {
        if (mctrl.nCount <= 0)
            return;
        if (nQuea2Count >= 2 || _dateobj.bQuaOK)
            return;
        if (bQuea2OK)
            return;
        _dateobj.SetAnimalHeadBigEffect();
        bool bisok = false;
        if (nQuea2Count == 0)
        {
            if (_dateobj == targetA)
            { bisok = true; }
        }
        else if (nQuea2Count == 1)
        {
            if (_dateobj == targetB)
            { bisok = true; }
        }

        if (bisok)
        {
            bQuea2OK = true;
            nQuea2Count++;
            _dateobj.bQuaOK = true;
            _dateobj.SetOKColor();
            mctrl.StartCoroutine(ieToCheckZuiShao());
        }
        else
        {
            _dateobj.SetRedColor();
            //Debug.Log("错误了");
            //faile sound
            mctrl.SoundCtrl.StopTipSound();
            string strcp0 = "sound_fail_" + Random.Range(1, 6);
            AudioClip mcp = mctrl.GetClip(strcp0);
            mctrl.SoundCtrl.PlaySound(mcp, 1f);
        }     
    }


    #endregion

    /// <summary>
    /// 关卡1提示语音
    /// </summary>
    /// <returns></returns>
    public IEnumerator ieTipSoundLv1()
    {
        yield return new WaitForSeconds(0.1f);
        List<AudioClip> cpList = new List<AudioClip>();
        if (!bNextQua)
        {
            cpList.Add(mctrl.GetClip("z_additional_gezibiaoshi" + mctrl.nBeiShu));
            cpList.Add(mctrl.GetClip("game-tips1-3-2"));
        }
        else
        {
            if (nNextQuaID == 1)
            {
                cpList.Add(mctrl.GetClip("数量最多"));
            }
            else
            { cpList.Add(mctrl.GetClip("数量最少")); }
        }
        for (int i = 0; i < cpList.Count; i++)
        {
            mctrl.SoundCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }
    }
    /// <summary>
    /// 关卡2提示语音
    /// </summary>
    /// <returns></returns>
    public IEnumerator ieTipSoundLv2()
    {
        yield return new WaitForSeconds(0.1f);
        List<AudioClip> cpList = new List<AudioClip>();
        if (mctrl.nCount < 5)
        {
            cpList.Add(mctrl.GetClip("z_additional_gezibiaoshi" + mctrl.nBeiShu));
            cpList.Add(mctrl.GetClip("game-tips1-3-2"));
        }
        else
        {
            cpList.Add(mctrl.GetClip("z比"));

            AnimalStatistics_DataObj mtheObj = null;
            string strTheName = "";
            int nncount = 0;
            if (nQuea2Count == 0)
            {
                mtheObj = mQuea2DataObjList[0];
                nncount = targetA.nNum - mtheObj.nNum;
                strTheName = MDefine.GetAnimalNameByID_CH(mtheObj.nAnimalType);
                //Debug.Log("比" + strTheName + "多" + nncount + "只的是谁?");
                cpList.Add(ResManager.GetClip("aa_animal_name", strTheName));
                cpList.Add(mctrl.GetClip("z多"));
                cpList.Add(ResManager.GetClip("number_sound", nncount.ToString()));
            }
            else
            {
                mtheObj = mQuea2DataObjList[4];
                nncount = mtheObj.nNum - targetB.nNum;
                strTheName = MDefine.GetAnimalNameByID_CH(mtheObj.nAnimalType);
                //Debug.Log("比" + strTheName + "少" + nncount + "只的是谁?");
                cpList.Add(ResManager.GetClip("aa_animal_name", strTheName));
                cpList.Add(mctrl.GetClip("z少"));
                cpList.Add(ResManager.GetClip("number_sound", nncount.ToString()));
            }

            cpList.Add(mctrl.GetClip("z只"));
            cpList.Add(mctrl.GetClip("z的是谁"));
        }
        for (int i = 0; i < cpList.Count; i++)
        {
            mctrl.SoundCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }
    }

    //数字从小到大排序
    public List<AnimalStatistics_DataObj> GetOrderDataObjList()
    {
        List<AnimalStatistics_DataObj> mmList = new List<AnimalStatistics_DataObj>();
        for (int i = 0; i < mDataObj.Length; i++)
        {
            mmList.Add(mDataObj[i]);
        }
        for (int i = 0; i < mmList.Count - 1; i++)
        {
            for (int j = i + 1; j < mmList.Count; j++)
            {
                if (mmList[i].nNum > mmList[j].nNum)
                {
                    AnimalStatistics_DataObj mhehe = mmList[i];
                    mmList[i] = mmList[j];
                    mmList[j] = mhehe;
                }
            }
        }
        return mmList;
    }

}
