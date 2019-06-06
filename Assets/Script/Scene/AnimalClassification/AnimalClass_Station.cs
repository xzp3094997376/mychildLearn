using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class AnimalClass_Station : MonoBehaviour
{

    private AnimalClassificationCtrl mCtrl;
    private PolygonCollider2D mBoxCollider;

    private Image bg;

    public List<AnimalClass_Animal> mAnimalList = new List<AnimalClass_Animal>();
    public List<Transform> setposList = new List<Transform>();

    private Image mTipImage;
    //private Text mTipText;

    /// <summary>
    /// 当前动物属性
    /// </summary>
    public AnimalValueType theAnimalValueType = AnimalValueType.None;

    public void InitAwake(string _strHouse)
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as AnimalClassificationCtrl;
        mBoxCollider = transform.Find("collider").GetComponent<PolygonCollider2D>();
        mBoxCollider.isTrigger = true;

        bg = gameObject.GetComponent<Image>();
        bg.sprite = ResManager.GetSprite("animalclass_sprite", _strHouse);

        mAnimalList.Clear();

        Transform animalpos = transform.Find("mpos");
        for (int i = 0; i < 12; i++)
        {
            Transform tf = animalpos.Find("setpos" + i);
            if (tf != null)
                setposList.Add(tf);
        }

        CreateTipText();
    }

    public void SetLimiteVetect(LimiteVetect v)
    {
    }
    //设置栏
    public void SetLan()
    {      
    }


    public void SetLocalPos(Vector3 _pos)
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition3D = _pos;
    }

    /// <summary>
    /// 拖出
    /// </summary>
    /// <param name="_anmal"></param>
    public void DropOut(AnimalClass_Animal _anmal)
    {
        _anmal.nStatinIndexPos = -1;
        mAnimalList.Remove(_anmal);
    }

    /// <summary>
    /// 拖入
    /// </summary>
    /// <param name="_anmal"></param>
    public void DropIn(AnimalClass_Animal _anmal)
    {
        mAnimalList.Add(_anmal);
        _anmal.transform.SetParent(transform);

        float mmScale = 0.6f;
        _anmal.transform.localScale = Vector3.one * mmScale;
        //分配位置
        SetAnimalPosInStation(_anmal);

        mCtrl.PlaySortSound("sound_animal" + _anmal.nType + "_2");
    }

    /// <summary>
    /// 根据属性检测是否达成分类
    /// </summary>
    public bool CheckAnimalTypeIsOK(AnimalValueType _valueType, out AnimalStationValue _outInfos)
    {
        bool retIsOK = true;

        _outInfos = null; 

        int _type = (int)_valueType;
        List<int> checkList = mCtrl.dicCheckList[_type];
        for (int i=0;i<checkList.Count;i++)
        {
            int _checkID = checkList[i];
            AnimalClass_Animal theGet = mAnimalList.Find((x) => { return x.nType == _checkID; });
            if (theGet == null)
            {
                retIsOK = false;
                break;
            }
        }

        if (mAnimalList.Count >= 14)
        {
            retIsOK = false;
        }

        if (retIsOK)
        {
            _outInfos = new AnimalStationValue();
            _outInfos.mValueType = _valueType;
            _outInfos.mStation = this;
            _outInfos.mAnimalList = mAnimalList;
        }

        return retIsOK;
    }

    /// <summary>
    /// 检测是否有这个动物
    /// </summary>
    /// <param name="_ID">aniaml类型</param>
    /// <returns></returns>
    public bool CheckIsHaveAnimal(int _ID)
    {
        AnimalClass_Animal getAnimal = null;
        getAnimal = mAnimalList.Find((x) => { return x.nType == _ID; });
        if (getAnimal != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetInfos()
    {
        mAnimalList.Clear();
        mTipImage.gameObject.SetActive(false);
    }

    //为动物分配位置
    private void SetAnimalPosInStation(AnimalClass_Animal _anmal)
    {
        for (int i = 0; i < 12; i++)
        {
            if (!CheckHasPos(i))
            {
                _anmal.nStatinIndexPos = i;
                _anmal.transform.position = setposList[i].position;
                break;
            }
        }
    }
    private bool CheckHasPos(int _index)
    {
        AnimalClass_Animal ggg = mAnimalList.Find((x) => { return x.nStatinIndexPos == _index; });
        if (ggg != null)
            return true;
        else
            return false;
    }

    public void SceneMove(bool _in)
    {
        if (_in)
        {
            transform.localScale = Vector3.one * 0.001f;
            transform.DOScale(Vector3.one * mCtrl.GetStationScale(), 1f);
        }
        else
        {
            transform.DOScale(Vector3.one * 0.001f, 1f);
        }
    }

    public void SetHouse()
    {
        //bg.sprite = ResManager.GetSprite("animalclass_sprite", "hj_fl_home1");
        //DOTween.To(() => bg.color, (x) => bg.color = x, new Color(1f, 1f, 1f, 0f), 1f).OnComplete(() => 
        //{
        //    bg.sprite = ResManager.GetSprite("animalclass_sprite", "hj_fl_home2");
        //    DOTween.To(() => bg.color, (x) => bg.color = x, new Color(1f, 1f, 1f, 1f), 1f);
        //});
    }


    //tip text 创建
    private void CreateTipText()
    {
        mTipImage = UguiMaker.newImage("mTipImage", transform, "animalclass_sprite", "tag_1_1", false);
        mTipImage.transform.localScale = Vector3.one * 0.55f;
        float tipheight = 140f;
        if (mCtrl.nLevel == 3)
            tipheight = 80f;
        mTipImage.rectTransform.anchoredPosition3D = new Vector3(0f, tipheight, 0f);

        mTipImage.gameObject.SetActive(false);
    }
    /// <summary>
    /// 提示文本显示
    /// </summary>
    public void ShowTipText(string _txt)
    {
        mTipImage.sprite = ResManager.GetSprite("animalclass_sprite", _txt);
        mTipImage.gameObject.SetActive(true);
    }

    /// <summary>
    /// 变大变小
    /// </summary>
    public void SetBigEffect()
    {
        Vector3 voldscal = transform.localScale;
        transform.DOScale(voldscal * 1.25f, 0.2f).OnComplete(() =>
         {
             transform.DOScale(voldscal, 0.2f);
         });
    }


    public string GetTipName()
    {
        mTipImage.rectTransform.anchoredPosition3D = new Vector3(0f, 80f, 0f);
        string strtxt = "tag_30_1";
        switch (theAnimalValueType)
        {
            case AnimalValueType.Poultry:
                strtxt = "tag_30_1";
                break;
            case AnimalValueType.Livestock:
                strtxt = "tag_31_1";
                break;
            case AnimalValueType.Birds:
                strtxt = "tag_32_1";
                break;
            case AnimalValueType.Beast:
                strtxt = "tag_33_1";
                break;
        }
        return strtxt;
    }


    bool bTipBtnInit = false;
    /// <summary>
    /// 创建提示tipimg按钮
    /// </summary>
    public void CreateImgTipBtn()
    {
        voldscale = mTipImage.transform.localScale;
        mTipImage.raycastTarget = true;
        if (bTipBtnInit)
            return;
        Button mbtn = mTipImage.gameObject.AddComponent<Button>();
        mbtn.transition = Selectable.Transition.None;
        EventTriggerListener.Get(mbtn.gameObject).onClick = ClickImgTip;
        bTipBtnInit = true;
    }
    Vector3 voldscale = Vector3.one;
    private void ClickImgTip(GameObject _go)
    {
        int ntype = (int)theAnimalValueType;
        AudioClip cp = mCtrl.GetClip("type" + ntype);
        AudioSource.PlayClipAtPoint(cp, Camera.main.transform.position);
        mTipImage.transform.DOScale(voldscale * 1.3f, 0.15f).OnComplete(() =>
        {
            mTipImage.transform.DOScale(voldscale, 0.15f);
        });
    }


}


