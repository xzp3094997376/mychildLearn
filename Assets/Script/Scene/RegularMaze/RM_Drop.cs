using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RM_Drop : MonoBehaviour
{
    private Image img;


    private BoxCollider2D mbox2D;
    private Rigidbody2D rig2D;

	public void InitAwake ()
    {
        img = gameObject.AddComponent<Image>();
        img.rectTransform.sizeDelta = Vector2.one * 35f;
        //img.color = Color.black;
        img.sprite = ResManager.GetSprite("regularmaze_sprite", "droppoint");


        mbox2D = gameObject.AddComponent<BoxCollider2D>();
        mbox2D.size = Vector2.one * 6f;
        mbox2D.isTrigger = true;

        rig2D = gameObject.AddComponent<Rigidbody2D>();
        rig2D.isKinematic = true;
    }

    public void Box2DActive(bool _active)
    {
        mbox2D.enabled = _active;
    }

    private System.Action<GameObject> hitCall = null;
    public void SetHitCall(System.Action<GameObject> _hitcall)
    {
        hitCall = _hitcall;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitCall != null)
        {
            hitCall(collision.gameObject);
        }
    }

}
