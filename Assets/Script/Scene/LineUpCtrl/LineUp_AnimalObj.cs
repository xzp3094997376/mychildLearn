using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Spine.Unity;

public class LineUp_AnimalObj : MonoBehaviour
{

    public int nAnimalID = 0;
    public string strAnimalName = "";
    public string strAnimalNameCN = "";
    public int nIndexID = 0;


    private Image imgAnimal;
    private Image imgying;
    private BoxCollider2D mbox2D;

    private SkeletonGraphic mspine;

    public void InitAwake(int _animalID)
    {
        nAnimalID = _animalID;
        strAnimalName = MDefine.GetAnimalNameByID_EN(nAnimalID);
        strAnimalNameCN = MDefine.GetAnimalNameByID_CH(nAnimalID);

        imgAnimal = UguiMaker.newImage("imganimal", transform, "lineupctrl_sprite", strAnimalName, false);
        imgAnimal.rectTransform.anchoredPosition = new Vector2(0f, imgAnimal.rectTransform.sizeDelta.y * 0.5f);
        imgAnimal.enabled = false;

        imgying = UguiMaker.newImage("imgying", transform, "lineupctrl_sprite", "xs_ke_03", false);
        imgying.rectTransform.anchoredPosition = new Vector2(0f, 3f);
        imgying.transform.SetSiblingIndex(0);
        if (_animalID == 11)
            imgying.transform.localScale = Vector3.one * 0.63f;

        mbox2D = gameObject.AddComponent<BoxCollider2D>();
        mbox2D.size = new Vector2(125f, imgAnimal.rectTransform.sizeDelta.y);
        mbox2D.offset = imgAnimal.rectTransform.anchoredPosition;

        GameObject mgo = ResManager.GetPrefab("aa_animal_person_prefab", strAnimalName);
        mgo.transform.SetParent(this.transform);
        mgo.transform.localPosition = Vector3.zero;
        mgo.transform.localScale = Vector3.one;
        mspine = mgo.transform.Find("spine").GetComponent<SkeletonGraphic>();

        PlayAnimation("face_idle", true);
    }

    public void BoxActive(bool _active)
    {
        mbox2D.enabled = _active;
    }



    /// <summary>
    /// 播放动画 face_idle/face_sayno/face_sayyes/face_walk
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_loop"></param>
    public void PlayAnimation(string _name,bool _loop)
    {
        StopCoroutine(ResetToIdle());

        aanimation = mspine.AnimationState.Data.SkeletonData.FindAnimation(_name);
        if (aanimation != null)
            mspine.AnimationState.SetAnimation(1, aanimation, _loop);
        
        if (aanimation != null && !_loop)
            StartCoroutine(ResetToIdle());
    }

    Spine.Animation aanimation = null;
    private IEnumerator ResetToIdle()
    {
        float ftime = aanimation.Duration;
        yield return new WaitForSeconds(ftime);
        PlayAnimation("face_idle", true);
    }
}
