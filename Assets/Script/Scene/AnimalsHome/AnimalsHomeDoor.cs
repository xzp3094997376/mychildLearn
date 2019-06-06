using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class AnimalsHomeDoor : MonoBehaviour
{
    /// <summary>
    /// 需要的点数
    /// </summary>
    public int nCount = 0;
    /// <summary>
    /// 已添加的点
    /// </summary>
    public List<AnimalsHomePoint> imaPointList = new List<AnimalsHomePoint>();
    /// <summary>
    /// 门添加点数量是否OK
    /// </summary>
    public bool bOK = false;
    //左门 / 右门 标识
    private bool bLeft = true;

    private Vector3 vDoor;
    private Image image;
    private Button doorBtn;
    private GridLayoutGroup grid;
    private Image checkBtn;  
    private ParticleSystem parsys0;

    /// <summary>
    /// 门按钮事件激活
    /// </summary>
    public bool bDoorBtnActive = true;

    private AnimalsHomeCtrl mCtrl;
    private AnimalsHomeWindow mWin;

    public void InitAwake(int _count, bool _left, AnimalsHomeWindow _Win)
    {
        nCount = _count;
        mCtrl = SceneMgr.Instance.GetNowScene() as AnimalsHomeCtrl;
        mWin = _Win;
        bLeft = _left;
        vDoor = transform.localPosition;

        image = gameObject.GetComponent<Image>();
        if (bLeft)
        { image.sprite = ResManager.GetSprite("animalshome_sprite", "winyellow0"); }
        else
        { image.sprite = ResManager.GetSprite("animalshome_sprite", "winred0"); }

        doorBtn = gameObject.AddComponent<Button>();
        doorBtn.transition = Selectable.Transition.None;
        EventTriggerListener.Get(gameObject).onClick = ClickDoor;

        GameObject mgo0 = UguiMaker.newGameObject("grid", transform);
        grid = mgo0.AddComponent<GridLayoutGroup>();
        SetGrid(grid);

        CreateCheckBtn();
    }
    private void SetGrid(GridLayoutGroup _grid)
    {
        _grid.transform.localPosition = new Vector3(0f, 8f, 0f);
        _grid.cellSize = new Vector2(12f, 12f);
        _grid.spacing = new Vector2(5f, 8f);
        _grid.childAlignment = TextAnchor.MiddleCenter;
        _grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _grid.constraintCount = 2;
    }
    private void CreateCheckBtn()
    {
        //checkbtn0
        Button btn0 = UguiMaker.newButton("checkBtn", transform, "animalshome_sprite", "checkbtn0");
        btn0.transform.localPosition = new Vector3(15f, -30f, 0f);
        btn0.transform.localScale = Vector3.one * 0.1f;
        EventTriggerListener.Get(btn0.gameObject).onDown = CheckBtnDown;
        EventTriggerListener.Get(btn0.gameObject).onUp = CheckBtnUp;
        checkBtn = btn0.GetComponent<Image>();
        checkBtn.gameObject.SetActive(false);
    }
    //回到起始点
    public void ResetStartPos()
    {
        transform.localPosition = vDoor;
    }

    /// <summary>
    /// 门按钮 开/关
    /// </summary>
    /// <param name="_active"></param>
    public void DoorBtnActive(bool _active)
    {
        bDoorBtnActive = _active;
    }

    public void ResetInfos()
    {
        DoorBtnActive(false);
        checkBtn.gameObject.SetActive(false);
        Common.DestroyChilds(grid.transform);
        imaPointList.Clear();
        SetDoorResetBig();
        bOK = false;
    }



    /// <summary>
    /// 添加圆点
    /// </summary>
    public void AddPoint()
    {
        mCtrl.StopGuideHand();

        if (imaPointList.Count >= 4)
            return;

        Image img = UguiMaker.newImage("point", grid.transform, "animalshome_sprite", "point1");
        AnimalsHomePoint pointCtrl = img.gameObject.AddComponent<AnimalsHomePoint>();
        pointCtrl.InitAwake();
        pointCtrl.SetClickCall(DesPoint);
        imaPointList.Add(pointCtrl);

        checkBtn.gameObject.SetActive(true);
        mCtrl.PlayDesPointTipSound();
        AudioClip cp = ResManager.GetClip("animalshome_sound", "addpoint");
        mCtrl.MSoundCtrl.PlaySortSound(cp);
    }
    /// <summary>
    /// 删除圆点
    /// </summary>
    public void DesPoint(GameObject _img)
    {
        AnimalsHomePoint pointCtrl = _img.GetComponent<AnimalsHomePoint>();
        if (pointCtrl != null)
        {      
            if (imaPointList.Contains(pointCtrl))
            {
                AudioClip cp = ResManager.GetClip("animalshome_sound", "despoint");
                mCtrl.MSoundCtrl.PlaySortSound(cp);
                imaPointList.Remove(pointCtrl);
                GameObject.Destroy(_img.gameObject);
                if (imaPointList.Count <= 0)
                {
                    checkBtn.gameObject.SetActive(false);
                    //SetDoorResetBig();
                }
            }
        }
    }
    /// <summary>
    /// 门变大
    /// </summary>
    public void SetDoorBig()
    {
        if (bLeft)
        { image.sprite = ResManager.GetSprite("animalshome_sprite", "winyellowbig"); }
        else
        { image.sprite = ResManager.GetSprite("animalshome_sprite", "winredbig"); }

        transform.SetSiblingIndex(7);
        transform.DOScale(Vector3.one * 5f, 0.2f);
        transform.DOMove(mCtrl.mTargetZero.transform.position, 0.2f);
        int sibling = transform.GetSiblingIndex();
        mCtrl.imgBlackPanel.transform.SetParent(transform.parent);
        mCtrl.imgBlackPanel.transform.SetSiblingIndex(sibling-1);
        mCtrl.imgBlackPanel.gameObject.SetActive(true);

        mCtrl.SetGuideScale(0.3f);
        mCtrl.SetGuideParent(transform);

        AudioClip cp = ResManager.GetClip("animalshome_sound", "windowout");
        if (cp != null)
        { mCtrl.MSoundCtrl.PlaySortSound(cp,0.5f); }
    }
    /// <summary>
    /// 重置门大小
    /// </summary>
    public void SetDoorResetBig()
    {
        if (bLeft)
        { image.sprite = ResManager.GetSprite("animalshome_sprite", "winyellow0"); }
        else
        { image.sprite = ResManager.GetSprite("animalshome_sprite", "winred0"); }

        bBigState = false;
        if (mWin.nWinOpenState == 2)
            return;
        transform.DOScale(Vector3.one, 0.2f);
        transform.DOLocalMove(vDoor, 0.2f);
        mCtrl.imgBlackPanel.gameObject.SetActive(false);
        //退出删除点
        if (!bOK)
        {
            Common.DestroyChilds(grid.transform);
            imaPointList.Clear();
            checkBtn.gameObject.SetActive(false);
        }

        mCtrl.SetGuideScale(1f);
    }
    /// <summary>
    /// 圆点Btn 开/关
    /// </summary>
    /// <param name="_active"></param>
    public void PointsBtnActive(bool _active)
    {
        for (int i = 0; i < imaPointList.Count; i++)
        {
            imaPointList[i].ButtonActive(false);
        }
    }

    //错误震动
    IEnumerator ieShakeDoor()
    {
        AudioClip cp = ResManager.GetClip("animalshome_sound", "inputerror");
        if (cp != null)
        {
            mCtrl.MSoundCtrl.PlaySortSound(cp);
        }
        GameObject _go = gameObject;
        for (float j = 0; j < 1f; j += 0.05f)
        {
            float p = Mathf.Sin(Mathf.PI * j) * 0.8f;
            _go.transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * 6 * j) * 10);
            yield return new WaitForSeconds(0.01f);
        }
        _go.transform.localEulerAngles = Vector3.zero;
    }

    
    /// <summary>
    /// 播放btnok特效
    /// </summary>
    public void PlayBtnOKEffect()
    {
        if (parsys0 == null)
        {
            parsys0 = ResManager.GetPrefab("effect_okbtn", "okbtn_effect").GetComponent<ParticleSystem>();
            parsys0.transform.SetParent(checkBtn.transform);
            parsys0.transform.localPosition = Vector3.zero;
            parsys0.transform.localScale = Vector3.one;
        }
        parsys0.Play();

        AudioClip cp = ResManager.GetClip("animalshome_sound", "stareffect");
        mCtrl.MSoundCtrl.PlaySortSound(cp);
    }



    /// <summary>
    /// 预设置好点数
    /// </summary>
    public void PreSetPoint(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            Image img = UguiMaker.newImage("point", grid.transform, "animalshome_sprite", "point1");
            AnimalsHomePoint po = img.gameObject.AddComponent<AnimalsHomePoint>();
            imaPointList.Add(po);
        }
    }

    
    public bool bBigState = false;
    /// <summary>
    /// 点击门call
    /// </summary>
    /// <param name="_go"></param>
    public void ClickDoor(GameObject _go)
    {
        if (!bDoorBtnActive)
            return;
        if (mCtrl.bLvPass)
            return;
        if (!mCtrl.bStartClick)
            return;

        if (!bBigState)
        {
            mWin.transform.SetSiblingIndex(30);
            SetDoorBig();
            mCtrl.mNowSelectDoor = this;
            bBigState = true;
        }
        else
        {
            if (mCtrl.mNowSelectDoor != null)
                AddPoint();
        }
    }

    public void CheckBtnShow(bool _show)
    {
        checkBtn.gameObject.SetActive(_show);
    }

    /// <summary>
    /// 按钮按下
    /// </summary>
    /// <param name="_go"></param>
    public void CheckBtnDown(GameObject _go)
    {
        if (mCtrl.bLvPass)
            return;
        if (bOK)
        {
            return;
        }
        Image imge = _go.GetComponent<Image>();
        imge.sprite = ResManager.GetSprite("animalshome_sprite", "checkbtn1");
        if (btnDownCp == null)
            btnDownCp = Resources.Load<AudioClip>("sound/button_down");
        mCtrl.MSoundCtrl.PlaySortSound(btnDownCp);
    }
    private AudioClip btnDownCp;
    /// <summary>
    /// 按钮弹起
    /// </summary>
    /// <param name="_go"></param>
    public void CheckBtnUp(GameObject _go)
    {
        if (mCtrl.bLvPass)
            return;
        if (bOK)
        {
            return;
        }

        if (nCount == imaPointList.Count)
        {
            bOK = true;
            mWin.CheckWindowIsOK();
            DoorBtnActive(false);
            SetDoorResetBig();
            PointsBtnActive(false);
            StartCoroutine(ieShowOKEffect());         
        }
        else
        {
            StartCoroutine(ieShakeDoor());
        }

        if (!bOK)
        {
            //Debug.Log("unok---");
            Image imge = _go.GetComponent<Image>();
            imge.sprite = ResManager.GetSprite("animalshome_sprite", "checkbtn0");
        }
    }

    IEnumerator ieShowOKEffect()
    {
        yield return new WaitForSeconds(0.2f);
        PlayBtnOKEffect();
        yield return new WaitForSeconds(1f);
        checkBtn.gameObject.SetActive(false);
    }

}
