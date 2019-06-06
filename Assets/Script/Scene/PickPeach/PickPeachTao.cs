using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PickPeachTao : MonoBehaviour {
    public static PickPeachTao gSelect { get; set; }

    public RectTransform mRtran { get; set; }
    BoxCollider mBox { get; set; }
    Image mTao { get; set; }
    //Vector3 temp_end_ground_pos = Vector3.zero;
    Vector3 temp_end_ground_scale = Vector3.one;

	void Start ()
    {
        mRtran = gameObject.GetComponent<RectTransform>();
        mBox = gameObject.AddComponent<BoxCollider>();
        mBox.size = new Vector3(73, 76, 1);
        mBox.enabled = false;
        mTao = gameObject.GetComponent<Image>();

    }
	
    public void SetBoxEnable(bool _enable)
    {
        mBox.enabled = _enable;
    }
    public void Select()
    {
        PickPeachCtl.instance.mSound.PlayShort("素材出现通用音效");
        mRtran.SetAsLastSibling();
        mTao.sprite = ResManager.GetSprite("pickpeach_sprite", "tao");
        mTao.SetNativeSize();

    }

    public void FallDown()
    {
        //Debug.Log("fallDown()");
        mRtran.SetParent(PickPeachCtl.instance.mScene.transform);
        transform.SetAsLastSibling();
        StopCoroutine("TFallDown");
        StartCoroutine("TFallDown");
    }
    IEnumerator TFallDown()
    {
        PickPeachCtl.instance.mSound.PlayShort("素材出去通用");
        mBox.enabled = false;
        float speed = 0;
        while (true)
        {
            mRtran.anchoredPosition -= new Vector2(0, speed);
            mRtran.localEulerAngles = new Vector3(0, 0, mRtran.localEulerAngles.z * 0.9f);
            speed += 1f;

            if (mRtran.anchoredPosition.y < -310)
                break;

            yield return new WaitForSeconds(0.01f);

        }

        mBox.enabled = true;
        mTao.sprite = ResManager.GetSprite("pickpeach_sprite", "tao1");
        mTao.SetNativeSize();
        PickPeachCtl.instance.mGuanka1.PushInGround_Taozi(this);

        //temp_end_ground_pos = mRtran.anchoredPosition3D;
        temp_end_ground_scale = mRtran.localScale;

    }

    public void OutLanzi()
    {
        StartCoroutine("TOutLanzi");
    }
    IEnumerator TOutLanzi()
    {
        mBox.enabled = false;
        transform.SetParent(PickPeachCtl.instance.mScene.transform);
        transform.localScale = temp_end_ground_scale;
        PickPeachCtl.instance.mGuanka1.PushInGround_Taozi(this);
        Vector3 pos0 = mRtran.anchoredPosition3D;
        Vector3 pos1 = pos0 + new Vector3(Random.Range(-250, 250), -170, 0);
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(pos0, pos1, i) + new Vector3(0, Mathf.Sin(Mathf.PI * i) * 50, 0);
            yield return new WaitForSeconds(0.01f);
        }

        mBox.enabled = true;
        mTao.sprite = ResManager.GetSprite("pickpeach_sprite", "tao1");
        mTao.SetNativeSize();

    }

}
