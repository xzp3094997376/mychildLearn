using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GoodFriendCarGuanka2 : MonoBehaviour
{
    public RectTransform mRtran { get; set; }

    RawImage mBg { get; set; }
    Image mJiantou { get; set; }
    //Image[] mGos { get; set; }
    GoodFriendCarGo[] mBtnGos;

    List<int> data_ids = new List<int>();
    List<int> data_animal_ids = new List<int>();
    int data_play_count = 0;
    int data_play_max_count = 3;
    int data_finish_car_count = 0;

    void Start()
    {


    }

    Vector3 temp_begin_pos = Vector3.zero;
    Vector3 temp_end_pos = Vector3.zero;
    Vector3 temp_go_reset_pos = Vector3.zero;
    bool temp_control = true;
    //int go_index = -1;
    void Update()
    {
        if (GoodFriendCarCtl.instance.data_curent_guanka != 2 || !temp_control)
            return;
        
        if (Input.GetMouseButtonDown(0))
        {
            GoodFriendCarAnimal.gSelect = null;
            //go_index = -1;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, 100);
            foreach (RaycastHit h in hits)
            {
                GoodFriendCarAnimal anim = h.collider.gameObject.GetComponent<GoodFriendCarAnimal>();
                if (null != anim)
                {
                    GoodFriendCarAnimal.gSelect = anim;
                    break;
                }

                GoodFriendCarGo.gSelect = h.collider.gameObject.GetComponent<GoodFriendCarGo>();
                if (null != GoodFriendCarGo.gSelect)
                {
                    //go_index = int.Parse(h.collider.gameObject.name.Split('=')[1]);
                    GoodFriendCarCtl.instance.mSound.PlayShortDefaultAb("出现0");
                    GoodFriendCarGo.gSelect.OnClickDown();
                    temp_go_reset_pos = GoodFriendCarGo.gSelect.mRtran.anchoredPosition3D;
                    break;
                }

            }

            if(null != GoodFriendCarAnimal.gSelect)
            {
                temp_begin_pos = transform.worldToLocalMatrix.MultiplyPoint( GoodFriendCarAnimal.gSelect.mImageNumberBg.transform.position);
                temp_begin_pos.z = 0;

                mJiantou.gameObject.SetActive(true);
                mJiantou.rectTransform.anchoredPosition3D = temp_begin_pos;
                mJiantou.rectTransform.sizeDelta = new Vector2(58, 36);
                mJiantou.transform.SetAsLastSibling();


                GoodFriendCarCtl.instance.mSound.PlayShortDefaultAb("07- 拖动连线", 1);

            }

        }
        else if (Input.GetMouseButton(0))
        {
            if (null != GoodFriendCarAnimal.gSelect)
            {
                temp_end_pos = Common.getMouseLocalPos(transform);
                int size = (int)(Vector3.Distance(temp_begin_pos, temp_end_pos));
                if (size < 58)
                    size = 58;
                mJiantou.rectTransform.sizeDelta = new Vector2(size, 36);
                mJiantou.rectTransform.anchoredPosition3D = (temp_begin_pos + temp_end_pos) * 0.5f;

                Vector3 dir = temp_end_pos - temp_begin_pos;
                float angle = Vector3.Angle(dir, Vector3.right);
                if (Vector3.Cross(dir, Vector3.right).z > 0)
                {
                    angle *= -1;
                }
                mJiantou.rectTransform.localEulerAngles = new Vector3(0, 0, angle);
            }
            else if (null != GoodFriendCarGo.gSelect)
            {
                Vector3 pos = new Vector3(Common.getMouseLocalPos(transform).x, temp_go_reset_pos.y, 0);
                if (pos.x < temp_go_reset_pos.x)
                    pos.x = temp_go_reset_pos.x;
                GoodFriendCarGo.gSelect.mRtran.anchoredPosition3D = pos;
                //刷新大小
                //FlushGoScale(go_index);
                //发射
                if (pos.x - temp_go_reset_pos.x > 50)
                {
                    GoodFriendCarGo.gSelect.Shoot();
                    GoodFriendCarCtl.instance.mSound.PlayShortDefaultAb("10-点击滑动路标", 1);
                    //StartCoroutine("TShoot");
                    GoodFriendCarGo.gSelect = null;
                }
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if(null != GoodFriendCarAnimal.gSelect)
            {
                GoodFriendCarAnimal animal = null;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray, 100);
                foreach (RaycastHit h in hits)
                {
                    animal = h.collider.gameObject.GetComponent<GoodFriendCarAnimal>();
                    if (null != animal)
                    {
                        break;
                    }

                }

                if (null != animal && GoodFriendCarAnimal.gSelect != animal)
                {
                    GoodFriendCarCtl.instance.mSound.PlayShortDefaultAb("09-连上对调位置", 1);
                    StartCoroutine(TJiaoHuan(GoodFriendCarAnimal.gSelect, animal));
                }
                else
                {
                    mJiantou.gameObject.SetActive(false);
                    GoodFriendCarCtl.instance.mSound.PlayShort("button_up");
                }
                GoodFriendCarAnimal.gSelect = null;

            }

            else if (null != GoodFriendCarGo.gSelect)
            {
                GoodFriendCarGo.gSelect.mRtran.anchoredPosition3D = temp_go_reset_pos;
                GoodFriendCarGo.gSelect.Show();
            }

        }
        
    }

    public void StartGame()
    {
        if(null == mJiantou)
        {
            mJiantou = UguiMaker.newImage("mJiantou", transform, "goodfriendcar_sprite", "jiantou_double");
            mJiantou.type = Image.Type.Sliced;
            mJiantou.gameObject.SetActive(false);
        }

        data_play_count++;
        if(data_play_count > data_play_max_count)
        {
            data_play_count = 1;
        }

        data_finish_car_count = 0;
        StartCoroutine("TStartGame");

    }
    public void OverGame()
    {
        GameOverCtl.GetInstance().Show(2, GoodFriendCarCtl.instance.Reset);
    }
    public void callback_CarShootBegin(int car_index)
    {
        data_finish_car_count++;
        GoodFriendCarCtl.instance.GetCar(car_index).CarGoOut();
        if (data_finish_car_count == 3)
        {
            if (data_play_count == data_play_max_count)
            {
                TopTitleCtl.instance.AddStar();
                Invoke("OverGame", 3f);
            }
            else
            {
                Invoke("StartGame", 4f);
            }
        }
    }

    IEnumerator TStartGame()
    {
        #region 定义数据
        List<int> temp_list = Common.GetMutexValue(2, 8, 3);
        data_ids.Clear();
        for(int i = 0; i < temp_list.Count; i++)
        {
            data_ids.Add(temp_list[i] - 1);
            data_ids.Add(temp_list[i]);
            data_ids.Add(temp_list[i] + 1);
        }
        data_ids = Common.BreakRank<int>(data_ids);
        data_animal_ids = Common.GetMutexValue(1, 14, 9);



        if (null == mRtran)
        {
            mRtran = gameObject.GetComponent<RectTransform>();
            mRtran.sizeDelta = new Vector2(1423, 800);
            mRtran.pivot = new Vector2(0.5f, 0.5f);
            mRtran.anchoredPosition = Vector2.zero;
            mBg = UguiMaker.newRawImage("bg", transform, "goodfriendcar_texture", "bg2", false);
            mBg.rectTransform.sizeDelta = new Vector2(1423, 800);

            mBtnGos = new GoodFriendCarGo[3];
            mBtnGos[0] = UguiMaker.newGameObject("mBtnGos0", mBg.transform).AddComponent<GoodFriendCarGo>();
            mBtnGos[1] = UguiMaker.newGameObject("mBtnGos1", mBg.transform).AddComponent<GoodFriendCarGo>();
            mBtnGos[2] = UguiMaker.newGameObject("mBtnGos2", mBg.transform).AddComponent<GoodFriendCarGo>();
            mBtnGos[0].Init(0);
            mBtnGos[1].Init(1);
            mBtnGos[2].Init(2);

        }
        #endregion

        yield return new WaitForSeconds(1);
        GoodFriendCarCtl.instance.mSound.PlayTipDefaultAb("tip第二关规则", 1, true);

        //生成车
        GoodFriendCarCar car0 =  GoodFriendCarCtl.instance.GetCar(0);
        GoodFriendCarCar car1 = GoodFriendCarCtl.instance.GetCar(1);
        GoodFriendCarCar car2 = GoodFriendCarCtl.instance.GetCar(2);
        car0.transform.SetParent(transform);
        car1.transform.SetParent(transform);
        car2.transform.SetParent(transform);
        //小动物上车
        for(int i = 0; i < 3; i++)
        {
            GoodFriendCarCar car = null;
            switch(i)
            {
                case 0:
                    car = car0;
                    break;
                case 1:
                    car = car1;
                    break;
                default:
                    car = car2;
                    break;
            }
            for(int j = 0; j < 3; j++)
            {
                int index = i * 3 + j;
                GoodFriendCarAnimal a = GoodFriendCarCtl.instance.GetAnimal(data_animal_ids[index]);
                Vector3 sit_pos = car.SitInCar(a);
                a.mRtran.anchoredPosition = sit_pos;
                a.SetNumber(data_ids[index]);
                a.SetSpine(new List<string>() { "face_idle"});
                //a.ShowShadow();
            }
        }

        //车来
        for (float i = 0; i < 1f; i += 0.012f)
        {
            car0.mRtran.anchoredPosition = Vector2.Lerp(new Vector2(-1000, 43.6f), new Vector2(-334.6f, 43.6f), Mathf.Sin(Mathf.PI * 0.5f * i));
            car1.mRtran.anchoredPosition = Vector2.Lerp(new Vector2(-1000, -151.52f), new Vector2(-334.6f, -151.52f), Mathf.Sin(Mathf.PI * 0.5f * i));
            car2.mRtran.anchoredPosition = Vector2.Lerp(new Vector2(-1000, -351), new Vector2(-334.6f, -351), Mathf.Sin(Mathf.PI * 0.5f * i));
            car0.FlushLunzi();
            car1.FlushLunzi();
            car2.FlushLunzi();
            yield return new WaitForSeconds(0.01f);
        }
        car0.mRtran.anchoredPosition = new Vector2(-334.6f, 43.6f);
        car1.mRtran.anchoredPosition = new Vector2(-334.6f, -151.52f);
        car2.mRtran.anchoredPosition = new Vector2(-334.6f, -351);

        GoodFriendCarCtl.instance.SetEnableAnimals(true);

        

    }
    IEnumerator TCorrect()
    {
        GoodFriendCarCtl.instance.mSound.PlayShortDefaultAb("出现0");
        mBtnGos[0].Show();
        mBtnGos[1].Show();
        mBtnGos[2].Show();
        yield break;
        

    }
    IEnumerator TError()
    {
        mBtnGos[0].Hide();
        mBtnGos[1].Hide();
        mBtnGos[2].Hide();
        yield break;

        //if (null == mGos)
        //    yield break;

        //if (!mGos[0].gameObject.activeSelf)
        //    yield break;

        //for (float i = 0; i < 1; i += 0.1f)
        //{
        //    mGos[0].transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i);
        //    mGos[1].transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i);
        //    mGos[2].transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i);
        //    yield return new WaitForSeconds(0.01f);
        //}

        //mGos[0].gameObject.SetActive(false);
        //mGos[1].gameObject.SetActive(false);
        //mGos[2].gameObject.SetActive(false);

    }
    IEnumerator TJiaoHuan(GoodFriendCarAnimal animal0, GoodFriendCarAnimal animal1)
    {
        temp_control = false;
        GoodFriendCarCar car0 = animal0.transform.parent.parent.GetComponent<GoodFriendCarCar>();
        GoodFriendCarCar car1 = animal1.transform.parent.parent.GetComponent<GoodFriendCarCar>();
        Vector3 sit_pos0 = animal0.mRtran.anchoredPosition3D;
        Vector3 sit_pos1 = animal1.mRtran.anchoredPosition3D;
        car0.SitOutCar(animal0);
        car1.SitOutCar(animal1);
        animal0.transform.SetParent(transform);
        animal1.transform.SetParent(transform);
        animal0.ShowShadow(false);
        animal1.ShowShadow(false);
        mJiantou.transform.SetAsLastSibling();

        Vector2 out_pos0_0 = animal0.mRtran.anchoredPosition3D;
        Vector2 out_pos1_0 = animal1.mRtran.anchoredPosition3D;
        Vector2 jiantou_size = mJiantou.rectTransform.sizeDelta;
        for (float i = 0; i < 1f; i += 0.1f)
        {
            float p = Mathf.Cos(Mathf.PI * 0.5f * (i - 1));
            mJiantou.rectTransform.sizeDelta = Vector2.Lerp(jiantou_size, new Vector2(0, 36), p);
            animal0.mRtran.anchoredPosition3D = Vector3.Lerp(out_pos0_0, mJiantou.rectTransform.anchoredPosition3D, p);
            animal1.mRtran.anchoredPosition3D = Vector3.Lerp(out_pos1_0, mJiantou.rectTransform.anchoredPosition3D, p);
            animal0.mRtran.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.5f, 0.5f, 1), p);
            animal1.mRtran.localScale = animal0.mRtran.localScale;

            yield return new WaitForSeconds(0.01f);
        }
        mJiantou.gameObject.SetActive(false);
        for (float i = 0; i < 1f; i += 0.1f)
        {
            float p = Mathf.Sin(Mathf.PI * 0.5f * i);
            //mJiantou.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(0, 36), jiantou_size, p);
            animal0.mRtran.anchoredPosition3D = Vector3.Lerp(mJiantou.rectTransform.anchoredPosition3D, out_pos1_0, p);
            animal1.mRtran.anchoredPosition3D = Vector3.Lerp(mJiantou.rectTransform.anchoredPosition3D, out_pos0_0, p);
            animal0.mRtran.localScale = Vector3.Lerp(new Vector3(0.5f, 0.5f, 1), Vector3.one, p);
            animal1.mRtran.localScale = animal0.mRtran.localScale;
            yield return new WaitForSeconds(0.01f);
        }
        animal0.mRtran.localScale = Vector3.one;
        animal1.mRtran.localScale = Vector3.one;
        car0.SitInCar(animal1);
        car1.SitInCar(animal0);
        animal0.mRtran.anchoredPosition3D = sit_pos1;
        animal1.mRtran.anchoredPosition3D = sit_pos0;

        yield return new WaitForSeconds(0.1f);

        animal0.ShowShadow(true);
        animal1.ShowShadow(true);
        temp_control = true;

        car0.Check();
        car1.Check();

        StopCoroutine("TError");
        StopCoroutine("TCorrect");
        if ( GoodFriendCarCtl.instance.GetCar(0).Check() &&
            GoodFriendCarCtl.instance.GetCar(1).Check() &&
            GoodFriendCarCtl.instance.GetCar(2).Check() )
        {
            GoodFriendCarCtl.instance.SetEnableAnimals(false);
            StartCoroutine("TCorrect");
            yield return new WaitForSeconds(1f);
        }
        else
        {
            StartCoroutine("TError");
        }
        

    }
    IEnumerator TGoEffect(Button go_btn)
    {
        go_btn.enabled = false;
        for(float i = 0; i < 1f; i += 0.2f)
        {
            go_btn.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.5f, 0.5f, 1), i);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.2f);
        Vector3 pos0 = go_btn.image.rectTransform.anchoredPosition3D;
        Vector3 pos1 = pos0 + new Vector3(1000, 0, 0);
        for(float i = 0; i < 1f; i += 0.1f)
        {
            float p = Mathf.Sin(Mathf.PI * 0.5f * (i - 1)) + 1;
            go_btn.transform.localScale = Vector3.Lerp(new Vector3(0.5f, 0.5f, 1), new Vector3(1.5f, 1.5f, 1), p);
            go_btn.image.rectTransform.anchoredPosition3D = Vector3.Lerp(pos0, pos1, p);

            yield return new WaitForSeconds(0.01f);
        }

        //go_btn.transform.localScale = Vector3.one;

        yield break;
    }
    //IEnumerator TShoot()
    //{
    //    int index = go_index;
    //    go_index = -1;

    //    mGos[index].gameObject.GetComponent<BoxCollider>().enabled = false;
    //    GoodFriendCarCtl.instance.GetCar(index).SetAnimalBox(false);

    //    float speed = 100;
    //    while (mGos[index].rectTransform.anchoredPosition.x < 846)
    //    {

    //        float p = (mGos[index].rectTransform.anchoredPosition.x - temp_go_reset_pos.x) / (846 - temp_go_reset_pos.x);

    //        mGos[index].rectTransform.anchoredPosition3D += new Vector3(speed * p, 0, 0);

    //        FlushGoScale(index);
    //        yield return new WaitForSeconds(0.01f);
    //    }

    //    mGos[index].gameObject.GetComponent<BoxCollider>().enabled = true;

    //    data_finish_car_count++;
    //    GoodFriendCarCtl.instance.GetCar(index).CarGoOut();
    //    if (data_finish_car_count == 3)
    //    {
    //        if (data_play_count == data_play_max_count)
    //        {
    //            TopTitleCtl.instance.AddStar();
    //            Invoke("OverGame", 3f);
    //        }
    //        else
    //        {
    //            Invoke("StartGame", 3f);
    //        }
    //    }

    //}
    //IEnumerator TGoSake0()
    //{
    //    float p1 = 0;
    //    float p0 = 0;
    //    while (true)
    //    {
    //        p0 = Mathf.Sin(p1) * 0.1f;
    //        mGos[0].transform.localScale = Vector3.one + new Vector3(p0, p0, 0);

    //        p1 += 0.2f;
    //        yield return new WaitForSeconds(0.01f);
    //    }

    //}
    //IEnumerator TGoSake1()
    //{
    //    float p1 = 0;
    //    float p0 = 0;
    //    while (true)
    //    {
    //        p0 = Mathf.Sin(p1) * 0.1f;
    //        mGos[1].transform.localScale = Vector3.one + new Vector3(p0, p0, 0);

    //        p1 += 0.2f;
    //        yield return new WaitForSeconds(0.01f);
    //    }

    //}
    //IEnumerator TGoSake2()
    //{
    //    float p1 = 0;
    //    float p0 = 0;
    //    while (true)
    //    {
    //        p0 = Mathf.Sin(p1) * 0.1f;
    //        mGos[2].transform.localScale = Vector3.one + new Vector3(p0, p0, 0);

    //        p1 += 0.2f;
    //        yield return new WaitForSeconds(0.01f);
    //    }

    //}

}
