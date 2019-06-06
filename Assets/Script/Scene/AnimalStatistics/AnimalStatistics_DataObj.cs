using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class AnimalStatistics_DataObj : MonoBehaviour
{

    public int nAnimalType = 0;
    public int nNum = 0;

    /// <summary>
    /// 提问是否完成
    /// </summary>
    public bool bQuaOK = false;

    private Image baseblock;
    private Transform blocks;
    public List<Image> mblockList = new List<Image>();

    private float fHeadScale = 0.7f;
    private Image animalHead;
    public Image Animalhead { get { return animalHead; } }
    private Button headBtn;
    private SkeletonGraphic spine;

    public AnimalStatistics_TipNum tipNum;
    AnimalStatisticsCtrl mCtrl;
    private Vector3 vStart;

    public void InitAwake()
    {
        vStart = transform.localPosition;

        mCtrl = SceneMgr.Instance.GetNowScene() as AnimalStatisticsCtrl;

        baseblock = transform.Find("baseblock").GetComponent<Image>();
        baseblock.sprite = ResManager.GetSprite("animalstatistics_sprite", "block_yellow");

        baseblock.gameObject.SetActive(false);
        blocks = transform.Find("blocks");

        animalHead = transform.Find("animalHead").GetComponent<Image>();
        animalHead.rectTransform.sizeDelta = new Vector2(130f, 125f);
        animalHead.color = new Color(1f, 1f, 1f, 0f);
        animalHead.transform.localScale = Vector3.one * fHeadScale;
        animalHead.transform.localPosition = new Vector3(0f, -100f, 0f);
        headBtn = animalHead.gameObject.AddComponent<Button>();
        EventTriggerListener.Get(headBtn.gameObject).onUp = HeadClick;
        EventTriggerListener.Get(headBtn.gameObject).onDown = HeadClickDown;

        tipNum = transform.Find("tipNum").GetComponent<AnimalStatistics_TipNum>();
        if (tipNum == null)
        {
            tipNum = transform.Find("tipNum").gameObject.AddComponent<AnimalStatistics_TipNum>();
        }
        tipNum.InitAwake();
    }

    /// <summary>
    /// 创建spine
    /// </summary>
    /// <param name="_id"></param>
    private void CreateSpine(int _id)
    {
        string _strname = MDefine.GetAnimalHeadResNameByID(_id);
        spine = ResManager.GetPrefab("animalhead_prefab", _strname).GetComponent<SkeletonGraphic>();
        spine.transform.SetParent(animalHead.transform);
        spine.transform.localScale = Vector3.one * 0.45f;
        spine.transform.localPosition = new Vector3(0f, -80f, 0f);
    }

    #region //data 1 2
    /// <summary>
    /// 数据设置 1 2
    /// </summary>
    public void SetDataLv1To2(int _animalType, int _num)
    {
        ResetInfos();
        
        nAnimalType = _animalType;
        nNum = _num;
        tipNum.SetNumber(_num);
        tipNum.HideNumber();
        animalHead.sprite = null;
        CreateBlocks();
        animalHead.transform.localScale = Vector3.one * 0.7f;
        CreateSpine(_animalType);
        //ScreenDebug.Log("ok--" + _num);
    }
    public void CreateBlocks()
    {
        int createCount = nNum / mCtrl.nBeiShu;
        for(int i=0;i<createCount;i++)
        {
            Image bObj = GameObject.Instantiate(baseblock) as Image;
            bObj.gameObject.SetActive(true);
            bObj.transform.SetParent(blocks);
            bObj.transform.localScale = Vector3.one;
            bObj.rectTransform.anchoredPosition3D = Vector3.zero + new Vector3(0f, 40f, 0f) * i;
            mblockList.Add(bObj);

            Button btnonj = bObj.gameObject.AddComponent<Button>();
            btnonj.transition = Selectable.Transition.None;
            EventTriggerListener.Get(bObj.gameObject).onClick = RemoveClick;
        }

        //数量位置
        Vector3 addPos = Vector3.zero;
        if (mblockList.Count > 0)
        {
            addPos = mblockList[mblockList.Count - 1].rectTransform.anchoredPosition3D;
        }
        tipNum.rt.anchoredPosition3D = addPos + new Vector3(0f, 80f, 0f);
        tipNum.SetScale(1.3f);
    }

    #endregion

    public void ResetInfos()
    {
        transform.localPosition = vStart + new Vector3(1500f, 0f, 0f);

        nAnimalType = 0;
        nNum = 0;
        tipNum.transform.localPosition = Vector3.zero;
        tipNum.SetBlueBGActive(true);
        tipNum.ResetInfos();

        animalHead.gameObject.SetActive(false);

        for (int i = 0; i < mblockList.Count; i++)
        {
            if (mblockList[i].gameObject != null)
                GameObject.Destroy(mblockList[i].gameObject);
        }
        mblockList.Clear();

        mCtrl.mUFO.ShowFlower(mCtrl.bLittleFlower);

        if (spine != null)
        {
            if (spine.gameObject != null)
                GameObject.Destroy(spine.gameObject);
        }
    }

    //设置最后提问call
    private System.Action<AnimalStatistics_DataObj> mCheckEndCall = null;
    /// <summary>
    /// 设置最后提问call
    /// </summary>
    public void SetCheckEndCall(System.Action<AnimalStatistics_DataObj> _call)
    {
        mCheckEndCall = _call;
    }

    #region//data 3 4
    /// <summary>
    /// 数据设置 3 4
    /// </summary>
    public void SetDataLv3To4(int _animalType, int _num)
    {
        ResetInfos();
        nAnimalType = _animalType;
        tipNum.SetNumber(_num);
        tipNum.SetBlueBGActive(false);
        tipNum.transform.localPosition = new Vector3(0f, -50f, 0f);
        animalHead.sprite = null;
        animalHead.transform.localScale = Vector3.one * 0.7f;
        tipNum.SetScale(1f * 0.75f);
        CreateSpine(_animalType);
    }

    /// <summary>
    /// 点击头像添加一个
    /// </summary>
    public void HeadClick(GameObject _go)
    {
        if (mCtrl.nLevel <= 2)
        {
            //if (mCtrl.mPanel.bNextQua)
            //{
            //    mCtrl.mPanel.CheckTheNum(this);
            //}
            if (mCheckEndCall != null)
            { mCheckEndCall(this); }
            return;
        }
        if (!mCtrl.bContrlFinish)
        { return; }
        if (!mCtrl.bGameReady)
        { return; }
        if (mCtrl.bLevelPass)
            return;
        mCtrl.GuideHandStopLv3();
        Vector3 vPos = _go.transform.position;
        Vector3 vOldUfo = mCtrl.mUFO.transform.position;
        float dis = Vector3.Distance(new Vector3(vPos.x, vOldUfo.y, vOldUfo.z), vOldUfo);
        //Debug.Log(dis);
        float fWaiteAddIn = 0.3f;
        if (dis <= 0.03f)
        {
            fWaiteAddIn = 0.01f;
        }
        mCtrl.mUFO.UFOMoveTo(false, new Vector3(vPos.x, vOldUfo.y, vOldUfo.z), fWaiteAddIn * 0.5f, AddBlock);
        if (mblockList.Count >= 10)
        {
            return;
        }      
        mCtrl.bContrlFinish = false;     
    }
    private void AddBlock()
    {
        if (mblockList.Count >= 10)
        {
            return;
        }
        StartCoroutine(IEWaiteCreate());
    }
    IEnumerator IEWaiteCreate()
    {
        //实例一个block
        Vector3 blockStartPos = mCtrl.mUFO.GetCreatePos();     
        Image bObj = GameObject.Instantiate(baseblock) as Image;
        bObj.gameObject.SetActive(true);
        bObj.transform.SetParent(blocks);
        bObj.transform.localScale = Vector3.one;
        bObj.transform.position = blockStartPos;
        bObj.gameObject.AddComponent<Button>();
        EventTriggerListener.Get(bObj.gameObject).onClick = RemoveClick;
        //ufo block 隐藏
        mCtrl.mUFO.SetBlockState(false);
        //新block下移
        Vector3 vto = new Vector3(0f, 40f, 0f) * mblockList.Count;
        bObj.transform.DOLocalMove(vto, 0.5f).OnComplete(()=> 
        {
            //click guide fx
            if (!mCtrl.bguideLv3Finish)
                mCtrl.GuideHandShowLv3(bObj.transform.position);
        });
        mCtrl.PlaySortSound("sound_blockdown");

        mblockList.Add(bObj);
        nNum += mCtrl.nBeiShu;
        
        mCtrl.mPanel.CheckBtnActive();

        yield return new WaitForSeconds(0.2f);
        //ufo 重新出现block
        mCtrl.mUFO.BlockShowOutEffect();
        mCtrl.mUFO.SetBlockSprite("block_yellow");
        mCtrl.mUFO.SetBlockState(true);

        mCtrl.bContrlFinish = true;

        //for (int i = 0; i < mblockList.Count; i++)
        //{
        //    mblockList[i].sprite = ResManager.GetSprite("animalstatistics_sprite", "block_yellow");
        //}
    }

    private void HeadClickDown(GameObject _go)
    {
        SetAnimalHeadBigEffect();
        if (spine != null)
        {
            if (mdelayTween != null && mdelayTween.IsPlaying())
            {
                return;
            }
            //播放动物声音
            string stranimalname = MDefine.GetAnimalNameByID_CH(nAnimalType);
            AudioClip cp = ResManager.GetClip("aa_animal_sound", stranimalname + 0);
            AudioSource.PlayClipAtPoint(cp, Camera.main.transform.position, 0.5f);

            spine.AnimationState.SetAnimation(1, "Click", true);
            mdelayTween = spine.transform.DOScale(spine.transform.localScale, 1f).OnComplete(() => 
            {
                spine.AnimationState.SetAnimation(1, "Idle", true);
            });        
        }
    }
    Tween mdelayTween = null;


    /// <summary>
    /// 移除操作
    /// </summary>
    /// <param name="_go"></param>
    public void RemoveClick(GameObject _go)
    {
        if (mCtrl.bLevelPass)
            return;
        if (mCtrl.nLevel <= 2)
        {
            //if (mCtrl.mPanel.bNextQua)
            //{
            //    mCtrl.mPanel.CheckTheNum(this);
            //}
            if (mCheckEndCall != null)
            { mCheckEndCall(this); }
            return;
        }
        if (!mCtrl.bContrlFinish)
        { return; }
        if (mblockList.Count <= 0)
            return;
        //stop click guide fx
        mCtrl.bguideLv3Finish = true;
        mCtrl.GuideHandStopLv3();       

        Image theImage = mblockList[mblockList.Count - 1];
        if (_go == mblockList[mblockList.Count - 1].gameObject)
        {
            AudioClip cp0 = ResManager.GetClip("animalstatistics_sound", "z_flowerandblockclick");
            AudioSource.PlayClipAtPoint(cp0, Camera.main.transform.position);
            mblockList.Remove(theImage);
            GameObject.Destroy(theImage.gameObject);
            nNum -= mCtrl.nBeiShu;
        }
    }


    /// <summary>
    /// 检测值是否匹配
    /// </summary>
    /// <returns></returns>
    public bool CheckNumIsOK()
    {
        return nNum == tipNum.nNum;
    }

    /// <summary>
    /// 方块绿色设置
    /// </summary>
    public void SetOKColor()
    {
        StopCoroutine("IESetRedColor");
        StartCoroutine(IESetOKColor());
    }
    IEnumerator IESetOKColor()
    {
        for (int i = 0; i < mblockList.Count; i++)
        {
            mblockList[i].sprite = ResManager.GetSprite("animalstatistics_sprite", "block_green");
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void SetRedColor()
    {
        StopCoroutine("IESetRedColor");
        if (mblockList.Count > 0)
        {
            StartCoroutine(IESetRedColor());
        }
    }
    IEnumerator IESetRedColor()
    {
        //1
        for (int i = 0; i < mblockList.Count; i++)
        {
            mblockList[i].sprite = ResManager.GetSprite("animalstatistics_sprite", "block_red");
        }
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < mblockList.Count; i++)
        {
            mblockList[i].sprite = ResManager.GetSprite("animalstatistics_sprite", "block_yellow");
        }
        //2
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < mblockList.Count; i++)
        {
            mblockList[i].sprite = ResManager.GetSprite("animalstatistics_sprite", "block_red");
        }
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < mblockList.Count; i++)
        {
            mblockList[i].sprite = ResManager.GetSprite("animalstatistics_sprite", "block_yellow");
        }
        //3
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < mblockList.Count; i++)
        {
            mblockList[i].sprite = ResManager.GetSprite("animalstatistics_sprite", "block_red");
        }
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < mblockList.Count; i++)
        {
            mblockList[i].sprite = ResManager.GetSprite("animalstatistics_sprite", "block_yellow");
        }
    }

    #endregion


    public void SceneMove(bool _in)
    {
        if (_in)
        {
            AudioClip cp = Resources.Load <AudioClip>("sound/素材出现通用音效");
            AudioSource.PlayClipAtPoint(cp, Camera.main.transform.position);

            animalHead.gameObject.SetActive(true);
            animalHead.transform.localScale = Vector3.one * 0.7f;
            transform.localPosition = vStart + new Vector3(1500f, 0f, 0f);
            transform.DOLocalMove(vStart, 0.5f);
        }
    }

    /// <summary>
    /// 显示头像
    /// </summary>
    public void HeadShow()
    {
        animalHead.gameObject.SetActive(true);
        animalHead.transform.localScale = Vector3.one * 0.001f;
        animalHead.transform.DOScale(Vector3.one * 0.7f, 0.3f).SetEase(Ease.OutBack);
    }

    /// <summary>
    /// 头像变大效果
    /// </summary>
    public void SetAnimalHeadBigEffect()
    {
        animalHead.transform.DOScale(Vector3.one * 1.1f, 0.2f).OnComplete(() =>
         {
             animalHead.transform.DOScale(Vector3.one * fHeadScale, 0.2f);
         });
    }

    public void SetBlockSprite(string _name)
    {
        baseblock.sprite = ResManager.GetSprite("animalstatistics_sprite", _name);
    }

}
