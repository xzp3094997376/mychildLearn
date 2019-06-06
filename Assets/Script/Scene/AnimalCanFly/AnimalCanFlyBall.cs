using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AnimalCanFlyBall : MonoBehaviour
{
    public Image mImage { get; set; }
    public string mdata_side { get; set; }
    public RectTransform mRtranSit { get; set; }
    public RectTransform mRtran { get; set; }
    


    List<bool> mSitFree { get; set; }
    List<int> mSitFreeTemp = new List<int>();
    List<Vector3> mSitPos { get; set; }


    // Use this for initialization
    public void Init () {
        mImage = UguiMaker.newImage("mImage", transform, "animalcanfly_sprite", "san", false);
        mImage.rectTransform.pivot = new Vector2(0.5f, 0);
        mImage.rectTransform.anchoredPosition = Vector2.zero;
        mRtranSit = UguiMaker.newGameObject("mRtranSit", transform).GetComponent<RectTransform>();
        mRtran = gameObject.GetComponent<RectTransform>();
        mRtran.anchoredPosition = new Vector2(0, -9999);

        mSitFree = new List<bool>() { };
        for(int i = 0; i < 18; i++)
        {
            mSitFree.Add(true);
        }

        mSitPos = new List<Vector3>();
        List<Vector3> poss = Common.PosSortByWidth(293, 6, 4.6f);
        for(int i = 0; i < poss.Count; i++)
        {
            mSitPos.Add(poss[i]);
        }
        poss.Clear();
        poss = Common.PosSortByWidth(293, 6, 55);
        for (int i = 0; i < poss.Count; i++)
        {
            mSitPos.Add(poss[i]);
        }
        poss.Clear(); poss = Common.PosSortByWidth(293, 6, 106f);
        for (int i = 0; i < poss.Count; i++)
        {
            mSitPos.Add(poss[i]);
        }
        poss.Clear();

    }

    public void PushAnimal(AnimalCanFlyAnimal animal)
    {
        int index = GetPushIndex();
        PushAnimal(animal, index);
    }
    public void PushAnimal(AnimalCanFlyAnimal animal, int index)
    {
        animal.mRtran.SetParent(mRtranSit);
        animal.mRtran.anchoredPosition3D = mSitPos[index];
    }
    public void PopAnimal(int sit_index)
    {
        mSitFree[sit_index] = true;
    }
    public void ResetData()
    {
        for(int i = 0; i < mSitFree.Count; i++)
        {
            mSitFree[i] = true;
        }
    }
    public int GetPushIndex()
    {
        mSitFreeTemp.Clear();
        for (int i = 0; i < mSitFree.Count; i++)
        {
            if (mSitFree[i])
            {
                mSitFreeTemp.Add(i);
            }
        }

        if (0 == mSitFreeTemp.Count)
            return 0;

        int index = mSitFreeTemp[Random.Range(0, 1000) % mSitFreeTemp.Count];
        mSitFree[index] = false;

        return index;

    }
    public Vector3 GetSitPos_Parent(int index)
    {
        //Debug.LogError(mSitPos[index] + " " + mRtran.anchoredPosition3D);

        return mSitPos[index] + mRtran.anchoredPosition3D;
    }

    public void FlyOut()
    {
        StopAllCoroutines();
        StartCoroutine("TFlyOut");
    }
    IEnumerator TFlyOut()
    {
        while(mRtran.anchoredPosition.y < 420)
        {
            mRtran.anchoredPosition += new Vector2(0, 10);
            yield return new WaitForSeconds(0.01f);
        }
    }


    public void Fly()
    {
        StartCoroutine("TFly");
    }
    IEnumerator TFly()
    {

        yield return new WaitForSeconds(1);
        AnimalCanFlyCtl.instance.mSound.PlayShortDefaultAb("科学幻想小说菜单输入");
        Vector3 pos0 = new Vector3(0, -558, 0);
        Vector3 pos1 = new Vector3(0, 0, 0);
        switch(mdata_side)
        {
            case "left":
                pos0.x = Common.gWidth * -0.25f;
                pos1.x = Common.gWidth * -0.25f;
                break;
            case "right":
                pos0.x = Common.gWidth * 0.25f;
                pos1.x = Common.gWidth * 0.25f;
                break;
                
        }

        for (float i = 0; i < 1f; i += 0.03f)
        {
            mRtran.anchoredPosition = Vector3.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }

        //yield break;

        float speed = Random.Range(0.01f, 0.02f);
        float p = 0;
        while(true)
        {
            mRtran.anchoredPosition = pos1 + new Vector3(0, Mathf.Sin(p) * 30, 0);
            p += speed;
            yield return new WaitForSeconds(0.01f);
        }


    }

}

