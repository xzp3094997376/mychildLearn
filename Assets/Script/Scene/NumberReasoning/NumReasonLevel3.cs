using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class NumReasonLevel3 : NumReasonLevelBase
{

    public NumReasonBus mBusUp;
    public NumReasonBus mBusDown;

    private Button imgGOBtn;
    private Image imgGO;
    private Image imgRang;
    private Tween tweenScale;
    private Tween tweenColor;

    public override void ResetInfos()
    {
        base.ResetInfos();
        Common.DestroyChilds(transform);      
    }

    public override void SetData()
    {
        ResetInfos();

        gameObject.SetActive(true);
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        transform.localScale = Vector3.one;
        //transform.DOLocalMove(Vector3.zero, 1f);
        int nUpLostCount = Random.Range(2, 4);
        int nDownLostCount = 2;
        nToCount = nUpLostCount + nDownLostCount;
        nCount = 0;

        if (imgBG == null)
        {
            imgBG = UguiMaker.newRawImage("bg", transform, "numberreasoning_texture", "numreasonimg_bj3", false);
            imgBG.rectTransform.sizeDelta = new Vector2(1280f, 800f);
        }

        List<int> animalIDs = Common.GetIDList(1, 14, 8, -1);
        List<int> animalIDUp = animalIDs.GetRange(0, 4);
        List<int> animalIDDown = animalIDs.GetRange(4, 4);

        bool bRandom = true;
        if (Random.value > 0)
            bRandom = false;
        List<int> numIdUp = GetSingleNumList(bRandom);
        List<int> numIdDown = GetSingleNumList(!bRandom);

        mBusUp = UguiMaker.newGameObject("mBusUp", transform).AddComponent<NumReasonBus>();  
        mBusUp.InitAwake(1, animalIDUp.ToArray(), numIdUp.ToArray());
        mBusUp.transform.localPosition = new Vector3(1200f, 110f, 0f);
        mBusUp.transform.DOLocalMoveX(160f, 5f).SetDelay(1f);

        mBusDown = UguiMaker.newGameObject("mBusDown", transform).AddComponent<NumReasonBus>();
        mBusDown.InitAwake(2, animalIDDown.ToArray(), numIdDown.ToArray());
        mBusDown.transform.localPosition = new Vector3(1200f, -220f, 0f);       
        mBusDown.transform.DOLocalMoveX(120f, 5f);

        imgGO = UguiMaker.newImage("imgGO", transform, "numberreasoning_sprite", "img_stop");
        imgGO.transform.localPosition = new Vector3(-410f, -400f + imgGO.rectTransform.sizeDelta.y * 0.5f, 0f);

        imgGOBtn = imgGO.gameObject.AddComponent<Button>();
        imgGOBtn.transition = Selectable.Transition.None;
        imgGOBtn.onClick.AddListener(GoButtonClick);

        float ftime = 0.75f;
        imgRang = UguiMaker.newImage("imgRang", imgGO.transform, "numberreasoning_sprite", "img_stoprang");
        imgRang.transform.localPosition = new Vector3(0f, 25f, 0f);
        tweenScale = imgRang.transform.DOScale(Vector3.one * 1.8f, ftime).SetLoops(-1, LoopType.Restart);
        tweenColor = DOTween.To(()=> imgRang.color,x=> imgRang.color = x,new Color(1f,1f,1f,0f), ftime).SetLoops(-1, LoopType.Restart);

        ActiveTweenRang(false);

        //set lost number
        mBusUp.SetLost(nUpLostCount);
        mBusDown.SetLost(nDownLostCount);

        mCtrl.StartCoroutine(ieWaiteForTime());
    }
    IEnumerator ieWaiteForTime()
    {
        yield return new WaitForSeconds(0.5f);
        mCtrl.SoundCtrl.PlaySortSound("numberreasoning_sound", "车喇叭");
        yield return new WaitForSeconds(2.5f);
        mCtrl.PlayTipSound();
    }

    NumReasonNumObj mSelect;
    private void Update()
    {
        if (!bInit)
            return;
        if (mCtrl.bLvPass)
            return;
        if (mCtrl.MInputNumObj.gameObject.activeSelf)
        {
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            mSelect = null;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                NumReasonNumObj com = hit.collider.gameObject.GetComponent<NumReasonNumObj>();
                if (com != null)
                {
                    mSelect = com;
                }
            }
            if (mSelect != null)
            {
                mCtrl.MInputNumObj.transform.position = mSelect.transform.position;
                mCtrl.MInputNumObj.ShowEffect();

                Vector3 vpos = mCtrl.MInputNumObj.transform.localPosition;               
                float fY = vpos.y;
                if (fY > 0)
                { vpos = vpos + new Vector3(0f, -190f, 0f); }
                else
                { vpos = vpos + new Vector3(0f, 190f, 0f); }
                mCtrl.MInputNumObj.transform.localPosition = vpos;

                mCtrl.MInputNumObj.SetInputNumberCallBack(InputNumbCallback);
                mCtrl.MInputNumObj.SetFinishInputCallBack(InputNumFinishCallback);
                mCtrl.MInputNumObj.SetClearNumberCallBack(InputClearCallback);

                mSelect.WenHaoActive(false);
                mCtrl.mInputTip.transform.position = mSelect.transform.position;
                mCtrl.mInputTip.gameObject.SetActive(true);
            }
        }
    }

    #region//callback set
    //输入数字
    public override void InputNumbCallback()
    {
        mSelect.WenHaoActive(false);
        mSelect.MiniNumberActive(true);
        int nid = 0;
        if (int.TryParse(mCtrl.MInputNumObj.strInputNum, out nid))
        { }
        else { Debug.LogError("不能转成 int 类型:" + mCtrl.MInputNumObj.strInputNum); }
        mSelect.SetNumber(nid);
        //光标位置设置
        if (nid < 10)
        {
            mCtrl.mInputTip.gameObject.SetActive(true);
            Image imgTar = mSelect.GetMiniNumber.GetMaxRight();
            if (imgTar != null)
            {
                mCtrl.mInputTip.transform.position = imgTar.transform.position;
                mCtrl.mInputTip.transform.localPosition = mCtrl.mInputTip.transform.localPosition + new Vector3(21.1f, 0f, 0f);
            }
            else
            { mCtrl.mInputTip.transform.position = mSelect.transform.position; }
        }
        else
        {
            mCtrl.mInputTip.gameObject.SetActive(false);
        }
    }
    //确认输入
    public override void InputNumFinishCallback()
    {
        mCtrl.mInputTip.gameObject.SetActive(false);

        if (mCtrl.MInputNumObj.strInputNum.CompareTo("") == 0)
        {
            mSelect.WenHaoActive(true);
            mSelect.MiniNumberActive(false);
            return;
        }

        int thenum = mCtrl.MInputNumObj.nInputNum;
        mSelect.SetNumber(thenum);

        if (mSelect.CheckIsOK())
        {
            //Debug.Log("---ok---");
            nCount++;
            mSelect.Box2DActive(false);
            if (nCount >= nToCount && nToCount > 0)
            {
                //ActiveGoButton();
                mCtrl.StartCoroutine(ieActiveGoButton());
            }
            PlaySucSound();
        }
        else
        {
            mSelect.WenHaoActive(true);
            mSelect.MiniNumberActive(false);
            mSelect.ShakeObj();

            PlayFaileSoune();
        }
    }
    //清除数字
    public override void InputClearCallback()
    {
        mCtrl.mInputTip.gameObject.SetActive(true);
        mCtrl.mInputTip.transform.position = mSelect.transform.position;
        //mSelect.WenHaoActive(true);
        mSelect.MiniNumberActive(false);
    }
    #endregion

    /// <summary>
    /// 光圈激活/隐藏
    /// </summary>
    /// <param name="_active"></param>
    private void ActiveTweenRang(bool _active)
    {
        imgGOBtn.interactable = _active;
        if (_active)
        {
            imgRang.gameObject.SetActive(true);
            tweenScale.Restart();
            tweenColor.Restart();
        }
        else
        {
            imgRang.gameObject.SetActive(false);
            tweenScale.Pause();
            tweenColor.Pause();
        }
    }

    /// <summary>
    /// 激活bus开走按钮
    /// </summary>
    private void ActiveGoButton()
    {
        imgGO.sprite = ResManager.GetSprite("numberreasoning_sprite", "img_go");
        ActiveTweenRang(true);
    }
    IEnumerator ieActiveGoButton()
    {
        yield return new WaitForSeconds(0.32f);
        ActiveGoButton();
    }

    private void GoButtonClick()
    {
        Debug.Log("level 3 pass");
        mCtrl.bPlayOtherTip = true;
        mCtrl.bLvPass = true;
        mCtrl.StartCoroutine(ieToNextLevel());

        ActiveTweenRang(false);
    }

    //关卡1完成
    IEnumerator ieToNextLevel()
    {
        AudioClip cpHappy = ResManager.GetClip("numberreasoning_sound", "欢呼");
        mCtrl.SoundCtrl.PlaySortSound(cpHappy);
        mBusUp.PlayHappyAnimaltion();
        mBusDown.PlayHappyAnimaltion();
        yield return new WaitForSeconds(1f);
        mBusUp.transform.DOLocalMoveX(-1300f, 5f);
        mBusDown.transform.DOLocalMoveX(-1350f, 5f);
        yield return new WaitForSeconds(0.32f);
        PlayBusMoveOutSound();
        yield return new WaitForSeconds(3f);
        mCtrl.LevelCheckNext();
    }

    /// <summary>
    /// 移出界面并清除信息
    /// </summary>
    public override void MoveOutAndReset()
    {
        if (mBusUp != null)
            mBusUp.transform.localPosition = new Vector3(1400f, 0f, 0f);
        if (mBusDown != null)
            mBusDown.transform.localPosition = new Vector3(1400f, 0f, 0f);
        transform.DOLocalMove(new Vector3(1400f, 0f, 0f), 1f).OnComplete(() =>
        {
            ResetInfos();
            gameObject.SetActive(false);
            gameObject.transform.localPosition = new Vector3(-1400f, 0f, 0f);
        });
    }


    #region//语音---
    //玩法提示语音
    public override void PlayTipSound()
    {
        AudioClip cp = mCtrl.SoundCtrl.GetClip("numberreasoning_sound", "s观察车子上面的数字规律");
        mCtrl.SoundCtrl.PlaySound(cp, 1f);
    }
    public void PlaySucSound()
    {
        mCtrl.SoundCtrl.PlaySortSound("numberreasoning_sound", "选择正确");

        mCtrl.SoundCtrl.StopTipSound();
        if (Random.value > 0.5f)
        {
            AudioClip cp = mCtrl.SoundCtrl.GetClip("numberreasoning_sound", "s你对数字很敏感");
            mCtrl.SoundCtrl.PlaySound(cp, 1f);
        }
        else
        {
            mCtrl.PlayGoodGoodSound();
        }
    }
    public void PlayFaileSoune()
    {
        mCtrl.SoundCtrl.PlaySortSound("numberreasoning_sound", "车子错误");

        mCtrl.SoundCtrl.StopTipSound();
        AudioClip cp = null;
        int id = Random.Range(1, 5);
        if (id == 1)
        { cp = ResManager.GetClip("numberreasoning_sound", "s有的车子是偶数"); }
        else if (id == 2)
        { cp = ResManager.GetClip("numberreasoning_sound", "s有的车子是奇数"); }
        else if (id == 3)
        { cp = ResManager.GetClip("numberreasoning_sound", "s每个数字之间都相差"); }
        else
        { cp = ResManager.GetClip("numberreasoning_sound", "s用后面的数字减去前面的"); }
        if (cp != null)
            mCtrl.SoundCtrl.PlaySound(cp, 1f);
    }
    public void PlayBusMoveOutSound()
    {
        mCtrl.SoundCtrl.StopTipSound();
        AudioClip cp = mCtrl.SoundCtrl.GetClip("numberreasoning_sound", "s哇车子开走啦");
        mCtrl.SoundCtrl.PlaySound(cp, 1f);
    }
    #endregion

    /// <summary>
    /// 取得奇数/偶数 列
    /// </summary>
    /// <param name="_isSingleNumber"></param>
    /// <returns></returns>
    public List<int> GetSingleNumList(bool _isSingleNumber)
    {
        List<int> getList = new List<int>();

        List<int> baseList = new List<int>() { 1, 3, 5, 7, 9 };

        int nbaseID = baseList[Random.Range(1, baseList.Count)];
        for (int i = 0; i < 6; i++)
        {
            if (_isSingleNumber)
                getList.Add(nbaseID + i * 2);
            else
                getList.Add(nbaseID + i * 2 + 1);
        }

        return getList;
    }


}
