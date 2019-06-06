using UnityEngine;
using System.Collections;

public class PickPeachYunUpDown : MonoBehaviour {

	void Start () {
        StartCoroutine(TStart());
	}
	
    IEnumerator TStart()
    {
        RectTransform rtran = gameObject.GetComponent<RectTransform>();
        Vector3 pos0 = rtran.anchoredPosition;
        float speed = Random.Range(0.001f, 0.005f);
        float hight = Random.Range(10f, 15);
        while(true)
        {
            for(float i = 0; i < 1f; i += speed)
            {
                rtran.anchoredPosition = pos0 + new Vector3(0, hight * Mathf.Sin(Mathf.PI * 2 * i));
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

}
