using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PickPeachTreeCell : MonoBehaviour {
    public static PickPeachTreeCell gSelect = null;

    public RectTransform mRtran { get; set; }
    public Vector3 mResetAngle = Vector3.zero;
    public List<PickPeachTao> mTaos = new List<PickPeachTao>();

	// Use this for initialization
	void Start ()
    {
        mRtran = gameObject.GetComponent<RectTransform>();
        mResetAngle = mRtran.localEulerAngles;

    }

    //int count = 0;
    public void Shake()
    {
        //count++;
        StopCoroutine("TShake");
        StartCoroutine("TShake");
    }
    IEnumerator TShake()
    {
        PickPeachCtl.instance.mSound.PlayShortDefaultAb("摇晃1");
        //if(count > 1)
        //{
        //    if(mTaos.Count > 0)
        //    {
        //        int index = Random.Range(0, 1000) % mTaos.Count;
        //        PickPeachTao tao = mTaos[index];
        //        mTaos.Remove(tao);
        //        tao.FallDown();
        //        PickPeachCtl.instance.mSound.PlayShort("pickpeach_sound", "掉下来", 0.4f);
        //    }
        //    count = 0;
        //    //count--;
        //}

        if (mTaos.Count > 0)
        {
            int index = Random.Range(0, 1000) % mTaos.Count;
            PickPeachTao tao = mTaos[index];
            mTaos.Remove(tao);
            tao.FallDown();
            PickPeachCtl.instance.mSound.PlayShort("pickpeach_sound", "掉下来", 0.4f);
        }

        for (float i = 0; i < 1f; i += 0.15f)
        {
            mRtran.localEulerAngles = mResetAngle + new Vector3(0, 0, Mathf.Sin(Mathf.PI * 2 * i) * 8);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.localEulerAngles = mResetAngle;
        
        //if(count > 0)
        //    count--;
    }

}
