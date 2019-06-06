using UnityEngine;
using System.Collections;

public class particleSystest : MonoBehaviour
{
    private RectTransform rectT;
    private ParticleSystem mParticleSystem;

    private Canvas mCanvas;

    private void Awake()
    {
        mCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        GameObject mgo = transform.Find("clickInputEffect").gameObject;
        mParticleSystem = mgo.GetComponentInChildren<ParticleSystem>();
        rectT = mgo.GetComponent<RectTransform>();
        mParticleSystem.Pause();
        mParticleSystem.Stop();
    }

    public void ActiveEffect(bool _active)
    {
        mParticleSystem.Stop();
        mParticleSystem.gameObject.SetActive(_active);
    }

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //mParticleSystem.Pause();
            rectT.anchoredPosition = GetInputPos();
            mParticleSystem.Play();
        }
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    mParticleSystem.Stop();
        //}
    }



    Vector2 GetInputPos()
    {
        Vector2 v2 = Vector2.one;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mCanvas.transform as RectTransform,
            Input.mousePosition, mCanvas.worldCamera, out v2);

        return v2;
    }

}
