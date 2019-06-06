using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConservationCoutCtr : MonoBehaviour {
    public static Transform mTransform { get; set; }
    // Use this for initialization
    void Start () {
        mTransform = transform;
        gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

        Image bg = UguiMaker.newImage("bg", transform, "conservationcout_sprite", "bg");
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);
        bg.rectTransform.anchoredPosition = Vector3.zero;

        StartCoroutine("TCreateView");
    }
	IEnumerator TCreateView()
    {
        yield return new WaitForSeconds(1);
        GameObject animalsGo = UguiMaker.newGameObject("animalsGo", mTransform);
        ConservationCoutAnimalGridCtr gridctr = animalsGo.AddComponent<ConservationCoutAnimalGridCtr>();
        gridctr.setData();
       

    }
    // Update is called once per frame
    void Update () {
	
	}
}
