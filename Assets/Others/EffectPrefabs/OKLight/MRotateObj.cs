using UnityEngine;
using System.Collections;

public class MRotateObj : MonoBehaviour
{
    public bool bRotate = true;
    public float fspeed = -2f;
    Vector3 vrotate = Vector3.zero;
	// Update is called once per frame
	void Update () {

        vrotate += new Vector3(0f, 0f, Time.deltaTime + fspeed);
        transform.localEulerAngles = vrotate;
	}
}
