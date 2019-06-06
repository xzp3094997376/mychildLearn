using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BirthdayCakeFlower : MonoBehaviour {
    static Image mBi { get; set; }
    Image image { get; set; }

    //public Button mBtn { get; set; }
    public BoxCollider mBox { get; set; }
    public int mColor { get; set; }

	// Use this for initialization
	void Start ()
    {
        mColor = 3;
        image = UguiMaker.newImage("image", transform, "birthdaycake_sprite", "flower3");
        image.material = BirthdayCakeCtl.instance.mMat;
        mBox = gameObject.AddComponent<BoxCollider>();
        mBox.size = new Vector3(60.83f, 53.1f, 1);

        //mBtn = image.gameObject.AddComponent<Button>();
        //mBtn.onClick.AddListener(OnClick);

    }
    void OnDestroy()
    {
        if (mBi != null)
        {
            Destroy(mBi.gameObject);
        }
        mBi = null;
    }

    public void Reset()
    {
        mColor = 3;
        image.sprite = ResManager.GetSprite("birthdaycake_sprite", "flower3");
        mBox.enabled = true;
        //mBtn.enabled = true;
    }
	
    public void OnClick()
    {
        if( 0 != BirthdayCakeCtl.instance.temp_select_color &&
            1 != BirthdayCakeCtl.instance.temp_select_color &&
            2 != BirthdayCakeCtl.instance.temp_select_color)
        {
            return;
        }
        if (mColor == BirthdayCakeCtl.instance.temp_select_color)
            return;

        mColor = BirthdayCakeCtl.instance.temp_select_color;
        image.sprite = ResManager.GetSprite("birthdaycake_sprite", "flower" + BirthdayCakeCtl.instance.temp_select_color);
        transform.SetAsLastSibling();

        //BirthdayCakeCtl.instance.Callback_ClickFlower();
        StartCoroutine("TEffect");

        BirthdayCakeCtl.instance.mSound.PlayShort("birthdaycake_sound", "click_flower");

    }
    IEnumerator TEffect()
    {
        //mBi.gameObject.SetActive(true);
        Vector3 pos0 = transform.localPosition + new Vector3(0, 8, 0);
        Vector3 pos1 = transform.localPosition;
        for (float i = 0; i < 1f; i += 0.1f)
        {
            transform.localPosition = Vector3.Lerp(pos0, pos1, i);
            //mBi.transform.localEulerAngles = Vector3.Lerp( new Vector3(0, 0, 320), new Vector3(0, 0, 370), i);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localPosition = pos1;
        //mBi.gameObject.SetActive(false);
    }

}


