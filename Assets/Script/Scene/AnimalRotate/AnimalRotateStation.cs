using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class AnimalRotateStation : MonoBehaviour
{
    public int nType = 0;
    public bool bHit = false;
    /// <summary>
    /// 角度类型
    /// </summary>
    public int nRotateID = 0;
    public bool bCanRotate = true;

    /// <summary>
    /// 顺时针-1 , 逆时针1
    /// </summary>
    public int nRotateDir = -1;


    private Image imgbg;
    
    private Image rotateimg;
    private Button imgbtn;
    private Image imgpoint;

    [HideInInspector]
    public RectTransform rectTransform;
    private BoxCollider2D mbox2d;
    private Rigidbody2D rig2D;
    private Transform[] mPoss = new Transform[4];
    public AnimalRotateCell[] mCells = new AnimalRotateCell[4];
    private List<int> mCellIDs = new List<int>();

    private AnimalRotateCtrl mCtrl;
    private Vector3 vRemember;

    private ParticleSystem mEffect;

    private System.Action ClickCallBack = null;
    public void SetClickCallBack(System.Action _clickCallBack)
    {
        ClickCallBack = _clickCallBack;
    }


    public void InitAwake(int _type, int[] _cellIDs)
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as AnimalRotateCtrl;
        rectTransform = transform as RectTransform;

        nType = _type;
        imgbg = transform.Find("imgbg").GetComponent<Image>();
        imgbg.sprite = ResManager.GetSprite("animalrotate_sprite", "block" + nType);

        rotateimg = transform.Find("rotate").GetComponent<Image>();
        rotateimg.rectTransform.sizeDelta = new Vector2(70f, 70f);
        rotateimg.color = new Color(1f, 1f, 1f, 0f);
        imgpoint = rotateimg.transform.Find("point").GetComponent<Image>();
        imgpoint.sprite = ResManager.GetSprite("animalrotate_sprite", "rotatepoint");
        imgpoint.raycastTarget = false;
        imgpoint.rectTransform.sizeDelta = new Vector2(60f, 60f);
        imgpoint.rectTransform.localScale = new Vector3(-1f, 1f, 1f);
        //ImgPointFlash();

        rig2D = gameObject.AddComponent<Rigidbody2D>();
        rig2D.isKinematic = true;
        mbox2d = gameObject.AddComponent<BoxCollider2D>();
        mbox2d.size = imgbg.rectTransform.sizeDelta;
        mbox2d.isTrigger = true;
        mbox2d.offset = imgbg.rectTransform.anchoredPosition;

        for (int i=0;i<_cellIDs.Length;i++)
        {
            if (_cellIDs[i] != 0)
            {
                mPoss[i] = rotateimg.transform.Find("pos" + i);
                int cellid = _cellIDs[i];
                mCells[i] = mCtrl.CreateAnimalRotateCell(cellid, mPoss[i]);
                mCellIDs.Add(cellid);
            }
        }

        GameObject mgo = ResManager.GetPrefab("fx_animalrotate", "animalrotatefx");
        mgo.transform.SetParent(transform);
        mgo.transform.localScale = Vector3.one;
        mgo.transform.localPosition = Vector3.zero;
        mEffect = mgo.GetComponent<ParticleSystem>();
        mEffect.gameObject.SetActive(false);
        mEffect.Stop();

        imgbtn = rotateimg.gameObject.AddComponent<Button>();
        imgbtn.transition = Selectable.Transition.None;
        EventTriggerListener.Get(rotateimg.gameObject).onClick = ClickBtn;
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
    /// 点击旋转
    /// </summary>
    /// <param name="_go"></param>
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

        mCtrl.mSoundCtrl.PlaySortSound("animalrotate_sound", "点击大框转动");
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


    /// <summary>
    /// 设置角度
    /// </summary>
    /// <param name="_rotateID"></param>
    public void SetRotateID(int _rotateID)
    {
        nRotateID = _rotateID;
        rotateimg.transform.localEulerAngles = new Vector3(0f, 0f, nRotateID * AnimalRotateDefine.fRotateIndex * nRotateDir);
    }
    /// <summary>
    /// 设置cell rotate id
    /// </summary>
    public void SetCellRotateID(int _index, int _rotateID)
    {
        mCells[_index].SetRotateID(_rotateID);
    }
    /// <summary>
    /// station碰撞开/关
    /// </summary>
    public void BoxActive(bool _active)
    {
        mbox2d.enabled = _active;
    }
    /// <summary>
    /// cells碰撞开/关
    /// </summary>
    public void CellBoxActive(bool _active)
    {
        for (int i = 0; i < mCells.Length; i++)
        {
        }
    }
    /// <summary>
    /// station size
    /// </summary>
    public Vector2 GetSize()
    {
        return mbox2d.size;
    }
    /// <summary>
    /// 检测是否与另外一个station的cells相同
    /// </summary>
    public bool CheckCellSame(AnimalRotateCell[] _cells)
    {      
        for (int i = 0; i < _cells.Length; i++)
        {
            int theid = _cells[i].nID;
            if (!mCellIDs.Contains(theid))
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 设置记录点
    /// </summary>
    public void SetRememberPos(Vector3 _localPos)
    {
        vRemember = _localPos;
    }

    /// <summary>
    /// 返回记录点
    /// </summary>
    public void MoveToRemeberPos()
    {
        transform.DOLocalMove(vRemember, 0.3f).OnComplete(()=> 
        {
            bHit = false;
        });
    }


    /// <summary>
    /// 完成重置信息
    /// </summary>
    public void PassReset()
    {
        RotateTipPointActive(false);
        for (int i = 0; i < mCells.Length; i++)
        {
            mCells[i].RotateTipPointActive(false);
        }
    }
}
