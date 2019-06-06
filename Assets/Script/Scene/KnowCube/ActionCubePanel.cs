using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class ActionCubePanel : MonoBehaviour
{

    public bool bOpen = false;
    private Image img;
    public Vector3 vRotate = Vector3.zero;

    private ActionCube mActioncube;

    private BoxCollider2D box2d;

    private Transform blackline;
    private Image[] mImaLines = null;

    public void InitAwake(ActionCube _acCube)
    {
        mActioncube = _acCube;
        img = gameObject.GetComponent<Image>();
        img.sprite = ResManager.GetSprite("knowcube_sprite", "square");
        box2d = gameObject.AddComponent<BoxCollider2D>();
        box2d.size = img.rectTransform.sizeDelta;
        box2d.offset = img.rectTransform.anchoredPosition;

        blackline = transform.Find("blackline");
        if (blackline != null)
        {
            mImaLines = blackline.GetComponentsInChildren<Image>();
            for (int i = 0; i < mImaLines.Length; i++)
            {
                mImaLines[i].sprite = ResManager.GetSprite("public", "line");
                mImaLines[i].rectTransform.sizeDelta = new Vector2(205f, 15f);
            }
        }
    }


    public void OpenPanel()
    {
        mActioncube.bOpenState = true;
        StartCoroutine(IEOpen());
    }
    IEnumerator IEOpen()
    {
        for (float j = 0; j < 1f; j += 0.05f)
        {
            float p = Mathf.Sin(Mathf.PI * j) * 0.5f;
            transform.localEulerAngles = Vector3.Lerp(vRotate, Vector3.zero, j);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localEulerAngles = Vector3.zero;
        mActioncube.bOpenState = false;

        mActioncube.DoCallBack();
    }


    public void ClosePanel()
    {
        mActioncube.bOpenState = true;
        StartCoroutine(IEClose());
    }
    IEnumerator IEClose()
    {
        for (float j = 0; j < 1f; j += 0.05f)
        {
            float p = Mathf.Sin(Mathf.PI * j) * 0.5f;
            transform.localEulerAngles = Vector3.Lerp(Vector3.zero, vRotate, j);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localEulerAngles = vRotate;
        mActioncube.bOpenState = false;
    }

}
