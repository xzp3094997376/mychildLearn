using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Spine.Unity;

public class NumReasonAnimal : MonoBehaviour
{

    public int nAnimalID = 0;

    private SkeletonGraphic spine;

    public void InitAwake(int _animalid, Vector3 _pos, bool _ying = false)
    {
        nAnimalID = _animalid;
        CreateAnimalSpine(nAnimalID, _pos, _ying);
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



    private void CreateAnimalSpine(int _animalId, Vector3 _pos, bool _yinying = false)
    {
        nAnimalID = _animalId;
        string strAnimalName = MDefine.GetAnimalNameByID_EN(_animalId);
        GameObject mgo = ResManager.GetPrefab("aa_animal_person_prefab", strAnimalName, transform);
        mgo.transform.localPosition = _pos;
        spine = mgo.transform.Find("spine").GetComponent<SkeletonGraphic>();
        PlayAnimation("face_idle", true);
        if (_yinying)
        {
            Image imgAnimalYing = UguiMaker.newImage("ying", mgo.transform, "numberreasoning_sprite", "dw_ying", false);
            imgAnimalYing.transform.localPosition = new Vector3(25f, 45f);
            imgAnimalYing.transform.SetSiblingIndex(0);
        }
    }


}
