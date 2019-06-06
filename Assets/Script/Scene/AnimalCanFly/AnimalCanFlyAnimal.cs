using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Spine.Unity;

public class AnimalCanFlyAnimal : MonoBehaviour {
    public int mdata_animal_id { get; set; }
    public int mdata_sit_index { get; set; }
    Image mMachine0 { get; set; }
    Image mMachine1 { get; set; }
    //Image mSanOne { get; set; }
    public RectTransform mRtran { get; set; }
    BoxCollider mBox { get; set; }
    SkeletonGraphic mAnimal { get; set; }

	public void SetData (int animal_id) {
        if(null == mRtran)
        {
            mMachine0 = UguiMaker.newImage("mMachine0", transform, "animalcanfly_sprite", "machine0", false);
            mMachine1 = UguiMaker.newImage("mMachine1", transform, "animalcanfly_sprite", "machine1", false);
            //mSanOne.rectTransform.pivot = new Vector2(0.5f, 0);
            //mSanOne.rectTransform.anchoredPosition = new Vector2(0, 0);
            mBox = gameObject.AddComponent<BoxCollider>();
            mBox.size = new Vector3(61.9f, 48.4f, 1);
            mBox.center = new Vector3(0, 23, 0);

            mRtran = gameObject.GetComponent<RectTransform>();
        }
        //mSanOne.rectTransform.anchoredPosition = Vector2.zero;
        //mSanOne.rectTransform.localEulerAngles = Vector3.zero;


        if (mdata_animal_id != animal_id)
        {
            if(null != mAnimal)
                Destroy(mAnimal.gameObject);
            GameObject obj = ResManager.GetPrefab("aa_animal_env_prefab", MDefine.GetAnimalNameByID_EN(animal_id));
            AnimalPrefabData data = obj.GetComponent<AnimalPrefabData>();
            RectTransform data_rect = data.GetComponent<RectTransform>();
            data_rect.pivot = new Vector2(0.5f, 0);
            data_rect.anchoredPosition = Vector2.zero;

            float scale = 0.25f;// - (data.m_body_side - 1.4f);
            Vector3 offset = Vector3.zero;
            switch(animal_id)
            {
                case 13:
                    offset = new Vector3(8.03f, 0, 0);
                    break;
            }
            UguiMaker.InitGameObj(obj, transform, animal_id.ToString(), offset, new Vector3(scale, scale, 1));
            mAnimal = obj.transform.Find("spine").GetComponent<SkeletonGraphic>();
            mAnimal.AnimationState.SetAnimation(1, "Idle", true);


        }
        mdata_animal_id = animal_id;
        mAnimal.transform.localEulerAngles = Vector3.zero;


        mMachine0.transform.SetAsLastSibling();
        mMachine1.transform.SetAsLastSibling();
        mMachine0.gameObject.SetActive(false);
        mMachine1.gameObject.SetActive(false);

    }
    public void SetBoxEnable(bool _enable)
    {
        mBox.enabled = _enable;
    }

    //bool playing_shoot = false;
    //bool playing_fly = false;
    //bool playing_fall = false;
    public void Shoot()
    {
        StartCoroutine("TShoot");
        
    }
    IEnumerator TShoot()
    {
        AnimalCanFlyCtl.instance.mSound.PlayShortDefaultAb("科学幻想小说菜单输入");
        mMachine0.gameObject.SetActive(true);
        mMachine1.gameObject.SetActive(true);
        mMachine1.transform.localScale = Vector3.one;
        mMachine0.transform.localScale = Vector3.one;

        mRtran.SetParent(AnimalCanFlyCtl.instance.mRtranSky);

        mdata_sit_index = AnimalCanFlyCtl.instance.mBallRight.GetPushIndex();
        Vector3 pos = AnimalCanFlyCtl.instance.mBallRight.GetSitPos_Parent(mdata_sit_index);
        Vector3 pos0 = new Vector3(pos.x, -200, 0);
        Vector3 pos1 = pos + new Vector3(0, 20, 0);
        //Debug.Log(pos + " " + pos0 + " " + pos1);
        mRtran.anchoredPosition3D = pos0;
        //yield break;
        for (float i = 0; i < 1f; i += 0.02f)
        {
            mMachine1.transform.localEulerAngles += new Vector3(0, 0, 10);
            mRtran.anchoredPosition3D = Vector3.Lerp(pos0, pos1, i);// Mathf.Sin(Mathf.PI * 0.5f * i));
            float dis = AnimalCanFlyCtl.instance.mBallRight.GetSitPos_Parent(mdata_sit_index).y - mRtran.anchoredPosition3D.y;
            //Debug.Log("dis=" + dis);
            if(Mathf.Abs(dis) < 2)
            {
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        AnimalCanFlyCtl.instance.mBallRight.PushAnimal(this, mdata_sit_index);
        for (float i = 0; i < 1f; i += 0.1f)
        {
            mMachine1.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i);
            mMachine0.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i);
            yield return new WaitForSeconds(0.01f);
        }
        mMachine0.gameObject.SetActive(false);
        mMachine1.gameObject.SetActive(false);

        
    }

    
    public void Fall()
    {
        mBox.enabled = false;
        StopAllCoroutines();
        StartCoroutine("TFall");
    }
    IEnumerator TFall()
    {

        AnimalCanFlyCtl.instance.mSound.PlayShortDefaultAb("系统-取消-中-001", 0.4f);
        AnimalCanFlyCtl.instance.mBallRight.PopAnimal(mdata_sit_index);
        mMachine0.gameObject.SetActive(false);
        mMachine1.gameObject.SetActive(false);
        mRtran.SetParent(AnimalCanFlyCtl.instance.mRtranSky);


        
        Vector3 angle = Common.Parse180(mRtran.localEulerAngles);
        Vector2 speed = Vector2.zero;
        while (true)
        {
            mRtran.anchoredPosition += speed;
            speed.y -= 1f;
            mRtran.localEulerAngles += new Vector3(0, 0, 5);
            if (mRtran.anchoredPosition.y < -250)
                break;
            yield return new WaitForSeconds(0.01f);

        }
        mRtran.localEulerAngles = Vector3.zero;
        gameObject.SetActive(false);

    }
    /*
    public void Fly()
    {
        mBox.enabled = false;
        playing_fly = true;
        if (playing_shoot)
        {

        }
        else
        {
            StartCoroutine("TFly");
        }

        StartCoroutine(TFly());
    }
    IEnumerator TFly()
    {
        while(true)
        {
            mRtran.anchoredPosition += new Vector2(0, 5);
            yield return new WaitForSeconds(0.01f);
            if(mRtran.anchoredPosition.y > 420)
            {
                break;
            }
        }
        playing_fly = false;
    }
    */

}
