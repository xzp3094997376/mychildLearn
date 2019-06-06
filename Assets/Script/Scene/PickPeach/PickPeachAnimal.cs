using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Spine.Unity;

public class PickPeachAnimal : MonoBehaviour {
    public int mdata_index = 0;
    public int mdata_id = 0;
    public Vector2 mdata_pos_end = Vector3.zero;
    public Vector2 mdata_pos_beg = Vector3.zero;

    public Image mYun { get; set; }
    public SkeletonGraphic mSpine { get; set; }
    public PickPeachLanzi mLanzi { get; set; }
    public RectTransform mRtran { get; set; }


    public void Init(int id, int index)
    {
        mdata_id = id;
        mdata_index = index;

        mRtran = gameObject.GetComponent<RectTransform>();

        mYun = UguiMaker.newImage("mYun", transform, "pickpeach_sprite", "yun5", false);
        mSpine = ResManager.GetPrefab("aa_animal_person_prefab", MDefine.GetAnimalNameByID_EN(id)).transform.Find("spine").GetComponent<SkeletonGraphic>();
        UguiMaker.InitGameObj(mSpine.transform.parent.gameObject, transform, "spine", new Vector3(0, 0, 0), Vector3.one);
        mSpine.AnimationState.SetAnimation(1, "face_idle", true);

        PushLanzi();
        mLanzi.mRtran.anchoredPosition = new Vector2(0, 0);

        mYun.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        mLanzi.mRtran.anchoredPosition = new Vector2(40.8f, 73.2f);
        mSpine.transform.parent.localPosition = new Vector3(-31.9f, 12.1f, 0);
        mSpine.transform.parent.localScale = new Vector3(1.2f, 1.2f, 1);

        Vector2[] poss0 = new Vector2[] { new Vector2(-890, -205), new Vector2(841, -273), new Vector2(-586, -653) };
        Vector2[] poss1 = new Vector2[] { new Vector2(-380, -210), new Vector2(367, -210), new Vector2(4, -210)};
        mdata_pos_end = poss1[mdata_index];
        mdata_pos_beg = poss0[mdata_index];

    }
    public void PlaySayNo()
    {
        mSpine.AnimationState.ClearTracks();
        mSpine.AnimationState.AddAnimation(0, "face_sayno", false, 0);
        //mSpine.AnimationState.AddAnimation(0, "face_sayno", false, 0);
        mSpine.AnimationState.AddAnimation(0, "face_idle", false, 0);
    }
    public void PlaySayYes()
    {
        mSpine.AnimationState.ClearTracks();
        mSpine.AnimationState.AddAnimation(0, "face_sayyes", false, 0);
        mSpine.AnimationState.AddAnimation(0, "face_sayyes", false, 0);
        mSpine.AnimationState.AddAnimation(0, "face_idle", false, 0);
        PickPeachCtl.instance.mSound.PlayShort("aa_animal_sound", MDefine.GetAnimalNameByID_CH(mdata_id) + "0");

    }

    public void PushLanzi()
    {
        if (null != mLanzi)
            return;
        mLanzi = PickPeachCtl.instance.mLanzi[mdata_index];
        mLanzi.transform.SetParent(transform);
        mLanzi.transform.SetAsLastSibling();

    }
    public void PopLanzi()
    {
        if (null == mLanzi)
            return;
        mLanzi.transform.SetParent(PickPeachCtl.instance.transform);
        mLanzi = null;

    }

    public void MoveIn()
    {
        StartCoroutine(TMoveIn());
    }
    IEnumerator TMoveIn()
    {
        for(float i = 0; i < 1f; i += 0.008f)
        {
            mRtran.anchoredPosition = Vector2.Lerp(mdata_pos_beg, mdata_pos_end, Mathf.Sin(Mathf.PI * 0.5f * i)) + new Vector2(0, Mathf.Sin(Mathf.PI * 12 * i) * 5);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition = mdata_pos_end;
        float p = 0;
        float speed = Random.Range(0.05f, 0.1f);
        while(true)
        {
            mRtran.anchoredPosition = mdata_pos_end + new Vector2(0, 10 * Mathf.Sin(p));
            p += speed;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void MoveOut()
    {
        StopAllCoroutines();
        StartCoroutine(TMoveOut());
    }
    IEnumerator TMoveOut()
    {
        for (float i = 0; i < 1f; i += 0.008f)
        {
            mRtran.anchoredPosition = Vector2.Lerp(mdata_pos_end, mdata_pos_beg, Mathf.Sin(Mathf.PI * 0.5f * i)) + new Vector2(0, Mathf.Sin(Mathf.PI * 12 * i) * 5);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition = mdata_pos_beg;

    }

}
