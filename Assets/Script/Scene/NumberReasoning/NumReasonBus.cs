using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NumReasonBus : MonoBehaviour
{
    public int nBusID = 0;
    private Image imgbus;

    private GameObject mAnimalT;
    private GameObject mNumObjT;

    public NumReasonAnimal[] mAnimals = new NumReasonAnimal[4];
    public NumReasonNumObj[] mNumObjs = new NumReasonNumObj[6];


    public void InitAwake(int _busID, int[] _animalIDs, int[] _numbers)
    {
        nBusID = _busID;  
        //动物s
        mAnimalT = UguiMaker.newGameObject("mAnimalT", transform);
        mAnimalT.transform.localPosition = new Vector3(8f, -20f);
        List<float> fAnimals = Common.GetOrderList(4, 130f);
        for (int i = 0; i < _animalIDs.Length; i++)
        {
            mAnimals[i] = UguiMaker.newGameObject("animal", mAnimalT.transform).AddComponent<NumReasonAnimal>();
            mAnimals[i].InitAwake(_animalIDs[i], Vector3.zero);
            mAnimals[i].transform.localScale = Vector3.one * 0.8f;
            mAnimals[i].transform.localPosition = new Vector3(fAnimals[i], 0f, 0f);
        }
        //imgbg
        imgbus = UguiMaker.newImage("imgbus", transform, "numberreasoning_sprite", "bus" + nBusID, false);
        //数字s
        mNumObjT = UguiMaker.newGameObject("mNumObjT", transform);
        mNumObjT.transform.localPosition = new Vector3(8f, -20f);
        List<float> fNums = Common.GetOrderList(6, 88f);
        for (int i = 0; i < _numbers.Length; i++)
        {
            mNumObjs[i] = UguiMaker.newGameObject("number", mNumObjT.transform).AddComponent<NumReasonNumObj>();
            mNumObjs[i].InitAwake(3, _numbers[i]);
            mNumObjs[i].transform.localPosition = new Vector3(fNums[i], 0f, 0f);
            mNumObjs[i].SetBoxSize(new Vector2(75f, 85f));
        }
    }

    /// <summary>
    /// 设置丢失的数字
    /// </summary>
    /// <param name="_count"></param>
    public void SetLost(int _count)
    {
        List<int> indexLost = Common.GetIDList(0, 5, _count, -1);
        for (int i = 0; i < _count; i++)
        {
            NumReasonNumObj thelost = mNumObjs[indexLost[i]];
            thelost.Box2DActive(true);
            thelost.WenHaoActive(true);
            thelost.MiniNumberActive(false);
        }
    }

    //播放欢呼动画
    public void PlayHappyAnimaltion()
    {
        for (int i = 0; i < mAnimals.Length; i++)
        {
            mAnimals[i].PlayAnimation("face_sayyes", true);
        }
        StartCoroutine(ieResetAnimation());
    }
    IEnumerator ieResetAnimation()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < mAnimals.Length; i++)
        {
            mAnimals[i].PlayAnimation("face_idle", true);
        }
    }

}
