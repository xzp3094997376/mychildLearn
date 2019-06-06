using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class AnimalStatistics_dataObj4 : MonoBehaviour
{

    public int nAnimalType = 0;
    public int nNum = 0;

    public int nCountClick = 0;

    private Transform flowers;
    public List<AnimalStatistics_flower> mflowerList = new List<AnimalStatistics_flower>();

    public Image animalHead;
    private Button headBtn;
    private SkeletonGraphic spine;

    public AnimalStatistics_TipNum tipNum;
    private AnimalStatisticsCtrl mCtrl;
    private Vector3 vStart;

    private Button btnQuea2;

    public void InitAwake()
    {
        vStart = transform.localPosition;
        transform.localPosition = new Vector3(1500f, vStart.y, vStart.z);

        mCtrl = SceneMgr.Instance.GetNowScene() as AnimalStatisticsCtrl;

        flowers = transform.Find("flowers");

        animalHead = transform.Find("animalHead").GetComponent<Image>();
        animalHead.rectTransform.sizeDelta = new Vector2(130f, 125f);
        animalHead.color = new Color(1f, 1f, 1f, 0f);
        headBtn = animalHead.gameObject.AddComponent<Button>();
        EventTriggerListener.Get(headBtn.gameObject).onUp = HeadClick;
        EventTriggerListener.Get(headBtn.gameObject).onDown = HeadClickDown;

        tipNum = transform.Find("tipNum").gameObject.AddComponent<AnimalStatistics_TipNum>();
        tipNum.InitAwake();
        tipNum.transform.localScale = Vector3.one * 0.75f;
        tipNum.SetNumColor(new Color(68f / 255, 26f / 255, 6f / 255, 1f));

        Image imgBtn = UguiMaker.newGameObject("btnQuea2", transform).AddComponent<Image>();
        imgBtn.color = new Color(1f, 1f, 1f, 0f);
        imgBtn.transform.localPosition = new Vector3(0f, -70f, 0f);
        imgBtn.rectTransform.sizeDelta = new Vector2(150f, 700f);
        btnQuea2 = imgBtn.gameObject.AddComponent<Button>();
        btnQuea2.transition = Selectable.Transition.None;
        btnQuea2.onClick.AddListener(Quea2ClickCheck);
    }

    public void SetData(int _animalType, int _num)
    {
        ShowQuea2Btn(false);

        nAnimalType = _animalType;
        nNum = _num;
        tipNum.SetNumber(_num);
        tipNum.ShowNumber();
        animalHead.sprite = null;    
        animalHead.transform.localScale = Vector3.one;
        CreateSpine(_animalType);

        float indexY = 83f;
        for (int i = 0; i < 6; i++)
        {
            GameObject mgo = UguiMaker.newGameObject("flower" + i, flowers);
            mgo.transform.localPosition = new Vector3(0f, indexY * i, 0f);
            AnimalStatistics_flower flowerctrl = mgo.AddComponent<AnimalStatistics_flower>();
            flowerctrl.InitAwake(this, 0, i);
            mflowerList.Add(flowerctrl);
        }

        nCountClick = 0;
    }

    /// <summary>
    /// 花朵点击消息
    /// </summary>
    /// <param name="_flower"></param>
    public void FlowerClickCallback(AnimalStatistics_flower _flower)
    {
        if (mCtrl.bLevelPass)
            return;
        if (bfuckclose)
            return;
        if (!_flower.bHadClick)
        {
            if (CheckTheFlowerIsTop(_flower, true))
            { 
                _flower.ChangeType(mCtrl.mPanel4.nChooseType);
                _flower.bHadClick = true;
                nCountClick++;
                _flower.ClickBig();
                AudioClip cp0 = ResManager.GetClip("animalstatistics_sound", "z_flowerclick");
                AudioSource.PlayClipAtPoint(cp0, Camera.main.transform.position);
                SetAnimalHeadBigEffect();
            }
            else
            {
                //Debug.Log("请从下往上点击");
            }
        }
        else
        {
            if (CheckTheFlowerIsTop(_flower, false))
            {
                _flower.ChangeType(0);
                _flower.bHadClick = false;
                nCountClick--;
                if (nCountClick <= 0)
                    nCountClick = 0;
                _flower.ClickBig();
                AudioClip cp0 = ResManager.GetClip("animalstatistics_sound", "z_flowerandblockclick");
                AudioSource.PlayClipAtPoint(cp0, Camera.main.transform.position);
                SetAnimalHeadBigEffect();
            }
            else
            {
                //Debug.Log("请从上往下点击");
            }
        }
        
        mCtrl.mPanel4.CheckBtnShow();
    }

    /// <summary>
    /// 检测是否达成
    /// </summary>
    /// <returns></returns>
    public bool CheckNumIsOK()
    {
        if (nCountClick * 3 == nNum)
        { return true; }
        else
            return false;
    }

    bool bfuckclose = false;
    public void CloseFlowerClickAndHeadClick()
    {
        bfuckclose = true;
    }


    Tween mdelayTween = null;
    private void HeadClickDown(GameObject _go)
    {
        if (bfuckclose)
            return;
        SetAnimalHeadBigEffect();
        if (spine != null)
        {
            if (mdelayTween != null && mdelayTween.IsPlaying())
            {
                return;
            }
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
    private void HeadClick(GameObject _go)
    {
        if (bfuckclose)
            return;
        if (!mCtrl.bGameReady)
        { return; }
    }

    /// <summary>
    /// 检测是否能点击
    /// </summary>
    public bool CheckTheFlowerIsTop(AnimalStatistics_flower _theFlower,bool _in)
    {
        if (_in)
        {
            for (int i = 0; i < _theFlower.nIndex; i++)
            {
                if (!mflowerList[i].bHadClick)
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            for (int i = mflowerList.Count-1; i > _theFlower.nIndex; i--)
            {
                if (mflowerList[i].bHadClick)
                {
                    return false;
                }
            }
            return true;
        }
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

    /// <summary>
    /// 绿色
    /// </summary>
    public void SetOKColor()
    {
        StopCoroutine("IEOKColor");
        StartCoroutine(IEOKColor());
    }
    IEnumerator IEOKColor()
    {
        for (int i = 0; i < mflowerList.Count; i++)
        {
            mflowerList[i].SetOKColor();
            yield return new WaitForSeconds(0.2f);
        }
    }

    /// <summary>
    /// flower 红闪
    /// </summary>
    public void SetRedColor()
    {
        for (int i=0;i<mflowerList.Count;i++)
        {
            mflowerList[i].SetColorRed();
        }
    }


    public void SceneMove(bool _in)
    {
        if (_in)
        {
            AudioClip cp = Resources.Load<AudioClip>("sound/素材出现通用音效");
            AudioSource.PlayClipAtPoint(cp, Camera.main.transform.position);
            transform.localPosition = vStart + new Vector3(1500f, 0f, 0f);
            transform.DOLocalMove(vStart, 0.5f);
        }
    }

    /// <summary>
    /// 头像变大效果
    /// </summary>
    public void SetAnimalHeadBigEffect()
    {
        animalHead.transform.DOScale(Vector3.one * 1.25f, 0.2f).OnComplete(() =>
        {
            animalHead.transform.DOScale(Vector3.one, 0.2f);
        });
    }

    /// <summary>
    /// 问题按钮显示
    /// </summary>
    /// <param name="_active"></param>
    public void ShowQuea2Btn(bool _active)
    {
        btnQuea2.gameObject.SetActive(_active);
    }
    System.Action<AnimalStatistics_dataObj4> quea2ClickCall = null;
    public void SetQuea2ClickCall(System.Action<AnimalStatistics_dataObj4> _call)
    {
        quea2ClickCall = _call;
    }
    private void Quea2ClickCheck()
    {
        if (quea2ClickCall != null)
        { quea2ClickCall(this); }
    }


    public void SetClickQuea2Active(bool _active)
    {
        btnQuea2.enabled = _active;
    }

}

public class AnimalStatistics_flower : MonoBehaviour
{
    public int nType = 0;
    public bool bHadClick = false;
    public int nIndex = 0;

    private Image img;
    private Button btn;

    AnimalStatistics_dataObj4 mDataObj;
    public void InitAwake(AnimalStatistics_dataObj4 _dataObj, int _flowerType,int _index)
    {
        mDataObj = _dataObj;
        nIndex = _index;
        img = gameObject.AddComponent<Image>();
        btn = gameObject.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        img.sprite = ResManager.GetSprite("animalstatistics_sprite", "mflower" + _flowerType);
        img.SetNativeSize();

        EventTriggerListener.Get(gameObject).onClick = ClickBtn;
    }


    public void ChangeType(int _type)
    {
        nType = _type;
        img.sprite = ResManager.GetSprite("animalstatistics_sprite", "mflower" + nType);
    }


    private void ClickBtn(GameObject _go)
    {
        mDataObj.FlowerClickCallback(this);
    }

    public void ClickBig()
    {
        transform.DOScale(Vector3.one * 1.2f, 0.15f).OnComplete(() =>
         {
             transform.DOScale(Vector3.one, 0.15f);
         });
    }

    /// <summary>
    /// 红闪
    /// </summary>
    public void SetColorRed()
    {
        if (bHadClick)
        {
            StopCoroutine("IESetRedColor");
            StartCoroutine(IESetRedColor());
        }
    }
    IEnumerator IESetRedColor()
    {
        //1
        img.sprite = ResManager.GetSprite("animalstatistics_sprite", "mflower1");       
        yield return new WaitForSeconds(0.2f);
        img.sprite = ResManager.GetSprite("animalstatistics_sprite", "mflower" + nType);
        //2
        yield return new WaitForSeconds(0.2f);
        img.sprite = ResManager.GetSprite("animalstatistics_sprite", "mflower1");
        yield return new WaitForSeconds(0.2f);
        img.sprite = ResManager.GetSprite("animalstatistics_sprite", "mflower" + nType);
        //3
        yield return new WaitForSeconds(0.2f);
        img.sprite = ResManager.GetSprite("animalstatistics_sprite", "mflower1");
        yield return new WaitForSeconds(0.2f);
        img.sprite = ResManager.GetSprite("animalstatistics_sprite", "mflower" + nType);
    }

    /// <summary>
    /// 方块绿色设置
    /// </summary>
    public void SetOKColor()
    {
        if (bHadClick)
            img.sprite = ResManager.GetSprite("animalstatistics_sprite", "mflower2");
    }

}
