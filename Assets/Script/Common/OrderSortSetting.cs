using UnityEngine;
using System.Collections;

public class OrderSortSetting : MonoBehaviour
{

    private Canvas canvas;

    public int sortOrder = 2;

    private void Awake()
    {
        canvas = gameObject.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = sortOrder;
    }

    public void SetSortingOrder(int _order)
    {
        sortOrder = _order;
        canvas.sortingOrder = sortOrder;
    }

}
