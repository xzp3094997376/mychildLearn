using UnityEngine;
using System.Collections;
using Spine.Unity;

public class EquationSpine : MonoBehaviour {
    public SkeletonGraphic mSpine { get; set; }
    public RectTransform mrtran { get; set; }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void PlaySpine(string name, bool isloop,float alhp = 1f)
    {
        if (null == mSpine)
        {
            mrtran = GetComponent<RectTransform>();
            mSpine = transform.Find("spine").GetComponent<SkeletonGraphic>();

        }
        Color color = mSpine.color;
        color.a = alhp;
        mSpine.color = color;
        mSpine.AnimationState.ClearTracks();
        mSpine.AnimationState.SetAnimation(2, name, isloop);
    }
    public void PlaySpineEffect(string name)
    {
        if (!gameObject.active) return;

        if (null == mSpine)
        {
            mrtran = GetComponent<RectTransform>();
            mSpine = transform.Find("spine").GetComponent<SkeletonGraphic>();
        }
        
        Color color = mSpine.color;
        //mSpine.AnimationState.SetAnimation(2, name, true);\
        mSpine.AnimationState.ClearTracks();
        mSpine.AnimationState.AddAnimation(1, name, false, 0);
        mSpine.AnimationState.AddAnimation(1, name, false, 0);
        mSpine.AnimationState.AddAnimation(1, name, false, 0);
        mSpine.AnimationState.AddAnimation(1, "Idle", true, 0);


        //StartCoroutine(TPlayDef());
    }
    //IEnumerator TPlayDef()
    //{
    //    yield return new WaitForSeconds(1f);
    //    mSpine.AnimationState.SetAnimation(2, "Idle", true);
    //}
}
