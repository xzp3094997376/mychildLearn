using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GoodFriendCarGuanka1 : MonoBehaviour
{
    public RectTransform mRtran { get; set; }

    RectTransform mRtranScene { get; set; }
    RawImage mBg { get; set; }
    //Image mHand { get; set; }
    Image mGrass0 { get; set; }
    Image mGrass1 { get; set; }
    Image mGrass2 { get; set; }
    Image mEffectQizi0 { get; set; }
    Image mEffectQizi1 { get; set; }
    BoxCollider mBoxLeft { get; set; }
    BoxCollider mBoxRight { get; set; }

    List<GoodFriendCarAnimal> data_animals = new List<GoodFriendCarAnimal>();
    List<int> data_animal_ids = null;
    List<int> data_nums = null;
    int find_id = 0;
    int play_num = 0;
    int play_max_num = 3;

    void Start()
    {
        //mHand = UguiMaker.newImage("hand", transform, "public", "hand_open");
        //mHand.gameObject.SetActive(false);
    }
    void Update()
    {
        if (GoodFriendCarCtl.instance.data_curent_guanka != 1)
            return;
        if(Input.GetMouseButtonDown(0))
        {
            GoodFriendCarAnimal.gSelect = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, 100);
            foreach(RaycastHit h in hits)
            {
                GoodFriendCarAnimal anim = h.collider.gameObject.GetComponent<GoodFriendCarAnimal>();
                if(null != anim)
                {
                    GoodFriendCarAnimal.gSelect = anim;
                    break;
                }

            }
            
            if(null == GoodFriendCarAnimal.gSelect)
            {
            }
            else
            {
                GoodFriendCarAnimal.gSelect.SetSpine(new List<string>() { "face_idle" });
                GoodFriendCarAnimal.gSelect.Select();
                GoodFriendCarAnimal.gSelect.ShowShadow(false);
                GoodFriendCarAnimal.gSelect.PlayVoice();
                GoodFriendCarCtl.instance.mSound.PlayShort("按钮2");
            }

        }
        else if(Input.GetMouseButton(0))
        {
            if(null != GoodFriendCarAnimal.gSelect)
            {
                GoodFriendCarAnimal.gSelect.mRtran.anchoredPosition = Common.getMouseLocalPos(transform); 

            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if(null != GoodFriendCarAnimal.gSelect)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray, 100);
                bool correct = false;
                foreach (RaycastHit h in hits)
                {
                    if (h.collider.gameObject.name.Equals("box_left"))
                    {
                        if(GoodFriendCarAnimal.gSelect.data_number + 1 == find_id)
                        {
                            GoodFriendCarAnimal.gSelect.SetBoxEnable(false);
                            GoodFriendCarAnimal.gSelect.SetResetPos(h.collider.gameObject.GetComponent<RectTransform>().anchoredPosition + new Vector2(-5, 18));
                            correct = true;
                        }
                        else
                        {
                            SayNo();
                            GoodFriendCarCtl.instance.mSound.PlayShortDefaultAb("08-连线反弹", 1);
                            ErrorTip();
                            //GoodFriendCarCtl.instance.mSound.PlayTipDefaultAb("tip错误");
                        }
                        break;
                    }
                    else if (h.collider.gameObject.name.Equals("box_right"))
                    {
                        if (GoodFriendCarAnimal.gSelect.data_number - 1 == find_id)
                        {
                            GoodFriendCarAnimal.gSelect.SetBoxEnable(false);
                            GoodFriendCarAnimal.gSelect.SetResetPos(h.collider.gameObject.GetComponent<RectTransform>().anchoredPosition + new Vector2(-5, 18));
                            correct = true;
                        }
                        else
                        {
                            SayNo();
                            GoodFriendCarCtl.instance.mSound.PlayShortDefaultAb("08-连线反弹", 1);
                            ErrorTip();
                            //GoodFriendCarCtl.instance.mSound.PlayTipDefaultAb("tip错误");

                        }
                        break;
                    }

                }
                
                GoodFriendCarAnimal.gSelect.ShowShadow(!correct);
                GoodFriendCarAnimal.gSelect.UnSelect();
                GoodFriendCarAnimal.gSelect = null;

                Common.SortDepth(mRtranScene);

                if(correct)
                {

                    for (int i = 0; i < data_animals.Count; i++)
                    {
                        data_animals[i].SetSpine(new List<string>() { "face_sayyes" });
                    }
                    CancelInvoke();
                    Invoke("InvokePlayIdle", 2);

                    int temp = 0;
                    for(int i = 0; i < data_animals.Count; i++)
                    {
                        if( 1 >= Mathf.Abs(data_animals[i].data_number - find_id))
                        {
                            if (!data_animals[i].mBox.enabled)
                                temp++;
                        }
                    }
                    GoodFriendCarCtl.instance.mSound.PlayShortDefaultAb("欢呼0");
                    if(temp == 3)
                        StartCoroutine("TCorrect");
                    else
                        GoodFriendCarCtl.instance.mSound.PlayTipDefaultAb("tip感谢" + Random.Range(0, 1000) % 2, 1);

                }
                else
                {
                   
                }

            }
        }

    }

    void SayNo()
    {
        for (int i = 0; i < data_animals.Count; i++)
        {
            data_animals[i].SetSpine(new List<string>() { "face_sayno" });
        }
        CancelInvoke();
        Invoke("InvokePlayIdle", 2);
        GoodFriendCarCtl.instance.mSound.PlayShortDefaultAb("游戏失败惋惜");

    }
    void ErrorTip()
    {
        if(0 == Random.Range(0, 1000) % 2)
        {
            //GoodFriendCarCtl.instance.mSound.PlayTipDefaultAb("tip错误");
            GoodFriendCarCtl.instance.mSound.PlayTipList(
                new List<string>() { "goodfriendcar_sound", "aa_animal_name", "goodfriendcar_sound" },
                new List<string>() { "tip错误", MDefine.GetAnimalNameByID_CH(data_animal_ids[0]), "tip错误1" },
                new List<float>() { 1f, 1f, 1f } );
        }
        else
        {
            GoodFriendCarCtl.instance.mSound.PlayTipList(
                new List<string>() { "goodfriendcar_sound", "aa_animal_name", "goodfriendcar_sound", "goodfriendcar_sound" },
                new List<string>() { "tip好朋友身上的数字比", MDefine.GetAnimalNameByID_CH(data_animal_ids[0]), "tip身上的数字", "tip多一或者少一" },
                new List<float>() { 1f, 1f, 1f, 1f });

        }


    }
    void InvokePlayIdle()
    {
        for (int i = 0; i < data_animals.Count; i++)
        {
            data_animals[i].SetSpine(new List<string>() { "face_idle" });
        }
    }

    bool is_play_rule = false;
    public void StartGame()
    {
        play_num++;
        if(play_num > 3)
        {
            play_num = 1;
        }
        StartCoroutine("TStartGame");
    }
    IEnumerator TStartGame()
    {
        #region 定义数据
        data_animal_ids = Common.GetMutexValue(1, 14, 10);
        data_nums = Common.BreakRank<int>(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        find_id = Random.Range(0, 1000) % 8 + 2;
        data_nums.Remove(find_id);
        data_animals.Clear();

        #endregion


        #region 出来草丛
        if (null == mGrass0)
        {
            mRtran = gameObject.GetComponent<RectTransform>();
            mRtran.sizeDelta = new Vector2(1423, 800);

            mBg = UguiMaker.newRawImage("bg", transform, "goodfriendcar_texture", "bg1", false);
            mBg.rectTransform.sizeDelta = new Vector2(1423, 800);
            yield return new WaitForSeconds(0.5f);


            mRtranScene = UguiMaker.newGameObject("scene", transform).GetComponent<RectTransform>();
            mGrass0 = UguiMaker.newImage("grass0", transform, "goodfriendcar_sprite", "grass0", false);
            mGrass1 = UguiMaker.newImage("grass0", transform, "goodfriendcar_sprite", "grass1", false);
            mGrass2 = UguiMaker.newImage("grass0", transform, "goodfriendcar_sprite", "grass2", false);

            mGrass0.rectTransform.pivot = new Vector2(0.5f, 0);
            mGrass1.rectTransform.pivot = new Vector2(0.5f, 0);
            mGrass2.rectTransform.pivot = new Vector2(0.5f, 0);

            mGrass0.rectTransform.anchoredPosition3D = new Vector3(-400, -410, 0);
            mGrass1.rectTransform.anchoredPosition3D = new Vector3(0, -400, 0);
            mGrass2.rectTransform.anchoredPosition3D = new Vector3(400, -450, 0);

            mBoxLeft = UguiMaker.newGameObject("box_left", transform).AddComponent<BoxCollider>();
            mBoxRight = UguiMaker.newGameObject("box_right", transform).AddComponent<BoxCollider>();
            mBoxLeft.size = new Vector3(148.84f, 100, 1);
            mBoxRight.size = new Vector3(148.84f, 100, 1);
            mBoxLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2(-156, -311.2f);
            mBoxRight.GetComponent<RectTransform>().anchoredPosition = new Vector2(173.1f, -311.2f);

        }
        mRtran.pivot = new Vector2(0.5f, 0.5f);
        mRtran.anchoredPosition = Vector2.zero;
        mBoxLeft.gameObject.SetActive(true);
        mBoxRight.gameObject.SetActive(true);

        Vector3 s1 = new Vector3(0.5f, 0.5f, 1);
        Vector3 s2 = Vector3.one;
        Vector3 s = Vector3.zero;
        for (float i = 0; i < 1f; i += 0.08f)
        {
            float p = Mathf.Sin(Mathf.PI * i) * 0.3f;
            s = Vector3.Lerp(s1, s2, i) + new Vector3(p, p, 0);
            mGrass0.transform.localScale = s;
            mGrass1.transform.localScale = s;
            mGrass2.transform.localScale = s;

            yield return new WaitForSeconds(0.01f);
        }
        mGrass0.transform.localScale = Vector3.one;
        mGrass1.transform.localScale = Vector3.one;
        mGrass2.transform.localScale = Vector3.one;
        #endregion

        #region 变身

        //List<GoodFriendCarAnimal> temp_rtrans = new List<GoodFriendCarAnimal>();

        GoodFriendCarAnimal anim_mid = GoodFriendCarCtl.instance.GetAnimal(data_animal_ids[0]);
        anim_mid.transform.SetParent(mRtranScene);
        anim_mid.transform.localScale = Vector3.zero;
        anim_mid.mRtran.anchoredPosition = new Vector2(0, -300);
        anim_mid.SetNumber(find_id);
        anim_mid.ShowShadow(false);
        anim_mid.SetBoxEnable(false);
        //temp_rtrans.Add(anim_mid);
        data_animals.Add(anim_mid);


        float sub_angle = Mathf.PI / (data_animal_ids.Count - 1 - 1);
        float temp_angle = 0;
        for (int i = 1; i < data_animal_ids.Count; i++)
        {
            GoodFriendCarAnimal anim = GoodFriendCarCtl.instance.GetAnimal(data_animal_ids[i]);
            anim.transform.SetParent(mRtranScene);
            anim.transform.localScale = Vector3.zero;
            anim.SetNumber(data_nums[i - 1]);
            anim.ShowShadow(true);
            anim.SetBoxEnable(true);
            anim.SetSpine(new List<string>() { "face_idle" });

            temp_angle = Mathf.PI * -0.5f + (i - 1) * sub_angle;
            anim.mRtran.anchoredPosition = new Vector2(
                Mathf.Sin(temp_angle) * 450,
                Mathf.Cos(temp_angle) * 300 - 300);
            anim.SetResetPos(anim.mRtran.anchoredPosition);

            //temp_rtrans.Add(anim);
            data_animals.Add(anim);
        }

        Common.SortDepth(mRtranScene);

        GoodFriendCarCtl.instance.mSound.PlayShort("素材出现通用音效");
        for (float i = 0; i < 1f; i += 0.1f)
        {
            for(int j = 0; j < data_animals.Count; j++)
            {
                data_animals[j].transform.localEulerAngles = Vector3.Lerp( new Vector3(0, 0, 90), Vector3.zero, i);
                data_animals[j].transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, i);
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (int j = 0; j < data_animals.Count; j++)
        {
            data_animals[j].transform.localEulerAngles = Vector3.zero;
        }

        #endregion

        yield return new WaitForSeconds(2f);

        GoodFriendCarCtl.instance.mSound.PlayTipList(new List<string>() { "goodfriendcar_sound", "aa_animal_name", "goodfriendcar_sound", "aa_animal_name", "goodfriendcar_sound" },
            new List<string>() { "tip第一关规则0", MDefine.GetAnimalNameByID_CH(anim_mid.data_animal_id), "tip第一关规则1", MDefine.GetAnimalNameByID_CH(anim_mid.data_animal_id), "tip第一关规则2" },
            new List<float>() { 1f, 1, 1f, 1, 1f },
            true);

    }
    IEnumerator TCorrect()
    {

        if(!is_play_rule)
        {
            is_play_rule = true;
            GoodFriendCarCtl.instance.mSound.PlayTipList(
                new List<string>() { "goodfriendcar_sound", "aa_animal_name", "goodfriendcar_sound", "goodfriendcar_sound" },
                new List<string>() { "tip谢谢你帮", MDefine.GetAnimalNameByID_CH(data_animal_ids[0]), "tip找到它的好朋友", "tip你发现了没" },
                new List<float>() { 1f, 1, 1f, 1f });
            yield return new WaitForSeconds(12);
        }
        else
        {
            GoodFriendCarCtl.instance.mSound.PlayTipList(
                new List<string>() { "goodfriendcar_sound", "aa_animal_name", "goodfriendcar_sound" },
                new List<string>() { "tip谢谢你帮", MDefine.GetAnimalNameByID_CH(data_animal_ids[0]), "tip找到它的好朋友" },
                new List<float>() { 1f, 1, 1f } );
            yield return new WaitForSeconds(2);

        }

        Vector2 pos0 = Vector2.zero;
        Vector2 pos1 = Vector2.zero;
        Vector3 angle0_0 = new Vector3(0, 0, -30);
        Vector3 angle0_1 = new Vector3(0, 0, 22);
        Vector3 angle1_0 = new Vector3(0, 0, 32.5f);
        Vector3 angle1_1 = new Vector3(0, 0, -36);


        if (null == mEffectQizi0)
        {

            mEffectQizi0 = UguiMaker.newImage("mEffectQizi0", transform, "goodfriendcar_sprite", "qizi", false);
            mEffectQizi1 = UguiMaker.newImage("mEffectQizi1", transform, "goodfriendcar_sprite", "qizi", false);

            mEffectQizi0.rectTransform.pivot = new Vector2(1, 1);
            mEffectQizi0.rectTransform.anchoredPosition = new Vector2(207, 498);
            mEffectQizi0.rectTransform.localEulerAngles = angle0_0;

            mEffectQizi1.transform.localScale = new Vector3(0.85f, 0.85f, 1);
            mEffectQizi1.rectTransform.pivot = new Vector2(0, 1);
            mEffectQizi1.rectTransform.anchoredPosition = new Vector2(271, 476);
            mEffectQizi1.rectTransform.localEulerAngles = angle1_0;
            
        }

        //旗子出现
        GoodFriendCarCtl.instance.mSound.PlayShortDefaultAb("旗子");
        mEffectQizi0.transform.SetAsLastSibling();
        mEffectQizi1.transform.SetAsLastSibling();
        mEffectQizi0.gameObject.SetActive(true);
        mEffectQizi1.gameObject.SetActive(true);
        for (float i = 0; i < 1f; i += 0.08f)
        {
            mEffectQizi0.rectTransform.localEulerAngles = Vector3.Lerp(angle0_0, angle0_1, i) + new Vector3(0, 0, Mathf.Sin(Mathf.PI * i) * 30);
            mEffectQizi1.rectTransform.localEulerAngles = Vector3.Lerp(angle1_0, angle1_1, i) + new Vector3(0, 0, Mathf.Sin(Mathf.PI * i) * -30);
            yield return new WaitForSeconds(0.01f);
        }
        mEffectQizi0.rectTransform.localEulerAngles = angle0_1;
        mEffectQizi1.rectTransform.localEulerAngles = angle1_1;


        //收起动物for (float i = 0; i < 1f; i += 0.1f)
        for (float i = 0; i < 1f; i += 0.1f)
        {
            for (int j = 0; j < data_animals.Count; j++)
            {
                if(1 < Mathf.Abs(data_animals[j].data_number - find_id))
                {
                    data_animals[j].transform.localEulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, -90), i);
                    data_animals[j].transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i);
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (int j = 0; j < data_animals.Count; j++)
        {
            if (1 < Mathf.Abs(data_animals[j].data_number - find_id))
            {
                GoodFriendCarCtl.instance.ResetAnimal(data_animals[j]);
            }
        }


        //车来
        GoodFriendCarCar car = GoodFriendCarCtl.instance.GetCar(0);
        car.transform.SetParent(transform);
        car.transform.SetSiblingIndex(mRtranScene.transform.GetSiblingIndex());
        for (float i = 0; i < 1f; i += 0.02f)
        {
            car.mRtran.anchoredPosition = Vector2.Lerp(new Vector2(-1000, -242), new Vector2(0, -242), Mathf.Sin(Mathf.PI * 0.5f * i));
            car.FlushLunzi();

            yield return new WaitForSeconds(0.01f);

        }
        car.mRtran.anchoredPosition = new Vector2(0, -242);

        //上车
        for(int id = find_id - 1; id <= find_id + 1; id++)
        {
            GoodFriendCarAnimal animal = null;
            for(int j = 0; j < data_animals.Count; j++)
            {
                if(data_animals[j].data_number == id)
                {
                    animal = data_animals[j];
                    break;
                }
            }

            GoodFriendCarCtl.instance.mSound.PlayShortDefaultAb("上车");

            pos0 = animal.mRtran.anchoredPosition;
            pos1 = pos0 + new Vector2(0, 150);

            for(float i = 0; i < 1f; i += 0.1f)
            {
                animal.mRtran.anchoredPosition = Vector2.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * i));
                yield return new WaitForSeconds(0.01f);
            }
            pos1 = car.SitInCar(animal);
            pos0 = animal.mRtran.anchoredPosition;
            for (float i = 0; i < 1; i += 0.1f)
            {
                animal.mRtran.anchoredPosition = Vector2.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * i));

                //float p = 1 - Mathf.Sin(Mathf.PI * 0.5f * i) * 0.1f;
                //car.transform.localScale = new Vector3(p, p, 1);
                yield return new WaitForSeconds(0.01f);
            }
            animal.mRtran.anchoredPosition = pos1;
            car.transform.localScale = Vector3.one;

        }


        //车走
        pos0 = car.mRtran.anchoredPosition;
        pos1 = pos0 + new Vector2(1000, 0);
        for (float i = 0; i < 1f; i += 0.02f)
        {
            car.mRtran.anchoredPosition = Vector2.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * (i - 1)) + 1);
            car.FlushLunzi();
            yield return new WaitForSeconds(0.01f);

        }
        car.mRtran.anchoredPosition = pos1;
        GoodFriendCarAnimal a = car.PopAnimal();
        while(null != a)
        {
            GoodFriendCarCtl.instance.ResetAnimal(a);
            a = car.PopAnimal();
        }
        GoodFriendCarCtl.instance.ResetCar(car);

        //旗子收起
        GoodFriendCarCtl.instance.mSound.PlayShortDefaultAb("旗子");
        for (float i = 0; i < 1f; i += 0.08f)
        {
            mEffectQizi0.rectTransform.localEulerAngles = Vector3.Lerp(angle0_1, angle0_0, i) + new Vector3(0, 0, Mathf.Sin(Mathf.PI * i) * 30);
            mEffectQizi1.rectTransform.localEulerAngles = Vector3.Lerp(angle1_1, angle1_0, i) + new Vector3(0, 0, Mathf.Sin(Mathf.PI * i) * -30);
            yield return new WaitForSeconds(0.01f);
        }
        mEffectQizi0.rectTransform.localEulerAngles = angle0_0;
        mEffectQizi1.rectTransform.localEulerAngles = angle1_0;
        
        mEffectQizi0.gameObject.SetActive(false);
        mEffectQizi1.gameObject.SetActive(false);

        if(play_num == play_max_num)
        {
            GoodFriendCarCtl.instance.Goto2();
        }
        else
        {
            StartGame();
        }

    }

}
