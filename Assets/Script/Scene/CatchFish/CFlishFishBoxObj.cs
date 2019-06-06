using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class CFlishFishBoxObj : MonoBehaviour
{
    /// <summary>
    /// 鱼缸stationID
    /// </summary>
    public int nBoxID = 0;

    private Image imgbox;
    private BoxCollider2D mbox2D;

    private float fMoveMinX = -100f;
    private float fMoveMaxX = 100f;

    private Image imgNum;

    public List<CFishFishObj> mFishList = new List<CFishFishObj>();

    private Image imgMask;
    private WaterMove mWaterMove;

    public void InitAwake(int _id)
    {
        nBoxID = _id;

        imgbox = UguiMaker.newImage("imgbox", transform, "catchfish_sprite", "waterbox", false);
        mbox2D = gameObject.AddComponent<BoxCollider2D>();
        mbox2D.size = imgbox.rectTransform.sizeDelta;

        fMoveMinX = imgbox.rectTransform.sizeDelta.x * 0.5f * -1;
        fMoveMaxX = imgbox.rectTransform.sizeDelta.x * 0.5f;

        imgNum = UguiMaker.newImage("imgnum", transform, "catchfish_sprite", "0", false);
        imgNum.rectTransform.anchoredPosition = new Vector2(0f, 80f);
        SetNumberImage(0);

        vstart = transform.localPosition;

        imgMask = UguiMaker.newGameObject("mask", transform).AddComponent<Image>();
        imgMask.raycastTarget = false;
        imgMask.rectTransform.sizeDelta = new Vector2(387f, 270f);
        Mask maskMSK = imgMask.gameObject.AddComponent<Mask>();
        maskMSK.showMaskGraphic = false;

        mWaterMove = UguiMaker.newGameObject("watermove", imgMask.transform).AddComponent<WaterMove>();
        mWaterMove.strAB = "catchfish_sprite";
        mWaterMove.strResSprite = "water0";
        mWaterMove.transform.localPosition = new Vector3(0f, -37f, 0f);
        mWaterMove.InitAwake(382f, 134f, 0.63f);
        mWaterMove.fspeed = 25f;
    }

    /// <summary>
    /// 计算加入的位置Y值
    /// </summary>
    /// <param name="_fish"></param>
    /// <returns></returns>
    public float CatAddToPosY(CFishFishObj _fish)
    {
        float _sizeY = _fish.GetSize().y * 0.5f;
        float fY = Random.Range(-100f + _sizeY, 20f - _sizeY);
        return fY;
    }

    /// <summary>
    /// 加入鱼完成回调
    /// </summary>
    private System.Action mAddCallBack = null;

    /// <summary>
    /// 加入一条鱼
    /// </summary>
    /// <param name="_fish"></param>
    /// <param name="_callback"></param>
    public void AddFish(CFishFishObj _fish, System.Action _callback = null)
    {
        mAddCallBack = _callback;

        _fish.transform.SetParent(transform);
        _fish.SetFishSize(0.8f);
        _fish.PlayAnimation("Idle");

        float _sizeX = _fish.GetSize().x * 0.5f;
        _fish.SetMoveLimiteX(fMoveMinX + _sizeX + 20f, fMoveMaxX - _sizeX - 20f);

        float _getY = CatAddToPosY(_fish);
        _fish.transform.SetParent(transform);
        Vector3 _vto = new Vector3(_fish.transform.localPosition.x, _getY, 0f);
        _fish.transform.DOLocalMove(_vto, 0.3f).OnComplete(() =>
        {
            _fish.SetRandomDirction();
            _fish.DoFishMove();
            if (mAddCallBack != null)
            {
                mAddCallBack();
            }
        });


        _fish.nInStation = nBoxID;
        mFishList.Add(_fish);

        SetNumberImage(mFishList.Count);

        imgMask.transform.SetSiblingIndex(30);
    }

    /// <summary>
    /// 移出一条鱼
    /// </summary>
    /// <param name="_fish"></param>
    public void RemoveOutFish(CFishFishObj _fish)
    {
        if (mFishList.Contains(_fish))
        {
            mFishList.Remove(_fish);
        }

        SetNumberImage(mFishList.Count);
    }

    /// <summary>
    /// 设置数字
    /// </summary>
    /// <param name="_id"></param>
    public void SetNumberImage(int _id)
    {
        imgNum.sprite = ResManager.GetSprite("catchfish_sprite", _id.ToString());
        imgNum.SetNativeSize();
        imgNum.transform.localScale = Vector3.one * 0.8f;
    }

    public void NumberTipReset()
    {
        SetNumberImage(0);
        imgNum.transform.localScale = Vector3.one * 0.001f;
        imgNum.transform.DOScale(Vector3.one * 0.8f, 0.2f);
    }

    public void NumberTipActive(bool _active)
    {
        imgNum.enabled = _active;
    }

    private Vector3 vstart;
    public void ShakeObj()
    {
        StartCoroutine(TScale());
    }
    IEnumerator TScale()
    {
        for (float j = 0; j < 1f; j += 0.05f)
        {
            //float p = Mathf.Sin(Mathf.PI * j) * 0.8f;
            transform.localPosition = vstart + new Vector3(Mathf.Sin(Mathf.PI * 6 * j) * 10f, 0f, 0f);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localPosition = vstart;
    }

}
