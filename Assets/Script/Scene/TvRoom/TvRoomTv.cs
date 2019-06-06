using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TvRoomTv : MonoBehaviour {
    List<Image> mAnimals;
    public RectTransform mRtran { get; set; }
    public GameObject mAnimalParent { get; set; }
    public Vector2 mResetPos = Vector2.zero;
    public Dictionary<int, int> mCurTypeData { get; set; }

    // Use this for initialization
    void Start () {
	
	}
	
    public void Init(int index)
    {
        if (null == mAnimals)
            mAnimals = new List<Image>();
        else
            mAnimals.Clear();

        mRtran = gameObject.GetComponent<RectTransform>();
        UguiMaker.newImage("bg", transform, "tvroom_sprite", "tv" + index, false);
        mAnimalParent = UguiMaker.newGameObject("animal", transform);
        for (int j = 0; j < 6; j++)
        {
            mAnimals.Add(UguiMaker.newImage(j.ToString(), mAnimalParent.transform, "tvroom_sprite", "animal0", false));
        }

    }


    public int GetTypeKind(int type)
    {
        return mCurTypeData[type];
    }
    public Vector3 GetAngle(int type_duixing)
    {
        if(TvRoomCtl.instance.mGuanka.types.Contains(2))
        {
            return TvRoomCtl.instance.mGuanka.angle;
        }
        else
        {
            switch (type_duixing)
            {
                case 0:
                    return new Vector3(0, 0, Random.Range(0, 1000));
                case 1:
                    return new Vector3(0, 0, Random.Range(0, 1000));
                case 2:
                    return new Vector3(0, 0, (Random.Range(0, 1000) % 2 == 0 ? 0 : 180));
                case 3:
                    return new Vector3(0, 0, ((Random.Range(0, 1000) % 8) * 180));
                case 4:
                    return new Vector3(0, 0, Random.Range(-35, 35));
            }
            return Vector3.zero;
        }

    }
    public List<Vector3> GetPoss(int type_kind)
    {
        List<Vector3> poss = new List<Vector3>();
        switch (type_kind)
        {
            case 0:
                poss = new List<Vector3>(){
                    new Vector3( 0f, 114f, 0f),
                    new Vector3( 101.9f, 47.7f, 0f),
                    new Vector3( 107.1f, -70.5f, 0f),
                    new Vector3( 0f, -126f, 0f),
                    new Vector3( -113.5f, -73.7f, 0f),
                    new Vector3( -107.2f, 46.8f, 0f),
                };
                break;
            case 1:
                poss = new List<Vector3>(){
                    new Vector3( -100f, 50f, 0f),
                    new Vector3( 0f, 50f, 0f),
                    new Vector3( 100f, 50f, 0f),
                    new Vector3( -100f, -50f, 0f),
                    new Vector3( 0f, -50f, 0f),
                    new Vector3( 100f, -50f, 0f),
                };
                break;
            case 2:
                poss = new List<Vector3>(){
                    new Vector3( -5f, 125f, 0f),
                    new Vector3( -3f, 16f, 0f),
                    new Vector3( -67f, -82f, 0f),
                    new Vector3( -179f, -123f, 0f),
                    new Vector3( 82f, -72f, 0f),
                    new Vector3( 185f, -130f, 0f),
                };
                break;
            case 3:
                poss = new List<Vector3>(){
                    new Vector3( 0f, 102f, 0f),
                    new Vector3( -57.7f, 0f, 0f),
                    new Vector3( -115.4f, -102f, 0f),
                    new Vector3( 0f, -102f, 0f),
                    new Vector3( 115.4f, -102f, 0f),
                    new Vector3( 57.7f, 0f, 0f),
                };
                break;
            case 4:
                poss = new List<Vector3>(){
                    new Vector3( -255, 0f, 0f),
                    new Vector3( -153f, 0f, 0f),
                    new Vector3( -51f, 0f, 0f),
                    new Vector3( 51f, 0f, 0f),
                    new Vector3( 153f, 0f, 0f),
                    new Vector3( 255, 0f, 0f),
                };
                break;
        }
        return poss;
    }




    public void SetData(TvRoomData data, bool move_tv = true)
    {
        if (mCurTypeData == null)
        {
            mCurTypeData = new Dictionary<int, int>();
            mCurTypeData.Add(1, 0);
            mCurTypeData.Add(2, 0);
            mCurTypeData.Add(3, 0);
            mCurTypeData.Add(4, 0);
            mCurTypeData.Add(5, 0);
        }

        List<int> type_data = data.GetSetData();
        for (int type = 1; type <= 5; type++)
            mCurTypeData[type] = type_data[type - 1];


        //大小
        switch (mCurTypeData[3])
        {
            case 0:
                mAnimalParent.transform.localScale = new Vector3(0.65f, 0.65f, 1);
                break;
            case 1:
                mAnimalParent.transform.localScale = new Vector3(0.45f, 0.45f, 1);
                break;
            case 2:
                break;
        }

        //队形
        List<Vector3> poss = GetPoss(mCurTypeData[2]);
        for (int i = 0; i < poss.Count; i++)
        {
            mAnimals[i].rectTransform.anchoredPosition3D = poss[i];
            mAnimals[i].transform.eulerAngles = Vector3.zero;
        }
        mAnimalParent.transform.localEulerAngles = GetAngle(mCurTypeData[2]);

        //种类, 颜色
        for (int i = 0; i < mAnimals.Count; i++)
        {
            mAnimals[i].sprite = ResManager.GetSprite("tvroom_sprite", "animal" + mCurTypeData[5]);
        }


        if (move_tv)
        {
            StartCoroutine(TSetData());
        }

    }
    IEnumerator TSetData()
    {
        Vector2 pos0 = mResetPos - new Vector2(0, 800);
        
        for (float i = 0; i < 1f; i += 0.04f)
        {
            float p = Mathf.Sin(Mathf.PI * i) * 300;
            mRtran.anchoredPosition = Vector2.Lerp( pos0, mResetPos, i) +  new Vector2(0, p);
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition = mResetPos;

    }



    public void PlayError()
    {
        StartCoroutine("TPlayError");
    }
    IEnumerator TPlayError()
    {
        Vector3 angle = mAnimalParent.transform.localEulerAngles;
        float p = 0;
        for(float  i = 0; i < 1f; i += 0.08f)
        {
            p = Mathf.Sin(Mathf.PI * 2 * i) * 10;
            mAnimalParent.transform.localEulerAngles = angle + new Vector3(0, 0, p);
            yield return new WaitForSeconds(0.01f);
        }
        mAnimalParent.transform.localEulerAngles = angle;
    }


    //播放随机生成动画
    bool temp_can_move = false;
    public void Move()
    {
        StartCoroutine(TMove());
    }
    public void StopMove()
    {
        temp_can_move = false;
    }
    IEnumerator TMove()
    {
        temp_can_move = true;
        bool temp_can_move2 = true;
        List<Vector2> begpos = new List<Vector2>() { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero };
        List<int> data = new List<int>();
        Vector3 tempangle = new Vector3(0.1f, 0.1f, 1);
        while (temp_can_move2)
        {
            //1-数量，2-队形，3-大小，4-颜色，5-种类
            if(temp_can_move)
            {
                data = TvRoomCtl.instance.mGuanka.GetRandomData();
            }
            else
            {
                temp_can_move2 = false;
                data = TvRoomCtl.instance.mGuanka.GetSetData();
            }





            for (int i = 0; i < mAnimals.Count; i++)
            {
                begpos[i] = mAnimals[i].rectTransform.anchoredPosition;
                //队形待定
                //大小待定
                //颜色
                mAnimals[i].sprite = ResManager.GetSprite("tvroom_sprite", "animal" + data[3]);
            }

            List<Vector3> endpos = GetPoss(data[1]);
            Vector3 begscale = mAnimalParent.transform.localScale;
            Vector3 endscale = new Vector3(0.55f, 0.55f, 1);
            Vector3 begangle = Common.Parse360(mAnimalParent.transform.localEulerAngles);
            Vector3 endangle = Common.Parse360(GetAngle(data[1]));
            if(endangle.z - begangle.z > 180)
            {
                endangle = Common.Parse180(endangle);
            }
            else if(endangle.z - begangle.z < -180)
            {
                begangle = Common.Parse180(begangle);
            }

            if (data[2] == 1)
                endscale = new Vector3(0.4f, 0.4f, 1);

            for (float i = 0; i < 1f; i += 0.08f)
            {
                for(int j = 0; j < mAnimals.Count; j++)
                {
                    mAnimals[j].rectTransform.anchoredPosition = Vector2.Lerp(begpos[j], endpos[j], i);
                }
                if(i < 0.5f)
                {
                    mAnimalParent.transform.localScale = Vector3.Lerp(begscale, tempangle, i);
                }
                else
                {
                    mAnimalParent.transform.localScale = Vector3.Lerp(tempangle, endscale, i);
                }
                mAnimalParent.transform.localEulerAngles = Vector3.Lerp(begangle, endangle, i);
                yield return new WaitForSeconds(0.01f);
            }
            for (int j = 0; j < mAnimals.Count; j++)
            {
                mAnimals[j].rectTransform.anchoredPosition = endpos[j];
            }
            mAnimalParent.transform.localEulerAngles = endangle;
            mAnimalParent.transform.localScale = endscale;

        }

        mCurTypeData[1] = data[0];
        mCurTypeData[2] = data[1];
        mCurTypeData[3] = data[2];
        mCurTypeData[4] = data[3];
        mCurTypeData[5] = data[4];


    }



}
