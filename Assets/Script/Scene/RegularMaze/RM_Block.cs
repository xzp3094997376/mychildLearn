using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class RM_Block : MonoBehaviour
{
    public int nObjID = 0;
    /// <summary>
    /// 顺序id(-1表示路周围填充)
    /// </summary>
    public int nOrderID = 0;
    /// <summary>
    /// passID(0表示可以通过, 1表示不能通过)
    /// </summary>
    public int nPassID = 0;
    /// <summary>
    /// 位置x
    /// </summary>
    public int nPosX = 0;
    /// <summary>
    /// 位置y
    /// </summary>
    public int nPosY = 0;

    private Image imgParent;
    private Image imgbg;
    private GameObject mObj;
    private Button mBtn;
    private BoxCollider2D mbox2D;

    private RegularMazeCtrl mCtrl;

    float fScale = 0.95f;

    /// <summary>
    /// 背景资源名
    /// </summary>
    private string strResName
    {
        get
        {
            if (mCtrl.nLevel == 2)
            { return "biankuang"; }
            else
            { return "rect0"; }
        }
    }
    //初始化
    public void InitAwake(int _posX,int _posY)
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as RegularMazeCtrl;

        nPosX = _posX;
        nPosY = _posY;

        imgParent = gameObject.AddComponent<Image>();
        imgParent.color = new Color(1f, 1f, 1f, 0f);
        imgParent.rectTransform.sizeDelta = Vector2.one * 78f;

        imgbg = UguiMaker.newImage("imgbg", transform, "regularmaze_sprite", strResName, false);

        if (_posX <= 1 && _posY >= 7)
        { nPassID = 1; }
        if (_posX >= 7 && _posY <= 1)
        { nPassID = 1; }
        if (nPassID == 1)
        {
            gameObject.SetActive(false);
        }

        mBtn = gameObject.AddComponent<Button>();
        mBtn.transition = Selectable.Transition.None;
        mBtn.onClick.AddListener(ClickCheck);

        mbox2D = gameObject.AddComponent<BoxCollider2D>();
        mbox2D.isTrigger = true;
        mbox2D.size = Vector2.one * 66f;//imgbg.rectTransform.sizeDelta;

        transform.localScale = Vector3.one * fScale;
    }
    /// <summary>
    /// 设置背景sprite
    /// </summary>
    /// <param name="_strName"></param>
    public void SetBGSprite(string _strName)
    {
        imgbg.sprite = ResManager.GetSprite("regularmaze_sprite", _strName);
        imgbg.SetNativeSize();
        transform.localScale = Vector3.one * fScale;
    }
    /// <summary>
    /// 重置信息
    /// </summary>
    public void ResetInfos()
    {
        transform.gameObject.SetActive(true);
        transform.localScale = Vector3.one * fScale;

        nPassID = 0;
        nOrderID = 0;

        if (mObj != null)
        {
            GameObject.Destroy(mObj);
            mObj = null;
        }

        if (nPosX <= 1 && nPosY >= 7)
        { nPassID = 1; }
        if (nPosX >= 7 && nPosY <= 1)
        { nPassID = 1; }
        if (nPassID == 1)
        {
            gameObject.SetActive(false);
        }

        if (nPosX == 6 && nPosY == 2)
        {
            nPassID = 1;
        }

        SetColor(Color.white);

        ButtonActive(true);
        BGImageActive(true);
        SetBGSprite(strResName);
        BoxActive(true);
        //关卡2隐藏bg rect
        if (mCtrl.nLevel == 2)
        {
            BGImageActive(false);
        }      
    }
    /// <summary>
    /// 设置位置
    /// </summary>
    /// <param name="_vlocalpos"></param>
    public void SetPos(Vector3 _vlocalpos)
    {
        transform.localPosition = _vlocalpos;
    }
    /// <summary>
    /// 背景颜色设置
    /// </summary>
    /// <param name="_color"></param>
    public void SetColor(Color _color)
    {
        //imgbg.color = _color;
    }
    /// <summary>
    /// 按钮
    /// </summary>
    /// <param name="_active"></param>
    public void ButtonActive(bool _active)
    {
        imgParent.raycastTarget = _active;
    }
    /// <summary>
    /// 背景显示/隐藏
    /// </summary>
    /// <param name="_active"></param>
    public void BGImageActive(bool _active)
    {
        imgbg.enabled = _active;
    }

    public void BoxActive(bool _active)
    {
        mbox2D.enabled = _active;
    }

    //btn点击call
    public void ClickCheck()
    {
        #region//改成了连线
        //if (mCtrl.bLvPass)
        //    return;
        //transform.SetSiblingIndex(150);
        //ButtonActive(false);
        //SetBGSprite("rect1");
        ////关卡2的obj颜色设置灰色
        //if (mCtrl.nLevel == 2)
        //{
        //    SetObjGray(true);           
        //}
        //bool bok = CheckPathBlock();
        //if (bok)
        //{
        //    //Debug.Log("ok path block---");
        //    mCtrl.LevelCheckNext();
        //    BGImageActive(true);
        //}
        //else
        //{
        //    //Debug.Log("umm~ ---");
        //    PlayAnimation("Click");
        //}
        //transform.DOScale(Vector3.one * fScale, 0.2f).OnComplete(() =>
        // {
        //     transform.DOScale(Vector3.one * fScale, 0.2f).OnComplete(()=> 
        //     {
        //         if (!bok)
        //         {
        //             SetBGSprite(strResName);
        //             ButtonActive(true);
        //             SetObjGray(false);
        //         }
        //     });
        // });
        #endregion
    }
    /// <summary>
    /// 检测是否匹配
    /// </summary>
    /// <returns></returns>
    public bool CheckPathBlock()
    {
        int nindex = mCtrl.nCount;
        if (nindex < mCtrl.roadList.Count)
        {
            if (mCtrl.roadList[nindex] == this)
            {
                return true;
            }
        }
        return false;
    }



    /// <summary>
    /// 连线ok set block bg
    /// </summary>
    public void SetSucBG()
    {
        if (theTween != null)
            theTween.Pause();
        if (mCtrl.nLevel != 2)
        { SetBGSprite("msuc"); }
        else
        {
            imgbg.enabled = true;
            SetBGSprite("biankuang");
            imgbg.rectTransform.sizeDelta = Vector2.one * 85.4f;
        }
    }
    Tween theTween = null;
    /// <summary>
    /// 连线failed set block bg
    /// </summary>
    public void SetFaileBG()
    {
        if (theTween != null)
        {
            if (theTween.IsPlaying())
                return;
        }
        if (mCtrl.nLevel != 2)
        { SetBGSprite("mfaile"); }
        else
        {
            imgbg.enabled = false;
            SetObjGray(true, 0.5f);
        }
        theTween = transform.DOScale(Vector3.one * fScale, 0.2f).OnComplete(() =>
        {
            if (mCtrl.nLevel != 2)
            { SetBGSprite(strResName); }
            else
            {
                SetObjGray(false, 0.5f);
            }
        });
    }
    


    /// <summary>
    /// 创建Obj
    /// </summary>
    /// <param name="_orderID"></param>
    /// <param name="_strObjName"></param>
    public void CreateObj(int _orderID,string _strObjName)
    {
        nOrderID = _orderID;
        nObjID = mCtrl.GetObjIDByOrderID(nOrderID);
        if (mObj != null)
        {
            StopCoroutine(ResetToIdle());
            GameObject.Destroy(mObj);
            mObj = null;
            mAnimalSpine = null;
        }
        mObj = UguiMaker.newGameObject("mobj", transform);
        if (mCtrl.nLevel > 1)
        {
            imgobj = UguiMaker.newImage("imgobj", mObj.transform, "regularmaze_sprite", _strObjName + nObjID, false);
        }
        else
        {
            mAnimalSpine = ResManager.GetPrefab("regularmaze_prefab", _strObjName + nObjID, mObj.transform).GetComponent<SkeletonGraphic>();
            mAnimalSpine.transform.localPosition = new Vector3(1f, -37f, 0f);
            mAnimalSpine.transform.localScale = Vector3.one * 0.24f;
            mAnimalSpine.timeScale = 0;
        }        
    }
    /// <summary>
    /// 路的周边填充创建obj
    /// </summary>
    public void CreateOnlyObj(string _strObjName)
    {
        int objid = 0;
               
        if (mCtrl.nLevel == 2)
        {
            if (nOrderID == -1)//第2关(order:1,1,2),还剩 3,4
            {
                objid = Random.Range(3, 5);
                CreateObj(objid, _strObjName);
            }
            else if (nOrderID == 0)
            {
                objid = Random.Range(1, 5);
                CreateObj(objid, _strObjName);
            }
        }
        else if (mCtrl.nLevel == 3)
        {
            if (nOrderID == -1)//第3关(order:1,1,2,2),还剩 3,4,5  
            {
                objid = Random.Range(3, 6);
                CreateObj(objid, _strObjName);
            }
            else if (nOrderID == 0)
            {
                objid = Random.Range(1, 6);
                CreateObj(objid, _strObjName);
            }
        }

        
    }


    /// <summary>
    /// 显示发光
    /// </summary>
    public void ShowLine()
    { }

    bool bisHide = true;
    /// <summary>
    /// 晃动消失
    /// </summary>
    public void HideByShake(bool _isHide = true)
    {
        bisHide = _isHide;
        //blockshake
        mCtrl.SoundCtrl.PlaySortSound("regularmaze_sound", "blockshake", 0.3f);
        StartCoroutine(TScale());
        if (bisHide)
        {
            transform.DOScale(Vector3.one * fScale * 1.2f, 0.25f).OnComplete(() =>
             {
                 transform.DOScale(Vector3.one * 0.001f, 0.7f);
             });
        }
    }
    IEnumerator TScale()
    {
        for (float j = 0; j < 1f; j += 0.05f)
        {
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * 6 * j) * 10);
            yield return new WaitForSeconds(0.01f);
        }             
        transform.localEulerAngles = Vector3.zero;
        if (bisHide)
            gameObject.SetActive(false);
    }



    #region//obj set
    Image imgobj = null;
    public void SetObjGray(bool _gray, float _value)
    {
        if (imgobj != null)
        {
            if (_gray)
                imgobj.color = new Color(_value, _value, _value, 1f);
            else
                imgobj.color = Color.white;
        }
    }
    private SkeletonGraphic mAnimalSpine = null;
    /// <summary>
    /// 播放动画 Idle/Click
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_loop"></param>
    public void PlayAnimation(string _name)
    {
        if (bplayClick)
            return;
        if (mAnimalSpine != null)
        {
            bplayClick = true;
            mAnimalSpine.timeScale = 1;
            aanimation = mAnimalSpine.AnimationState.Data.SkeletonData.FindAnimation(_name);
            if (aanimation != null)
                mAnimalSpine.AnimationState.SetAnimation(1, aanimation, true);
            if (aanimation != null)
                StartCoroutine(ResetToIdle());
        }
    }
    bool bplayClick = false;
    Spine.Animation aanimation = null;
    private IEnumerator ResetToIdle()
    {
        if (mAnimalSpine != null)
        {
            yield return new WaitForSeconds(1.5f);
            mAnimalSpine.AnimationState.SetAnimation(1, "Idle", true);
            yield return new WaitForSeconds(0.2f);
            bplayClick = false;
            mAnimalSpine.timeScale = 0;
        }
    }
    #endregion

}



