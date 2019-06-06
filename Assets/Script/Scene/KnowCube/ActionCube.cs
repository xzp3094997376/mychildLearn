using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class ActionCube : MonoBehaviour
{
    public int nType = 0;
    public int nOpenIndex = 0;
    public bool bOpenState = false;
    public bool isOpen = true;

    private ActionCubePanel[] imgPanels = new ActionCubePanel[6];


    public void InitAwake(int _id)
    {
        nType = _id;

        for (int i = 0; i < 6; i++)
        {
            imgPanels[i] = GetGameObjFromChild(gameObject, "Image" + i).AddComponent<ActionCubePanel>();
            imgPanels[i].InitAwake(this);
        }

        if (_id == 1)
        {
            imgPanels[1].vRotate = new Vector3(90f, 0f, 0f);
            imgPanels[2].vRotate = new Vector3(90f, 0f, 0f);
            imgPanels[3].vRotate = new Vector3(0f, 90f, 0f);
            imgPanels[4].vRotate = new Vector3(0f, -90f, 0f);
            imgPanels[5].vRotate = new Vector3(-90f, 0f, 0f);
        }
        else
        {
            imgPanels[1].vRotate = new Vector3(0f, 90f, 0f);
            imgPanels[2].vRotate = new Vector3(0f, 90f, 0f);
            imgPanels[3].vRotate = new Vector3(90f, 0f, 0f);
            imgPanels[4].vRotate = new Vector3(-90f, 0f, 0f);
            imgPanels[5].vRotate = new Vector3(0f, -90f, 0f);
        }

        nOpenIndex = 0;     
    }


    /// <summary>
    /// 展开/关闭
    /// </summary>
    public void ChangeOpenClose()
    {
        if (isOpen)
        {
            nOpenIndex++;
            if (nOpenIndex <= 5)
            {
                ActionCubePanel panel = imgPanels[nOpenIndex];
                panel.OpenPanel();
            }
            else
            {
                isOpen = false;
                nOpenIndex--;
                ActionCubePanel panel = imgPanels[nOpenIndex];
                panel.ClosePanel();
            }
        }
        else
        {
            nOpenIndex--;
            if (nOpenIndex > 0)
            {
                ActionCubePanel panel = imgPanels[nOpenIndex];
                panel.ClosePanel();
            }
            else
            {
                isOpen = true;
                nOpenIndex++;
                ActionCubePanel panel = imgPanels[nOpenIndex];
                panel.OpenPanel();
            }
        }
    }


    System.Action mFirstZhankaiOverCall;
    /// <summary>
    /// 设置第一次展开完成回调
    /// </summary>
    /// <param name="_call"></param>
    public void SetFirstOverCallback(System.Action _call)
    {
        mFirstZhankaiOverCall = _call;
    }

    public void DoCallBack()
    {
        if (nOpenIndex == 5)
        {
            if (mFirstZhankaiOverCall != null)
            {
                mFirstZhankaiOverCall();
                mFirstZhankaiOverCall = null;
            }
        }
    }


    public GameObject GetGameObjFromChild(GameObject _go, string _childName, bool _includeInactive = false)
    {
        GameObject returnObj = null;
        Transform[] _transF = _go.GetComponentsInChildren<Transform>(_includeInactive);
        if (_transF == null)
            return returnObj;
        for (int i = 0; i < _transF.Length; i++)
        {
            if (_transF[i].gameObject.name.CompareTo(_childName) == 0)
            {
                returnObj = _transF[i].gameObject;
                break;
            }
        }
        return returnObj;
    }

}
