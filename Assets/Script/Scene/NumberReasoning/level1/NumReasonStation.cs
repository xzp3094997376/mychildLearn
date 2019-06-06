using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 客箱
/// </summary>
public class NumReasonStation : MonoBehaviour
{

    private Image imgbg;
    

    public NumReasonNumObj numleft;//id=1
    public NumReasonNumObj numright;//id=2
    public NumReasonNumObj numcenter;//id=3


    private bool bInit = false;

    public void InitAwake(int _left,int _right)
    {
        imgbg = transform.Find("imgbg").GetComponent<Image>();
        imgbg.sprite = ResManager.GetSprite("numberreasoning_sprite", "mtl_" + Random.Range(0, 4));
        
        numleft = transform.Find("left").gameObject.AddComponent<NumReasonNumObj>();
        numright = transform.Find("right").gameObject.AddComponent<NumReasonNumObj>();
        numcenter = transform.Find("center").gameObject.AddComponent<NumReasonNumObj>();
        numleft.InitAwake(1, _left);
        numright.InitAwake(2, _right);
        numcenter.InitAwake(3, _left + _right);

        

        bInit = true;
    }

    /// <summary>
    /// 随机设置一个lost的NumObj
    /// </summary>
    public void SetTheLostNumObj()
    {
        int nindex = Random.Range(1, 4);
        numleft.Box2DActive(false);
        numright.Box2DActive(false);
        numcenter.Box2DActive(false);

        NumReasonNumObj targetObj = null;
        if (nindex == 1)
        {
            targetObj = numleft;
        }
        else if (nindex == 2)
        { targetObj = numright; }
        else
        { targetObj = numcenter; }

        targetObj.WenHaoActive(true);
        targetObj.MiniNumberActive(false);
        targetObj.Box2DActive(true);
    }


   




    void Update ()
    {

        if (!bInit)
        {
            return;
        }

        transform.eulerAngles = Vector3.zero;
	}
}
