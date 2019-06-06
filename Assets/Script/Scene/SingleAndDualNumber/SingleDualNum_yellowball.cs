using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class SingleDualNum_yellowball : MonoBehaviour
{

    private Image image;
    private BoxCollider2D box2D;
    private Image imgFlash;

    private bool hitChange = false;

    public RectTransform rectTransform;

    public void InitAwake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        image = gameObject.GetComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 0f);
        box2D = gameObject.GetComponent<BoxCollider2D>();
        if(box2D == null)
            box2D = gameObject.AddComponent<BoxCollider2D>();
        box2D.size = image.rectTransform.sizeDelta;
        box2D.isTrigger = true;

        imgFlash = UguiMaker.newImage("flash", transform, "singledualnum_sprite", "flash" + Random.Range(0, 4), false);
        imgFlash.rectTransform.localScale = Vector3.one * 0.6f;
    }



    /// <summary>
    /// 碰撞体开/关
    /// </summary>
    /// <param name="_active"></param>
    public void SetBoxActive(bool _active)
    {
        box2D.enabled = _active;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (hitChange)
            return;

        if (collision.gameObject.name.CompareTo("imageDrop") == 0)
        {
            hitChange = true;

            AudioClip cp = ResManager.GetClip("singledualnum_sound", "biut");
            AudioSource.PlayClipAtPoint(cp, Camera.main.transform.position);

            transform.DOScale(Vector3.one * 1.5f, 0.2f).OnComplete(() =>
            {
                transform.DOScale(Vector3.one, 0.2f).OnComplete(() =>
                {
                    hitChange = false;
                });
            });
        }
    }

    public void EffectEnter()
    {
        transform.localScale = Vector3.one * 0.001f;
        transform.DOScale(Vector3.one, 0.2f);
    }
    public void EffectOut()
    {
        transform.DOScale(Vector3.one * 0.001f, 0.2f);
    }
}
