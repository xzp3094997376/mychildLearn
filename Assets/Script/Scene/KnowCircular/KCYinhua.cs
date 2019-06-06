using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class KCYinhua : MonoBehaviour {


    private Image img;
    private Vector3 vStart;
    KnowCircularCtrl mCtrl;
    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as KnowCircularCtrl;
        img = gameObject.GetComponent<Image>();
        img.color = new Color(1f, 1f, 1f, 0f);
        vStart = transform.localPosition;
    }


    public float fDurTime = 0.2f;
    public float fHide = 0.8f;
    public float fDelayHide = 0.1f;

    public int nCount = 3;
    private int nindex = 0;
    public void PlayYinhua()
    {
        nindex = 0;
        ReDo();
    }
    private void ReDo()
    {
        Vector2 v2 = UnityEngine.Random.insideUnitCircle * 80f;
        transform.localPosition = vStart + new Vector3(v2.x, v2.y, 0f);

        img.color = new Color(1f, 1f, 1f, 1f);
        transform.localScale = Vector3.one * 0.001f;
        transform.DOScale(Vector3.one, fDurTime);
        mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "boom"),0.4f);
        DOTween.To(() => img.color, x => img.color = x, new Color(1f, 1f, 1f, 0f), fHide).SetDelay(fDelayHide).OnComplete(() =>
        {
            nindex++;
            if (nindex < nCount)
            {
                ReDo();
            }
        });
    }

}


public class KCJiaZiLight : MonoBehaviour
{
    public int nIndex = 0;
    private Image[] imgs;
    //ship_cle0
    public void InitAwake(int _count)
    {
        imgs = new Image[_count];

        for (int i = 0; i < imgs.Length; i++)
        {
            imgs[i] = transform.GetChild(i).GetComponent<Image>();
            imgs[i].color = Color.white;
            string strSpName = imgs[i].gameObject.name;
            imgs[i].sprite = ResManager.GetSprite("knowcircular_sprite", strSpName);
        }

        nIndex = UnityEngine.Random.Range(0, 4);

        ToMoveUp();
    }


    private void ToMoveUp()
    {
        StartCoroutine(ieToMoveUp());
    }
    IEnumerator ieToMoveUp()
    {
        yield return new WaitForSeconds(0.2f);
        imgs[nIndex].sprite = ResManager.GetSprite("knowcircular_sprite", imgs[nIndex].gameObject.name);
        nIndex++;
        if (nIndex >= imgs.Length)
        {
            nIndex = 0;
        }
        imgs[nIndex].sprite = ResManager.GetSprite("knowcircular_sprite", "ship_cle0");
        yield return new WaitForSeconds(0.2f);
        ToMoveUp();
    }

}


