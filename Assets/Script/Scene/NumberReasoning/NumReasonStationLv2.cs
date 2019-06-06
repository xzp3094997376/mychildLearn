using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class NumReasonStationLv2 : MonoBehaviour
{

    public int nId = 0;
    public int nAnimalID = 0;
    private Image imgbg;
    private Image imgLeft;
    private Image imgRight;

    private Vector3 vstart;

    public SkeletonGraphic spine;
    public NumReasonNumObj mNumLeft;
    public NumReasonNumObj mNumRight;

    public void InitAwake(int _id, int _left,int _right)
    {
        nId = _id;

        imgbg = UguiMaker.newImage("imgbg", transform, "numberreasoning_sprite", "dw_dikuang", false);
        imgLeft = UguiMaker.newImage("imgLeft", transform, "numberreasoning_sprite", "dw_diyuan", false);
        imgRight = UguiMaker.newImage("imgRight", transform, "numberreasoning_sprite", "dw_diyuan", false);

        imgbg.rectTransform.sizeDelta = new Vector2(200f, 120f);

        imgLeft.transform.localPosition = new Vector3(-50f, 0f);
        imgRight.transform.localPosition = new Vector3(50f, 0f);

        mNumLeft = imgLeft.gameObject.AddComponent<NumReasonNumObj>();
        mNumRight = imgRight.gameObject.AddComponent<NumReasonNumObj>();

        mNumLeft.InitAwake(3, _left);
        mNumRight.InitAwake(4, _right);

        transform.localScale = new Vector3(0.001f, 1f, 1f);
        if (nId % 2 == 0)
        {
            transform.localPosition = transform.localPosition - new Vector3(100f, 0f, 0f);
        }
        else
        { transform.localPosition = transform.localPosition + new Vector3(100f, 0f, 0f); }
    }


    public void SetBgColor(Color _color)
    {
        imgbg.color = _color;
    }

    public void SetStartPos(Vector3 _pos)
    {
        vstart = _pos;
    }

    public void SetLostNumObj()
    {
        if (Random.value > 0.5f)
        {
            mNumLeft.MiniNumberActive(false);
            mNumLeft.WenHaoActive(true);
            mNumLeft.Box2DActive(true);
        }
        else
        {
            mNumRight.MiniNumberActive(false);
            mNumRight.WenHaoActive(true);
            mNumRight.Box2DActive(true);
        }
    }

    public void CardShowOut()
    {
        transform.DOScale(Vector3.one, 0.25f);
        transform.DOLocalMove(vstart, 0.25f);
    }


    /// <summary>
    /// 创建动物spine
    /// </summary>
    /// <param name="_animalId"></param>
    /// <param name="_pos"></param>
    public void CreateAnimalSpine(int _animalId, Vector3 _pos, bool _yinying = false)
    {
        nAnimalID = _animalId;
        string strAnimalName = MDefine.GetAnimalNameByID_EN(_animalId);
        GameObject mgo = ResManager.GetPrefab("aa_animal_person_prefab", strAnimalName, transform.parent);
        mgo.transform.localPosition = _pos;
        spine = mgo.transform.Find("spine").GetComponent<SkeletonGraphic>();
        mgo.transform.SetSiblingIndex(1);
        mgo.transform.localScale = Vector3.one * 0.001f;
        mgo.transform.DOScale(Vector3.one, 0.25f);

        PlayAnimation("face_idle", true);

        if (_yinying)
        {
            Image imgAnimalYing = UguiMaker.newImage("ying", mgo.transform, "numberreasoning_sprite", "dw_ying", false);
            imgAnimalYing.transform.localPosition = new Vector3(25f, 45f);
            imgAnimalYing.transform.SetSiblingIndex(0);
        }
    }


    /// <summary>
    /// 播放动画 face_idle/face_sayno/face_sayyes/face_walk
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_loop"></param>
    public void PlayAnimation(string _name, bool _loop)
    {
        StopCoroutine(ResetToIdle());

        spAnimation = spine.AnimationState.Data.SkeletonData.FindAnimation(_name);
        if (spAnimation != null)
            spine.AnimationState.SetAnimation(1, spAnimation, _loop);

        if (spAnimation != null && !_loop)
            StartCoroutine(ResetToIdle());
    }

    Spine.Animation spAnimation = null;
    private IEnumerator ResetToIdle()
    {
        float ftime = spAnimation.Duration;
        yield return new WaitForSeconds(ftime);
        PlayAnimation("face_idle", true);
    }


}
