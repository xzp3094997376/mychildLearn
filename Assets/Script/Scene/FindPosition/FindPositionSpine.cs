using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Spine.Unity;

public class FindPositionSpine : MonoBehaviour {
    public static FindPositionSpine gSelect { get; set; }
    public SkeletonGraphic mSpine { get; set; }
    public RectTransform mrtran { get; set; }
    public int mh { get; set; }
    public int mv { get; set; }
    private Vector3 defPos { get; set; }
    public string mAnimalName { get; set; }
    public int mid { get; set; }
    public bool isok { get; set; }

    private Image bg { get; set; }

    public void setData(int h,int v,string animalName,int id)
    {
        mh = h;
        mv = v;
        mAnimalName = animalName;
        mid = id;
        defPos = transform.localPosition;
        isok = false;
        if(null == bg)
        {
            bg = UguiMaker.newImage("rolebg", transform, "findposition_sprite", "photobg");
            bg.rectTransform.sizeDelta = new Vector2(350, 350);
            bg.rectTransform.localPosition = new Vector3(0, 156, 0);
            Canvas canvas = bg.gameObject.AddComponent<Canvas>();
            bg.gameObject.layer = LayerMask.NameToLayer("UI");
            canvas.overrideSorting = true;
            canvas.sortingOrder = 0;

            Canvas canvasSpine = gameObject.AddComponent<Canvas>();
            gameObject.layer = LayerMask.NameToLayer("UI");
            canvasSpine.overrideSorting = true;
            canvasSpine.sortingOrder = 1;
        }
    }
    public void PlaySpine(string name, bool isloop)
    {
        if (null == mSpine)
        {
            mrtran = GetComponent<RectTransform>();
            mSpine = gameObject.GetComponent<SkeletonGraphic>();
        }
        mSpine.AnimationState.SetAnimation(2, name, isloop);
    }
    public void setDefPos()
    {
        if (!gameObject.active) return;

        StartCoroutine(TToDef());
    }
    IEnumerator TToDef()
    {
        Vector3 startPos = transform.localPosition;
        for (float i = 0; i < 1f; i += 0.2f)
        {
            transform.localPosition = Vector3.Lerp(startPos, defPos, i);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localPosition = defPos;
    }
}