using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class AnimalRotateLv3Station : MonoBehaviour
{
    public int nType = 0;
    public int nRotateID = 0;

    /// <summary>
    /// 顺时针-1 , 逆时针1
    /// </summary>
    public int nRotateDir = -1;

    Button btn;
    private Image imgbg;
    private BoxCollider2D mbox2d;

    private Image rotateimg;
    private Image imgpoint;

    public AnimalRotateLv3CellStation[] mCellStations = new AnimalRotateLv3CellStation[4];

    private AnimalRotateCtrl mCtrl;
    public void InitAwake(int _type)
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as AnimalRotateCtrl;

        nType = _type;
        imgbg = transform.Find("imgbg").GetComponent<Image>();
        imgbg.sprite = ResManager.GetSprite("animalrotate_sprite", "block" + nType);

        rotateimg = transform.Find("rotate").GetComponent<Image>();
        rotateimg.rectTransform.sizeDelta = new Vector2(70f, 70f);
        rotateimg.color = new Color(1f, 1f, 1f, 0f);
        imgpoint = rotateimg.transform.Find("point").GetComponent<Image>();
        imgpoint.raycastTarget = false;
        imgpoint.gameObject.SetActive(false);
        //ImgPointFlash();

        mbox2d = gameObject.AddComponent<BoxCollider2D>();
        mbox2d.size = imgbg.rectTransform.sizeDelta;
        mbox2d.isTrigger = true;
        mbox2d.offset = imgbg.rectTransform.anchoredPosition;

        for (int i = 0; i < 4; i++)
        {
            mCellStations[i] = rotateimg.transform.Find("pos" + i).gameObject.AddComponent<AnimalRotateLv3CellStation>();
            mCellStations[i].InitAwake(i);
        }
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
    /// 设置角度
    /// </summary>
    /// <param name="_rotateID"></param>
    public void SetRotateID(int _rotateID)
    {
        nRotateID = _rotateID;
        //nRotateID = 0;
        rotateimg.transform.localEulerAngles = new Vector3(0f, 0f, nRotateID *  AnimalRotateDefine.fRotateIndex * nRotateDir);
    }

    /// <summary>
    /// 添加按钮事件
    /// </summary>
    public void CreateButton()
    {
        //btn = rotateimg.gameObject.AddComponent<Button>();
        //btn.transition = Selectable.Transition.None;
        //EventTriggerListener.Get(rotateimg.gameObject).onClick = BtnClick;
        //ShowPoint();
    }
    private void BtnClick(GameObject _go)
    {
        bool bKongCell = true;
        for (int i = 0; i < mCellStations.Length; i++)
        {
            if (mCellStations[i].mcell != null)
            { bKongCell = false;
                break;
            }
        }

        if (bKongCell)
            return;

        rotateimg.transform.localEulerAngles = rotateimg.transform.localEulerAngles + new Vector3(0f, 0f, AnimalRotateDefine.fRotateIndex * nRotateDir);
        mCtrl.mLevel3.CheckPass();

        mCtrl.mSoundCtrl.PlaySortSound("animalrotate_sound", "点击大框转动");
    }

    public void ShowPoint()
    {
        imgpoint.gameObject.SetActive(true);
        imgpoint.sprite = ResManager.GetSprite("animalrotate_sprite", "rotatepoint");
    }
    /// <summary>
    /// 旋转点提示显示/隐藏
    /// </summary>
    /// <param name="_active"></param>
    public void RotateTipPointActive(bool _active)
    {
        imgpoint.enabled = _active;
    }

    public void HideBtn()
    {
        if (btn != null)
            btn.enabled = false;
        imgbg.raycastTarget = false;
    }
}
