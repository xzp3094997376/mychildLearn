using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class AnimalClass_LvStation : MonoBehaviour
{

    private AnimalClassificationCtrl mCtrl;
    private PolygonCollider2D collideroutside;

    public List<AnimalClass_Station> mStationList = new List<AnimalClass_Station>();

    private Vector3 vStart;

    public List<Transform> animalposList = new List<Transform>();

    public void InitAwake(int _stationCount)
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as AnimalClassificationCtrl;

        collideroutside = transform.Find("collideroutside").GetComponent<PolygonCollider2D>();
        collideroutside.isTrigger = true;

        List<LimiteVetect> vetectList = GetLimiteVetectList(_stationCount);
        for (int i = 0; i < _stationCount; i++)
        {
            AnimalClass_Station st = transform.Find("station" + (i+1)).gameObject.AddComponent<AnimalClass_Station>();
            if (_stationCount <= 3)
            {
                if (i % 2 == 0)
                    st.InitAwake("home0");
                else
                    st.InitAwake("home1");
            }
            else
            {
                st.InitAwake("mhome" + i);
            }
            st.SetLimiteVetect(vetectList[i]);
            st.SetLan();       
            mStationList.Add(st);
        }

        vStart = transform.localPosition;

        int posCount = 14;
        Transform animalpos = transform.Find("animalpos");
        for (int i=0;i< posCount; i++)
        {
            Transform tf = animalpos.Find("pos" + i);
            animalposList.Add(tf);
        }
    }

    private List<LimiteVetect> GetLimiteVetectList(int _stationCount)
    {
        List<LimiteVetect> resultList = new List<LimiteVetect>();
        if (_stationCount == 2)
        {
            resultList.Add(new LimiteVetect(-170f, 180f, 73f, -50f));
            resultList.Add(new LimiteVetect(-170f, 180f, 73f, -50f));
        }
        else if (_stationCount == 3)
        {
            resultList.Add(new LimiteVetect(-170f * 0.75f, 180f * 0.75f, 60f, -20f));
            resultList.Add(new LimiteVetect(-170f * 0.75f, 180f * 0.75f, 60f, -20f));
            resultList.Add(new LimiteVetect(-170f * 0.75f, 180f * 0.75f, 60f, -20f));
        }
        else
        {
            resultList.Add(new LimiteVetect(-90f, 75f, 52f, -65f));
            resultList.Add(new LimiteVetect(-160f, 150f, 55f, -70f));
            resultList.Add(new LimiteVetect(-90f, 80f, 50f, -70f));
            resultList.Add(new LimiteVetect(-180f, 155f, 35f, -3f));
        }
        return resultList;
    }

    public void SceneMove(bool _in)
    {
        for (int i = 0; i < mStationList.Count; i++)
        {
            mStationList[i].SceneMove(_in);
        }
    }

    public void DoShake()
    {
        transform.DOLocalMove(vStart + new Vector3(10f, 0f, 0f),0.15f).OnComplete(()=> 
        {
            transform.DOLocalMove(vStart + new Vector3(-10f, 0f, 0f), 0.3f).OnComplete(() => 
            {
                transform.DOLocalMove(vStart, 0.15f);
            });
        });
    }

    public void ResetInfos()
    {
        for(int i=0;i<mStationList.Count;i++)
        {
            mStationList[i].ResetInfos();
        }
    }


    List<int> mAnimalValueTypeList = new List<int>();
    public void SetLv3TipObj()
    {
        mAnimalValueTypeList = Common.GetIDList(30, 33, 4, -1);
        for (int i = 0; i < mStationList.Count; i++)
        {
            mStationList[i].theAnimalValueType = (AnimalValueType)mAnimalValueTypeList[i];
            string mtxt = mStationList[i].GetTipName();
            mStationList[i].ShowTipText(mtxt);
            mStationList[i].CreateImgTipBtn();
        }
    }
}


