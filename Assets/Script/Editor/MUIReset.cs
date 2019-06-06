using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class MUIReset
{

    [MenuItem("UIReset/reset")]
    static void UIReset ()
    {
        Transform[] mselects = Selection.GetTransforms(SelectionMode.Deep);

        for (int i = 0; i < mselects.Length; i++)
        {
            Image msp = mselects[i].GetComponent<Image>();
            RawImage ratext = mselects[i].GetComponent<RawImage>();
            if (msp != null)
            {
                msp.sprite = null;
            }
            if (ratext != null)
            {
                ratext.texture = null;
            }
        }
    }
	

}
