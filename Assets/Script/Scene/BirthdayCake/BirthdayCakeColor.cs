using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BirthdayCakeColor : MonoBehaviour {
    public int mColor { get; set; }
    static Image mBi { get; set; }

    Image mImage { get; set; }

    void OnDestroy()
    {
        if(mBi != null)
        {
            Destroy(mBi.gameObject);
        }
        mBi = null;
    }
    public void Init(int color)
    {
        mImage = UguiMaker.newImage("image", transform, "birthdaycake_sprite", "color" + color.ToString());
        mImage.material = BirthdayCakeCtl.instance.mMat;

        if(null == mBi)
        {
            mBi = UguiMaker.newImage("bi", transform, "birthdaycake_sprite", "bi");
            mBi.material = BirthdayCakeCtl.instance.mMat;
            mBi.rectTransform.pivot = new Vector2(0.5f, 0);
            mBi.rectTransform.anchoredPosition = new Vector2(0, 0);
            mBi.transform.localEulerAngles = new Vector3(0, 0, 338);
            mBi.gameObject.AddComponent<CanvasGroup>().blocksRaycasts = false;

            mBi.gameObject.SetActive(false);
        }

        mColor = color;

        Button btn = mImage.gameObject.AddComponent<Button>();
        btn.onClick.AddListener(OnClick);
        btn.transition = Selectable.Transition.None;


    }

    public void Reset()
    {
        StopAllCoroutines();
        temp_shake = false;
        mBi.gameObject.SetActive(false);
    }

    public void Stop()
    {
        mBi.gameObject.SetActive(false);
        temp_shake = false;
    }
    public void OnClick()
    {
        if (temp_shake)
        {
            BirthdayCakeCtl.instance.temp_select_color = 3;
            Stop();
        }
        else if(BirthdayCakeCtl.instance.GetState() == BirthdayCakeCtl.State.select_flower)
        {
            BirthdayCakeCtl.instance.temp_select_color = mColor;

            BirthdayCakeCtl.instance.Callback_SelectColor(this);
            mBi.gameObject.SetActive(true);
            mBi.transform.SetParent(transform);
            mBi.transform.localPosition = Vector3.zero;

            transform.SetAsLastSibling();
            temp_shake = true;
            StartCoroutine("TSake");

            BirthdayCakeCtl.instance.mSound.PlayShort("birthdaycake_sound", "click_color");
            
        }
    }

    bool temp_shake = false;
    IEnumerator TSake()
    {
        Vector3 pos1 = new Vector3(GlobalParam.screen_width * -0.5f + 136 * 1f, transform.localPosition.y, 0);
        while(transform.localPosition.x < pos1.x)
        {
            transform.localPosition += new Vector3(10, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localPosition = pos1;

        float temp = 0;
        float p = 0;
        while (temp_shake)
        {
            //transform.localPosition = pos1 + new Vector3( Mathf.Sin(temp) * 20, 0, 0);
            p = Mathf.Sin(temp);
            mBi.transform.localEulerAngles = new Vector3(0, 0, 338 + p * 5);
            mBi.transform.localPosition = new Vector3(0, 8 + 8 * p, 0);
            temp += 0.1f;
            yield return new WaitForSeconds(0.01f);

        }

        float posx = GlobalParam.screen_width * -0.5f + 136 * 0.5f;
        while (transform.localPosition.x > posx)
        {
            transform.localPosition -= new Vector3(10, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localPosition = new Vector3(posx, transform.localPosition.y, 0);


    }


}
