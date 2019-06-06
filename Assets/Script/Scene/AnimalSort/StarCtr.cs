using UnityEngine;
using System.Collections;

public class StarCtr : MonoBehaviour {
    private bool ismove { get; set; }
	// Use this for initialization
	void Start () {
        //ismove = false;

    }

	public void begian()
    {
        ismove = true;
    }
    private float speed = 0.5f;//0.5f
	// Update is called once per frame
	void Update () {
        transform.localPosition += new Vector3(0, 0.2f * Mathf.Sin(Time.time * Mathf.PI / 5),0);
        if (ismove)
        {
            transform.localPosition += new Vector3(speed, 0, 0);
        }
        if(transform.localPosition.x >= GlobalParam.screen_width * 0.5f)
        {
            GameObject.Destroy(gameObject);
        }
	}
}
