using UnityEngine;
using System.Collections;

public class ChookOKEffect : MonoBehaviour {
    RectTransform mRtran { get; set; }

	// Use this for initialization
	void Start () {
        mRtran = gameObject.GetComponent<RectTransform>();

	}
	
	// Update is called once per frame
	void Update () {
        mRtran.localEulerAngles += new Vector3(0, 0, 10);
	}



}
