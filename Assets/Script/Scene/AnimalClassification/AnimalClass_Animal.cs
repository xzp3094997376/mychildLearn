using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using Spine.Unity;

/// <summary>
/// 动物类
/// </summary>
public class AnimalClass_Animal : MonoBehaviour
{
    //ID
    public int nType = 0;
    /// <summary>
    /// 动物类型
    /// </summary>
    public AnimalClassType mAnimalType = AnimalClassType.None;

    /// <summary>
    /// 在station的位置编号
    /// </summary>
    public int nStatinIndexPos = -1;

    private RectTransform rectTransform;
    private BoxCollider2D mBocCollider;

    private Image imgYingzi;
    private SkeletonGraphic mSpine;

    //bool bInit = false;
    Vector3 vStart = Vector3.zero;
    Vector3 vRemaidPos = Vector3.zero;

    private List<int> mAnimalValueTypeList = new List<int>();


    public AnimalClassificationCtrl mCtrl;

    public void InitAwake(int _type)
    {
        rectTransform = gameObject.GetComponent<RectTransform>();

        nType = _type;
        mAnimalType = (AnimalClassType)_type;

        SetOthersType();

        gameObject.name = "m" + mAnimalType.ToString();

        mBocCollider = gameObject.GetComponent<BoxCollider2D>();
        mBocCollider.isTrigger = true;

        mSpine = transform.Find("spine").GetComponent<SkeletonGraphic>();
        PlayAnimation("Idle", true);

        mCtrl = SceneMgr.Instance.GetNowScene() as AnimalClassificationCtrl;

        imgYingzi = transform.Find("yingzi").GetComponent<Image>();
        imgYingzi.sprite = ResManager.GetSprite("animalclass_sprite", "hj_fl_yingzi");
    }

    public void SetLocalPos(Vector3 _pos)
    {
        rectTransform.anchoredPosition = _pos;
    }

    public void SetRemaidPos(Vector3 _pos)
    {
        vRemaidPos = _pos;
    }
    public void BackToRemaidPos()
    {
        transform.DOLocalMove(vRemaidPos, 0.2f).OnComplete(()=> 
        {
            YingziActive(true);
        });
        //rectTransform.anchoredPosition3D = vRemaidPos;
    }

    /// <summary>
    /// 拖动设置
    /// </summary>
    public void DropSet()
    {
        transform.localScale = Vector3.one * mCtrl.fScale * 1.2f;
        PlayAnimation("Click");
        mCtrl.PlaySortSound("sound_animal" + nType + "_1");
    }

    public void DropReset()
    {
        transform.localScale = Vector3.one * mCtrl.fScale;
        PlayAnimation("Idle");
    }

    /// <summary>
    /// 播放动画 (Idle / Click)
    /// </summary>
    public void PlayAnimation(string _name, bool _loop = true)
    {
        mSpine.AnimationState.SetAnimation(1, _name, _loop);
    }


    public Vector3 GetSize()
    {
        return mBocCollider.size * mCtrl.fScale;     
    }
    public Vector3 GetColliderOffset()
    {
        return mBocCollider.offset;
    }

    //设置属性类型
    private void SetOthersType()
    {
        mAnimalValueTypeList.Clear();
        switch (nType)
        {
            case 1: //牛
                mAnimalValueTypeList = new List<int>() { 2, 3, 6, 8, 10, 21, 31 };
                break;
            case 2: //燕子
                mAnimalValueTypeList = new List<int>() { 1, 4, 6, 7, 9, 20, 32 };
                break;
            case 3: //熊
                mAnimalValueTypeList = new List<int>() { 2, 4, 6, 8, 10, 21, 33 };
                break;
            case 4: //鸡
                mAnimalValueTypeList = new List<int>() { 1, 4, 6, 8, 9, 21, 30 };
                break;
            case 5: //羊
                mAnimalValueTypeList = new List<int>() { 2, 3, 6, 8, 10, 21, 31 };
                break;
            case 6: //老虎
                mAnimalValueTypeList = new List<int>() { 2, 4, 6, 8, 10, 21, 33 };
                break;
            case 7: //猫头鹰
                mAnimalValueTypeList = new List<int>() { 1, 4, 6, 7, 10, 20, 32 };
                break;
            case 8: //马
                mAnimalValueTypeList = new List<int>() { 2, 4, 6, 8, 9, 21, 31 };
                break;
            case 9: //鸽子
                mAnimalValueTypeList = new List<int>() { 1, 4, 6, 7, 9, 20, 30 };
                break;
            case 10: //鸭子
                mAnimalValueTypeList = new List<int>() { 1, 4, 5, 8, 10, 21, 22, 30 };
                break;
            case 11: //兔子
                mAnimalValueTypeList = new List<int>() { 2, 4, 6, 8, 10, 21, 31 };
                break;
            case 12: //豹子
                mAnimalValueTypeList = new List<int>() { 2, 4, 6, 8, 10, 21, 33 };
                break;
            case 13: //鹅
                mAnimalValueTypeList = new List<int>() { 1, 4, 5, 8, 9, 21, 22, 30 };
                break;
            case 14: //猪
                mAnimalValueTypeList = new List<int>() { 2, 4, 6, 8, 10, 21, 31 };
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 影子显示/隐藏
    /// </summary>
    /// <param name="_active"></param>
    public void YingziActive(bool _active)
    {
        imgYingzi.enabled = _active;
    }

    public void SceneMove(bool _in,float _time = 1f)
    {
        if (_in)
        {
            transform.localScale = Vector3.one * 0.01f;
            transform.DOScale(Vector3.one * mCtrl.fScale, _time);
        }
        else
        { }
    }


}
