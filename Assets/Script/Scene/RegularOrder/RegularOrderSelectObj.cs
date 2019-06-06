using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
public class RegularOrderSelectObj : MonoBehaviour
{
    /// <summary>
    /// 1随机动物 2花 3随机动物
    /// </summary>
    public int nType = 0;

    private Button btn;

    [HideInInspector]
    public RectTransform rectTransform;

    private SkeletonGraphic mSpine;

    private float fScele = 0.65f;

    public void InitAwake(int _type, Transform _parent)
    {
        nType = _type;
        rectTransform = gameObject.GetComponent<RectTransform>();
        btn = gameObject.GetComponent<Button>();
        EventTriggerListener.Get(gameObject).onClick = BtnClick;

        gameObject.name = "mSelect" + _type;
        transform.SetParent(_parent);
        transform.localScale = Vector3.one * fScele;

        Image image = gameObject.GetComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 0f);

        //小花默认
        Transform tr = transform.Find("spine");
        if (tr != null)
        {
            mSpine = transform.Find("spine").GetComponent<SkeletonGraphic>();
            PlayAnimation("Idle");
        }
    }

    /// <summary>
    /// 当前动物类型
    /// </summary>
    public int nTheAnimalType = 0;
    /// <summary>
    /// 设置动物的类型
    /// </summary>
    /// <param name="_animalType"></param>
    public void SetAnimalType(int _animalType)
    {
        if (mSpine != null)
        {
            if (mSpine.gameObject != null)
            {
                GameObject.Destroy(mSpine.gameObject);
            }
        }
        mSpine = null;

        nTheAnimalType = _animalType;

        string headname = MDefine.GetAnimalHeadResNameByID(nTheAnimalType);
        GameObject mgo = ResManager.GetPrefab("animalhead_prefab", headname);
        mgo.transform.SetParent(transform);
        mgo.transform.localPosition = new Vector3(0f,-92f,0f);
        mgo.transform.localScale = Vector3.one *0.65f;

        mSpine = mgo.GetComponent<SkeletonGraphic>();
        PlayAnimation("Idle");
    }


    /// <summary>
    /// 播放动画 (Idle / Click)
    /// </summary>
    public void PlayAnimation(string _name, bool _loop = true)
    {
        if (mSpine != null)
            mSpine.AnimationState.SetAnimation(1, _name, _loop);
    }


    private void BtnClick(GameObject go)
    {
        RegularOrderCtrl mctrl = SceneMgr.Instance.GetNowScene() as RegularOrderCtrl;
        if (mctrl != null)
        {
            mctrl.SetSelectObj(this);
        }
    }



    /// <summary>
    /// 拖动设置
    /// </summary>
    public void DropSet()
    {
        transform.DOScale(Vector3.one * fScele * 1.2f, 0.2f);
        PlayAnimation("Click");
    }

    /// <summary>
    /// 拖动重置
    /// </summary>
    public void DropReset()
    {
        transform.DOScale(Vector3.one * fScele, 0.2f);
        PlayAnimation("Idle");
    }


    public void ResetInfos()
    {
        transform.localScale = Vector3.one * fScele;
        if (mSpine != null)
        {
            PlayAnimation("Idle");
        }
    }


    public void SceneMove(bool _in)
    {
        if (_in)
        {
            transform.localScale = Vector3.one * fScele * 0.001f;
            transform.DOScale(Vector3.one * fScele, 0.3f).SetEase(Ease.OutBack);
        }
    }

}
