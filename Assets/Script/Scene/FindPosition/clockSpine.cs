using UnityEngine;
using System.Collections;
using Spine.Unity;

public class clockSpine : MonoBehaviour {
    public SkeletonGraphic mSpine { get; set; }
    public RectTransform mrtran { get; set; }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlaySpine(string name, bool isloop)
    {
        if (null == mSpine)
        {
            mrtran = GetComponent<RectTransform>();
            mSpine = transform.Find("spine").GetComponent<SkeletonGraphic>();
        }
        mSpine.AnimationState.SetAnimation(2, name, isloop);
    }
}