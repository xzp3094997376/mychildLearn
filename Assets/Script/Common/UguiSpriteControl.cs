using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UguiSpriteControl : MonoBehaviour
{
    public List<Sprite> msprites_list = new List<Sprite>();
    public Dictionary<string, Sprite> msprite_dic = new Dictionary<string, Sprite>();
    
    public void Init()
    {
        if(null == msprite_dic || 0 == msprite_dic.Count)
        {
            for (int i = 0; i < msprites_list.Count; i++)
            {
                //Debug.Log(msprites_list[i].name);
                if(null != msprites_list[i])
                {
                    msprite_dic.Add(msprites_list[i].name, msprites_list[i]);
                }

            }
        }

    }
  

}
