using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaterMove : MonoBehaviour
{

    private Image water0;
    private Image water1;

    public string strAB = "";
    public string strResSprite = "";

    public float fspeed = 50f;

    private bool bInit = false;
    public void InitAwake(float _fwidth,float _fheight, float _apth = 1f)
    {
        water0 = UguiMaker.newImage("water0", transform, strAB, strResSprite, false);
        water1 = UguiMaker.newImage("water1", transform, strAB, strResSprite, false);

        water0.rectTransform.sizeDelta = new Vector2(_fwidth, _fheight);
        water1.rectTransform.sizeDelta = new Vector2(_fwidth, _fheight);

        water0.color = new Color(1f, 1f, 1f, _apth);
        water1.color = new Color(1f, 1f, 1f, _apth);

        fgetX = water0.rectTransform.sizeDelta.x;
        water0.transform.localPosition = new Vector3(-water0.rectTransform.sizeDelta.x, 0f, 0f);

        bInit = true;
    }

    float fgetX = 0f;
	// Update is called once per frame
	void Update ()
    {
        if (!bInit)
            return;

        if (transform.localPosition.x >= fgetX)
        {
            transform.localPosition = new Vector3(0f, transform.localPosition.y, 0f);
        }
        else
        {
            transform.localPosition = transform.localPosition + new Vector3(fspeed * Time.deltaTime, 0f, 0f);
        }
	}
}
