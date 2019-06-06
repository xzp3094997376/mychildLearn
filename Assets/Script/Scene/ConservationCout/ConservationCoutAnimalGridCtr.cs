using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConservationCoutAnimalGridCtr : MonoBehaviour {
    public static Transform mTransform { get; set; }
    private bool isCreate { get; set; }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    private void createView()
    {
        if (null == mTransform)
        {
            mTransform = gameObject.GetComponent<RectTransform>();
        }
        List<Vector3> poss = new List<Vector3>()
            {
             new Vector3(-GlobalParam.screen_width / 4,  GlobalParam.screen_height / 4,0),
             new Vector3(-GlobalParam.screen_width / 4,  -GlobalParam.screen_height / 4,0),
             new Vector3(GlobalParam.screen_width / 4,  GlobalParam.screen_height / 4,0),
             new Vector3(GlobalParam.screen_width / 4, - GlobalParam.screen_height / 4,0),
            };
        for (int i = 0; i < 4; i++)
        {
            GameObject gridGo = UguiMaker.newGameObject("animals" + i, mTransform);
            ConservationCoutAnimalGrid grid = gridGo.AddComponent<ConservationCoutAnimalGrid>();
            gridGo.transform.localPosition = poss[i];
        }

        isCreate = true;
    }
    public void setData()
    {
        if (!isCreate)
        {
            createView();
        }
    }
}
