using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimalRotateDropLineST : MonoBehaviour
{

    public bool bDropOK = false;
    public Image mDropPoint;
    public AnimalRotateStation mstation;


    public void InitAwake(AnimalRotateStation _st)
    {
        mstation = _st;       
    }

    public void CreateDropPoint(Vector3 _vpos)
    {
        GameObject mgo = UguiMaker.newGameObject("droppoint", transform);
        mgo.transform.localPosition = _vpos;
        mDropPoint = mgo.AddComponent<Image>();
        mDropPoint.rectTransform.sizeDelta = new Vector2(30f, 30f);
        mDropPoint.sprite = ResManager.GetSprite("animalrotate_sprite", "mpoint");
        mDropPoint.enabled = false;
        //BoxCollider2D cbox = mgo.AddComponent<BoxCollider2D>();
        //cbox.size = mDropPoint.rectTransform.sizeDelta;
    }

    /// <summary>
    /// 随机旋转
    /// </summary>
    public void SetRandomRotateInfo()
    {
        //mstation.SetRotateID(Random.Range(0, AnimalRotateDefine.nRotateMaxType + 1));
        mstation.SetCellRotateID(0, Random.Range(0, AnimalRotateDefine.nRotateMaxType + 1));
        mstation.SetCellRotateID(1, Random.Range(0, AnimalRotateDefine.nRotateMaxType + 1));
        mstation.SetCellRotateID(2, Random.Range(0, AnimalRotateDefine.nRotateMaxType + 1));
        mstation.SetCellRotateID(3, Random.Range(0, AnimalRotateDefine.nRotateMaxType + 1));

        mstation.ButtonEnbal(false);
        for (int i = 0; i < mstation.mCells.Length; i++)
        {
            mstation.mCells[i].ButtonEnbal(false);
        }
    }

    /// <summary>
    /// 外框旋转
    /// </summary>
    /// <param name="_rotateID">0-3</param>
    public void SetBigRotate(int _rotateID)
    {
        mstation.SetRotateID(_rotateID);
    }

}

/// <summary>
/// 连线点
/// </summary>
public class AnimalRotateDropPointST : MonoBehaviour
{
    public Image mDropPoint;
    public AnimalRotateDropLineST mDropLineST;
}
