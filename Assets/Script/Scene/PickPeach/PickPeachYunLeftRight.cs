using UnityEngine;
using System.Collections;

public class PickPeachYunLeftRight : MonoBehaviour {

    void Start()
    {
        StartCoroutine(TStart());
    }

    IEnumerator TStart()
    {
        RectTransform rtran = gameObject.GetComponent<RectTransform>();
        Vector3 pos0 = rtran.anchoredPosition;
        Vector3 pos1 = rtran.anchoredPosition;
        pos0.x = -720;
        pos1.x = 720;
        bool forward = rtran.anchoredPosition.x > 0;
        float speed = Random.Range(0.0001f, 0.001f);

        while (true)
        {
            if(forward)
            {
                for (float i = 0; i < 1f; i += speed)
                {
                    rtran.anchoredPosition = Vector3.Lerp(pos0, pos1, i);
                    yield return new WaitForSeconds(0.01f);
                }
                forward = false;
            }
            else
            {
                for (float i = 0; i < 1f; i += speed)
                {
                    rtran.anchoredPosition = Vector3.Lerp(pos1, pos0, i);
                    yield return new WaitForSeconds(0.01f);
                }
                forward = false;
            }
            speed = Random.Range(0.0001f, 0.001f);
        }
    }

}
