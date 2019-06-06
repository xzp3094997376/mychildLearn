using UnityEngine;
using System.Collections;

public class PickPeachSelectOption : MonoBehaviour
{
    public static PickPeachSelectOption gSelect { get; set; }


    public RectTransform mRtran { get; set; }
    //public Vector3 mResetPos { get; set; }

    BoxCollider mBox { get; set; }
    ParticleSystem mEffect { get; set; }

    //public int mdata_num {get;set;}


	void Start () {
        mRtran = gameObject.GetComponent<RectTransform>();
        mBox = gameObject.AddComponent<BoxCollider>();
        mBox.size = new Vector3(160, 126, 1);

        mEffect = ResManager.GetPrefab("effect_green_boom", "effect_green_boom0").GetComponent<ParticleSystem>();
        UguiMaker.InitGameObj(mEffect.gameObject, transform, "mEffect", Vector3.zero, Vector3.one);

	}
	//public void Init(int num)
 //   {
 //       mdata_num = num;
 //   }
    public void SetBoxEnable(bool _enable)
    {
        mBox.enabled = _enable;
    }
    public bool GetBoxEnable()
    {
        return mBox.enabled;
    }
    public void PlayEffect()
    {
        mEffect.Play();
    }

    public void Show()
    {
        //mRtran.anchoredPosition3D = mResetPos;
        StartCoroutine(TShow());
    }
    IEnumerator TShow()
    {
        float p = 0;
        Vector3 scale = new Vector3(0.4f, 0.4f, 1);
        for (float i = 0; i < 1f; i += 0.07f)
        {
            p = Mathf.Sin(Mathf.PI * 2 * i) * 0.25f;
            mRtran.localScale = Vector3.Lerp(Vector3.zero, scale, i) + new Vector3(p, p, 0);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.localScale = scale;
    }


}
