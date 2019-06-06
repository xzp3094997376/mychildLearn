using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Spine.Unity;

public class AnimalPartyGuanka1 : MonoBehaviour
{
    public RectTransform mRtran { get; set; }

    Image mBg0 { get; set; }
    Image mBg1 { get; set; }
    Image mYun3 { get; set; }
    Image mRi { get; set; }
    SkeletonGraphic mSpine { get; set; }
    List<AnimalPartyBalloon> mBalloons { get; set; }
    RectTransform mHandR = null;
    RectTransform mHandL = null;
    Text mText { get; set; }
    GameObject mInput { get; set; }


    List<int> data1_hand_sides = null;
    List<int> data1_balloon_ids = null;
    int data1_cur_side_index = 0;//1右，0左
    //第二关数据
    List<int> data2_balloon_que = null;
    int data2_count_side = 0;//问题，从那边开始数0-左，1-右
    int data2_count_side_number = 0;//问题，第几个
    //第3关数据
    List<int> data3_balloon_que = null;
    int data3_select_balloon_id = 0;
    int data3_side = 0;
    int data3_question_balloon_id = 0;
    //第4关数据
    int data4_play_count = 0;
    List<int> data4_balloon_que = null;
    List<int> data4_find_balloon_ids = null;//先是左边，再是右边




    //当前小关
    //1-k宝
    //2-从左/右边数起，第X个气球是哪个
    //3-红/粉/蓝/黄/紫/白/绿气球的左/右边是什么气球？
    //4-从左起，红/粉/蓝/黄/紫/白/绿气球排在第几？从右起，红/粉/蓝/黄/紫/白/绿气球排在第几？
    public int cur_guanka = 1;

    //Vector3 temp_drag_pos = Vector3.zero;
    public void Update()
    {
        if (AnimalPartyCtl.instance.mGuanka.guanka != 1)
            return;
        
        if(1 == cur_guanka)
        {
            Update1();
        }
        else if(2 == cur_guanka)
        {
            Update2();
        }
        else if (3 == cur_guanka)
        {
            Update3();
        }



    }
    public void Update1()
    {
        if (null != mSpine)
        {

            //Spine.BoneData bone_r = mSpine.SkeletonData.FindBone("R");//mSpine.AnimationState.Data.SkeletonData.FindBone("R");
            Spine.Bone bone_r = mSpine.Skeleton.FindBone("L");
            Spine.Bone bone_l = mSpine.Skeleton.FindBone("R");
            if (null != bone_r)
            {
                Vector3 pos = new Vector3(bone_r.WorldX, bone_r.WorldY, 0);
                pos = mSpine.transform.worldToLocalMatrix.MultiplyPoint(pos);
                pos.z = 0;
                mHandR.anchoredPosition3D = pos * 0.165f + new Vector3(5, -85, 0);
            }
            if (null != bone_l)
            {
                Vector3 pos = new Vector3(bone_l.WorldX, bone_l.WorldY, 0);
                pos = mSpine.transform.worldToLocalMatrix.MultiplyPoint(pos);
                pos.z = 0;
                mHandL.anchoredPosition3D = pos * 0.165f + new Vector3(-5, -85, 0);
            }


        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                AnimalPartyBalloon.gSelect = hit.collider.gameObject.GetComponent<AnimalPartyBalloon>();
                if (null != AnimalPartyBalloon.gSelect)
                {
                    AnimalPartyBalloon.gSelect.transform.SetAsLastSibling();
                    //temp_drag_pos = hit.collider.gameObject.transform.position - Common.getMouseWorldPos();
                    AnimalPartyBalloon.gSelect.Select();

                    AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "选住气球");
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (null != AnimalPartyBalloon.gSelect)
            {
                //Vector3 pos = transform.worldToLocalMatrix.MultiplyPoint(temp_drag_pos + Common.getMouseWorldPos());
                Vector3 pos = transform.worldToLocalMatrix.MultiplyPoint(Common.getMouseWorldPos());
                pos.z = 0;
                AnimalPartyBalloon.gSelect.mRtran.anchoredPosition3D = (pos + AnimalPartyBalloon.gSelect.mRtran.anchoredPosition3D) * 0.5f;
                //Debug.Log(pos);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {

            if (null != AnimalPartyBalloon.gSelect)
            {
                GameObject hand = null;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray, 100);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject.name.Equals("mHandR"))
                    {
                        if (data1_balloon_ids[data1_cur_side_index] == AnimalPartyBalloon.gSelect.mId)
                        {
                            if (data1_hand_sides[data1_cur_side_index] == 1)
                            {
                                hand = hit.collider.gameObject;
                            }
                            else
                            {
                                //不对呢，你再想想！
                                AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-wrong6-1-2");
                                AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "选错飞回去音效");
                            }
                        }
                        else
                        {
                            //这个不是我要的气球呢！
                            AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-wrong6-1-1");
                            AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "选错飞回去音效");
                        }
                        break;
                    }
                    else if (hit.collider.gameObject.name.Equals("mHandL"))
                    {

                        if (data1_balloon_ids[data1_cur_side_index] == AnimalPartyBalloon.gSelect.mId)
                        {
                            if (data1_hand_sides[data1_cur_side_index] == -1)
                            {
                                hand = hit.collider.gameObject;
                            }
                            else
                            {
                                //不对呢，你再想想！
                                AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-wrong6-1-2");
                                AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "选错飞回去音效");
                            }
                        }
                        else
                        {
                            //这个不是我要的气球呢！
                            AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-wrong6-1-1");
                            AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "选错飞回去音效");
                        }
                        break;

                    }
                }

                if (null != hand && data1_cur_side_index <= 2)
                {
                    //正确
                    //对对，正是我想要的！
                    AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-right6-1-1");
                    AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "k宝拿住气球的声音");
                    AnimalPartyBalloon.gSelect.transform.SetParent(hand.transform);
                    AnimalPartyBalloon.gSelect.mRtran.anchoredPosition3D = Vector3.zero;
                    AnimalPartyBalloon.gSelect.SetMoveState();


                    //mSpine.AnimationState.ClearTrack(1);
                    if (0 == data1_cur_side_index)
                    {
                        mSpine.AnimationState.ClearTrack(1);
                        if (-1 == data1_hand_sides[data1_cur_side_index])
                        {
                            mSpine.AnimationState.AddAnimation(1, "back_to_face", false, 0);
                            mSpine.AnimationState.AddAnimation(1, "face_handup_left", false, 0);
                            mSpine.AnimationState.AddAnimation(1, "face_left_handup", true, 0);
                        }
                        else
                        {
                            mSpine.AnimationState.AddAnimation(1, "back_to_face", false, 0);
                            mSpine.AnimationState.AddAnimation(1, "face_handup_right", false, 0);
                            mSpine.AnimationState.AddAnimation(1, "face_right_handup", true, 0);
                        }
                        data1_cur_side_index++;
                        Invoke("PlayTip", 3);
                    }
                    else
                    {
                        mSpine.AnimationState.AddAnimation(1, "face_handup_all", false, 0);
                        mSpine.AnimationState.SetAnimation(1, "face_all_handup", true);
                        data1_cur_side_index++;
                        StartCoroutine("TSetGuanka2");
                    }

                }
                else
                {
                    //错误
                    AnimalPartyBalloon.gSelect.UnSelect();
                }
                AnimalPartyBalloon.gSelect = null;

            }


        }

    }
    public void Update2()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                AnimalPartyBalloon.gSelect = hit.collider.gameObject.GetComponent<AnimalPartyBalloon>();
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if(AnimalPartyBalloon.gSelect == hit.collider.gameObject.GetComponent<AnimalPartyBalloon>())
                {
                    int target_id = 0;
                    if(0 == data2_count_side)
                    {
                        target_id = data2_balloon_que[data2_count_side_number - 1];
                    }
                    else
                    {
                        target_id = data2_balloon_que[7 - data2_count_side_number];
                    }
                    if(AnimalPartyBalloon.gSelect.mId == target_id)
                    {
                        //对对，正是我想要的！
                        //AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-right6-1-1");
                        AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", GetBalloonSoundName(target_id));
                        
                        //正确
                        AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "气球上升");
                        for (int i = 0; i < 7; i++)
                        {
                            if(mBalloons[i].mId == target_id)
                            {
                                mBalloons[i].PlayFlyOut();
                            }
                            else
                            {
                                mBalloons[i].PlayLiHua();
                            }
                            mBalloons[i].BoxEnable(false);
                        }
                        StartCoroutine("TSetGuanka3");
                    }
                    else
                    {
                        //错误
                        AnimalPartyBalloon.gSelect.PlaySake();
                        //不对呢，你再想想！
                        AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-wrong6-1-2");

                    }
                }
            }
            AnimalPartyBalloon.gSelect = null;
        }
    }
    public void Update3()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                AnimalPartyBalloon.gSelect = hit.collider.gameObject.GetComponent<AnimalPartyBalloon>();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (AnimalPartyBalloon.gSelect == hit.collider.gameObject.GetComponent<AnimalPartyBalloon>())
                {

                    if (AnimalPartyBalloon.gSelect.mId == data3_select_balloon_id)
                    {
                        //对对，正是我想要的！
                        //AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-right6-1-1");
                        AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", GetBalloonSoundName(data3_select_balloon_id));
                        //正确
                        AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "气球上升");
                        for (int i = 0; i < 7; i++)
                        {
                            if (mBalloons[i].mId == data3_select_balloon_id)
                            {
                                mBalloons[i].PlayFlyOut();
                            }
                            else
                            {
                                mBalloons[i].PlayLiHua();
                            }
                            mBalloons[i].BoxEnable(false);
                        }
                        StartCoroutine("TSetGuanka4");
                    }
                    else
                    {
                        //错误
                        AnimalPartyBalloon.gSelect.PlaySake();
                        //不对呢，你再想想！
                        AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-wrong6-1-2");

                    }
                }
            }
            AnimalPartyBalloon.gSelect = null;
        }
    }

    public void Init ()
    {

        if (null == mBg0)
        {
            mRtran = GetComponent<RectTransform>();

            mBg0 = UguiMaker.newImage("mBg0", transform, "animalparty_sprite", "bg0", false);
            mBg0.rectTransform.sizeDelta = new Vector2(1423, 1600);

            mBg1 = UguiMaker.newImage("mBg1", transform, "animalparty_sprite", "bg1", false);
            mBg1.rectTransform.anchoredPosition3D = new Vector3(0, -800 + 92, 0);

            mYun3 = UguiMaker.newImage("mYun3", transform, "animalparty_sprite", "yun3", false);
            mRi = UguiMaker.newImage("mRi", transform, "animalparty_sprite", "ri", false);

            mSpine = ResManager.GetPrefab("animalparty_prefab", "kbao").GetComponent<SkeletonGraphic>();
            UguiMaker.InitGameObj(mSpine.gameObject, transform, "kbao", new Vector3(0, -636, 0), new Vector3(0.65f, 0.65f, 1));


            mHandR = UguiMaker.newGameObject("mHandR", mSpine.transform).GetComponent<RectTransform>();
            mHandR.gameObject.AddComponent<BoxCollider>().size = new Vector3(150, 150, 1);
            mHandL = UguiMaker.newGameObject("mHandL", mSpine.transform).GetComponent<RectTransform>();
            mHandL.gameObject.AddComponent<BoxCollider>().size = new Vector3(150, 150, 1);

            mText = UguiMaker.newText("text", transform);
            mText.color = new Color32(255, 254, 204, 255);
            mText.fontSize = 65;
            mText.rectTransform.sizeDelta = new Vector2(1280, 100);
            mText.rectTransform.anchoredPosition = new Vector2(0, -164);
            mText.alignment = TextAnchor.MiddleCenter;
            mText.gameObject.SetActive(false);

            mInput = UguiMaker.newGameObject("input", mText.transform);
            mInput.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -81.7f, 0);

            Image input_bg = UguiMaker.newImage("bg", mInput.transform, "public", "inputbg", false);
            input_bg.rectTransform.sizeDelta = new Vector2(1064, 85.3f);
            input_bg.color = new Color32(45, 127, 167, 255);
            input_bg.type = Image.Type.Sliced;

            GameObject input_btns = UguiMaker.newGameObject("btns", mInput.transform);
            GridLayoutGroup input_group = input_btns.AddComponent<GridLayoutGroup>();
            input_group.cellSize = new Vector2(110.9f, 68.2f);
            input_group.spacing = new Vector2(5.7f, 3.8f);
            input_group.startCorner = GridLayoutGroup.Corner.UpperLeft;
            input_group.startAxis = GridLayoutGroup.Axis.Horizontal;
            input_group.childAlignment = TextAnchor.MiddleCenter;
            input_group.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            input_group.constraintCount = 9;

            for(int i = 1; i <= 9; i++)
            {
                Image input_btn = UguiMaker.newImage(i.ToString(), input_group.transform, "public", "inputbg", true);
                Image input_btn_contex = UguiMaker.newImage("btn", input_btn.transform, "public", "input" + i.ToString(), false);
                input_btn_contex.transform.localScale = new Vector3(1.2f, 1.2f, 1);
                input_btn_contex.color = new Color32(45, 127, 167, 255);
                //input_btn.gameObject.AddComponent<Button>().onClick.AddListener();
                EventTriggerListener.Get(input_btn.gameObject).onClick = OnClkTextBtn;
            }


        }

        if(null != mBalloons)
        {
            for(int i = 0; i < mBalloons.Count; i++)
            {
                mBalloons[i].mRtran.anchoredPosition3D = new Vector3(2000, 2000, 0);
            }
        }


        mInput.gameObject.SetActive(false);

        mYun3.rectTransform.anchoredPosition = new Vector2(251, 77);
        mRi.rectTransform.anchoredPosition = new Vector2(-429, 563);
        mYun3.transform.localScale = Vector3.one;

        PlayRi();

        mSpine.AnimationState.SetAnimation(0, "face_idle", true);

        int temp_int = 0;
        //第一关数据
        data1_hand_sides = new List<int>() { 1, -1 };
        data1_balloon_ids = Common.GetMutexValue(1, 7, 2);
        data1_cur_side_index = 0;
        //第二关数据
        data2_balloon_que = Common.BreakRank<int>(new List<int>() { 1, 2, 3, 4, 5, 6, 7});
        data2_count_side = Random.Range(0, 1000) % 2;
        data2_count_side_number = Random.Range(0, 1000) % 7 + 1;
        //第三关
        data3_balloon_que = Common.BreakRank<int>(new List<int>() { 1, 2, 3, 4, 5, 6, 7 });
        temp_int = Random.Range(0, 1000) % 7;
        data3_select_balloon_id = data3_balloon_que[temp_int];
        if (temp_int == 0)
        {
            data3_side = 0;
        }
        else if (temp_int == 6)
        {
            data3_side = 1;
        }
        else
        {
            data3_side = Random.Range(0, 1000) % 2;
        }
        if(0 == data3_side)
        {
            data3_question_balloon_id = data3_balloon_que[temp_int + 1];
        }
        else
        {
            data3_question_balloon_id = data3_balloon_que[temp_int - 1];
        }
        //第四关
        //data4_balloon_que = Common.BreakRank<int>(new List<int>() { 1, 2, 3, 4, 5, 6, 7 });
        //data4_find_balloon_ids = Common.GetMutexValue(1, 7, 2);
        //data4_play_count = 0;


        cur_guanka = 1;

    }
	
    public void PlayRi()
    {
        StopCoroutine("TRi");
        StartCoroutine("TRi");
    }
    public void PlayYun3()
    {
        StartCoroutine("TYun");
    }
    public void PlayGuanka()
    {
        StopCoroutine("TPlayGuanka");
        StartCoroutine("TPlayGuanka");
    }
    public void StopRi()
    {
        StopCoroutine("TRi");
    }
    public void StopAll()
    {
    }
    public void MoveText(bool move_in)
    {
        StartCoroutine(TMoveText(move_in));
    }


    IEnumerator TRi()
    {
        while(true)
        {
            mRi.transform.localEulerAngles += new Vector3(0, 0, 12);
            yield return new WaitForSeconds(0.2f);
        }
    }
    IEnumerator TYun()
    {
        Vector2 pos = mYun3.rectTransform.anchoredPosition;
        for(float i = 0; i < 1f; i += 0.04f)
        {
            mYun3.rectTransform.anchoredPosition = Vector2.Lerp(pos, new Vector2(0, -400), 1);
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator TPlayGuanka()
    {
        
        //Debug.LogError("开场对白");
        //嗨
        AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-tips6-2-1");
        
        mSpine.timeScale = 0.5f;
        mSpine.AnimationState.AddAnimation(1, "face_handup_all", false, 0);
        mSpine.AnimationState.AddAnimation(1, "face_handup_all", false, 0);
        mSpine.AnimationState.AddAnimation(1, "face_handup_all", false, 0);
        mSpine.AnimationState.AddAnimation(1, "face_handup_all", false, 0);
        mSpine.AnimationState.AddAnimation(1, "face_handup_all", false, 0);
        mSpine.AnimationState.AddAnimation(1, "face_handup_all", false, 0);
        mSpine.AnimationState.AddAnimation(1, "face_handup_all", false, 0);
        mSpine.AnimationState.AddAnimation(1, "face_handup_all", false, 0);
        mSpine.AnimationState.AddAnimation(1, "face_handup_all", false, 0);
        mSpine.AnimationState.AddAnimation(1, "face_handup_all", false, 0);
        yield return new WaitForSeconds(1.25f);
        
        if(Application.platform == RuntimePlatform.Android)
        {
            //小朋友名字
            if(0 != AndroidDataCtl.GetDataFromAndroid<int>("sayChildName"))
            {
                yield return new WaitForSeconds(1.3f);
            }
        }
        
        //谢谢经常来看我
        AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-tips6-2-2");
        yield return new WaitForSeconds(3f);
        //跟我一起来来做操
        AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-tips6-2-3");
        mSpine.timeScale = 1;
        mSpine.AnimationState.ClearTrack(1);
        mSpine.AnimationState.AddAnimation(1, "face_to_back", false, 0);
        mSpine.AnimationState.AddAnimation(1, "back_idle", true, 0);
        yield return new WaitForSeconds(2.3f);
        //举左手
        mSpine.AnimationState.AddAnimation(1, "back_handup_left", false, 0);
        mSpine.AnimationState.AddAnimation(1, "back_left_handup", true, 0);
        yield return new WaitForSeconds(1.2f);
        AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-tips6-2-4");
        yield return new WaitForSeconds(1f);
        //举右手
        mSpine.AnimationState.AddAnimation(1, "back_handup_right", false, 0);
        mSpine.AnimationState.AddAnimation(1, "back_right_handup", true, 0);
        yield return new WaitForSeconds(2f);
        AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-tips6-2-5");
        yield return new WaitForSeconds(1.5f);
        //放下双手转身
        mSpine.timeScale = 0.5f;
        mSpine.AnimationState.ClearTracks();
        mSpine.AnimationState.AddAnimation(1, "back_to_face", false, 0);
        mSpine.AnimationState.AddAnimation(1, "face_handup_all", false, 0);
        mSpine.AnimationState.AddAnimation(1, "face_handup_all", false, 0);
        mSpine.AnimationState.AddAnimation(1, "face_handup_all", false, 0);
        mSpine.AnimationState.AddAnimation(1, "face_idle", true, 0);
        yield return new WaitForSeconds(2f);
        
        
        //我们玩点别的吧！展示左右手拿什么气球
        AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-tips6-2-6");
        AnimalPartyBalloon ball_right = UguiMaker.newGameObject("ball_right", mSpine.transform).AddComponent<AnimalPartyBalloon>();
        AnimalPartyBalloon ball_left = UguiMaker.newGameObject("ball_left", mSpine.transform).AddComponent<AnimalPartyBalloon>();
        List<int> temp_ball_id = Common.GetMutexValue(1, 7, 2);
        ball_right.Init(temp_ball_id[0], new Vector3(-226, 63, 0));
        ball_left.Init(temp_ball_id[1], new Vector3(226, 63, 0));
        ball_right.BoxEnable(false);
        ball_left.BoxEnable(false);
        ball_right.transform.localScale = Vector3.one;
        ball_left.transform.localScale = Vector3.one;
        for(float i = 0; i < 1f; i += 0.01f)
        {
            ball_right.mRtran.anchoredPosition = Vector2.Lerp(new Vector2(-226, 958), mHandR.anchoredPosition, i);
            ball_left.mRtran.anchoredPosition = Vector2.Lerp(new Vector2(226, 958), mHandL.anchoredPosition, i);
            yield return new WaitForSeconds(0.01f);
        }
        ball_right.transform.SetParent(mHandR);
        ball_left.transform.SetParent(mHandL);
        ball_right.mRtran.anchoredPosition = Vector2.zero;
        ball_left.mRtran.anchoredPosition = Vector2.zero;
        yield return new WaitForSeconds(1f);
        AnimalPartyCtl.instance.mSound.PlayTipList( new List<string>() { "animalparty_sound", "animalparty_sound" }, new List<string>() { "tip我的右手拿了", GetBalloonSoundName(temp_ball_id[0])});
        mSpine.AnimationState.ClearTracks();
        mSpine.AnimationState.AddAnimation(1, "face_handup_right", false, 0);
        mSpine.AnimationState.AddAnimation(1, "face_right_handup", false, 0);
        mSpine.AnimationState.AddAnimation(1, "face_idle", true, 0);
        yield return new WaitForSeconds(6.5f);
        AnimalPartyCtl.instance.mSound.PlayTipList(new List<string>() { "animalparty_sound", "animalparty_sound" }, new List<string>() { "tip我的左手拿了", GetBalloonSoundName(temp_ball_id[1]) });
        mSpine.AnimationState.ClearTracks();
        mSpine.AnimationState.AddAnimation(1, "face_handup_left", false, 0);
        mSpine.AnimationState.AddAnimation(1, "face_left_handup", false, 0);
        mSpine.AnimationState.AddAnimation(1, "face_idle", true, 0);
        yield return new WaitForSeconds(6.5f);
        ball_right.transform.SetParent(transform);
        ball_left.transform.SetParent(transform);
        ball_right.PlayFlyOut();
        ball_left.PlayFlyOut();


        //yield break;
        //我们玩点别的吧,我想要一些气球，你能帮帮我吗？
        Debug.LogError("气球升起");
        if (null == mBalloons)
        {
            mBalloons = new List<AnimalPartyBalloon>();
            for (int i = 0; i < 21; i++)
            {
                mBalloons.Add(UguiMaker.newGameObject("balloon" + i, transform).AddComponent<AnimalPartyBalloon>());
            }
        }
        
        AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "气球上升");
        for (int i = 0; i < mBalloons.Count; i++)
        {
            mBalloons[i].Init(i % 7 + 1, new Vector3(Random.Range( -600, 600), Random.Range(-400, -520), 0));
            mBalloons[i].PlayFlyUp();
            mBalloons[i].transform.SetSiblingIndex(mBg0.transform.GetSiblingIndex() + 1);
            mBalloons[i].BoxEnable(false);
        }
        yield return new WaitForSeconds(2f);

        //转身向后
        mSpine.timeScale = 0.5f;
        mSpine.AnimationState.ClearTrack(1);
        mSpine.AnimationState.AddAnimation(1, "face_to_back", false, 0);
        mSpine.AnimationState.AddAnimation(1, "back_idle", true, 0);
        yield return new WaitForSeconds(1.5f);
        mSpine.timeScale = 0.5f;
        for (int i = 0; i < mBalloons.Count; i++)
        {
            mBalloons[i].BoxEnable(true);
        }


        //我想要一些气球，你能帮帮我吗？
        //AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-tips6-2-7");
        //yield return new WaitForSeconds(4.6f);
        PlayTip();
        yield break;


    }
    IEnumerator TSetGuanka2()
    {

        yield return new WaitForSeconds(3f);
        cur_guanka = 2;
        for (int i = 0; i < 7; i++)
        {
            mBalloons[i].SetMoveState();
        }

        List<Vector3> poss = Common.PosSortByWidth(1280, 7, 42.32f);//Common.PosSortByWidth(1280, 7, -80);
        List<Vector3> poss_temp = new List<Vector3>();
        for (int i = 0; i < mBalloons.Count; i++)
        {
            mBalloons[i].transform.SetParent(transform);
            mBalloons[i].transform.SetAsLastSibling();
            if (i < 7)
            {
                poss_temp.Add(mBalloons[i].mRtran.anchoredPosition3D);
            }
            else
            {
                mBalloons[i].PlayLiHua();
                yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
            }
        }

        AnimalPartyCtl.instance.NextGuanka();
        AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "气球上升");


        mSpine.AnimationState.SetAnimation(1, "face_idle", true);
        for (float i = 0; i < 1f; i += 0.02f)
        {
            for(int j = 0; j < 7; j++)
            {
                mBalloons[j].mRtran.anchoredPosition = Vector2.Lerp(
                    poss_temp[j],
                    poss[data2_balloon_que.IndexOf(mBalloons[j].mId)],
                    i);
            }
            yield return new WaitForSeconds(0.01f);
        }

        for (int i = 0; i < 7; i++)
        {
            mBalloons[i].BoxEnable(true);
            mBalloons[i].mResetPos = mBalloons[i].mRtran.anchoredPosition3D;
            mBalloons[i].StayInSky();
        }
        MoveText(true);
        PlayTip();

    }
    IEnumerator TSetGuanka3()
    {
        MoveText(false);
        yield return new WaitForSeconds(3);
        cur_guanka = 3;
        for (int i = 0; i < 7; i++)
        {
            mBalloons[i].SetMoveState();
            mBalloons[i].transform.SetParent(transform);
            mBalloons[i].mRtran.anchoredPosition3D = new Vector3(9999, 9999, 0);
        }

        AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "气球上升");
        List<Vector3> poss_beg = Common.PosSortByWidth(1280, 7, -600);
        List<Vector3> poss_end = Common.PosSortByWidth(1280, 7, -42.32f);
        int data3_que_index = 0;
        for (float i = 0; i < 1f; i += 0.02f)
        {
            for (int j = 0; j < 7; j++)
            {
                data3_que_index = data3_balloon_que.IndexOf(mBalloons[j].mId);
                mBalloons[j].mRtran.anchoredPosition = Vector2.Lerp(
                    poss_beg[data3_que_index],
                    poss_end[data3_que_index],
                    i);
            }
            yield return new WaitForSeconds(0.01f);
        }

        for (int i = 0; i < 7; i++)
        {
            mBalloons[i].BoxEnable(true);
            mBalloons[i].mResetPos = mBalloons[i].mRtran.anchoredPosition3D;
            mBalloons[i].StayInSky();
        }

        MoveText(true);
        PlayTip();

    }
    IEnumerator TSetGuanka4()
    {
        MoveText(false);
        if(0 == data4_play_count)
        {
            yield return new WaitForSeconds(3);
        }
        else
        {
            yield return new WaitForSeconds(2);

        }


        data4_balloon_que = Common.BreakRank<int>(new List<int>() { 1, 2, 3, 4, 5, 6, 7 });
        data4_find_balloon_ids = Common.GetMutexValue(1, 7, 2);
        cur_guanka = 4;
        if(data4_play_count > 1)
        {
            data4_play_count = 0;
        }

        for (int i = 0; i < 7; i++)
        {
            mBalloons[i].SetMoveState();
            mBalloons[i].transform.SetParent(transform);
            mBalloons[i].mRtran.anchoredPosition3D = new Vector3(9999, 9999, 0);
        }

        AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "气球上升");
        List<Vector3> poss_beg = Common.PosSortByWidth(1280, 7, -600);
        List<Vector3> poss_end = Common.PosSortByWidth(1280, 7, 42.32f);
        int data4_que_index = 0;
        for (float i = 0; i < 1f; i += 0.02f)
        {
            for (int j = 0; j < 7; j++)
            {
                data4_que_index = data4_balloon_que.IndexOf(mBalloons[j].mId);
                mBalloons[j].mRtran.anchoredPosition = Vector2.Lerp(
                    poss_beg[data4_que_index],
                    poss_end[data4_que_index],
                    i);
            }
            yield return new WaitForSeconds(0.01f);
        }

        for (int i = 0; i < 7; i++)
        {
            mBalloons[i].BoxEnable(true);
            mBalloons[i].mResetPos = mBalloons[i].mRtran.anchoredPosition3D;
            mBalloons[i].StayInSky();
        }


        mText.gameObject.SetActive(true);
        MoveText(true);
        PlayTip();
        mInput.gameObject.SetActive(true);


    }
    IEnumerator TGameOver()
    {
        TopTitleCtl.instance.mSoundTipData.Clean();
        MoveText(false);
        yield return new WaitForSeconds(2.5f);
        mText.gameObject.SetActive(false);

        AnimalPartyCtl.instance.NextGuanka();
    }
    IEnumerator TMoveText(bool move_in)
    {
        mText.gameObject.SetActive(true);
        if (move_in)
        {
            if (mText.rectTransform.anchoredPosition.y > -200)
                yield break;
            Vector3 endpos = Vector3.zero;
            if(4 == cur_guanka)
            {
                endpos = new Vector3(0, -124.2f, 0);
            }
            else
            {
                endpos = new Vector3(0, -164f, 0);
            }
            for (float i = 0; i < 1f; i += 0.02f)
            {
                mText.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(0, -353), endpos, Mathf.Sin(Mathf.PI * 0.5f * i));
                yield return new WaitForSeconds(0.01f);
            }
            mText.rectTransform.anchoredPosition = endpos;
        }
        else
        {
            if (mText.rectTransform.anchoredPosition.y < -200)
                yield break;
            for (float i = 0; i < 1f; i += 0.02f)
            {
                mText.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(0, -164f), new Vector2(0, -353), Mathf.Sin(Mathf.PI * 0.5f * i));
                yield return new WaitForSeconds(0.01f);
            }
            mText.rectTransform.anchoredPosition = new Vector2(0, -353);
        }

    }

    void PlayTip()
    {
        switch(cur_guanka)
        {
            case 1:
                {
                    string hand = "";
                    if (data1_hand_sides[data1_cur_side_index] == -1)
                    {
                        //我的左手要拿
                        hand = "game-tips6-2-8";
                    }
                    else
                    {
                        //我的右手要拿
                        hand = "game-tips6-2-9";
                    }
                    string balloon_name = GetBalloonSoundName(data1_balloon_ids[data1_cur_side_index]);
                  
                    AnimalPartyCtl.instance.mSound.PlayTipList(
                        new List<string>() { "animalparty_sound", "animalparty_sound" },
                        new List<string>() { hand, balloon_name }, true);
                    Debug.LogError("请把" + balloon_name + "色的气球，放在" + hand + "上");
                }
                break;
            case 2:
                {
                    
                    if(0 == data2_count_side)
                    {
                        //从左边数起,第,,个气球是哪个
                        AnimalPartyCtl.instance.mSound.PlayTipList(
                            new List<string>() { "animalparty_sound", "animalparty_sound", "number_sound", "animalparty_sound" },
                            new List<string>() { "game-tips6-2-17", "game-tips6-2-19", data2_count_side_number.ToString(), "game-tips6-2-20" }, true);
                        mText.text = "从左边数起,第" + data2_count_side_number.ToString() + "个气球是哪个?";
                    }
                    else
                    {
                        //从右边数起,第,,个气球是哪个
                        AnimalPartyCtl.instance.mSound.PlayTipList(
                            new List<string>() { "animalparty_sound", "animalparty_sound", "number_sound", "animalparty_sound" },
                            new List<string>() { "game-tips6-2-18", "game-tips6-2-19", data2_count_side_number.ToString(), "game-tips6-2-20" }, true);
                        mText.text = "从右边数起,第" + data2_count_side_number.ToString() + "个气球是哪个?";

                    }

                }
                break;
            case 3:
                {
                    string balloon_name = GetBalloonSoundName(data3_question_balloon_id);
                    //game-tips6-2-21:左边是什么气球
                    //game -tips6-2-22:右边是什么气球,

                    //string balloon_side = "";
                    if (data3_side == 0)
                    {
                        AnimalPartyCtl.instance.mSound.PlayTipList(
                            new List<string>() { "animalparty_sound", "animalparty_sound" },
                            new List<string>() { balloon_name, "game-tips6-2-21" }, true);

                        //balloon_side = "game-tips6-2-21";
                        mText.text = GetBalloonSoundContext(data3_question_balloon_id) + "左边是什么气球?";

                    }
                    else
                    {
                        AnimalPartyCtl.instance.mSound.PlayTipList(
                            new List<string>() { "animalparty_sound", "animalparty_sound" },
                            new List<string>() { balloon_name, "game-tips6-2-22" }, true);
                        //balloon_side = "game-tips6-2-22";
                        mText.text = GetBalloonSoundContext(data3_question_balloon_id) + "右边是什么气球?";

                    }





                }
                break;
            case 4:
                {
                    if(0 == data4_play_count)
                    {
                        //从左起
                        string balloon_name = GetBalloonSoundName(data4_find_balloon_ids[data4_play_count]);
                        AnimalPartyCtl.instance.mSound.PlayTipList(
                            new List<string>() { "animalparty_sound", "animalparty_sound", "animalparty_sound" },
                            new List<string>() { "game-tips6-2-23", balloon_name, "game-tips6-2-25" }, true);

                        mText.text = "从左边起" + GetBalloonSoundContext(data4_find_balloon_ids[data4_play_count]) + "排第几？";

                    }
                    else
                    {
                        //从右起
                        string balloon_name = GetBalloonSoundName(data4_find_balloon_ids[data4_play_count]);
                        AnimalPartyCtl.instance.mSound.PlayTipList(
                            new List<string>() { "animalparty_sound", "animalparty_sound", "animalparty_sound" },
                            new List<string>() { "game-tips6-2-24", balloon_name, "game-tips6-2-25" }, true);


                        mText.text = "从右边起" + GetBalloonSoundContext(data4_find_balloon_ids[data4_play_count]) + "排第几？";

                    }
                    

                }
                break;
        }
       

    }
    public string GetBalloonSoundName(int balloon_id)
    {
        switch (balloon_id)
        {
            case 1:
                //白气球
                return "game-tips6-2-15";
            case 2:
                //绿气球
                return "game-tips6-2-16";
            case 3:
                //紫气球
                return "game-tips6-2-14";
            case 4:
                //粉气球
                return "game-tips6-2-11";
            case 5:
                //红气球
                return "game-tips6-2-10";
            case 6:
                //黄气球
                return "game-tips6-2-13";
            case 7:
                //蓝气球
                return "game-tips6-2-12";
        }
        return "";
    }
    public string GetBalloonSoundContext(int balloon_id)
    {
        switch (balloon_id)
        {
            case 1:
                //白气球
                return "白气球";
            case 2:
                //绿气球
                return "绿气球";
            case 3:
                //紫气球
                return "紫气球";
            case 4:
                //粉气球
                return "粉气球";
            case 5:
                //红气球
                return "红气球";
            case 6:
                //黄气球
                return "黄气球";
            case 7:
                //蓝气球
                return "蓝气球";
        }
        return "";
    }
    public void OnClkTextBtn(GameObject obj)
    {
        SoundManager.instance.PlayShort("inputnumclick");
        Global.instance.PlayBtnClickAnimation(obj.transform);
        Debug.LogError(obj.name);

        int number = int.Parse(obj.name);
        int answer = 0;
        if (0 == data4_play_count)
        {
            for (int i = 0; i < 7; i++)
            {
                if (data4_find_balloon_ids[data4_play_count] == data4_balloon_que[i])
                {
                    answer = i + 1;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < 7; i++)
            {
                if (data4_find_balloon_ids[data4_play_count] == data4_balloon_que[6 - i])
                {
                    answer = i + 1;
                    break;
                }
            }
        }
        if (number == answer)
        {
            //正确,嗯嗯，你做的很好！
            AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-right6-1-2");
            //正确
            AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "气球上升");
            for (int i = 0; i < 7; i++)
            {
                if (mBalloons[i].mId == data4_find_balloon_ids[data4_play_count])
                {
                    mBalloons[i].PlayFlyOut();
                }
                else
                {
                    mBalloons[i].PlayLiHua();
                }
                mBalloons[i].BoxEnable(false);
            }

            if (0 == data4_play_count)
            {
                data4_play_count++;
                StartCoroutine("TSetGuanka4");
            }
            else
            {
                StartCoroutine("TGameOver");
            }
        }
        else
        {
            //错误,不对呢，你再想想！
            AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-wrong6-1-2");

        }
    }

}
