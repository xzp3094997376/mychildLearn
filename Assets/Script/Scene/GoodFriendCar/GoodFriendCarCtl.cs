using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GoodFriendCarCtl : BaseScene
{
    public static GoodFriendCarCtl instance = null;

    Dictionary<int, GoodFriendCarAnimal> dataAnimalDic = new Dictionary<int, GoodFriendCarAnimal>();
    Dictionary<int, GoodFriendCarCar> dataCars = new Dictionary<int, GoodFriendCarCar>();

    public GoodFriendCarGuanka1 mGuanka1 { get; set; }
    public GoodFriendCarGuanka2 mGuanka2 { get; set; }

    public SoundManager mSound { get; set; }

    public int data_curent_guanka = 1;
    bool isFirstTime = true;

    void Awake()
    {
        instance = this;
        mSound = gameObject.AddComponent<SoundManager>();
        mSound.SetAbName("goodfriendcar_sound");

    }
    void Start ()
    {
        mGuanka2 = UguiMaker.newGameObject("guanka2", transform).AddComponent<GoodFriendCarGuanka2>();
        mGuanka1 = UguiMaker.newGameObject("guanka1", transform).AddComponent<GoodFriendCarGuanka1>();
        mSceneType = SceneEnum.GoodFriendCar;
        CallLoadFinishEvent();
        StartCoroutine("TStart");

    }
    IEnumerator TStart()
    {
        if (isFirstTime)
        {
            SoundManager.instance.PlayBg("bgmusic_loop0", "bgmusic_loop0", 0.08f);
            //SoundManager.instance.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.08f);
            isFirstTime = false;
        }

        data_curent_guanka = 1;
        mGuanka1.StartGame();
        TopTitleCtl.instance.Reset();

        //TopTitleCtl.instance.ResetNotMoveIn();

        //yield return new WaitForSeconds(1);
        //Goto2();
        //yield break;

        //yield return new WaitForSeconds(3f);
        //TopTitleCtl.instance.MoveIn();

        //yield return new WaitForSeconds(10f);
        //if (isFirstTime)
        //{
        //    SoundManager.instance.PlayBg("bgmusic_loop0", "bgmusic_loop0", 0.08f);
        //    //SoundManager.instance.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.08f);
        //    isFirstTime = false;
        //}
        yield break;

    }



    // Update is called once per frame
    void Update () {
	
	}
    
    public void Reset()
    {
        foreach(GoodFriendCarCar c in dataCars.Values)
        {
            ResetCar(c);
        }
        foreach (GoodFriendCarAnimal a in dataAnimalDic.Values)
        {
            ResetAnimal(a);
        }
        StartCoroutine("TGoto1");
    }
    public GoodFriendCarAnimal GetAnimal(int mdefine_id)
    {
        if(dataAnimalDic.ContainsKey(mdefine_id))
        {
            dataAnimalDic[mdefine_id].gameObject.SetActive(true);
            dataAnimalDic[mdefine_id].mRtran.eulerAngles = Vector2.zero;
            dataAnimalDic[mdefine_id].mRtran.localScale = Vector3.one;
            dataAnimalDic[mdefine_id].SetBoxEnable(true);
            return dataAnimalDic[mdefine_id];
        }
        else
        {
            MAnimalType type = (MAnimalType)Enum.Parse(typeof(MAnimalType), mdefine_id.ToString());
            GoodFriendCarAnimal animal = UguiMaker.newGameObject(type.ToString(), transform).AddComponent<GoodFriendCarAnimal>();
            //animal.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
            animal.SetDate(mdefine_id);
            dataAnimalDic.Add(mdefine_id, animal);
            return animal;
        }

    }
    public GoodFriendCarCar GetCar(int car_id)
    {
        if (dataCars.ContainsKey(car_id))
        {
            dataCars[car_id].gameObject.SetActive(true);
            return dataCars[car_id];
        }
        else
        {
            GoodFriendCarCar car = UguiMaker.newGameObject("car" + car_id.ToString(), transform).AddComponent<GoodFriendCarCar>();
            car.SetData(car_id);
            dataCars.Add(car_id, car);
            return car;
        }

    }
    public void ResetAnimal(GoodFriendCarAnimal animal)
    {
        animal.transform.localEulerAngles = Vector3.zero;
        animal.transform.localScale = Vector3.one;
        animal.SetBoxEnable(true);
        animal.transform.SetParent(transform);
        animal.gameObject.SetActive(false);
    }
    public void ResetCar(GoodFriendCarCar car)
    {
        car.transform.SetParent(transform);
        car.transform.localScale = Vector3.one;
        car.gameObject.SetActive(false);
    }
    public void SetEnableAnimals(bool _enable)
    {
        foreach(GoodFriendCarAnimal a in dataAnimalDic.Values)
        {
            a.SetBoxEnable(_enable);
        }
    }

    public void Goto2()
    {
        data_curent_guanka = 2;
        TopTitleCtl.instance.AddStar();
        mGuanka2.StartGame();
        StartCoroutine("TGoto2");

    }
    IEnumerator TGoto2()
    {
        mGuanka1.mRtran.sizeDelta = new Vector2(1423, 800);
        mGuanka1.mRtran.pivot = new Vector2(1, 0);
        mGuanka1.mRtran.anchoredPosition = new Vector2(1423 * 0.5f, -400);
        for (float i = 0; i < 1f; i += 0.02f)
        {
            mGuanka1.mRtran.localEulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, 90), Mathf.Sin(Mathf.PI * 0.5f * (i - 1)) + 1);
            yield return new WaitForSeconds(0.01f);
        }
        mGuanka1.mRtran.localEulerAngles = new Vector3(0, 0, 90);
    }
    IEnumerator TGoto1()
    {
        mGuanka1.mRtran.sizeDelta = new Vector2(1423, 800);
        mGuanka1.mRtran.pivot = new Vector2(1, 0);
        mGuanka1.mRtran.anchoredPosition = new Vector2(1423 * 0.5f, -400);
        for (float i = 0; i < 1f; i += 0.02f)
        {
            mGuanka1.mRtran.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 90), Vector3.zero, Mathf.Sin(Mathf.PI * 0.5f * (i - 1)) + 1);
            yield return new WaitForSeconds(0.01f);
        }
        mGuanka1.mRtran.localEulerAngles = Vector3.zero;

        StartCoroutine("TStart");

    }


}
