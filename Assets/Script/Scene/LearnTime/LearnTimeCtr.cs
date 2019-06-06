using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LearnTimeCtr : MonoBehaviour {
    public Guanka mGuanka = new Guanka();
    private GameObject mClockGo { get; set; }
    private GameObject currClockGo { get; set; }
    private GameObject inputGo { get; set; }
    public static Image chooseImage { get; set; }
    private EquationInput inputobj { get; set; }
    private string currType { get; set; }
    private ParticleSystem mparticle { get; set; }
    private ParticleSystem mStarparticle { get; set; }
    private ParticleSystem mparticleInput { get; set; }
    public SoundManager mSound { get; set; }
    private LeanrTimeSpine spine { get; set; }
    private GameObject roleScane { get; set; }
    private bool inpass { get; set; }
    public class Guanka
    {
        public int guanka { get; set; }
        public int guanka_last { get; set; }
        public int gameTimes { get; set; }
        public int gameTimes_last { get; set; }
        public string clockbgType { get; set; }
        public string clockType { get; set; }
        public string lineType { get; set; }
        public string centerType { get; set; }
        public int currH { get; set; }
        public int currM { get; set; }
        public int inputH { get; set; }
        public int inputM { get; set; }

        public float angleH { get; set; }
        public float angleM { get; set; }

        public bool roundH { get; set; }
        public bool roundM { get; set; }

        public Vector3 clockPos { get; set; }

        public int timeNumM { get; set; }
        public int timeNumH { get; set; }
        public string gametips { get; set; }

        public int lastNumM { get; set; }
        public int lastNumH { get; set; }

        public string spineName { get; set; }
        public bool isface { get; set; }

        public float ScleClock{ get; set; }

        public float roleScane { get; set; }

        public Guanka()
        {
            gameTimes = 1;
            guanka_last = 3;
        }
        public void Set(int _guanka)
        {
            ScleClock = 1;
            inputH = -1;
            inputM = -1;
            clockbgType = "clock_1_" + Common.GetMutexValue(1,10) + "_";
            clockType = "clock_1_nomal_";
            lineType = "clock_1_nomal_";
            centerType = "clock_1_nomal_";

            switch (_guanka)
            {
                case 1:
                    timeNumH = Common.BreakRank(new List<int>() { 3,4,8,9,10})[0];// Common.GetRandValue(0, 11);

                    gametips = "game-tips5-1-1";
                    timeNumM = Common.GetMutexValue(0,1);
                    int bgindex = Common.GetMutexValue(1, 9);
                    if(bgindex == 1)
                    {
                        ScleClock = 1.1f;
                    }
                    clockbgType = "clock_1_" + bgindex + "_";//
                    clockType = Common.BreakRank(new List<string>() { "clock_1_1_"})[0];//, "clock_1_2_" 
                    //clockbgType = clockType;
                    lineType = clockType;
                    centerType = clockType;
                    gameTimes_last = 2;// 2;
                    clockPos = Vector3.zero;
                    break;
                case 2:
                    clockbgType = "clock_1_" + Common.GetMutexValue(1, 10) + "_";
                    gametips = "game-tips5-1-9";
                    timeNumH = Common.GetRandValue(0, 11);
                    //clockType = "clock_1_2_";
                    gameTimes_last = 4;//4
                    clockPos = new Vector3(-275, 0, 0);
                    timeNumM = Common.GetMutexValue(0, 1);
                    break;
                case 3:
                    clockbgType = "clock_1_" + Common.GetMutexValue(1, 8) + "_";
                    //clockType = "clock_1_1_";
                    gameTimes_last = 5;//5;
                    roundH = false;
                    roundM = false;
                    clockPos = new Vector3(266, 32, 0);
                    gametips = "game-tips5-1-1" + (gameTimes + 1);
                    if (gameTimes == 1)
                    {
                        lastNumM = 0;
                        lastNumH = 1;
                        timeNumM = 0;
                        timeNumH = 7;
                        spineName = "get_up";
                        isface = true;
                        roleScane = 1;
                    }
                    else if (gameTimes == 2)
                    {
                        spineName = "breakfast";
                        isface = true;
                        lastNumM = 0;
                        lastNumH = 2;
                        timeNumM = 1;
                        timeNumH = 7;
                        roleScane = 1;
                    }
                    else if (gameTimes == 3)
                    {
                        spineName = "school";
                        isface = false;
                        lastNumM = 0;
                        lastNumH = 3;

                        timeNumM = 0;
                        timeNumH = 8;
                        roleScane = 1.3f;
                    }
                    else if (gameTimes == 5)
                    {
                        spineName = "game";
                        isface = false;
                        lastNumM = 0;
                        lastNumH = 5;

                        timeNumM = 1;
                        timeNumH = 10;
                        roleScane = 1f;

                    }
                    else if (gameTimes == 4)
                    {
                        spineName = "study";
                       
                        isface = true;
                        lastNumM = 0;
                        lastNumH = 4;
                        timeNumM = 0;
                        timeNumH = 9;
                        roleScane = 1.4f;

                    }
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
        }
    }
    void Awake()
    {
        mSound = gameObject.AddComponent<SoundManager>();
    }
    // Use this for initialization
    void Start () {
        RawImage bg = UguiMaker.newRawImage("bg", transform, "learntime_texture", "bg", false);
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);
        StartCoroutine(TInit());
    }
    IEnumerator TInit()
    {
        yield return new WaitForSeconds(0.5f);
        mSound.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.1f);
        mClockGo = UguiMaker.newGameObject("ClockGo", transform);
        reGame();
    }
    // Update is called once per frame
    //UPDATA 刷新画面
    Vector3 temp_select_StartPos = new Vector3(0,0,0);
    Vector3 temp_angle = new Vector3(0, 0, 0);
    //Vector3 temp_h_angle = Vector3.zero;
    float startAngle = 0;

    Transform temp_tran_h = null;
    Transform temp_tran_m = null;
    float temp_h_sub_angle = 0;
    float temp_h_angle = 0;
    private bool isPlayzhiz { get; set; }
    private bool isMovePlayed { get; set; }
    void Update()
    {
        if (inpass) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            chooseImage = null;
            if (null != hits)
            {
                foreach (RaycastHit hit in hits)
                {
                    Image com = hit.collider.gameObject.GetComponent<Image>();
                    if(null != com && mGuanka.guanka == 1 && disPos.ContainsKey(com))
                    {
                        chooseImage = com;
                        Item item = chooseImage.gameObject.GetComponent<Item>();
                        if(null != item)
                        {
                            item.AutoScale(false);
                        }
                        mSound.PlayShort("learntime_sound", "点击拖动数字", 0.7f);
                        isMovePlayed = false;
                        StartCoroutine(TScaleGo(chooseImage, 2f));
                    }else if(null != com && mGuanka.guanka == 2)
                    {
                        currType = com.name;
                        if (com.name == "himage" || com.name == "mimage")
                        {
                            showInput();
                        }
                    }else if (null != com && mGuanka.guanka == 3)
                    {
                        if (com.name == mGuanka.clockType + "clock_h" || com.name == mGuanka.clockType + "clock_m")
                        {
                            chooseImage = com;
                            temp_angle = Common.getMouseLocalPos(transform);
                            startAngle = Mathf.Atan2(temp_angle.y - currClockGo.transform.localPosition.y, temp_angle.x - currClockGo.transform.localPosition.x) * (180 / Mathf.PI) - 90;
                            break;
                        }
                    }
                }
                temp_tran_m = currClockGo.transform.Find("Gameobjectm/" + mGuanka.clockType + "clock_m");
                temp_tran_h = currClockGo.transform.Find("Gameobjectp/" + mGuanka.clockType + "clock_h");
                temp_h_sub_angle = Common.Parse360( temp_tran_m.localEulerAngles).z;
                temp_select_StartPos = Common.getMouseLocalPos(transform);
            }
            
        }
        if (Input.GetMouseButton(0))
        {
            if (null != chooseImage && mGuanka.guanka == 1)
            {
                
                
                chooseImage.rectTransform.anchoredPosition3D = Common.getMouseLocalPos(transform);

                if(Vector3.Distance(temp_select_StartPos, Common.getMouseLocalPos(transform)) > 10)
                {
                    if (!isMovePlayed)
                    {
                        mSound.PlayShort("learntime_sound", "拖动数字指针", 0.7f);
                        isMovePlayed = true;
                    }
                }

            }else if(null != chooseImage && mGuanka.guanka == 3)
            {
                
                Vector3 pos = Common.getMouseLocalPos(transform);
                float angle = Mathf.Atan2(pos.y - currClockGo.transform.localPosition.y , pos.x - currClockGo.transform.localPosition.x)  * (180 / Mathf.PI) - 90;
                chooseImage.rectTransform.localEulerAngles = new Vector3(0, 0, angle);

                float temp = Common.Parse360(temp_tran_m.localEulerAngles).z;

                temp_h_angle = (temp - temp_h_sub_angle);

                if (temp_h_angle > 180)
                    temp_h_angle -= 360;
                else if (temp_h_angle < -180)
                    temp_h_angle += 360;

                temp_tran_h.localEulerAngles += new Vector3(0, 0, temp_h_angle / 360 * 30);
                

                if (!isPlayzhiz)
                {
                    if (Vector3.Distance(temp_select_StartPos, Common.getMouseLocalPos(transform)) > 2)
                    {
                        if (chooseImage.name.Split('_')[4] == "h")
                        {
                            mSound.PlayOnly("learntime_sound", "拨动时针A", 0.7f);
                        }
                        else
                        {

                            mSound.PlayOnly("learntime_sound", "拨动分针B", 0.7f);
                        }
                        isPlayzhiz = true;
                        StartCoroutine(TRestZhiZSoundStat());
                        
                    }
                }
                temp_select_StartPos = Common.getMouseLocalPos(transform);
                temp_h_sub_angle = temp;


            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(null != chooseImage && mGuanka.guanka == 1)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray);
                bool boo = false;
                Image com = null;
                bool ishit = false;
                if (null != hits)
                {
                    foreach (RaycastHit hit in hits)
                    {
                        com = hit.collider.gameObject.GetComponent<Image>();
                        string comNamestr = com.name;
                        //Debug.Log("com.name : " + com.name);
                        if(chooseImage.name + "_line" == comNamestr)
                        {
                            boo = true;
                            break;
                        }
                       if (com.name != chooseImage.name)
                        {
                            ishit = true;
                        }
                    }
                }
                //Debug.Log("ishit : " + ishit);
                if (!boo)
                {
                    StartCoroutine(TScaleGo(chooseImage, 1.2f));
                    if (ishit)
                    {
                        StartCoroutine(TPositionGo(chooseImage, disPos[chooseImage]));
                    }
                    else
                    {
                        disPos[chooseImage] = chooseImage.rectTransform.localPosition;
                    }
                    Item item = chooseImage.gameObject.GetComponent<Item>();
                    if (null != item)
                    {
                        item.AutoScale(true);
                    }
                    if (chooseImage.name.Split('_')[0] == "clock")
                    {
                        mSound.PlayShort("learntime_sound", "指针反弹", 0.7f);
                    }
                    else
                    {
                        mSound.PlayShort("learntime_sound", "错误反弹回去", 0.7f);
                    }
                       
                }
                else
                {
                    float ofsetscal = 0.6f;
                    if(chooseImage.name.Split('_')[0] == "clock")
                    {
                        ofsetscal = 1f;
                        chooseImage.rectTransform.pivot = new Vector2(0.5f, 0);
                        mSound.PlayShort("learntime_sound", "指针对位正确", 0.7f);
                        if (chooseImage.name.Split('_')[4] == "h")
                        {
                            if(mGuanka.timeNumM == 0)
                            {
                                mSound.PlayTip("learntime_sound", "game-tips5-1-3", 1);
                            }else
                            {
                                mSound.PlayTip("learntime_sound", "补充  additional-45", 1);
                            }
                            
                            GameObject Gameobjecth = currClockGo.transform.Find("Gameobjectp").gameObject;
                            GameObject effecth = Gameobjecth.transform.Find(mGuanka.clockType + "clock_h" + "/GameobjectHEffect").gameObject;
                            //StartCoroutine(TPlayZhizhenEffect(effecth));
                        }
                        else if (chooseImage.name.Split('_')[4] == "m")
                        {
                            if (mGuanka.timeNumM == 0)
                            {
                                mSound.PlayTip("learntime_sound", "game-tips5-1-2", 1);
                            }
                            else
                            {
                                mSound.PlayTip("learntime_sound", "补充  additional-44", 1);
                            }
                            GameObject Gameobjectm = currClockGo.transform.Find("Gameobjectm").gameObject;
                            GameObject effectm = Gameobjectm.transform.Find(mGuanka.clockType + "clock_m" + "/GameobjectMEffect").gameObject;
                            //StartCoroutine(TPlayZhizhenEffect(effectm));
                        }

                    }else
                    {
                        mSound.PlayTip("learntime_sound", chooseImage.name.Split('_')[1], 1);
                        mSound.PlayShort("learntime_sound", "数字放入正确", 0.7f);
                        //
                        PlayStarParticle(com.rectTransform.localPosition);
                    }
                    StartCoroutine(TScaleGo(chooseImage, ofsetscal));
                    StartCoroutine(TPositionGo(chooseImage, com.rectTransform.localPosition));
                    com.enabled = false;
                    disPos.Remove(chooseImage);
                    
                    if (disPos.Count <= 0)
                    {
                        //mSound.PlayShort("learntime_sound", "量1.2 三个小怪兽-鼓励提示 8-1", 1);
                        playParticle();
                        if (chooseImage.name.Split('_')[0] == "clock")
                        {
                            if(chooseImage.name.Split('_')[4] == "m")
                            {
                                if (mGuanka.timeNumM == 0)
                                {
                                    StartCoroutine(TNextGame(7));
                                }
                                else
                                {
                                    StartCoroutine(TNextGame(5));
                                }
                                    
                            }
                            else
                            {
                                if (mGuanka.timeNumM == 0)
                                {
                                    StartCoroutine(TNextGame(7));
                                }else
                                {
                                    StartCoroutine(TNextGame(10));
                                }
                                    
                            }
                            
                        }else
                        {
                            StartCoroutine(TNextGame(3));
                        }
                            
                    }
                }
            }else if(null != chooseImage && mGuanka.guanka == 3)
            {
                //Debug.Log(chooseImage.name + "的角度：" + Mathf.Abs(chooseImage.rectTransform.localEulerAngles.z - 360) + ";mGuanka.angleH : " + mGuanka.angleH);
                //Debug.Log(chooseImage.name + "的角度：" + Mathf.Abs(chooseImage.rectTransform.localEulerAngles.z - 360) + ";mGuanka.angleM : " + mGuanka.angleM);
                mSound.StopOnly();
                if (chooseImage.name == mGuanka.clockType + "clock_h")
                {
                    if(Mathf.Abs((chooseImage.rectTransform.localEulerAngles.z - 360) - mGuanka.angleH) <= 20)
                    {
                        Debug.Log("旋转时针正确");
                        //TODO 旋转时针正确
                        //mSound.PlayTip("learntime_sound", "game-tips5-1-3", 1);
                        chooseImage.rectTransform.localEulerAngles = new Vector3(0, 0, mGuanka.angleH);
                        mGuanka.roundH = true;
                        GameObject Gameobjecth = currClockGo.transform.Find("Gameobjectp").gameObject;
                        GameObject effecth = Gameobjecth.transform.Find(mGuanka.clockType + "clock_h" + "/GameobjectHEffect").gameObject;
                        //StartCoroutine(TPlayZhizhenEffect(effecth));

                        GameObject Gameobjectm = currClockGo.transform.Find("Gameobjectm").gameObject;
                        Image clock_m = Gameobjectm.transform.Find(mGuanka.clockType + "clock_m").gameObject.GetComponent<Image>();
                       
                        if (Mathf.Abs((clock_m.rectTransform.localEulerAngles.z) - mGuanka.angleM - 360) <= 5 || Mathf.Abs((clock_m.rectTransform.localEulerAngles.z) - mGuanka.angleM) <= 5)
                        {
                            //GameObject effectm = Gameobjectm.transform.FindChild(mGuanka.clockType + "clock_m" + "/GameobjectMEffect").gameObject;
                            //StartCoroutine(TPlayZhizhenEffect(effectm));
                            mGuanka.roundM = true;
                        }else
                        {
                            mGuanka.roundM = false;
                        }
                        //playParticle();
                    }
                }else if(chooseImage.name == mGuanka.clockType + "clock_m")
                {
                    if (Mathf.Abs((chooseImage.rectTransform.localEulerAngles.z - 360) - mGuanka.angleM) <= 20)
                    {
                        //Debug.Log("旋转分针正确");
                        //TODO 旋转时针正确
                        //mSound.PlayTip("learntime_sound", "game-tips5-1-2", 1);
                        chooseImage.rectTransform.localEulerAngles = new Vector3(0, 0, mGuanka.angleM);
                        mGuanka.roundM = true;
                        GameObject Gameobjectm = currClockGo.transform.Find("Gameobjectm").gameObject;
                        GameObject effectm = Gameobjectm.transform.Find(mGuanka.clockType + "clock_m" + "/GameobjectMEffect").gameObject;
                        //StartCoroutine(TPlayZhizhenEffect(effectm));

                        GameObject Gameobjecth = currClockGo.transform.Find("Gameobjectp").gameObject;
                        Image clock_h = Gameobjecth.transform.Find(mGuanka.clockType + "clock_h").gameObject.GetComponent<Image>();
                        //Debug.Log("时针的角度：" + Mathf.Abs(clock_h.rectTransform.localEulerAngles.z) + ";mGuanka.angleH : " + mGuanka.angleH);
                        if (Mathf.Abs((clock_h.rectTransform.localEulerAngles.z) - mGuanka.angleH - 360) <= 7 || Mathf.Abs((clock_h.rectTransform.localEulerAngles.z) - mGuanka.angleH) <= 5)
                        {
                            //GameObject effecth = Gameobjecth.transform.FindChild(mGuanka.clockType + "clock_h" + "/GameobjectHEffect").gameObject;
                            //StartCoroutine(TPlayZhizhenEffect(effecth));
                            mGuanka.roundH = true;
                        }else
                        {
                            mGuanka.roundH = false;
                        }
                        //playParticle();
                    }
                }
                if(mGuanka.roundH && mGuanka.roundM)
                {
                    mSound.StopTip();
                    playParticle();
                    StartCoroutine(TNextGame(2));
                }
            }
            chooseImage = null;
        }
    }
    
    //播放特效
    private void PlayStarParticle(Vector3 pos)
    {
        if (null == mStarparticle)
        {
            GameObject obj1 = ResManager.GetPrefab("effect_star2", "effect_star2"); 
            obj1.transform.parent = currClockGo.transform;
            mStarparticle = obj1.GetComponent<ParticleSystem>();
            mStarparticle.transform.localPosition = Vector3.zero;
            mStarparticle.transform.localScale = Vector3.one;
        }
        mStarparticle.transform.localPosition = pos;
        mStarparticle.Play();
    }
    private void playParticle()
    {
        if(null == mparticle)
        {
            GameObject obj1 = ResManager.GetPrefab("learntime_prefab", "LeanrTimeEffect");
            obj1.transform.parent = currClockGo.transform;
            mparticle = obj1.GetComponent<ParticleSystem>();
            mparticle.transform.localPosition = Vector3.zero;
            mparticle.transform.localScale = Vector3.one;
        }
        
        mparticle.Play();
        StartCoroutine(TEffectSound());
    }
    private void playInputEffect()
    {
        
        if (null == mparticleInput)
        {
            GameObject obj1 = ResManager.GetPrefab("learntime_prefab", "input");
            obj1.name = "particleInput";
            obj1.transform.parent = inputGo.transform;
            mparticleInput = obj1.GetComponent<ParticleSystem>();
            mparticleInput.transform.localPosition = Vector3.zero;// new Vector3(335, 159, 0);
            mparticleInput.transform.localScale = Vector3.one * 3f;
        }

        mparticleInput.Play();
    }
    IEnumerator TEffectSound()
    {
        yield return new WaitForSeconds(1);
        mSound.PlayShort("learntime_sound", "奖章解冻的特效声", 0.7f);
    }
    //显示输入框
    private void showInput()
    {
        if (inpass || (null != inputobj && inputobj.gameObject.active)) return;

        if (null == inputobj)
        {
            InputInfoData data = new InputInfoData();
            /*
            data.fNumScale = 0.7f;
            data.vBgSize = new Vector2(710, 700);
            data.vCellSize = new Vector2(213, 156);
            data.vSpacing = new Vector2(13, 13);
            data.bgcolor = new Color32(252, 229, 194, 255);
            data.color_blockBG = new Color32(179, 138, 89, 255);
            data.color_blockNum = new Color32(202, 183, 155, 255);
            data.color_blockSureBG = new Color32(172, 123, 66, 255);
            data.color_blockSureStart = new Color32(202, 183, 155, 255);
            data.strAlatsName = "learntime_sprite";
            //*/

            inputobj = UguiMaker.newGameObject("inputobj", transform).gameObject.AddComponent<EquationInput>(); //InputNumObj.Create(transform, data);
            inputobj.transform.localPosition = new Vector3(318, -116, 0);
            inputobj.init("learntime",4,true);
            /*
            inputobj.nCountLimit = 2;
            Canvas canvas = inputobj.gameObject.AddComponent<Canvas>();
            inputobj.gameObject.layer = LayerMask.NameToLayer("UI");
            canvas.overrideSorting = true;
            canvas.sortingOrder = 11;
            */
            inputobj.SetInputNumberCallBack(getNumfromInputNumObj);
            inputobj.SetClearNumberCallBack(CleanInputNum);
            inputobj.SetFinishInputCallBack(FinishInputNum);
            /*
            inputobj.SetClearNumberCallBack(CleanInputNum);
            inputobj.SetFinishInputCallBack(FinishInputNum);
            
            GraphicRaycaster gry = inputobj.gameObject.AddComponent<GraphicRaycaster>();
            gry.ignoreReversedGraphics = true;
            */
        }
        mSound.PlayShort("button_down");
        inputobj.ShowEffect();
    }
    //结束输入
    private void FinishInputNum()
    {
        //Debug.Log("FinishInputNum inputobj.strInputNum : " + inputobj.strInputNum);
        mSound.PlayShort("button_down");
        string setNum = inputobj.strInputNum;
        
        if (setNum == "")
        {
            return;
        }
        Debug.Log("mGuanka.currH : " + mGuanka.currH + ";mGuanka.currM : " + mGuanka.currM + ";mGuanka.inputH : " + mGuanka.inputH + ";mGuanka.inputM : " + mGuanka.inputM);
        bool booH = false;
        if(mGuanka.currH == 0)
        {
            if(mGuanka.inputH == 12)
            {
                booH = true;
            }
        }else
        {
            if(mGuanka.currH == mGuanka.inputH % 12 && mGuanka.inputH <= 24)
            {
                booH = true;
            }
        }
        bool booM = false;
        if (mGuanka.currM == mGuanka.inputM)
        {
            booM = true;
        }

        if(booH && booM)
        {
            Debug.Log("输入正确");
            string hSoundName = mGuanka.currH.ToString();
            if(hSoundName == "0")
            {
                hSoundName = "12";
            }else if(hSoundName == "2")
            {
                hSoundName = "2_1";
            }
            if (mGuanka.currM == 0)
            {
                mSound.PlayTipList(new List<string>() { "learntime_sound", "learntime_sound" }, new List<string> { hSoundName, "补充  additional-42" });
            }
            else
            {
                mSound.PlayTipList(new List<string>() { "learntime_sound", "learntime_sound", "learntime_sound" }, new List<string> { hSoundName, "补充  additional-42", "补充  additional-43" });
            }
            
            playParticle();
            //playInputEffect();
            StartCoroutine(TNextGame(3.5f));
        }
        else
        {
            GameObject go1 = inputGo.transform.Find("hGo").gameObject;
            Image img0 = go1.transform.Find("imageNum_0").gameObject.GetComponent<Image>();
            //img0.enabled = false;
            Image img1 = go1.transform.Find("imageNum_1").gameObject.GetComponent<Image>();
            //img1.enabled = false;

            GameObject go2 = inputGo.transform.Find("mGo").gameObject;
            Image img02 = go2.transform.Find("imageNum_0").gameObject.GetComponent<Image>();
            //img02.enabled = false;
            Image img12 = go2.transform.Find("imageNum_1").gameObject.GetComponent<Image>();
            //img12.enabled = false;

            if(img0.enabled && img1.enabled && img02.enabled && img12)
            {
                mSound.PlayTip("learntime_sound", "game-tips6-1-9", 1);
            } 
        }

    }
    //输入框清除数据
    private void CleanInputNum()
    {
        GameObject go = null;
        if (currType == "himage")
        {
            go = inputGo.transform.Find("hGo").gameObject;
            //设置小时
        }
        else if (currType == "mimage")
        {
            //设置分钟
            go = inputGo.transform.Find("mGo").gameObject;
        }
        Image img0 = go.transform.Find("imageNum_0").gameObject.GetComponent<Image>();
        img0.enabled = false;
        Image img1 = go.transform.Find("imageNum_1").gameObject.GetComponent<Image>();
        img1.enabled = false;
        Image wenhao = go.transform.Find("wenhao").gameObject.GetComponent<Image>();
        wenhao.enabled = true;
    }
    //获取选择的数字
    private void getNumfromInputNumObj()
    {
        string setNum = inputobj.strInputNum;
        mSound.PlayShort("inputnumclick");
        setInputData(currType,setNum);
    }
    private void setInputData(string type, string setNum)
    {
        GameObject go = null;
        if (type == "himage")
        {
            go = inputGo.transform.Find("hGo").gameObject;
            //设置小时
        }
        else if (type == "mimage")
        {
            //设置分钟
            go = inputGo.transform.Find("mGo").gameObject;
        }
        

        if (setNum == "" || null == inputobj)
        {
            return;
        }

        if (type == "himage")
        {
            mGuanka.inputH = int.Parse(setNum);
            //设置小时
        }
        else if (type == "mimage")
        {
            //设置分钟
            mGuanka.inputM = int.Parse(setNum);
        }
        if (setNum.Length > 2)
        {
            setNum = setNum.Substring(2, 1);
            /*
            if(null != inputobj)
            {
                setNum = setNum.Substring(2, 1);
                inputobj.strInputNum = setNum;
            }
            */
        }
        if (null == go) return;

        Image img0 = go.transform.Find("imageNum_0").gameObject.GetComponent<Image>();
        img0.enabled = false;
        Image img1 = go.transform.Find("imageNum_1").gameObject.GetComponent<Image>();
        img1.enabled = false;

        if (setNum.Length < 2)
        {
            img0.sprite = ResManager.GetSprite("learntime_sprite", "input0");
            img1.sprite = ResManager.GetSprite("learntime_sprite", "input" + setNum);

        }
        else
        {
            img0.sprite = ResManager.GetSprite("learntime_sprite", "input" + setNum.Substring(0, 1));
            img1.sprite = ResManager.GetSprite("learntime_sprite", "input" + setNum.Substring(1, 1));
        }
        Image wenhao = go.transform.Find("wenhao").gameObject.GetComponent<Image>();
        wenhao.enabled = false;
        img0.enabled = true;
        img0.SetNativeSize();
        img1.enabled = true;
        img1.SetNativeSize();
    }

    //创建单个钟表
    private GameObject createAClock(string bg, string type,string lineType,string centerType)
    {
        GameObject clockGo = UguiMaker.newGameObject(type, mClockGo.transform);
        clockGo.transform.localPosition = mGuanka.clockPos;
        Image clockbg = UguiMaker.newImage(type + "clockbg", clockGo.transform, "learntime_sprite", bg + "clock_bg");
        clockbg.rectTransform.localScale = Vector3.one * mGuanka.ScleClock;
        BoxCollider box = clockbg.gameObject.AddComponent<BoxCollider>();
        box.size = new Vector2(600, 600);

        for (int i = 1; i < 13; i++)
        {
            Vector3 pos = getPos(i, 30);
            Image line = UguiMaker.newImage("num_" + i + "_line", clockGo.transform, "learntime_sprite", "clock_1_1_line_num");
            line.rectTransform.localPosition = pos;
            line.rectTransform.sizeDelta = new Vector2(40, 40);
            BoxCollider boxnumline = line.gameObject.AddComponent<BoxCollider>();
            boxnumline.size = new Vector2(100, 100);
            line.enabled = false;

            Image num = UguiMaker.newImage("num_" + i, clockGo.transform, "learntime_sprite", i.ToString());
            num.rectTransform.localPosition = pos;
            num.rectTransform.localScale = Vector3.one * 0.6f;
            BoxCollider boxnum = num.gameObject.AddComponent<BoxCollider>();
            boxnum.size = new Vector2(100, 100);
        }
        //时针
        GameObject Gameobjectp = UguiMaker.newGameObject("Gameobjectp", clockGo.transform);
        //时针虚线
        Image clock_h_line = UguiMaker.newImage(type + "clock_h_line", Gameobjectp.transform, "learntime_sprite", lineType + "line_p");
        clock_h_line.rectTransform.pivot = new Vector2(0.5f, 0);
        clock_h_line.rectTransform.anchoredPosition = Vector2.zero;
        BoxCollider boxclock_h_line = clock_h_line.gameObject.AddComponent<BoxCollider>();
        boxclock_h_line.size = new Vector2(80, 124);
        boxclock_h_line.center = new Vector3(0, 52, 0);
        clock_h_line.enabled = false;
        
        Image clock_h = UguiMaker.newImage(type + "clock_h", Gameobjectp.transform, "learntime_sprite", type + "clock_p");
        clock_h.rectTransform.pivot = new Vector2(0.5f, 0);
        clock_h.rectTransform.anchoredPosition = Vector2.zero;
        BoxCollider boxclock_h = clock_h.gameObject.AddComponent<BoxCollider>();
        boxclock_h.size = new Vector2(100, 134);

        GameObject GameobjectpEffect = UguiMaker.newGameObject("GameobjectHEffect", clock_h.gameObject.transform);
        for(int i = 0;i < 2; i++)
        {
            Image effect = UguiMaker.newImage("effect_" + i, GameobjectpEffect.transform, "learntime_sprite", "clock_move");
            //effect.rectTransform.sizeDelta = new Vector2(100, 39);
            effect.rectTransform.localPosition = new Vector3(0, 40 * i, 0);
        }
        GameobjectpEffect.active = false;

        if (mGuanka.guanka == 3)
        {
            boxclock_h.center = new Vector3(0, 52, 0);
        }
        //boxclock_h.center = new Vector3(0, 52, 0);

        //分针
        GameObject Gameobjectm = UguiMaker.newGameObject("Gameobjectm", clockGo.transform);
        //分针虚线
        Image clock_m_line = UguiMaker.newImage(type + "clock_m_line", Gameobjectm.transform, "learntime_sprite", lineType + "line_m");
        clock_m_line.rectTransform.pivot = new Vector2(0.5f, 0);
        //clock_m_line.rectTransform.localPosition = new Vector3(0, clock_m_line.rectTransform.sizeDelta.y * 0.5f, 0);
        BoxCollider boxclock_m_line = clock_m_line.gameObject.AddComponent<BoxCollider>();
        boxclock_m_line.size = new Vector2(80, 150);
        boxclock_m_line.center = new Vector3(0, 64, 0);
        clock_m_line.enabled = false;

        Image clock_m = UguiMaker.newImage(type + "clock_m", Gameobjectm.transform, "learntime_sprite", type + "clock_m");
        clock_m.rectTransform.pivot = new Vector2(0.5f, 0);
        //clock_m.rectTransform.localPosition = new Vector3(0, clock_m.rectTransform.sizeDelta.y * 0.5f, 0);
        BoxCollider boxclock_m = clock_m.gameObject.AddComponent<BoxCollider>();
        boxclock_m.size = new Vector2(100, 150);
        GameObject GameobjectHEffect = UguiMaker.newGameObject("GameobjectMEffect", clock_m.gameObject.transform);
        for (int i = 0; i < 2; i++)
        {
            Image effect = UguiMaker.newImage("effect_" + i, GameobjectHEffect.transform, "learntime_sprite", "clock_move");
            //effect.rectTransform.sizeDelta = new Vector2(100, 39);
            effect.rectTransform.localPosition = new Vector3(0, 40 * i, 0);
        }
        GameobjectHEffect.active = false;

        if (mGuanka.guanka == 3)
        {
            boxclock_m.center = new Vector3(0, 64, 0);
            //clockGo.transform
        }
        //boxclock_m.center = new Vector3(0, 64, 0);
        //中心点
        Image center = UguiMaker.newImage("center", clockGo.transform, "learntime_sprite", centerType + "center");
        return clockGo;
    }
    
    //获取数字的坐标
    private Vector3 getPos(int _id, float disAngel)
    {
        float angle = ((_id * disAngel) / 180) * Mathf.PI;
        float vx = 0;
        float vy = 0;
        float r = 160;
        vx = Mathf.Sin(angle) * r;
        vy = Mathf.Cos(angle) * r;
        Vector3 pos = new Vector3(vx, vy, 0);
        return pos;
    }

    //设置钟表的随机时间点
    private void setClockTime(GameObject go, float numP, float numM)
    {
        string type = go.name;
        float ofset = (numM / 60f)  * (5 * 6);
        GameObject Gameobjecth = go.transform.Find("Gameobjectp").gameObject;
        Image clock_h = Gameobjecth.transform.Find(type + "clock_h").gameObject.GetComponent<Image>();
        Common.SetChildEulerAngles(Gameobjecth.transform, new Vector3(0, 0, -numP * 30 - ofset));
        
        GameObject Gameobjectm = go.transform.Find("Gameobjectm").gameObject;
        Image clock_m = Gameobjectm.transform.Find(type + "clock_m").gameObject.GetComponent<Image>();
        Common.SetChildEulerAngles(Gameobjectm.transform, new Vector3(0, 0, -numM * 6));

        if (null != spine)
        {
            GameObject.Destroy(spine.gameObject);
        }

        if (null != roleScane)
        {
            GameObject.Destroy(roleScane);
        }

        if (mGuanka.guanka == 1)
        {
            StartCoroutine(TbreakClock(go));
        }else //if(mGuanka.guanka == 2)
        {
            if(null != inputGo)
            {
                GameObject.Destroy(inputGo);
            }
            
            inputGo = UguiMaker.newGameObject("inputGo", mClockGo.transform);
            Canvas canvasinput = inputGo.gameObject.AddComponent<Canvas>();
            inputGo.gameObject.layer = LayerMask.NameToLayer("UI");
            canvasinput.overrideSorting = true;
            canvasinput.sortingOrder = 4;
            
            if (mGuanka.guanka == 2)
            {
                //inputGo.transform.localPosition = new Vector2(335, 159);
                StartCoroutine(TshowInputiGo(new Vector3(310, 170,0)));
                mSound.PlayTipList(new List<string>() { "learntime_sound"}, new List<string>() { mGuanka.gametips}, true);
            }
            else
            {
                StartCoroutine(TshowInputiGo(new Vector3(-335, -273,0)));
                mSound.PlayTipList(new List<string>() { "learntime_sound", "learntime_sound" }, new List<string>() { mGuanka.gametips, "game-tips5-1-8" }, true);
                
                GameObject effectm = Gameobjectm.transform.Find(mGuanka.clockType + "clock_m" + "/GameobjectMEffect").gameObject;

                GameObject effecth = Gameobjecth.transform.Find(mGuanka.clockType + "clock_h" + "/GameobjectHEffect").gameObject;

                StartCoroutine(TPlayZhizhenEffect(effecth, effectm));
                //inputGo.transform.localPosition = new Vector2(-335, -273);
            }
            Image inputbg = UguiMaker.newImage("inputbg", inputGo.transform, "learntime_sprite", "inputNumbg");
            inputbg.rectTransform.localPosition = new Vector3(0, -4, 0);
            ///*
            GameObject hGo = UguiMaker.newGameObject("hGo", inputGo.transform);
            Image himage = UguiMaker.newImage("himage", hGo.transform, "public", "white");
            himage.rectTransform.sizeDelta = new Vector2(70, 60);
            himage.rectTransform.anchoredPosition = new Vector2(-51, 0);
            Color color = himage.color;
            color.a = 0f;
            himage.color = color;
            BoxCollider hibox = himage.gameObject.AddComponent<BoxCollider>();
            hibox.size = new Vector2(70, 60);
            Image hwenhao = UguiMaker.newImage("wenhao", hGo.transform, "learntime_sprite", "wenhao");
            hwenhao.rectTransform.anchoredPosition = new Vector2(-51, 0);
            Item hItem = hwenhao.gameObject.AddComponent<Item>();
            hItem.AutoScale(true);

            Vector2 startPos = new Vector3(-70, 0);
            for (int i = 0; i < 2; i++)
            {
                Image imageNum = UguiMaker.newImage("imageNum_" + i, hGo.transform, "learntime_sprite", "input0");
                imageNum.rectTransform.anchoredPosition = startPos + new Vector2(i * 35, 0);
                imageNum.rectTransform.localScale = Vector3.one * 0.3f;
                imageNum.enabled = false;
            }


            Image pimage = UguiMaker.newImage("pimage", inputGo.transform, "learntime_sprite", "point");
            pimage.rectTransform.sizeDelta = new Vector2(13, 70);

            GameObject mGo = UguiMaker.newGameObject("mGo", inputGo.transform);
            Image mimage = UguiMaker.newImage("mimage", mGo.transform, "public", "white");
            mimage.rectTransform.sizeDelta = new Vector2(70, 60);
            mimage.rectTransform.anchoredPosition = new Vector2(55, 0);
            Color colorm = mimage.color;
            colorm.a = 0f;
            mimage.color = colorm;
            BoxCollider mibox = mimage.gameObject.AddComponent<BoxCollider>();
            mibox.size = new Vector2(70, 60);
            Image mwenhao = UguiMaker.newImage("wenhao", mGo.transform, "learntime_sprite", "wenhao");
            mwenhao.rectTransform.anchoredPosition = new Vector2(54, 0);
            Item mItem = mwenhao.gameObject.AddComponent<Item>();
            mItem.AutoScale(true);

            startPos = new Vector3(40, 0);
            for (int i = 0; i < 2; i++)
            {
                Image imageNum = UguiMaker.newImage("imageNum_" + i, mGo.transform, "learntime_sprite", "input0");
                imageNum.rectTransform.localScale = Vector3.one * 0.3f;
                imageNum.rectTransform.anchoredPosition = startPos + new Vector2(i * 35, 0);
                imageNum.enabled = false;
            }
            inputGo.transform.localScale = Vector3.one * 2;
           
            //*/
            if (mGuanka.guanka == 3)
            {
                setInputData("himage", mGuanka.currH.ToString());
                setInputData("mimage", mGuanka.currM.ToString());

                
                roleScane = UguiMaker.newGameObject("roleScane", transform);
                roleScane.transform.localPosition = new Vector3(-300, 50, 0);

                spine = ResManager.GetPrefab("learntime_prefab", mGuanka.spineName).GetComponent<LeanrTimeSpine>();
                spine.transform.parent = roleScane.transform;
                spine.transform.localScale = Vector3.one;
                roleScane.transform.localScale = Vector3.one;
                if (mGuanka.gameTimes == 4)
                {
                    spine.PlaySpine("Idle", true);
                    spine.transform.localPosition = new Vector3(13, -104, 0);
                    roleScane.transform.localScale = Vector3.one * 0.8f;
                }
                else if (mGuanka.gameTimes == 1)
                {
                    spine.PlaySpine("animation", true);
                    spine.transform.localPosition = new Vector3(70, -23, 0);
                    roleScane.transform.localScale = Vector3.one * 0.8f;
                }
                else if (mGuanka.gameTimes == 2)
                {
                    spine.PlaySpine("animation", true);
                    spine.transform.localPosition = new Vector3(0, -37, 0);
                }
                else if (mGuanka.gameTimes == 3)
                {
                    spine.PlaySpine("animation", true);
                    spine.transform.localPosition = new Vector3(0, -220, 0);
                    spine.transform.localScale = Vector3.one * 1.2f;
                    roleScane.transform.localScale = Vector3.one * 0.8f;
                }
                else if (mGuanka.gameTimes == 5)
                {
                    spine.PlaySpine("animation", true);
                    spine.transform.localPosition = new Vector3(72, -200, 0);
                }
                ///*
                Canvas canvas = spine.gameObject.AddComponent<Canvas>();
                //inputobj.gameObject.layer = LayerMask.NameToLayer("UI");
                canvas.overrideSorting = true;
                canvas.sortingOrder = 2;
                //*/

                Image back = UguiMaker.newImage("back", roleScane.transform, "learntime_sprite",mGuanka.spineName + "_back");
                back.rectTransform.localScale = Vector3.one * mGuanka.roleScane;
                if (mGuanka.gameTimes == 4)
                {
                    back.rectTransform.anchoredPosition = new Vector2(0, 37);
                }else
                {
                    back.rectTransform.anchoredPosition = new Vector2(0, 0);
                }
                if (mGuanka.isface)
                {
                    Image face = UguiMaker.newImage("face", roleScane.transform, "learntime_sprite", mGuanka.spineName + "_face");
                    face.rectTransform.localScale = Vector3.one * mGuanka.roleScane;
                    face.rectTransform.anchoredPosition = new Vector2(0, 40);
                    Canvas canvasf = face.gameObject.AddComponent<Canvas>();
                    face.gameObject.layer = LayerMask.NameToLayer("UI");
                    canvasf.overrideSorting = true;
                    canvasf.sortingOrder = 3;
                }
            }
        }
    }
    IEnumerator TPlayZhizhenEffect(GameObject goh, GameObject gom)
    {
        yield return new WaitForSeconds(6f);
        goh.active = true;
        Vector3 endPos = new Vector3(0, 100, 0);
        for (float j = 0; j < 1f; j += 0.05f)
        {
            goh.transform.localPosition = Vector3.Lerp(Vector3.zero, endPos, Mathf.Sin(Mathf.PI * 0.5f * j));
            yield return new WaitForSeconds(0.01f);
        }
        goh.transform.localPosition = endPos;
        goh.active = false;

        yield return new WaitForSeconds(0.1f);
        gom.active = true;
        //Vector3 endPos = new Vector3(0, 100, 0);
        for (float j = 0; j < 1f; j += 0.05f)
        {
            gom.transform.localPosition = Vector3.Lerp(Vector3.zero, endPos, Mathf.Sin(Mathf.PI * 0.5f * j));
            yield return new WaitForSeconds(0.01f);
        }
        gom.transform.localPosition = endPos;
        gom.active = false;
    }
    IEnumerator TshowInputiGo(Vector3 endPos)
    {
        Vector3 startPos = endPos + new Vector3(0, 600, 0);
        for (float j = 0; j < 1f; j += 0.1f)
        {
            inputGo.transform.localPosition = Vector3.Lerp(startPos, endPos, Mathf.Sin(Mathf.PI * 0.5f * j));
            yield return new WaitForSeconds(0.01f);
        }
        inputGo.transform.localPosition = endPos;
    }
    private void reGame()
    {
        mGuanka.guanka = 1;
        setGameData();
    }

    //设置游戏数据
    private void setGameData()
    {
        if (mGuanka.guanka == 1)
        {
            if (mGuanka.gameTimes == 1)
            {
                TopTitleCtl.instance.Reset();
            }
            
        }
        else 
        {
            mGuanka.inputH = -1;
            mGuanka.inputM = -1;
            if (mGuanka.gameTimes == 1)
            {
                TopTitleCtl.instance.AddStar();
            }
        }
        
        mGuanka.Set(mGuanka.guanka);
        
        mClockGo.transform.DetachChildren();

        if (null != currClockGo)
        {
            GameObject.Destroy(currClockGo);
        }
        currClockGo = createAClock(mGuanka.clockbgType,mGuanka.clockType, mGuanka.lineType, mGuanka.centerType);

        inpass = false;

        //随机时间
        mGuanka.currM = mGuanka.timeNumM * 30;//Common.GetRandValue(0, 11) * 5;
        mGuanka.angleM = -mGuanka.currM * 6;

        mGuanka.currH = mGuanka.timeNumH;
        float ofset = (mGuanka.currM / 60f) * (5 * 6);
        mGuanka.angleH = -mGuanka.currH * 30 - ofset;

        //Debug.Log(mGuanka.currH + "点" + mGuanka.currM + "分" + ";时针旋转：" + mGuanka.angleH + "度；" + ";分针旋转：" + mGuanka.angleM + "度；");
        
        if (mGuanka.guanka == 3)
        {
            mSound.StopTip();
            mSound.PlayTip("learntime_sound", mGuanka.gametips, 1);
            setClockTime(currClockGo, mGuanka.lastNumH, mGuanka.lastNumM);
        }
        else
        {
            setClockTime(currClockGo, mGuanka.currH, mGuanka.currM);
        }
        StartCoroutine(TClockIn());
    }
    
    IEnumerator TClockIn()
    {
        mSound.PlayShort("learntime_sound", "素材出现通用音效", 0.7f);
        Vector3 endPos = mGuanka.clockPos;
        Vector3 startPos =  Common.BreakRank(new List<Vector3>() { new Vector3(endPos.x, -700, 0), new Vector3(-700, 0, 0) , new Vector3(700, 0, 0) , new Vector3(endPos.x, 700, 0), new Vector3(0, 0, -700), new Vector3(0, 0, 700) })[0];
        
        if(startPos.z == 700 || startPos.z == -700)
        {
            for (float j = 0; j < 1f; j += 0.1f)
            {
                currClockGo.transform.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, Mathf.Sin(Mathf.PI * 0.5f * j));
                yield return new WaitForSeconds(0.01f);
            }
            currClockGo.transform.localScale = Vector3.one;
        }
        else
        {
            for (float j = 0; j < 1f; j += 0.1f)
            {
                currClockGo.transform.localPosition = Vector3.Lerp(startPos, endPos, Mathf.Sin(Mathf.PI * 0.5f * j));
                yield return new WaitForSeconds(0.01f);
            }
            currClockGo.transform.localPosition = endPos;
        }
        
    }
    //下一关
    IEnumerator TNextGame(float delayTime = 8)
    {
        inpass = true;
        yield return new WaitForSeconds(delayTime);
        mSound.StopTip();
        mSound.PlayTip("learntime_sound", "goodgood" + (Common.GetMutexValue(0, 7)), 1);
        yield return new WaitForSeconds(4);

        mGuanka.gameTimes++;
        if (mGuanka.gameTimes > mGuanka.gameTimes_last)
        {
            mGuanka.guanka++;
            mGuanka.gameTimes = 1;
            if (mGuanka.guanka > mGuanka.guanka_last)
            {
                TopTitleCtl.instance.AddStar();
                //yield return new WaitForSeconds(5f);
                GameOverCtl.GetInstance().Show(mGuanka.guanka_last, reGame);
            }else
            {
                Debug.Log("过关……");
                setGameData();
            }
        }else
        {
            setGameData();
        }
    }

    //自动打坏钟表
    private Dictionary<Image,Vector3> disPos { get; set; }
    IEnumerator TbreakClock(GameObject go)
    {
        if(null == disPos)
        {
            disPos = new Dictionary<Image, Vector3>();
        }
        else
        {
            disPos.Clear();
        }
        yield return new WaitForSeconds(1);


        string type = go.name;
        GameObject Gameobject_hour = go.transform.Find("Gameobjectp").gameObject;
        Image clock_h_line = Gameobject_hour.transform.Find(type + "clock_h_line").gameObject.GetComponent<Image>();
        Image clock_h = Gameobject_hour.transform.Find(type + "clock_h").gameObject.GetComponent<Image>();

        GameObject Gameobject_min = go.transform.Find("Gameobjectm").gameObject;
        Image clock_m_line = Gameobject_min.transform.Find(type + "clock_m_line").gameObject.GetComponent<Image>();
        Image clock_m = Gameobject_min.transform.Find(type + "clock_m").gameObject.GetComponent<Image>();

        List<Image> images = new List<Image>();

        //images.Add(clock_h);
        //images.Add(clock_m);

        List<int> listnum = Common.BreakRank(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });// 

        List<Vector3> listnumPos = Common.BreakRank(new List<Vector3>() { new Vector3(-411, 226, 0), new Vector3(-567, 116, 0), new Vector3(-451, -269, 0), new Vector3(411, 226, 0), new Vector3(567, 116, 0), new Vector3(451, -269, 0) });
        List<Vector3> timePos = Common.BreakRank(new List<Vector3>() { new Vector3(-421, 0, 0), new Vector3(420, 0, 0) });
        if(mGuanka.gameTimes == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                Image numimage = go.transform.Find("num_" + listnum[i]).gameObject.GetComponent<Image>();
                images.Add(numimage);

            }
        }else
        {
            images.Add(clock_h);
            images.Add(clock_m);
        }
        
        int numIndex = 0;
        for (int i = 0; i < images.Count; i++)
        {
            Image image = images[i];
            Image line = image.transform.parent.Find(image.name + "_line").gameObject.GetComponent<Image>();
            ///*
            for (float j = 0; j < 1f; j += 0.5f)
            {
                image.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, j);
                
                yield return new WaitForSeconds(0.01f);
            }
            //*/
            //yield return new WaitForSeconds(0.5f);
            line.enabled = true;
            image.rectTransform.localScale = Vector3.one * 1.2f;
            Vector3 startPos = image.rectTransform.localPosition;
            Vector3 endPos = startPos - new Vector3(420, 0, 0);
            Vector3 stagePos2 = image.transform.position;
            Vector3 loacl = transform.worldToLocalMatrix.MultiplyPoint(stagePos2);
            if (i >= 2)
            {
                endPos = listnumPos[numIndex];
                numIndex++;
            }
            else
            {
                endPos = timePos[i];
            }
            mSound.PlayShort("learntime_sound", "数字飞到右边", 0.7f);
            Vector3 endAngle = image.rectTransform.localEulerAngles;
            for (float j = 0; j < 1f; j += 0.2f)
            {
                image.rectTransform.anchoredPosition3D = Vector3.Lerp(startPos, endPos, j);
                image.rectTransform.localEulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, 360), j);
                yield return new WaitForSeconds(0.01f);
            }
            image.rectTransform.localEulerAngles = endAngle;
            image.rectTransform.anchoredPosition = endPos;
            image.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            //BoxCollider box = image.gameObject.GetComponent<BoxCollider>();
            disPos.Add(image, endPos);
            Item item = image.gameObject.AddComponent<Item>();
            item.setDefScale(image.rectTransform.localScale);
        }
        yield return new WaitForSeconds(0.6f);
        mSound.PlayTipList(new List<string>() { "learntime_sound" }, new List<string>() { mGuanka.gametips }, true);
    }
    //缩放缓释缩放动画
    IEnumerator TScaleGo(Image image,float num)
    {
        Vector3 startScale = image.rectTransform.localScale;
        Vector3 endScale = Vector3.one * num; //image.rectTransform.localScale +
        for (float j = 0; j < 1f; j += 0.2f)
        {
            image.rectTransform.localScale = Vector3.Lerp(startScale, endScale, j);
            yield return new WaitForSeconds(0.01f);
        }
        image.rectTransform.localScale = endScale;
    }
    //缩放缓释位移动画
    IEnumerator TPositionGo(Image image, Vector3 endPos)
    {
        Vector3 startPos = image.rectTransform.localPosition;
        //Vector3 endScale = image.rectTransform.localScale + Vector3.one * num;
        for (float j = 0; j < 1f; j += 0.1f)
        {
            image.rectTransform.localPosition = Vector3.Lerp(startPos, endPos, j);
            yield return new WaitForSeconds(0.01f);
        }
        image.rectTransform.localPosition = endPos;
    }

    IEnumerator TRestZhiZSoundStat()
    {
        yield return new WaitForSeconds(0.5f);
        isPlayzhiz = false;
    }
}
