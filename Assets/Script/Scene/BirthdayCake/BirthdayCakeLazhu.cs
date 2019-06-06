using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Spine.Unity;

public class BirthdayCakeLazhu : MonoBehaviour {
    public static BirthdayCakeLazhu gSelect { get; set; }
    Image image { get; set; }
    //public Button mBtn { get; set; }
    public int mColor { get; set; }
    public bool mIsFire { get; set; }
    SkeletonGraphic mSpineFire { get; set; }
    ParticleSystem mEffect { get; set; }

    // Use this for initialization
    void Start()
    {
        mColor = 3;
        image = UguiMaker.newImage("image", transform, "birthdaycake_sprite", "flower3");
        image.material = BirthdayCakeCtl.instance.mMat;
        image.rectTransform.pivot = new Vector2(0.5f, 0);
        image.gameObject.SetActive(false);

        mEffect = ResManager.GetPrefab("birthdaycake_prefab", "lazhu_tip").GetComponent<ParticleSystem>();
        UguiMaker.InitGameObj(mEffect.gameObject, transform, "lazhu_tip", new Vector3(0, 32, 0), Vector3.one);
        mEffect.gameObject.SetActive(false);

        //mBtn = image.gameObject.AddComponent<Button>();
        //mBtn.onClick.AddListener(OnClick);

    }
    void OnDestroy()
    {
    }

    public void Reset()
    {
        mColor = 3;
        image.gameObject.SetActive(false);
        ShowFire(false);
        ShowEffect(false);
    }
    public void ShowFire(bool is_show)
    {
        if(is_show)
        {
            BirthdayCakeCtl.instance.mSound.PlayShort("birthdaycake_sound", "lazhu_light");
        }
        else if(is_show != mIsFire)
        {
            BirthdayCakeCtl.instance.mSound.PlayShort("birthdaycake_sound", "lazhu_unlight");
        }
        if(null != mSpineFire)
        {
            mIsFire = is_show;
            mSpineFire.gameObject.SetActive(is_show);
        }
    }
    public void ShowEffect(bool is_show)
    {
        mEffect.gameObject.SetActive(is_show);
    }
    public void PushInLazhu(int color)
    {
        ShowEffect(false);
        BirthdayCakeCtl.instance.mSound.PlayShort("birthdaycake_sound", "lazhu_pushin");

        mColor = color;
        image.gameObject.SetActive(true);
        image.sprite = ResManager.GetSprite( "birthdaycake_sprite", "lazhu" + color.ToString());
        image.SetNativeSize();
        StartCoroutine(TPushInLazhu());

        if(null == mSpineFire)
        {
            GameObject obj = ResManager.GetPrefab("birthdaycake_prefab", "lazhu_fire");
            UguiMaker.InitGameObj(obj, image.transform, "fire", new Vector3(1.1f, 11.8f, 0), Vector3.one);
            mSpineFire = obj.GetComponent<SkeletonGraphic>();
            obj.SetActive(false);
        }


    }
    public void PushOutLazhu()
    {
        mColor = 3;
        StartCoroutine("TPushOutLazhu");
    }

    public void OnClick()
    {

    }

    IEnumerator TPushInLazhu()
    {
        Vector3 pos0 = new Vector3(0, 10, 0);
        Vector3 pos1 = new Vector3(0, -8.5f, 0);
        for(float i  =0; i < 1f; i += 0.1f)
        {
            image.transform.localPosition = Vector3.Lerp(pos0, pos1, i);
            yield return new WaitForSeconds(0.01f);
        }
        image.transform.localPosition = pos1;

    }
    IEnumerator TPushOutLazhu()
    {
        ShowFire(false);
        Vector3 pos0 = new Vector3(0, 10, 0);
        Vector3 pos1 = new Vector3(0, -8.5f, 0);
        for (float i = 0; i < 1f; i += 0.2f)
        {
            image.transform.localPosition = Vector3.Lerp(pos1, pos0, i);
            yield return new WaitForSeconds(0.01f);
        }
        image.transform.localPosition = pos0;
        image.gameObject.SetActive(false);
    }

}
