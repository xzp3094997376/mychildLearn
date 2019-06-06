using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CG_BlockStation : MonoBehaviour
{
    public string strName = "";
    public int nIndex = 0;
    public int nID = 0;

    private Image imgBlock;
    private BoxCollider2D box2D;

    public void InitAwake(string _name, int _index, int _id)
    {
        strName = _name;
        nIndex = _index;
        nID = _id;
        imgBlock = gameObject.GetComponent<Image>();
        box2D = gameObject.GetComponent<BoxCollider2D>();
        if (box2D == null)
        { box2D = gameObject.AddComponent<BoxCollider2D>(); }
        BoxActive(false);

        imgBlock.sprite = ResManager.GetSprite("combinegraphics_sprite", strName + nIndex.ToString());
        imgBlock.enabled = true;
        imgBlock.color = Color.white;
    }


    public void BoxActive(bool _active)
    {
        box2D.enabled = _active;
    }



}
