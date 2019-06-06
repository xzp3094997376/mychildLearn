using UnityEngine;
using System.Collections;
using Spine.Unity;

public class ManyEquationSpine : MonoBehaviour {
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
        mSpine.AnimationState.ClearTracks();
        mSpine.AnimationState.AddAnimation(1, name, false, 0);
        //mSpine.AnimationState.AddAnimation(1, name, false, 0);
        //mSpine.AnimationState.AddAnimation(1, name, false, 0);
        if(name == "Mistake")
        {
            mSpine.AnimationState.AddAnimation(1, "Mistake2", true, 0);
            //mSpine.AnimationState.AddAnimation(1, "Idle", true, 0);
            StartCoroutine(TPlayDef(2.5f));
        }
        else
        {
            StartCoroutine(TPlayDef(1.5f));
        }
        //StartCoroutine(TPlayDef());
        //mSpine.AnimationState.ClearTracks();
        //mSpine.AnimationState.AddAnimation(1, "Idle", true, 0);

    }
    IEnumerator TPlayDef(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySpine("Idle", true);
    }
}
