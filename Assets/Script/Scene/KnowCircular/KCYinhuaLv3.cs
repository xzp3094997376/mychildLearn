using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class KCYinhuaLv3 : MonoBehaviour
{

    private Image imgLight;
    private Image imgPoint;
    private Image imgBoom;

    private Vector3 vStart;

    KnowCircularCtrl mCtrl;

    public void InitAwake(int _type)
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as KnowCircularCtrl;
        imgLight = transform.Find("imgLight").GetComponent<Image>();
        imgPoint = transform.Find("imgPoint").GetComponent<Image>();
        imgBoom = transform.Find("imgBoom").GetComponent<Image>();

        imgLight.sprite = ResManager.GetSprite("knowcircular_sprite", "imglight");
        imgPoint.sprite = ResManager.GetSprite("knowcircular_sprite", "imgpoint");
        imgBoom.sprite = ResManager.GetSprite("knowcircular_sprite", "imgboom" + _type);

        imgPoint.transform.localScale = Vector3.one * 0.5f;

        vStart = transform.localPosition;

        imgLight.enabled = false;
        imgBoom.enabled = false;
        imgPoint.enabled = false;

        transform.SetSiblingIndex(0);
    }

    public void SetColor(Color _color)
    {
        imgLight.color = _color;
        imgPoint.color = _color;
    }


    public float fHeight = 220f;


    public float fDurTime = 0.17f;
    public float fHide = 0.8f;
    public float fDelayHide = 0.1f;

    public int nCount = 2;
    private int nindex = 0;
    public void PlayYinhua()
    {
        nindex = 0;
        ReDo();
    }
    private void ReDo()
    {
        Vector2 v2 = UnityEngine.Random.insideUnitCircle * 50f;
        transform.localPosition = vStart + new Vector3(v2.x, 0, 0f);

        fHeight = UnityEngine.Random.Range(160f, 240f);

        imgLight.enabled = true;
        imgBoom.enabled = false;
        imgPoint.enabled = true;

        imgPoint.transform.localPosition = Vector3.zero;

        float setY = 0.01f;
        DOTween.To(() => setY, x => setY = x, fHeight, fDurTime).OnUpdate(() =>
        {
            imgLight.rectTransform.sizeDelta = new Vector2(imgLight.rectTransform.sizeDelta.x, setY);
        });
        imgPoint.transform.DOLocalMoveY(fHeight, fDurTime).OnComplete(() =>
        {
            imgLight.enabled = false;
            imgBoom.enabled = true;
            imgPoint.enabled = false;

            imgBoom.color = new Color(1f, 1f, 1f, 1f);
            imgBoom.transform.localPosition = new Vector3(0f, fHeight, 0f);
            imgBoom.transform.localScale = Vector3.one * 0.001f;
            imgBoom.transform.DOScale(Vector3.one, 0.4f);
            mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "boom"), 0.4f);
            DOTween.To(() => imgBoom.color, x => imgBoom.color = x, new Color(1f, 1f, 1f, 0f), fHide).SetDelay(fDelayHide).OnComplete(() =>
            {
                nindex++;
                if (nindex < nCount)
                {
                    ReDo();
                }
            });
        });
    }

}


//public class KCYinhuaTest : MonoBehaviour
//{
//    private Vector3 vStart;
//    KnowCircularCtrl mCtrl;
//    ParticleSystem msys;

//    public void InitAwake()
//    {
//        mCtrl = SceneMgr.Instance.GetNowScene() as KnowCircularCtrl;
//        transform.localScale = Vector3.one * 0.25f;
//        msys = gameObject.GetComponent<ParticleSystem>();
//        vStart = transform.localPosition;
//    }

//    public int nCount = 3;
//    private int nindex = 0;
//    public void PlayYinhua()
//    {
//        nindex = 0;
//        ReDo();
//    }
//    private void ReDo()
//    {
//        Vector2 v2 = UnityEngine.Random.insideUnitCircle * 80f;
//        transform.localPosition = vStart + new Vector3(v2.x, v2.y, 0f);
//        msys.Play();
//        mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "boom"), 0.4f);
//        transform.DOLocalMove(transform.localPosition, 1f).OnComplete(() =>
//        {
//            nindex++;
//            if (nindex < nCount)
//            {
//                ReDo();
//            }
//        });
//    }
//}
