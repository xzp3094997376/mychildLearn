using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CG_TheObj : MonoBehaviour
{
    public string strName = "";
    private List<int> mIndexList = new List<int>();
    private List<int> mIDList = new List<int>();

    public List<CG_BlockStation> mStationList = new List<CG_BlockStation>();

    private Image img;

    public void InitAwake(string _name, List<int> _indexList, List<int> _idList)
    {
        img = gameObject.GetComponent<Image>();
        img.color = Color.white;
        img.enabled = false;

        strName = _name;
        mIndexList = _indexList;
        mIDList = _idList;
        for (int i = 0; i < mIndexList.Count; i++)
        {
            int nnindex = mIndexList[i];
            GameObject stGo = transform.Find(strName + nnindex.ToString()).gameObject;

            int nnid = mIDList[i];
            CG_BlockStation stCtrl = stGo.AddComponent<CG_BlockStation>();
            stCtrl.InitAwake(strName, nnindex, nnid);

            mStationList.Add(stCtrl);
        }
    }

    /// <summary>
    /// 取得相对位移量(base为第一个)
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public Vector3 GetLocalOffsetPos(int _index)
    {
        if (_index >= mStationList.Count)
        {
            Debug.LogError("out of list");
            return Vector3.zero;
        }

        Vector3 vlocalOffset = Vector3.zero;

        Vector3 vbase = mStationList[0].transform.localPosition;
        Vector3 vTarget = mStationList[_index].transform.localPosition;
        vlocalOffset = vTarget - vbase;

        return vlocalOffset;
    }

}
