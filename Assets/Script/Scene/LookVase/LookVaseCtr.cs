using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Spine.Unity;

public class LookVaseCtr : MonoBehaviour {
    public Guanka mGuanka = new Guanka();
    public SoundManager mSound { get; set; }
    private RawImage bg { get; set; }
    private bool inpass { get; set; }
    //private Image drawsprite { get; set; }
    private Image line { get; set; }
    private Image startImage { get; set; }
    private Image mbutton { get; set; }
    private ParticleSystem mOKBtn_Effect { get; set; }
    private bool isdownSub { get; set; }
    private bool isInit { get; set; }
    private Image pointDemo1 { get; set; }
    private Image pointDemo2 { get; set; }
    private bool isShowDemo { get; set; }
    public static Image praintImage { get; set; }
    private ParticleSystem mStarparticle { get; set; }
    public class Guanka
    {
        public int guanka { get; set; }
        public int guanka_last { get; set; }
        public List<string> vasedics { get; set; }
        public List<int> vaseIndexs = new List<int>() { 1, 2, 3, 4, 5 };
        public int vaseIndex { get; set; }
        public Dictionary<string, Image> drawlines = new Dictionary<string, Image>();
        public List<string> markLists = new List<string>();//角色和墙上图像
        public List<int> roleIds { get; set; }
        public List<Vector3> rolePoss = new List<Vector3>() { new Vector3(-377, -259, 0), new Vector3(-17, -122, 0), new Vector3(372, -276, 0) ,  new Vector3(-20, -475, 0) };
        public Dictionary<string, GameObject> spines = new Dictionary<string, GameObject>();
        public List<Image> photos = new List<Image>();
        public List<Image> photopoints = new List<Image>();
        public Image demo { get; set; }
        public Dictionary<string, Vector3> rolepointPoss = new Dictionary<string, Vector3>() { { "left", new Vector3(-306, -164, 0) }, { "right", new Vector3(240, -157, 0) }, { "botton", new Vector3(-136, -233, 0) }, { "top", new Vector3(-115, -59, 0) } };
        public List<PhotoCtr> pcs = new List<PhotoCtr>();
        public Guanka()
        {
            guanka_last = 2;
            
        }
        public void resetCurrTime()
        {
            //currtimes = 0;
        }
        public void Set(int _guanka)
        {
            if(1 == _guanka)
            {
                vaseIndexs = Common.BreakRank(vaseIndexs);
                roleIds = new List<int>() { 11, 12, 15, 9 };
            }
            guanka = _guanka;
            vasedics = Common.BreakRank(new List<string>() { "left", "top", "right", "botton" });
            vaseIndex = vaseIndexs[_guanka];
            //drawlines.Clear();
            //markLists.Clear();
            //spines.Clear();
            
            switch (_guanka)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
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
    private float scaleTime = 0;
    void Start()
    {
        bg = UguiMaker.newRawImage("bg", transform, "lookvase_texture", "bg", false);
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);
        StartCoroutine(TInit());
    }
    IEnumerator TInit()
    {
        yield return new WaitForSeconds(0.5f);
        //mSound.PlayBgAsync("bgmusic_loop1", "bgmusic_loop1", 0.1f);
        setGameData(1, true);
        isInit = true;
    }
    // Update is called once per frame
    Vector3 temp_select_offset = Vector3.zero;
    void Update()
    {
        /*
        if (isShowDemo)
        {
            scaleTime += 0.1f;
            pointDemo1.rectTransform.localScale = Vector3.one * (1f + 0.5f * Mathf.Sin(scaleTime));
            pointDemo2.rectTransform.localScale = Vector3.one * (1f + 0.5f * Mathf.Sin(scaleTime));
        }
        */
        if (inpass) return;

        if (Input.GetMouseButtonDown(0))
        {
            isdownSub = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            if (null != hits)
            {
                praintImage = null;
                startImage = null;
                foreach (RaycastHit hit in hits)
                {
                    Image com = hit.collider.gameObject.GetComponent<Image>();
                    //Debug.Log("com.name : " + com.name);
                    if (null != com && com.name != "submitbutton"  && com.name.Split('_')[0] != "praint" && false)
                    {
                        startImage = com;
                        if (startIsMark(com.name))
                        {
                            line = getMarkedLine(com.name);
                            removeMarked(getMarkedkey(com.name));
                        }
                        else
                        {
                            line = UguiMaker.newGameObject("line", transform).AddComponent<Image>();
                            line.rectTransform.sizeDelta = new Vector2(5, 5);
                            line.rectTransform.pivot = new Vector2(0, 0.5f);
                            line.color = new Color(37 / 255f, 160 / 255f, 210 / 255f);
                        }
                        line.transform.localPosition = startImage.transform.localPosition;
                        mSound.PlayShort("lookvase_sound", "dropline");

                    }else
                    {
                        if(com.name == "submitbutton")
                        {
                            //isdownSub = true;
                            //mSound.PlayTip("lookvase_sound", "submit_up", 1);
                            //mbutton.sprite = ResManager.GetSprite("lookvase_sprite", "submit_down");
                        }else if (com.name.Split('_')[0] == "praint")
                        {
                            praintImage = com;
                            temp_select_offset = praintImage.gameObject.GetComponent<RectTransform>().anchoredPosition3D - Common.getMouseLocalPos(transform);
                            mSound.PlayShort("lookvase_sound", "dropline");
                        }
                    }
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (null != startImage)
            {
                Vector3 pos = Common.getMouseLocalPos(transform);
                float dis = Vector3.Distance(startImage.transform.localPosition, pos);
                line.rectTransform.sizeDelta = new Vector2(dis, 5);
                float angle = Mathf.Atan2(pos.y - startImage.transform.localPosition.y, pos.x - startImage.transform.localPosition.x) * (180 / Mathf.PI);
                line.rectTransform.localEulerAngles = new Vector3(0, 0, angle);
            }
            if(null != praintImage)
            {
                praintImage.gameObject.GetComponent<RectTransform>().anchoredPosition3D = Common.getMouseLocalPos(transform) + temp_select_offset;
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            if (null != startImage)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray);
                
                if (null != hits)
                {
                    bool isfit = true;
                    Image com = null;
                    foreach (RaycastHit hit in hits)
                    {
                        com = hit.collider.gameObject.GetComponent<Image>();
                        if (null != com)
                        {
                            string endtype = com.name.Split('_')[1];
                            string endrOrp = com.name.Split('_')[0];
                            string startrOrp = startImage.name.Split('_')[0];

                            if(endrOrp == startrOrp || endIsMark(com.name))
                            {
                                isfit = false;
                                break;
                            }
                        }
                    }
                    if (isfit && null != com && null != startImage)
                    {
                        string endtype = com.name.Split('_')[1];
                        string marktype = startImage.name + "&" + com.name;
                        mGuanka.markLists.Add(marktype);
                        mGuanka.drawlines.Add(marktype, line);
                        float dis = Vector3.Distance(com.transform.localPosition, startImage.transform.localPosition);
                        line.rectTransform.sizeDelta = new Vector2(dis, 5);
                        float angle = Mathf.Atan2(com.transform.localPosition.y - startImage.transform.localPosition.y, com.transform.localPosition.x - startImage.transform.localPosition.x) * (180 / Mathf.PI);
                        line.rectTransform.localEulerAngles = new Vector3(0, 0, angle);
                        mSound.PlayShort("lookvase_sound", "linesuc");
                        if (isShowDemo)
                        {
                            isShowDemo = false;
                            pointDemo1.rectTransform.localScale = Vector3.one;
                            pointDemo2.rectTransform.localScale = Vector3.one;
                        }
                        StopCoroutine("TShowPointDemo");
                    }
                    else
                    {
                        if(null != startImage && null != com)
                        {
                            removeMarked(startImage.name + "&" + com.name);
                        }
                       if(null != line)
                        {
                            mSound.PlayShort("lookvase_sound", "lineback");
                            GameObject.Destroy(line.gameObject);
                        }
                    }
                }
            } 
            startImage = null;

            if (isdownSub)
            {
                //mbutton.sprite = ResManager.GetSprite("lookvase_sprite", "submit_up");
                //mbutton.gameObject.SetActive(true);
                //inpass = true;
                //mSound.StopTip();
                //checkPraintFinish();
                //if (mGuanka.drawlines.Count != 0)
                //{
                //    inpass = true;
                //    checkPraintFinish();
                //    //checkFinish();
                //}
            }

            if (null != praintImage)
            {
                string type = praintImage.name.Split('_')[1];
                StartCoroutine(TBackTo(praintImage,type));
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray);
                if (null != hits)
                {
                    PhotoCtr pc = null;
                    bool ishit = false;
                    foreach (RaycastHit hit in hits)
                    {
                        pc = hit.collider.gameObject.GetComponent<PhotoCtr>();
                        if (null != pc)
                        {
                            ishit = true;
                            pc.praint(type);
                            break;
                        }
                    }
                    if (ishit)
                    {
                        mSound.PlayShort("lookvase_sound", "linesuc");
                        PlayStarParticle(pc.transform.localPosition + new Vector3(0, -120, 0));
                    }
                    else
                    {
                        mSound.PlayShort("lookvase_sound", "lineback");
                    }
                }
            }
        }
    }
    //播放特效
    private void PlayStarParticle(Vector3 pos)
    {
        if (null == mStarparticle)
        {
            GameObject obj1 = ResManager.GetPrefab("effect_star2", "effect_star2");
            obj1.transform.parent = transform;
            mStarparticle = obj1.GetComponent<ParticleSystem>();
            mStarparticle.transform.localPosition = Vector3.zero;
            mStarparticle.transform.localScale = Vector3.one;
        }
        mStarparticle.transform.localPosition = pos;
        mStarparticle.Play();
    }
    private void playErr(string errName)
    {
        mSound.PlayTip("lookvase_sound", errName);
    }

    private void checkPraintFinish()
    {
        bool boo = true;
        int errRoleId = 0;
        bool isMarkAll = true;
        for (int i = 0; i < mGuanka.pcs.Count; i++)
        {
            PhotoCtr pc = mGuanka.pcs[i];
            if (!pc.isMark)
            {
                isMarkAll = false;
            }
        }
        List<int> errIds = new List<int>();
        List<int> okIds = new List<int>();
        for (int i = 0; i < mGuanka.pcs.Count; i++)
        {
            PhotoCtr pc = mGuanka.pcs[i];
            if(null != pc.mcurrType)
            {
                GameObject go = mGuanka.spines[pc.mcurrType];
                int id = int.Parse(go.name.Split('_')[1]);
                SkeletonGraphic spine = go.transform.Find("spine").gameObject.GetComponent<SkeletonGraphic>();
               
                if (!pc.checkFix())
                {
                    boo = false;
                    errIds.Add(id);
                    mSound.PlayShort("aa_animal_effect_sound", MDefine.GetAnimalEffectSoundNameByID_CH(id, "no"));
                    StartCoroutine(TplaySpine(spine, "Mistake", true));
                }
                else
                {
                    okIds.Add(id);
                    mSound.PlayShort("aa_animal_effect_sound", MDefine.GetAnimalEffectSoundNameByID_CH(id, "no"));
                    StartCoroutine(TplaySpine(spine, "Correct", false));
                }
            }
        }
        List<int> stampList = new List<int>();
        for (int i = 0; i < errIds.Count; i++)
        {
            int id = errIds[i];
            if (!okIds.Contains(id))
            {
                stampList.Add(id);
            }
        }

        if (!isMarkAll)
        {
            inpass = false;
        }
        if (boo && isMarkAll)
        {
            mSound.PlayTip("lookvase_sound", "game-tips3-6-6-8");
            mOKBtn_Effect.Play();
            mSound.PlayShort("lookvase_sound", "starmove");
            nextGame();
        }else
        {
            mbutton.sprite = ResManager.GetSprite("lookvase_sprite", "submit_up");
            mbutton.gameObject.SetActive(true);

            if (stampList.Count > 0)
            {
                errRoleId = Common.BreakRank(stampList)[0];
            }
            Debug.Log("errRoleId : " + errRoleId);
            if (errRoleId != 0 && errRoleId != 9)
            {
                StartCoroutine(TdelayPlayErr(errRoleId));
            }
            else
            {
                playErr("game-tips3-6-6-4");
            }
            inpass = false;
        }
    }
    //检测划线的完成情况
    /*
    private void checkFinish()
    {
        bool boo = true;
        int errRoleId = 0;
        foreach (string key in mGuanka.drawlines.Keys)
        {
            string[] strs = key.Split('&');
           
            if (strs[0].Split('_')[1] != strs[1].Split('_')[1])
            {
                boo = false;
                Image line = mGuanka.drawlines[key];
                if(null != line)
                {
                    StartCoroutine(TremoveLine(line, key));
                    //TODO 播放对应动画的伤心动作
                    
                    if (findMarked(strs[1].Split('_')[1]))
                    {
                        GameObject go = mGuanka.spines[strs[1].Split('_')[1]];
                        int id = int.Parse(go.name.Split('_')[1]);
                        SkeletonGraphic spine = go.transform.FindChild("spine").gameObject.GetComponent<SkeletonGraphic>();
                        mSound.PlayShort("aa_animal_effect_sound", MDefine.GetAnimalEffectSoundNameByID_CH(id, "no"));
                        errRoleId = id;
                        StartCoroutine(TplaySpine(spine, "Mistake", true));
                        
                    }
                    else
                    {
                        GameObject go = mGuanka.spines[strs[0].Split('_')[1]];
                        int id = int.Parse(go.name.Split('_')[1]);
                        mSound.PlayShort("aa_animal_effect_sound", MDefine.GetAnimalEffectSoundNameByID_CH(id, "no"));
                        errRoleId = id;
                        SkeletonGraphic spine = go.transform.FindChild("spine").gameObject.GetComponent<SkeletonGraphic>();
                        StartCoroutine(TplaySpine(spine, "Mistake",true));
                    }
                }
                
            }
            else
            {
                //TODO 播放对应动画的开心动作
                GameObject go = mGuanka.spines[strs[1].Split('_')[1]];
                int id = int.Parse(go.name.Split('_')[1]);
                mSound.PlayShort("aa_animal_effect_sound", MDefine.GetAnimalEffectSoundNameByID_CH(id, "yes"));
                SkeletonGraphic spine = go.transform.FindChild("spine").gameObject.GetComponent<SkeletonGraphic>();
                StartCoroutine(TplaySpine(spine, "Correct"));
            }
        }
        
        if (boo)
        {
            if (mGuanka.drawlines.Count < 4)
            {
                if (inpass)
                {
                    inpass = false;
                }
                playErr("game-tips3-6-11-7");
            }else
            {
                mSound.PlayTip("lookvase_sound", "game-tips3-6-6-8");
                mOKBtn_Effect.Play();
                mSound.PlayShort("lookvase_sound", "starmove");
                nextGame();
            }
            
        }
        else
        {
            Debug.Log("errRoleId : " + errRoleId);
            if(errRoleId != 0 && errRoleId != 9)
            {
                StartCoroutine(TdelayPlayErr(errRoleId));
            }
            else
            {
                playErr("game-tips3-6-6-4");
            }
            inpass = false;

        }
    }
    */

    private bool findMarked(string type)
    {
        bool boo = false;
        foreach (string key in mGuanka.drawlines.Keys)
        {
            string[] strs = key.Split('&');
            if (type == strs[0].Split('_')[1] || type == strs[1].Split('_')[1] && strs[1].Split('_')[0] == "role")
            {
                boo = true;
                break;
            }
        }
        return boo;
    }

    //清理所有已经标记过的数据
    private void cleanMarkedLine()
    {
        foreach (string key in mGuanka.drawlines.Keys)
        {
            Image line = mGuanka.drawlines[key];
            GameObject.Destroy(line.gameObject);
        }
        mGuanka.drawlines.Clear();
        mGuanka.markLists.Clear();
    }
    //清理当前已经标记过的数据
    private void removeMarked(string marktype)
    {
        //string marktype = getMarkedkey(startimage.name);
        if(null != marktype)
        {
            mGuanka.drawlines.Remove(marktype);
            mGuanka.markLists.Remove(marktype);
        }
    }
    //判断结束点是否已经连过线
    private bool endIsMark(string endname)
    {
        //Debug.Log("endname : " + endname);
        bool boo = false;
        for(int i = 0;i < mGuanka.markLists.Count;i++)
        {
            string[] strs = mGuanka.markLists[i].Split('&');
            if(endname == strs[0] || endname == strs[1])
            {
                boo = true;
                break;
            }
        }
        return boo;
    }
    //判断起点是否已经连过线
    private bool startIsMark(string startname)
    {
        //Debug.Log("startname : " + startname);
        bool boo = false;
        foreach (string key in mGuanka.drawlines.Keys)
        {
            string[] strs = key.Split('&');
            if (startname == strs[0] || startname == strs[1])
            {
                boo = true;
                break;
            }
        }
        return boo;
    }
    //获取已经画过的线
    private Image getMarkedLine(string startName)
    {
        Image outLine = null;
        foreach (string key in mGuanka.drawlines.Keys)
        {
            string[] strs = key.Split('&');
            if (startName == strs[0] || startName == strs[1])
            {
                outLine = mGuanka.drawlines[key];
                break;
            }
        }
        return outLine;
    }
    //获取已经画过的key
    private string getMarkedkey(string startName)
    {
        string outkey = null;
        foreach (string key in mGuanka.drawlines.Keys)
        {
            string[] strs = key.Split('&');
            if (startName == strs[0] || startName == strs[1])
            {
                outkey = key;
                break;
            }
        }
        return outkey;
    }

    private float GetAngle(Vector3 a, Vector3 b)
    {
       float angle = (180 / 3.14f) * (Mathf.Atan((b.y - a.y) / (b.x - a.x)));
        return angle;
    }

    private void nextGame()
    {
        mGuanka.guanka++;
        StartCoroutine(TNextGame());
    }
    private void reGame()
    {
        setGameData(1);
    }
    //设置游戏数据
    private bool isPlayBged { get; set; }
    private void setGameData(int guanka, bool isNextguanka = false)
    {
        if (guanka == 1)
        {
            //isShowDemo = true;
            TopTitleCtl.instance.Reset();
        }
        else
        {
            isShowDemo = false; 
            TopTitleCtl.instance.AddStar();
        }
        mGuanka.Set(guanka);
        inpass = false;
        isdownSub = false;
        if (!isInit)
        {
            StartCoroutine(initView());
        }else
        {
            cleanMarkedLine();
            resetView();
            if(null != mbutton)
            {
                mbutton.sprite = ResManager.GetSprite("lookvase_sprite", "submit_up");
                mbutton.gameObject.SetActive(true);
            }
            for (int i = 0; i < mGuanka.pcs.Count; i++)
            {
                PhotoCtr pc = mGuanka.pcs[i];
                pc.isMark = false;
            }
            mSound.PlayTipList(new List<string>() { "lookvase_sound", "lookvase_sound", "lookvase_sound" }, new List<string>() { "game-tips3-6-6-1", "game-tips3-6-6-2", "game-tips3-6-6-3" }, true);
        }

        if (mGuanka.guanka == 1)
        {
            StartCoroutine("TShowPointDemo");
        }
    }
    IEnumerator initView()
    {

        float boxsize = 60;
        Vector3 startPos = new Vector3(-462, 207, 0);
        Vector3 startScalebg = Vector3.zero;
        Vector3 endScalebg = Vector3.one;
        Vector3 endScalep = Vector3.one;
        playOut();
        for (int i = 0; i < mGuanka.vasedics.Count; i++)
        {
            string type = mGuanka.vasedics[i];
            Vector3 pos = startPos + new Vector3(i * 300, 0, 0);

            Image vpbg = UguiMaker.newImage("vpbg", transform, "lookvase_sprite", "vasebg");
            vpbg.rectTransform.localPosition = pos;
            vpbg.rectTransform.localScale = endScalebg;// Vector3.one * 1.2f;
            endScalebg = Vector3.one * 1.2f;
            Image vasephoto = UguiMaker.newImage("photo_" + type, transform, "lookvase_sprite", "vase_" + mGuanka.vaseIndex + "_" + type);
            vasephoto.rectTransform.localScale = endScalebg;// Vector3.one * 0.8f;
            endScalep = Vector3.one * 0.8f;
            vasephoto.rectTransform.localPosition = pos + new Vector3(0, 20, 0);
            PhotoCtr pc = vasephoto.gameObject.AddComponent<PhotoCtr>();
            pc.setData(type);
            mGuanka.pcs.Add(pc);
            BoxCollider boxphoto = vasephoto.gameObject.AddComponent<BoxCollider>();
            boxphoto.size = new Vector3(100, 100, 0);
            boxphoto.center = new Vector3(0, -180, 0);
            mGuanka.photos.Add(vasephoto);
            pc.ShowOut(vpbg,vasephoto);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);

        GameObject dengziGo = UguiMaker.newGameObject("dengziGo", transform);

        GameObject roleGo = UguiMaker.newGameObject("roleGo", transform);

        GameObject deskGo = UguiMaker.newGameObject("deskGo", transform);

        startScalebg = Vector3.zero;
        endScalebg = Vector3.one;
        Image desk = UguiMaker.newImage("desk", deskGo.transform, "lookvase_sprite", "desk");
        desk.rectTransform.localPosition = new Vector3(-26, -195, 0);
        playOut();
        for (float j = 0; j < 1f; j += 0.1f)
        {
            desk.rectTransform.localScale = Vector3.Lerp(startScalebg, endScalebg, j);
            yield return new WaitForSeconds(0.01f);
        }
        desk.rectTransform.localScale = endScalebg;

        List<Image> list = new List<Image>();
        Image dengzi_top = UguiMaker.newImage("dengzi_top", dengziGo.transform, "lookvase_sprite", "dengzi_top");
        dengzi_top.rectTransform.localPosition = new Vector3(-16, -72, 0);
        dengzi_top.rectTransform.localScale = startScalebg;
        list.Add(dengzi_top);

        Image dengzi_left = UguiMaker.newImage("dengzi_left", dengziGo.transform, "lookvase_sprite", "dengzi_left");
        dengzi_left.rectTransform.localPosition = new Vector3(-406, -183, 0);
        dengzi_left.rectTransform.localScale = startScalebg;
        list.Add(dengzi_left);

        Image dengzi_right = UguiMaker.newImage("dengzi_right", dengziGo.transform, "lookvase_sprite", "dengzi_right");
        dengzi_right.rectTransform.localPosition = new Vector3(356, -193, 0);
        dengzi_right.rectTransform.localScale = startScalebg;
        list.Add(dengzi_right);

        List<GameObject> roleList = new List<GameObject>();
        Dictionary<int, float> roleScaleDic = new Dictionary<int, float>();
        List<string> currs = new List<string>() { "left", "top", "right", "botton" };
        yield return new WaitForSeconds(0.1f);
        playOut();
        for (int i = 0; i < currs.Count - 1; i++)
        {
            string type = currs[i];
            GameObject prefabGo = ResManager.GetPrefab("lookvase_prefab", MDefine.GetAnimalNameByID_EN(mGuanka.roleIds[i]));
            prefabGo.name = "prefab_" + mGuanka.roleIds[i] + "_" + type;
            prefabGo.transform.parent = roleGo.transform;
            float cscale = 1;
            if(i == 1)
            {
                cscale = 1.5f;
            }
            prefabGo.transform.localScale = startScalebg;
            endScalebg = Vector3.one * cscale;
            prefabGo.transform.localPosition = mGuanka.rolePoss[i];
            SkeletonGraphic spine = prefabGo.transform.Find("spine").gameObject.GetComponent<SkeletonGraphic>();
            spine.AnimationState.SetAnimation(1, "Idle", true);
            mGuanka.spines.Add(type, prefabGo);
            roleList.Add(prefabGo);
            roleScaleDic.Add(i, cscale);
        }
        //第四个动物
        GameObject prefabGo4 = ResManager.GetPrefab("lookvase_prefab", MDefine.GetAnimalNameByID_EN(mGuanka.roleIds[3]));
        prefabGo4.name = "prefab_" + mGuanka.roleIds[3] + "_botton";
        prefabGo4.transform.parent = transform;
        prefabGo4.transform.localScale = startScalebg;
        
        prefabGo4.transform.localPosition = mGuanka.rolePoss[3];
        SkeletonGraphic spine4 = prefabGo4.transform.Find("spine").gameObject.GetComponent<SkeletonGraphic>();
        spine4.AnimationState.SetAnimation(1, "Idle", true);
        mGuanka.spines.Add("botton", prefabGo4);
        roleList.Add(prefabGo4);
        roleScaleDic.Add(3, 1.5f);

        Image dengzi_botton = UguiMaker.newImage("dengzi_botton", transform, "lookvase_sprite", "dengzi_botton");
        dengzi_botton.rectTransform.localPosition = new Vector3(-26, -377, 0);
        dengzi_botton.rectTransform.localScale = startScalebg;

        endScalebg = Vector3.one;
        list.Add(dengzi_botton);

        playOut();
        for (int i = 0;i < list.Count; i++)
        {
            StartCoroutine(Tshow(list[i].gameObject, Vector3.zero, Vector3.one));
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);

        playOut();
        for (int i = 0; i < roleScaleDic.Count; i++)
        {
            StartCoroutine(Tshow(roleList[i], Vector3.zero, Vector3.one * roleScaleDic[i]));
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);

        mGuanka.demo = UguiMaker.newImage("dengzi_demo", transform, "lookvase_sprite", "vase_" + mGuanka.vaseIndex);
        mGuanka.demo.rectTransform.localPosition = new Vector3(0, -109, 0);
        endScalebg = Vector3.one;
        playOut();
        StartCoroutine(Tshow(mGuanka.demo.gameObject, startScalebg, endScalebg));
        yield return new WaitForSeconds(0.5f);
        //盖章的按钮
        playOut();
        for (int i = 0; i < mGuanka.vasedics.Count; i++)
        {
            string type = mGuanka.vasedics[i];
            Image praint = UguiMaker.newImage("praint_" + type, transform, "lookvase_sprite", "praint_" + type);
            praint.rectTransform.localPosition = mGuanka.rolepointPoss[type];
            BoxCollider boxr = praint.gameObject.AddComponent<BoxCollider>();
            boxr.size = new Vector3(boxsize, boxsize, 0);
            //playOut();
            StartCoroutine(Tshow(praint.gameObject, startScalebg, endScalebg));
            yield return new WaitForSeconds(0.1f);
            /*
            for (float j = 0; j < 1f; j += 0.1f)
            {
                praint.transform.localScale = Vector3.Lerp(startScalebg, endScalebg, j);
                yield return new WaitForSeconds(0.01f);
            }
            praint.transform.localScale = endScalebg;
            */
        }
        if (null == mbutton)
        {
            mbutton = UguiMaker.newImage("submitbutton", transform, "lookvase_sprite", "submit_up");
            mbutton.transform.localPosition = new Vector3(526, -335, 0);//526
            EventTriggerListener.Get(mbutton.gameObject).onDown = ClickBtnDown;
            EventTriggerListener.Get(mbutton.gameObject).onUp = ClickBtnUp;
            mOKBtn_Effect = ResManager.GetPrefab("effect_star0", "effect_star1").GetComponent<ParticleSystem>();
            UguiMaker.InitGameObj(mOKBtn_Effect.gameObject, mbutton.transform, "effect", Vector3.zero, Vector3.one);
            BoxCollider buttonBox = mbutton.gameObject.AddComponent<BoxCollider>();
            buttonBox.size = new Vector3(150, 125, 10);
        }
        yield return new WaitForSeconds(0.5f);
        mSound.PlayTipList(new List<string>() { "lookvase_sound", "lookvase_sound", "lookvase_sound" }, new List<string>() { "game-tips3-6-6-1", "game-tips3-6-6-2", "game-tips3-6-6-3" }, true);
        if (!isPlayBged)
        {
            mSound.PlayBgAsync("bgmusic_loop1", "bgmusic_loop1", 0.1f);
            isPlayBged = true;
        }
    }
    IEnumerator Tshow(GameObject go,Vector3 startScale, Vector3 endScale)
    {
        for (float j = 0; j < 1f; j += 0.1f)
        {
            go.transform.localScale = Vector3.Lerp(startScale, endScale, j);
            yield return new WaitForSeconds(0.01f);
        }
        go.transform.localScale = endScale;
    }
    private void playOut()
    {
        mSound.PlayShort("素材出现通用音效",0.6f);
    }
    private void ClickBtnDown(GameObject _go)
    {
        if (inpass) return;

        isdownSub = true;
        mSound.PlayTip("lookvase_sound", "submit_up", 1);
        mbutton.sprite = ResManager.GetSprite("lookvase_sprite", "submit_down");
    }
    private void ClickBtnUp(GameObject _go)
    {
        if (inpass) return;
        inpass = true;
        mSound.StopTip();
        checkPraintFinish();
        if (mGuanka.drawlines.Count != 0)
        {
            //inpass = true;
            //checkPraintFinish();
        }
    }

    private void resetView()
    {
        mGuanka.demo.sprite = ResManager.GetSprite("lookvase_sprite", "vase_" + mGuanka.vaseIndex);
        mGuanka.demo.SetNativeSize();
        for (int i = 0; i < mGuanka.vasedics.Count; i++)
        {
            Image photo = mGuanka.photos[i];
            string type = mGuanka.vasedics[i];
            photo.sprite = ResManager.GetSprite("lookvase_sprite", "vase_" + mGuanka.vaseIndexs[mGuanka.guanka] + "_" + type);
            photo.name = "photo_" + type;
            photo.SetNativeSize();
            PhotoCtr pc = photo.gameObject.GetComponent<PhotoCtr>();
            pc.setData(type);
            /*
            Image photoPoint = mGuanka.photopoints[i];
            photoPoint.name = "photopoint_" + type;
            */
        }
    }

    IEnumerator TBackTo(Image img, string type)
    {
        Vector3 startPos = img.transform.localPosition;
        Vector3 endPos = mGuanka.rolepointPoss[type];
        for (float j = 0; j < 1f; j += 0.2f)
        {
            img.transform.localPosition = Vector3.Lerp(startPos, endPos, j);
            yield return new WaitForSeconds(0.001f);
        }
        img.transform.localPosition = endPos;
    }
    IEnumerator TdelayPlayErr(int errRoleId)
    {
        
        yield return new WaitForSeconds(2.5f);
        mSound.PlayTipList(new List<string>() { "lookvase_sound", "aa_animal_name", "lookvase_sound" }, new List<string>() { "game-tips3-6-6-5", MDefine.GetAnimalNameByID_CH(errRoleId), "game-tips3-6-6-6" });
    }
    ///*
    IEnumerator TShowPointDemo()
    {
        yield return new WaitForSeconds(10f);
        isShowDemo = true;

    }
    //*/

    IEnumerator TNextGame()
    {
        mSound.PlayTip("lookvase_sound", "game-tips3-6-6-8");
        yield return new WaitForSeconds(5f);
        if(mGuanka.guanka > mGuanka.guanka_last)
        {
            TopTitleCtl.instance.AddStar();
            yield return new WaitForSeconds(2f);
            GameOverCtl.GetInstance().Show(mGuanka.guanka_last, reGame);
        }
        else
        {
            setGameData(mGuanka.guanka, true);
        }
        
    }

    IEnumerator TremoveLine(Image line,string key)
    {
        Color startColor = line.color;
        Color endColor = Color.red;
        for (float j = 0; j < 1f; j += 0.04f)
        {
            line.color = Color.Lerp(startColor, endColor, Mathf.Sin(j * 8));
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(1f);
        removeMarked(key);
        GameObject.Destroy(line.gameObject);
    }
    IEnumerator TplaySpine(SkeletonGraphic spine,string anction,bool iserr = false)
    {
        spine.AnimationState.SetAnimation(1, anction, false);
        yield return new WaitForSeconds(1.5f);
        spine.AnimationState.SetAnimation(1, "Idle", true);
        if (iserr)
        {
            inpass = false;
        }
        
    }
}
