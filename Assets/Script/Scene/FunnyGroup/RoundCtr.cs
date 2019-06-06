using UnityEngine;
using System.Collections;

public class RoundCtr : MonoBehaviour {
    private bool roundStat = false;
    private float mAngle = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (!roundStat) return;

        mAngle -= 1;

        transform.localEulerAngles = new Vector3(0, 0, mAngle);

    }
    public void stop()
    {
        mAngle = 0;
        transform.localEulerAngles = new Vector3(0, 0, 0);
        roundStat = false;
    }
    public void round()
    {
        roundStat = true;
    }
}
