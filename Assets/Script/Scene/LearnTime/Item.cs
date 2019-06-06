using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
    private Vector3 defScale { get; set; }
	// Use this for initialization
	void Start () {
        isscale = true;

    }
    private float times = 0;
    private bool isscale { get; set; }
	// Update is called once per frame
	void Update () {
        if (!isscale) return;
        times += 0.1f;
        transform.localScale = Vector3.one + Vector3.one * 0.2f *  Mathf.Sin(times);
    }
    public void AutoScale(bool state)
    {
        isscale = state;
        if (!state)
        {
            transform.localScale = defScale;
        }
    }
    public void setDefScale(Vector3 _defScale)
    {
        defScale = _defScale;
    }
}
