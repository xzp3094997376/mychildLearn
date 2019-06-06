using UnityEngine;
using System.Collections;

public class KOAT_SceneBase : MonoBehaviour
{

    protected KnowOneAndTen mCtrl;

    public virtual void ResetInfos()
    {
        Common.DestroyChilds(transform);
    }

    public virtual void InitData()
    {
        if (mCtrl == null)
        {
            mCtrl = SceneMgr.Instance.GetNowScene() as KnowOneAndTen;
        }
    }

}
