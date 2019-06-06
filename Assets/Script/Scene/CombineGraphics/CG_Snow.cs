using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class CG_Snow : MonoBehaviour
{

    private Image img;
    public bool isFarSnow = false;

    public float fRotateSpeed = 10f;
    public int nDir = 1;

    bool binit = false;

    public void InitAwake()
    {
        img = UguiMaker.newImage("imgsnow", transform, "combinegraphics_sprite", "snow", false);
        binit = true;
    }

    void Update()
    {
        if (!binit)
            return;
        img.transform.localEulerAngles += new Vector3(0f, 0f, Time.deltaTime * fRotateSpeed * nDir);
    }

    public void PlaySnow()
    {
        if (UnityEngine.Random.value > 0.5f)
        {
            nDir = 1;
        }
        else
        { nDir = -1; }

        fRotateSpeed = UnityEngine.Random.Range(20f, 50f);

        float fScale = UnityEngine.Random.Range(0.75f, 1.2f);

        float fX = UnityEngine.Random.Range(-450f, 450f);
        gameObject.transform.localPosition = new Vector3(fX, 420f, 0f);
        float fToX = fX + UnityEngine.Random.Range(-250f, 250f);

        float fTime = UnityEngine.Random.Range(13f, 15f);
        if (isFarSnow)
        {
            fTime = fTime * UnityEngine.Random.Range(1.5f, 2.4f);
            img.transform.localScale = Vector3.one * fScale * UnityEngine.Random.Range(0.45f, 0.65f);
        }

        transform.DOLocalMove(new Vector3(fToX, -410f, 0f), fTime).SetEase(Ease.Linear).OnComplete(()=> 
        {
            PlaySnow();
        });
    }
	
}
