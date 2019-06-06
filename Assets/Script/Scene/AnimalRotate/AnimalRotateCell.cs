using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

/// <summary>
/// 小头像动物
/// </summary>
public class AnimalRotateCell : MonoBehaviour
{

    public int nID = 0;
    /// <summary>
    /// 角度类型
    /// </summary>
    public int nRotateID = 0;
    public bool bCanRotate = true;

    /// <summary>
    /// 顺时针-1 , 逆时针1
    /// </summary>
    public int nRotateDir = -1;

    public bool bInStation = true;
    public bool bCanDrop = true;
    private Vector3 vRemeber = Vector3.zero;
    public void SetRemeberPos(Vector3 vpos)
    {
        vRemeber = vpos;
    }

    private Button imgbtn;
    private SkeletonGraphic mspine;
    private BoxCollider2D mbox2d;
    private Image imgpoint;


    private System.Action ClickCallBack = null;
    public void SetClickCallBack(System.Action _clickCallBack)
    {
        ClickCallBack = _clickCallBack;
    }

    public void InitAwake(int _id)
    {
        nID = _id;
        CreateSpine(_id);

        mbox2d = gameObject.AddComponent<BoxCollider2D>();
        mbox2d.size = new Vector2(65f, 65f);
        mbox2d.isTrigger = true;
        mbox2d.enabled = false;

        Image img = gameObject.AddComponent<Image>();
        img.rectTransform.sizeDelta = new Vector2(65f, 65f);
        img.color = new Color(1f, 1f, 1f, 0f);
        imgbtn = gameObject.AddComponent<Button>();
        EventTriggerListener.Get(gameObject).onClick = ClickBtn;

        imgpoint = UguiMaker.newImage("point", transform, "animalrotate_sprite", "rotatepoint", false);
        imgpoint.rectTransform.sizeDelta = new Vector2(30f, 30f);
        imgpoint.transform.localScale = new Vector3(-1f, 1f, 1f);
        //ImgPointFlash();
    }

    /// <summary>
    /// 设置 顺时针-1 , 逆时针1
    /// </summary>
    /// <param name="_dir"></param>
    public void SetRotateDir(int _dir)
    {
        nRotateDir = _dir;
        imgpoint.transform.localScale = new Vector3(_dir, 1f, 1f);
    }

    private void ImgPointFlash()
    {
        DOTween.To(() => imgpoint.color, x => imgpoint.color = x, new Color(1f, 1f, 1f, 0f), 0.5f).OnComplete(() => 
        {
            DOTween.To(() => imgpoint.color, x => imgpoint.color = x, new Color(1f, 1f, 1f, 1f), 0.5f).OnComplete(() => 
            {
                ImgPointFlash();
            });
        });
    }

    /// <summary>
    /// 按钮触发开/关
    /// </summary>
    /// <param name="_active"></param>
    public void ButtonEnbal(bool _active)
    {
        imgbtn.enabled = _active;
        bCanRotate = _active;
        RotateTipPointActive(_active);
    }
    /// <summary>
    /// 旋转点提示显示/隐藏
    /// </summary>
    /// <param name="_active"></param>
    public void RotateTipPointActive(bool _active)
    {
        imgpoint.enabled = _active;
    }

    public Vector2 GetSize()
    {
        return mbox2d.size;
    }

    /// <summary>
    /// 设置角度
    /// </summary>
    /// <param name="_rotateID"></param>
    public void SetRotateID(int _rotateID)
    {
        nRotateID = _rotateID;
        transform.localEulerAngles = new Vector3(0f, 0f, nRotateID * AnimalRotateDefine.fRotateIndex * nRotateDir);
    }
    /// <summary>
    /// box2d
    /// </summary>
    /// <param name="_active"></param>
    public void BoxActive(bool _active)
    {
        mbox2d.enabled = _active;
    }

    private void ClickBtn(GameObject _go)
    {
        if (!bCanRotate)
            return;
        int theid = nRotateID + 1;
        if (theid > AnimalRotateDefine.nRotateMaxType)
            theid = 0;
        SetRotateID(theid);
        if (ClickCallBack != null)
            ClickCallBack();

        if (mCtrl == null)
        { mCtrl = SceneMgr.Instance.GetNowScene() as AnimalRotateCtrl; }
        mCtrl.mSoundCtrl.PlaySortSound("animalrotate_sound", "点击角色转动");
    }
    AnimalRotateCtrl mCtrl;

    /// <summary>
    /// lv3 忽略按钮点击事件
    /// </summary>
    public void SetLevel3()
    {
        ButtonEnbal(false);
        BoxActive(true);
    }

    /// <summary>
    /// 创建spine
    /// </summary>
    /// <param name="_id"></param>
    public void CreateSpine(int _id)
    {
        if (_id != 0)
        {
            string strspinename = MDefine.GetAnimalHeadResNameByID(_id);
            GameObject mgo = ResManager.GetPrefab("animalhead_prefab", strspinename);
            mgo.transform.SetParent(transform);
            mgo.transform.localScale = Vector3.one * 0.3f;
            mgo.transform.localPosition = new Vector3(0f, -45f, 0f);
            mspine = mgo.GetComponent<SkeletonGraphic>();
        }
        else
        {
            if (mspine != null)
            {
                if (mspine.gameObject != null)
                    GameObject.Destroy(mspine.gameObject);
                mspine = null;
            }
        }
    }

    public void MoveToRemeberPos()
    {
        transform.DOLocalMove(vRemeber, 0.3f);
        DropReset();
    }
    public void DropSet()
    {
        StopDropTween();
        dropTween = transform.DOScale(Vector3.one * 1.7f, 0.25f);
    }
    public void DropReset()
    {
        StopDropTween();
        dropTween = transform.DOScale(Vector3.one * 1.5f, 0.25f);
    }
    Tween dropTween = null;
    public void StopDropTween()
    {
        if (dropTween != null)
            dropTween.Pause();
    }
}
