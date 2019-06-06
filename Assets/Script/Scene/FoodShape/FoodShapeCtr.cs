using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class FoodShapeCtr : MonoBehaviour {
    public Guanka mGuanka = new Guanka();
    public SoundManager mSound { get; set; }
    private RawImage bg { get; set; }
    private RawImage zhuobu { get; set; }
    private GameObject foodContain { get; set; }
    private bool inpass { get; set; }
    private InputCte input { get; set; }
    private bool isShowInput { get; set; }
    private Image mbutton { get; set; }
    private ParticleSystem mOKBtn_Effect { get; set; }
    private bool isdownSub { get; set; }
    public class Guanka
    {
        public int guanka { get; set; }
        public int guanka_last { get; set; }
        public List<GameObject> pans = new List<GameObject>();
        public int breakID { get; set; }
        public int answerIndex { get; set; }
        public List<string> soundABs { get; set; }
        public List<string> soundNames { get; set; }
        public int dir { get; set; }
        public List<float> guanka3Angels4 { get; set; }
        public List<float> guanka3Angels3 { get; set; }
        public Guanka()
        {
            guanka_last = 3;
        }
        public void Set(int _guanka)
        {
            guanka = _guanka;
            pans.Clear();
            dir = Common.GetRandValue(0, 10) % 2 == 0 ? 1 : -1;
            Debug.Log("dir : " + dir);
            switch (_guanka)
            {
                case 1:
                    soundABs = new List<string>() { "foodshape_sound", "foodshape_sound" };
                    soundNames = new List<string>() { "game-tips3-6-13-1", "game-tips3-6-13-2" };
                    break;
                case 2:
                    soundABs = new List<string>() { "foodshape_sound" };
                    soundNames = new List<string>() { "game-tips3-6-13-8" };
                    break;
                case 3:
                    soundABs = new List<string>() { "foodshape_sound" };
                    soundNames = new List<string>() { "game-tips3-6-13-11" };
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
        }
        public void setPart3Angel()
        {
            if (null == guanka3Angels4)
            {
                guanka3Angels4 = new List<float>();
                guanka3Angels3 = new List<float>();
            }
            else
            {
                guanka3Angels4.Clear();
                guanka3Angels3.Clear();
            }
            for (int i = 0; i < 6; i++)
            {
                if (i != answerIndex && ((answerIndex * 90) % 360 != (i * 90) % 360))
                {
                    guanka3Angels4.Add(i * 90);
                    if (guanka3Angels4.Count >= 3)
                    {
                        break;
                    }
                }

            }
            guanka3Angels4.Add(answerIndex * 90);
            guanka3Angels4 = Common.BreakRank(guanka3Angels4);
        }
    }

    void Awake()
    {
        mSound = gameObject.AddComponent<SoundManager>();
    }
    // Use this for initialization
    void Start () {
        bg = UguiMaker.newRawImage("bg", transform, "foodshape_texture", "bg", false);
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);
        StartCoroutine(TInit());
    }
	IEnumerator TInit()
    {
        yield return new WaitForSeconds(0.2f);
        if (null == zhuobu)
        {
            zhuobu = UguiMaker.newRawImage("zhuobu", transform, "foodshape_texture", "zhuobu");
        }
        Vector3 startPos1 = new Vector3(0, 800, 0);
        Vector3 endPos1 = new Vector3(0, 0, 0);
        for (float i = 0; i < 1f; i += 0.02f)
        {
            zhuobu.transform.localPosition = Vector3.Lerp(startPos1, endPos1, i);
            yield return new WaitForSeconds(0.01f);
        }
        zhuobu.transform.localPosition = endPos1;
        mSound.PlayBgAsync("bgmusic_loop3", "bgmusic_loop3", 0.2f);
        setGameData(1);
    }
	// Update is called once per frame
	void Update () {
        
        if (inpass || isShowInput) return;

        if (Input.GetMouseButtonDown(0))
        {
            isdownSub = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            if (null != hits)
            {
                PanZiCtr.gSelect = null;
                foreach (RaycastHit hit in hits)
                {
                    PanZiCtr com = hit.collider.gameObject.GetComponent<PanZiCtr>();
                    if(null != com)
                    {
                        if (!com.checkFix())
                        {
                            mSound.StopTip();
                            mSound.PlayShort("foodshape_sound", "starmove");
                            com.playOkEffect();
                            inpass = true;
                            NextGame();
                        }
                        else
                        {
                            mSound.PlayShort("foodshape_sound", "choose_error");
                            if(mGuanka.dir == -1)
                            {
                                if (Common.GetRandValue(0, 10) % 2 == 0)
                                {
                                    mSound.PlayTip("foodshape_sound", "game-tips3-6-13-4");
                                }
                                else
                                {
                                    mSound.PlayTip("foodshape_sound", "game-tips3-6-13-7");
                                }
                            }else
                            {
                                if (Common.GetRandValue(0, 10) % 2 == 0)
                                {
                                    mSound.PlayTip("foodshape_sound", "game-tips3-6-13-5");
                                }
                                else
                                {
                                    mSound.PlayTip("foodshape_sound", "game-tips3-6-13-6");
                                }
                            }
                            
                            com.playErrEffect();
                        }
                    }

                    Image comImage = hit.collider.gameObject.GetComponent<Image>();
                    //Debug.Log(comImage.name.Split('_')[0] + "; comImage.name : " + comImage.name);
                    if(comImage.name.Split('_')[0] == "mkbg")
                    {
                        PanZi2Ctr.gSelectbg = comImage;
                        showInput(comImage.transform);
                    }
                    else if (comImage.name == "panzi3")
                    {
                        PanZi3Ctr.gSelect = comImage;
                        showInput(comImage.transform);
                    }
                    else if (comImage.name.Split('_')[0] == "sug")
                    {
                        PanZi3Ctr.gSelect = comImage.transform.parent.parent.gameObject.GetComponent<Image>();
                        PanZi3Ctr.gSelectItem = comImage;
                        showInput(comImage.transform);
                    }
                    else if (comImage.name == "submitbutton")
                    {
                        isdownSub = true;
                        mSound.PlayTip("foodshape_sound", "submit_up", 1);
                        mbutton.sprite = ResManager.GetSprite("foodshape_sprite", "submit_down");
                    }
                }

            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isdownSub)
            {
                inpass = true;
                mSound.StopTip();
                checkPraintFinish();
            }
            isdownSub = false;
        }

    }
    //关卡2判断结束
    private void checkPraintFinish()
    {
        bool boo = true;
        PanZi3Ctr pan3 = null;
        if (mGuanka.guanka == 2)
        {
            for (int i = 0; i < mGuanka.pans.Count; i++)
            {
                PanZi2Ctr pan = mGuanka.pans[i].GetComponent<PanZi2Ctr>();
                bool state = pan.checkFinish();
                if (!state)
                {
                    boo = false;
                }
            }
        }else
        {
            pan3 = mGuanka.pans[mGuanka.answerIndex].GetComponent<PanZi3Ctr>();
            if(null != pan3)
            {
                boo = pan3.checkFinish();
            }
        }
        

        if (boo)
        {
            mSound.PlayShort("foodshape_sound", "starmove");
            mOKBtn_Effect.Play();
            inpass = true;
            NextGame();
        }else
        {
            mbutton.sprite = ResManager.GetSprite("foodshape_sprite", "submit_up");
            mbutton.gameObject.SetActive(true);
            inpass = false;
            mSound.PlayShort("foodshape_sound", "choose_error");
            if (mGuanka.guanka == 2)
            {
                
                mSound.PlayTip("foodshape_sound", "game-tips3-6-13-9");
            }
            else
            {
                //mSound.PlayTip("foodshape_sound", "game-tips3-6-13-12");
                //StartCoroutine(TDelayOpenInpass());
            }
        }
    }
    //输入框选择关闭后回调
    private void getInputData()
    {
        string chooseName = input.chooseName;

        if(null != chooseName)
        {
            if(mGuanka.guanka == 2)
            {
                PanZi2Ctr pan = PanZi2Ctr.gSelectbg.transform.parent.parent.gameObject.GetComponent<PanZi2Ctr>();
                pan.setMake(chooseName, PanZi2Ctr.gSelectbg.name.Split('_')[1]);
            }else
            {
                PanZi3Ctr pan = PanZi3Ctr.gSelect.transform.gameObject.GetComponent<PanZi3Ctr>();
                //pan.setMake(chooseName, PanZi2Ctr.gSelectbg.name.Split('_')[1]);
                //Debug.Log(chooseName.Split('_')[0]);
                if(chooseName.Split('_')[0] == "sugbg")
                {
                    mGuanka.guanka3Angels3.Clear();
                    int chooseAngle = int.Parse(chooseName.Split('_')[1]);
                    for(int i = 0;i < mGuanka.guanka3Angels4.Count; i++)
                    {
                        if(Mathf.Abs(mGuanka.guanka3Angels4[i] - chooseAngle) > 10)
                        {
                            mGuanka.guanka3Angels3.Add(mGuanka.guanka3Angels4[i]);
                        }
                    }
                    pan.setLine(chooseAngle);

                }else
                {
                    pan.setItem(chooseName);
                }
                
            }
            mSound.PlayShort("foodshape_sound", "连接正确");
        }
        else
        {
            mSound.PlayShort("foodshape_sound", "色块落下的声音");
        }
        StartCoroutine(TDelaySetInputState());
    }
    private void showInput(Transform eventTra)
    {
        if(null == input)
        {
            GameObject inputGo = UguiMaker.newGameObject("input", transform);
            input = inputGo.AddComponent<InputCte>();

        }
        mSound.PlayShort("foodshape_sound", "色块落下的声音");
        int itemsNum = 6;
        if(mGuanka.guanka == 3)
        {
            PanZi3Ctr pan = PanZi3Ctr.gSelect.transform.gameObject.GetComponent<PanZi3Ctr>();
            itemsNum = 4;
            if (pan.isHasBg)
            {
                itemsNum = 6;
            }
        }
        input.setData(mGuanka, itemsNum, getInputData);
        Vector3 mousePos = Common.getMouseLocalPos(transform);
        //Debug.Log("eventTra.parent.name : " + eventTra.parent.name);
        input.transform.localPosition = mousePos - new Vector3(130, 0, 0);//eventTra.localPosition + eventTra.parent.localPosition - new Vector3(130, 0, 0);//
        if(input.transform.localPosition.y < -300)
        {
            Vector3 pos = input.transform.localPosition;
            pos.y = -300;
            input.transform.localPosition = pos;
        }
        isShowInput = true;
        input.show();
    }
    //下一关
    private void NextGame()
    {
        StartCoroutine(TNextGame());
    }
    private void reGame()
    {
        mGuanka.guanka = 1;
        setGameData(1);
    }
    //设置游戏数据
    private void setGameData(int guanka)
    {
        if (guanka == 1)
        {
            TopTitleCtl.instance.Reset();
        }
        else
        {
            TopTitleCtl.instance.AddStar();
        }
        mGuanka.Set(guanka);

        inpass = true;
        isShowInput = false;

        if(null != foodContain)
        {
            GameObject.Destroy(foodContain);
        }
        foodContain = UguiMaker.newGameObject("foodContain", transform);
        StartCoroutine(TshowDaocha());
        StartCoroutine(Tshowzhuangshi());
        if (mGuanka.guanka == 1)
        {
            StartCoroutine(TshowPanzi());
        }else if(mGuanka.guanka == 2)
        {
            StartCoroutine(TshowPanzi2());
        }
        else if (mGuanka.guanka == 3)
        {
            StartCoroutine(TshowPanzi3());
        }
    }
    IEnumerator TDelayOpenInpass()
    {
        yield return new WaitForSeconds(2f);
        inpass = false;
    }
    //显示关卡3盘子
    IEnumerator TshowPanzi3()
    {
        Vector3 startPanPos = new Vector3(-334, 175, 0);
        if(null != input)
        {
            GameObject.Destroy(input.gameObject);
            input = null;
        }

        mGuanka.answerIndex = Common.GetRandValue(0, 5);
        mGuanka.setPart3Angel();
        for (int j = 0; j < 6; j++)
        {
            Image toolpan = UguiMaker.newImage("pan_" + j, foodContain.transform, "foodshape_sprite", "pan_2");
            toolpan.transform.localScale = Vector3.one * 0.9f;
            BoxCollider box = toolpan.gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(280, 280, 0);
            PanZi3Ctr pzc = toolpan.gameObject.AddComponent<PanZi3Ctr>();
            int index = j + 1;
            bool boo = false;
            if(mGuanka.answerIndex == j)
            {
                boo = true;
            }

            pzc.setData(j, j * 90, boo, mGuanka.dir);
            GameObject numgo = UguiMaker.newGameObject("num_" + index, foodContain.transform);
            Image numImage = UguiMaker.newImage("numbg", numgo.transform, "foodshape_sprite", "numbg");
            Image num = UguiMaker.newImage("num", numgo.transform, "foodshape_sprite", index.ToString());
            num.transform.localScale = Vector3.one * 0.2f;
            Vector3 endPos = startPanPos + new Vector3((j % 3) * 330, -(int)(j / 3) * 365, 0);
            Vector3 endNumPos = endPos + new Vector3(0, -175, 0);
            Vector3 startPos = endPos + new Vector3(0, 800, 0);
            for (float i = 0; i < 1f; i += 0.2f)
            {
                toolpan.transform.localPosition = Vector3.Lerp(startPos, endPos, i);
                numgo.transform.localPosition = Vector3.Lerp(startPos, endNumPos, i);
                yield return new WaitForSeconds(0.01f);
            }
            toolpan.transform.localPosition = endPos;
            numgo.transform.localPosition = endNumPos;
            mGuanka.pans.Add(pzc.gameObject);
        }
        createSubBtn();
        inpass = false;
        yield return new WaitForSeconds(2f);
        mSound.PlayTipList(mGuanka.soundABs, mGuanka.soundNames, true);
        
    }
    //显示关卡2盘子
    IEnumerator TshowPanzi2()
    {
        Vector3 startPanPos = new Vector3(-334, 175, 0);
        int allCloseNum = Common.GetRandValue(3, 5);
        List<Vector3> vec3 = new List<Vector3>();
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int z = 0; z < 3; z++)
                {
                    if (x + y + z == 3)
                    {
                        vec3.Add(new Vector3(x, y, z));
                    }
                }
            }
        }
        Vector3 onev = Common.BreakRank(vec3)[0];
        //Debug.Log("onev : " + onev + ";allCloseNum : " + allCloseNum);
        int left = allCloseNum - 3;
        List<int> list = new List<int>() { (int)onev.x, (int)onev.y, (int)onev.z };
        for (int i = 0; i < left; i++)
        {
            list.Add(1);
        }
        //list = Common.BreakRank(list);
        for (int j = 0; j < 6; j++)
        {
            Image toolpan = UguiMaker.newImage("pan_" + j, foodContain.transform, "foodshape_sprite", "pan_2");// + mGuanka.guanka
            toolpan.transform.localScale = Vector3.one * 0.9f;
            BoxCollider box = toolpan.gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(280, 280, 0);
            PanZi2Ctr pzc = toolpan.gameObject.AddComponent<PanZi2Ctr>();
            int closeNum = 0;
            if(j > 0 && j <= list.Count)
            {
                closeNum = list[j - 1];
            }
            //Debug.Log("pan_" + j + " closeNum : " + closeNum);
            pzc.setData(j, closeNum,mGuanka.dir);
            int index = j + 1;
            GameObject numgo = UguiMaker.newGameObject("num_" + index, foodContain.transform);
            Image numImage = UguiMaker.newImage("numbg", numgo.transform, "foodshape_sprite", "numbg");
            Image num = UguiMaker.newImage("num", numgo.transform, "foodshape_sprite", index.ToString());
            num.transform.localScale = Vector3.one * 0.2f;
            Vector3 endPos = startPanPos + new Vector3((j % 3) * 330, -(int)(j / 3) * 365, 0);
            Vector3 endNumPos = endPos + new Vector3(0, -175, 0);
            Vector3 startPos = endPos + new Vector3(0, 800, 0);
            for (float i = 0; i < 1f; i += 0.2f)
            {
                toolpan.transform.localPosition = Vector3.Lerp(startPos, endPos, i);
                numgo.transform.localPosition = Vector3.Lerp(startPos, endNumPos, i);
                yield return new WaitForSeconds(0.01f);
            }
            toolpan.transform.localPosition = endPos;
            numgo.transform.localPosition = endNumPos;
            mGuanka.pans.Add(pzc.gameObject);
        }
        createSubBtn();

        mGuanka.breakID = Common.GetMutexValue(0, 5);
        inpass = false;
        yield return new WaitForSeconds(2f);
        mSound.PlayTipList(mGuanka.soundABs, mGuanka.soundNames, true);

    }
    private void createSubBtn()
    {
        if (null == mbutton)
        {
            mbutton = UguiMaker.newImage("submitbutton", transform, "foodshape_sprite", "submit_up");
            Canvas canvas = mbutton.gameObject.AddComponent<Canvas>();
            mbutton.gameObject.layer = LayerMask.NameToLayer("UI");
            canvas.overrideSorting = true;
            canvas.sortingOrder = 11;
            mbutton.transform.localPosition = new Vector3(515, -325, 0);
            mbutton.transform.localScale = Vector3.one * 0.8f;
            mOKBtn_Effect = ResManager.GetPrefab("effect_star0", "effect_star1").GetComponent<ParticleSystem>();
            UguiMaker.InitGameObj(mOKBtn_Effect.gameObject, mbutton.transform, "effect", Vector3.zero, Vector3.one);
            BoxCollider buttonBox = mbutton.gameObject.AddComponent<BoxCollider>();
            buttonBox.size = new Vector3(150, 125, 0);
        }else
        {
            mbutton.sprite = ResManager.GetSprite("foodshape_sprite", "submit_up");
            mbutton.gameObject.SetActive(true);
        }
    }
    //显示刀叉
    IEnumerator TshowDaocha()
    {
        Vector3 startS = Vector3.zero;
        Vector3 ends = Vector3.one;
        Vector3 startPosup = new Vector3(-340, 367, 0);
        for (int j = 0; j < 3; j++)
        {
            Image toolsup = UguiMaker.newImage("tool_up_" + j, foodContain.transform, "foodshape_sprite", "toos_up");
            toolsup.transform.localScale = Vector3.one * 0.9f;
            toolsup.transform.localPosition = startPosup + new Vector3(j * 338, 0, 0);
            mSound.PlayShort("foodshape_sound", "素材出现通用音效");
            for (float i = 0; i < 1f; i += 0.1f)
            {
                toolsup.transform.localScale = Vector3.Lerp(startS, ends, i);
                yield return new WaitForSeconds(0.01f);
            }
            toolsup.transform.localScale = ends;
        }
        Vector3 startPosdown = new Vector3(-340, -385, 0);
        for (int j = 0; j < 3; j++)
        {
            Image toolsdown = UguiMaker.newImage("tool_down_" + j, foodContain.transform, "foodshape_sprite", "toos_up");
            toolsdown.transform.localEulerAngles = new Vector3(0, 0, 180);
            toolsdown.transform.localScale = Vector3.one * 0.9f;
            toolsdown.transform.localPosition = startPosdown + new Vector3(j * 338, 0, 0);
            mSound.PlayShort("foodshape_sound", "素材出现通用音效");
            for (float i = 0; i < 1f; i += 0.1f)
            {
                toolsdown.transform.localScale = Vector3.Lerp(startS, ends, i);
                yield return new WaitForSeconds(0.01f);
            }
            toolsdown.transform.localScale = ends;
        }
    }
    //显示盘子
    IEnumerator TshowPanzi()
    {
        if (null != mbutton)
        {
            mbutton.gameObject.SetActive(false);
        }
        mGuanka.breakID = Common.GetMutexValue(0, 5);
        Vector3 startPanPos = new Vector3(-334, 175, 0);
        for (int j = 0; j < 6; j++)
        {
            Image toolpan = UguiMaker.newImage("pan_" + j, foodContain.transform, "foodshape_sprite", "pan_2");// + mGuanka.guanka
            toolpan.rectTransform.sizeDelta = new Vector2(330, 328);
            toolpan.transform.localScale = Vector3.one * 0.9f;
            BoxCollider box = toolpan.gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(280, 280, 0);
            PanZiCtr pzc = toolpan.gameObject.AddComponent<PanZiCtr>();
            if (j == mGuanka.breakID)
            {
                pzc.setData(j,-1,mGuanka.dir);
            }else
            {
                pzc.setData(j,j, mGuanka.dir);
            } 
            int index = j + 1;
            GameObject numgo = UguiMaker.newGameObject("num_" + index, foodContain.transform);
            Image numImage = UguiMaker.newImage("numbg", numgo.transform, "foodshape_sprite", "numbg");
            Image num = UguiMaker.newImage("num", numgo.transform, "foodshape_sprite", index.ToString());
            num.transform.localScale = Vector3.one * 0.2f;
            Vector3 endPos = startPanPos + new Vector3((j % 3) * 330, -(int)(j / 3) * 365, 0);
            Vector3 endNumPos = endPos + new Vector3(0,-175,0);
            Vector3 startPos = endPos + new Vector3(0, 800, 0);
            for (float i = 0; i < 1f; i += 0.2f)
            {
                toolpan.transform.localPosition = Vector3.Lerp(startPos, endPos, i);
                numgo.transform.localPosition = Vector3.Lerp(startPos, endNumPos, i);
                yield return new WaitForSeconds(0.01f);
            }
            toolpan.transform.localPosition = endPos;
            numgo.transform.localPosition = endNumPos;
            mGuanka.pans.Add(pzc.gameObject);
        }
        inpass = false;
        yield return new WaitForSeconds(2f);
        mSound.PlayTipList(mGuanka.soundABs, mGuanka.soundNames, true);
    }
    IEnumerator TDelaySetInputState()
    {
        yield return new WaitForSeconds(0.5f);
        isShowInput = false;
    }
    //显示装饰
    IEnumerator Tshowzhuangshi()
    {
        Vector3 startS = Vector3.zero;
        Vector3 ends = Vector3.one;
        Vector3 startPos = new Vector3(-175, -30, 0);
        if(mGuanka.guanka == 1)
        {
            for (int j = 1; j < 4; j++)
            {
                Image zhuangshi = UguiMaker.newImage("zhuangshi_" + j, foodContain.transform, "foodshape_sprite", "zhuangshi_" + j);
                zhuangshi.transform.localPosition = startPos + new Vector3((j - 1) * 350, 0, 0);
                mSound.PlayShort("foodshape_sound", "素材出现通用音效");
                if(j== 3)
                {
                    ends = Vector3.one * 0.8f;
                }
                for (float i = 0; i < 1f; i += 0.1f)
                {
                    zhuangshi.transform.localScale = Vector3.Lerp(startS, ends, i);
                    yield return new WaitForSeconds(0.01f);
                }
                zhuangshi.transform.localScale = ends;

            }
        }else if(mGuanka.guanka == 2 || mGuanka.guanka == 3)
        {
            Image zhuangshi = UguiMaker.newImage("zhuangshi_1", foodContain.transform, "foodshape_sprite", "zhuangshi_3");
            zhuangshi.transform.localPosition = startPos + new Vector3(2 * 350, 0, 0);
            mSound.PlayShort("foodshape_sound", "素材出现通用音效");
            ends = Vector3.one * 0.8f;
            for (float i = 0; i < 1f; i += 0.1f)
            {
                zhuangshi.transform.localScale = Vector3.Lerp(startS, ends, i);
                yield return new WaitForSeconds(0.01f);
            }
            zhuangshi.transform.localScale = ends;
        }
    }
    IEnumerator TNextGame()
    {
        mSound.PlayTip("foodshape_sound", "game-tips3-6-13-10");
        yield return new WaitForSeconds(3f);
        mGuanka.guanka++;
        if(mGuanka.guanka > mGuanka.guanka_last)
        {
            TopTitleCtl.instance.AddStar();
            GameObject.Destroy(input.gameObject);
            input = null;
            mbutton.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            GameOverCtl.GetInstance().Show(mGuanka.guanka_last, reGame);
        }else
        {
            mSound.PlayShort("foodshape_sound", "素材入画面");
            Vector3 startPos = Vector3.zero;
            Vector3 endPos = new Vector3(0, -850, 0);
            for (float i = 0; i < 1f; i += 0.02f)
            {
                foodContain.transform.localPosition = Vector3.Lerp(startPos, endPos, i);
                zhuobu.uvRect = new Rect(0, i, 1, 1);
                yield return new WaitForSeconds(0.01f);
            }
            foodContain.transform.localPosition = endPos;
            
            setGameData(mGuanka.guanka);
        }
        


    }
}
