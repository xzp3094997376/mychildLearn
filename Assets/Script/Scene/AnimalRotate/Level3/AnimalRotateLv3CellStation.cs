using UnityEngine;
using System.Collections;

public class AnimalRotateLv3CellStation : MonoBehaviour
{

    public int nID = 0;
    public AnimalRotateCell mcell;
    private BoxCollider2D mbox2d;

    public void InitAwake(int _id)
    {
        nID = _id;
        mbox2d = gameObject.AddComponent<BoxCollider2D>();
        mbox2d.size = new Vector2(65f,65f);
        mbox2d.isTrigger = true;
    }


    public void AddCell(AnimalRotateCell _cell)
    {
        mcell = _cell;
        _cell.StopDropTween();
        _cell.transform.SetParent(transform);
        _cell.transform.localPosition = Vector3.zero;
        _cell.transform.localScale = Vector3.one;
        _cell.bInStation = true;
    }

    public void RemoveCell()
    {
        mcell = null;
    }

}
