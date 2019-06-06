using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AnimalEquationCtr : MonoBehaviour {
    public SoundManager mSound { get; set; }
    public Guanka mGuanka = new Guanka();
    private EquationInput inputobj { get; set; }
    private RawImage bg { get; set; }
    private GameObject currGo { get; set; }
    private GameObject ques1Go { get; set; }
    private GameObject ques2Go { get; set; }
    private GameObject ques3Go { get; set; }
    private Image currchildImage { get; set; }
    private Image maxdemoimg { get; set; }
    private bool inpass { get; set; }
    private GameObject elbg { get; set; }
    private GameObject elmid { get; set; }
    private GameObject elpop { get; set; }
    // Use this for initialization
    public class Guanka
    {
        public int guanka { get; set; }
        public int guanka_last { get; set; }

        public int maxNum { get; set; }
        public int num1 { get; set; }
        public int num2 { get; set; }

        public string type { get; set; }

        public string scene { get; set; }

        public string danYuanSoundName { get; set; }
        public string animalPrefab { get; set; }
        public string animalPrefab2 { get; set; }
        public List<Vector3> part1Poss { get; set; }
        public List<Vector3> part2Poss { get; set; }

        public List<GameObject> spins = new List<GameObject>();

        public List<GameObject> timus = new List<GameObject>();
        public bool iselbg { get; set; }
        public Vector3 elbgPos { get; set; }
        public bool iselmid { get; set; }
        public Vector3 elmidPos { get; set; }
        public bool iselpop { get; set; }
        public Vector3 elpopPos { get; set; }

        public bool isRole { get; set; }
        public Vector3 rolePos { get; set; }
        public int roleNum { get; set; }
        public float rolescaleNum { get; set; }

        public string elType { get; set; }
        public float scaleNum { get; set; }
        public string part1Animation { get; set; }
        public bool part1State { get; set; }
        public bool part1IsLoop { get; set; }
        public bool part1Isdangban { get; set; }
        public string part2Animation { get; set; }
        public bool part2State { get; set; }
        public bool part2IsLoop { get; set; }
        public bool part2Isdangban { get; set; }

        public string prefabShader { get; set; }
        public string prefabShader2 { get; set; }
        public Vector3 shaderPos { get; set; }
        public bool isShowShader1 { get; set; }
        public bool isShowShader2 { get; set; }
        public float shaderScaleNum { get; set; }
        public bool isMoveElBg { get; set; }
        public float part1alph { get; set; }
        public float part2alph { get; set; }

        public bool isBomdBone { get; set; }

        public Vector3 boxsize { get; set; }

        public Vector3 rolecenter { get; set; }
        public Vector3 prefabCenter { get; set; }
        

        public List<GameObject> stempPart1s { get; set; }
        public List<GameObject> stempPart2s { get; set; }

        public List<string> animalSounds = new List<string>() { "兔子0", "燕子0", "猫头鹰0", "鹅0" };
        public bool ishas { get; set; }
        public string currAnimalSound { get; set; }

        public string currenType { get; set; }

        public Guanka()
        {
            guanka_last = 2;
        }
        public void Set(int _guanka)
        {
            guanka = _guanka;
            maxNum = Common.GetMutexValue(6, 9);
            num1 = Common.GetMutexValue(1, maxNum - 1);
            while(num1 * 2 == maxNum)
            {
                num1 = Common.GetMutexValue(1, maxNum - 1);
            }
            num2 = maxNum - num1;
            //Debug.Log("maxNum : " + maxNum + ";num1 : " + num1 + ";num2 : " + num2);
            for(int i = 0;i < spins.Count; i++)
            {
                GameObject spine = spins[i];
                GameObject.Destroy(spine);
            }
            spins.Clear();
            timus.Clear();
            animalPrefab = "scene_1";
            rolePos = Vector3.zero;
            switch (_guanka)
            {
                case 1:
                    type = "add";
                    break;
                case 2:
                    type = "drog";
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
            ishas = false;
            currAnimalSound = "";
            int sceneIndex = 0;
            iselbg = false;
            iselmid = false;
            iselpop = false;
            isMoveElBg = false;
            isRole = false;
            elType = "prefab";
            scaleNum = 1;
            rolescaleNum = 1;
            roleNum = 0;
            prefabShader = null;
            prefabShader = null;
            isShowShader1 = false;
            isShowShader2 = false;
            shaderScaleNum = 0.5f;
            part1State = true;
            part2State = true;
            part1IsLoop = part2IsLoop = true;
            part1Isdangban = part2Isdangban = false;
            part1alph = 1;
            part2alph = 1;
            isBomdBone = false;
            boxsize = new Vector3(500, 400);
            rolecenter = new Vector3(0, 150, 0);
            prefabCenter = new Vector3(0, 150, 0);
            if (null != stempPart2s)
            {
                stempPart2s.Clear();
            }else
            {
                stempPart2s = new List<GameObject>();
            }
            if (null != stempPart1s)
            {
                stempPart1s.Clear();
            }else
            {
                stempPart1s = new List<GameObject>();
            }
            if (type == "add")
            {
                sceneIndex = Common.GetMutexValue(1, 6);
            }
            else
            {
                sceneIndex = Common.GetMutexValue(7, 12);//12
            }
            switch (sceneIndex)
            {
                case 1:
                    scaleNum = 0.1f;
                    boxsize = new Vector3(1000, 800);
                    animalPrefab = animalPrefab2 = "scene_3";
                    part1Animation = "Idle";
                    part2Animation = "Idle";
                    part1Poss = new List<Vector3>() { new Vector3(-497, 69, 0), new Vector3(-353, 121, 0), new Vector3(-197, 98, 0), new Vector3(-497, -52, 0), new Vector3(-318, -17, 0), new Vector3(-151, -45, 0), new Vector3(-526, -184, 0), new Vector3(-388, -190, 0) };
                    part2Poss = new List<Vector3>() { new Vector3(-62, 258, 0), new Vector3(85, 228, 0), new Vector3(27, 181, 0), new Vector3(38, 107, 0), new Vector3(65, 34, 0), new Vector3(75, -278, 0), new Vector3(5, -318, 0), new Vector3(-3, -399, 0) };
                    danYuanSoundName = "scene_1_1";
                    ishas = true;
                    currAnimalSound = "燕子";
                    break;
                case 2:
                    iselbg = true;
                    elbgPos = new Vector3(-412, 51.19f, 0);
                    iselmid = true;
                    elmidPos = new Vector3(-188.3f, 55.1f, 0);
                    scaleNum = 0.5f;
                    boxsize = new Vector3(200, 160);
                    animalPrefab = animalPrefab2 = "scene_2";
                    part1Animation = "Idle";
                    part2Animation = "Idle";
                    shaderPos = new Vector3(0, 15, 0);
                    prefabShader = prefabShader2 = "scene_10_glass_shader";
                    isShowShader2 = true;
                    part1Poss = new List<Vector3>() { new Vector3(-474, -166, 0), new Vector3(-353, -157, 0), new Vector3(-360, -195, 0), new Vector3(-242, -178, 0), new Vector3(-375, 41, 0), new Vector3(-482, 47, 0), new Vector3(-306, 25, 0), new Vector3(-223, 15, 0) };
                    part2Poss = new List<Vector3>() { new Vector3(29, -175, 0), new Vector3(-13, -286, 0), new Vector3(-82, -327, 0), new Vector3(-100, -393, 0), new Vector3(-211, -372, 0), new Vector3(-327, -397, 0), new Vector3(-432, -378, 0), new Vector3(-576, -371, 0) };
                    danYuanSoundName = "scene_1_1";
                    ishas = true;
                    currAnimalSound = "公鸡";
                    break;
                case 3:
                    animalPrefab = animalPrefab2 = "scene_6";
                    part1Animation = "Idle";
                    part2Animation = "Idle";
                    scaleNum = 0.2f;
                    boxsize = new Vector3(500, 400);
                    prefabShader = prefabShader2 = "scene_10_glass_shader";
                    shaderPos = new Vector3(0, 0, 0);
                    part1Poss = new List<Vector3>() { new Vector3(-478, 147, 0), new Vector3(-302, 170, 0), new Vector3(-138, 173, 0), new Vector3(12, 163, 0), new Vector3(-510, 20, 0), new Vector3(-356, 54, 0), new Vector3(-179, 45, 0), new Vector3(-29, 33, 0) };
                    part2Poss = new List<Vector3>() { new Vector3(-478, 147 - 330, 0), new Vector3(-302, 170 - 330, 0), new Vector3(-138, 173 - 330, 0), new Vector3(12, 163 - 330, 0), new Vector3(-510, 20 - 330, 0), new Vector3(-356, 54 - 330, 0), new Vector3(-179, 45 - 330, 0), new Vector3(-29, 33 - 330, 0) };
                    ishas = true;
                    currAnimalSound = "鸽子";
                    danYuanSoundName = "scene_1_1";
                    break;
                case 4:
                    iselbg = true;
                    iselmid = true;
                    iselpop = true;
                    elbgPos = new Vector3(0, -154, 0);
                    elmidPos = new Vector3(0, -149, 0);
                    elpopPos = new Vector3(0, -158, 0);
                    part2State = true;
                    isMoveElBg = true;
                    animalPrefab = animalPrefab2 = "scene_4";
                    part1Animation = "Idle";
                    part2Animation = "Idle";
                    boxsize = new Vector3(200, 300);
                    prefabCenter = new Vector3(0, 200, 0);
                    scaleNum = 0.5f;
                    part2alph = 0.6f;
                    part1Poss = new List<Vector3>() { new Vector3(-499, -157, 0), new Vector3(-377, -123, 0), new Vector3(-246, -105, 0), new Vector3(-102, -151, 0), new Vector3(16, -54, 0), new Vector3(-114, 17, 0), new Vector3(-277, 47, 0), new Vector3(-451, 51, 0) };
                    part2Poss = new List<Vector3>() { new Vector3(-556 + 50, -395, 0), new Vector3(-442 + 50, -395, 0), new Vector3(-330 + 50, -395, 0), new Vector3(-227 + 50, -395, 0), new Vector3(-124 + 50, -395, 0), new Vector3(-28 + 50, -395, 0), new Vector3(78 + 50, -395, 0), new Vector3(189 + 50, -395, 0) };
                    ishas = true;
                    currAnimalSound = "鱼";
                    danYuanSoundName = "scene_4_1";
                    break;
                case 5:
                    scaleNum = 1f;
                    animalPrefab = animalPrefab2 = "scene_7_role_1";
                    part1Animation = "Idle";
                    part2Animation = "Idle";
                    part2State = false;
                    isShowShader1 = true;
                    isShowShader2 = true;
                    boxsize = new Vector3(100, 80);
                    prefabShader = prefabShader2 = "scene_10_glass_shader";
                    part1Poss = new List<Vector3>() { new Vector3(-493, -158, 0), new Vector3(-374, -140, 0), new Vector3(-244, -86, 0), new Vector3(-109, -69, 0), new Vector3(-172, -204, 0), new Vector3(-298, -281, 0), new Vector3(-439, -327, 0), new Vector3(-72, -281, 0) };
                    part2Poss = new List<Vector3>() { new Vector3(37, 150, 0), new Vector3(175, 156, 0), new Vector3(41, 28, 0), new Vector3(182, -6, 0), new Vector3(65, -92, 0), new Vector3(93, -237, 0), new Vector3(-96, 211, 0), new Vector3(42, -376, 0) };
                    ishas = true;
                    currAnimalSound = "兔子";
                    danYuanSoundName = "scene_1_1";
                    break;
                case 6:
                    scaleNum = 1f;
                    animalPrefab = animalPrefab2 = "scene_1";
                    part1Animation = "Idle";
                    part2Animation = "Idle";
                    part2State = false;
                    isShowShader1 = true;
                    isShowShader2 = true;
                    boxsize = new Vector3(100, 180);
                    shaderPos = new Vector3(0, 8, 0);
                    prefabCenter = new Vector3(0, 100, 0);
                    prefabShader = prefabShader2 = "scene_10_glass_shader";
                    part1Poss = new List<Vector3>() { new Vector3(-171, -165, 0), new Vector3(-9, -193, 0), new Vector3(-426, -102, 0), new Vector3(-493, -222, 0), new Vector3(-310, -186, 0), new Vector3(-291, -352, 0), new Vector3(-124, -353, 0), new Vector3(-472, -376, 0) };
                    part2Poss = new List<Vector3>() { new Vector3(-171, -165, 0), new Vector3(-9, -193, 0), new Vector3(-426, -102, 0), new Vector3(-493, -222, 0), new Vector3(-310, -186, 0), new Vector3(-291, -352, 0), new Vector3(-124, -353, 0), new Vector3(-472, -376, 0) };
                    ishas = true;
                    currAnimalSound = "熊";
                    danYuanSoundName = "scene_1_1";
                    break;
                case 7:
                    elType = "image";
                    boxsize = new Vector3(200, 160);
                    scaleNum = 0.5f;
                    isRole = true;
                    roleNum = 1;
                    boxsize = new Vector3(200,300,0);
                    rolePos = new Vector3(-142, -301, 0);
                    rolescaleNum = 2f;
                    part1Poss = new List<Vector3>() {new Vector3(-513, 203, 0f),new Vector3(-411, 213, 0f),new Vector3(-296, 203, 0f),new Vector3(-143, 214, 0f),new Vector3(-523, 48, 0f),new Vector3(-388, 107, 0f),new Vector3(-209, 100, 0f),new Vector3(-326, 56, 0f)};
                    part2Poss = new List<Vector3>() { new Vector3(-237, -257, 0), new Vector3(-152, -264, 0), new Vector3(-196, -282, 0), new Vector3(-110, -271, 0), new Vector3(-147, -312, 0), new Vector3(-53, -244, 0), new Vector3(-75, -293, 0), new Vector3(-234, -312, 0) };
                    ishas = true;
                    currAnimalSound = "兔子";
                    danYuanSoundName = "scene_7_1";
                    break;
                case 8:
                    isRole = true;
                    roleNum = 1;
                    boxsize = new Vector3(200, 300, 0);
                    rolePos = new Vector3(-422, -374, 0);
                    part1Poss = new List<Vector3>() { new Vector3(-474, 186, 0), new Vector3(-346, 180, 0), new Vector3(-187, 192, 0), new Vector3(-440, 93, 0), new Vector3(-52, 203, 0), new Vector3(-267, 98, 0), new Vector3(-138, 101, 0), new Vector3(-8, 109, 0) };
                    part2Poss = new List<Vector3>() { new Vector3(-342, -244, 0), new Vector3(-264, -153, 0), new Vector3(-262, -286, 0), new Vector3(-457, -294, 0), new Vector3(-161, -156, 0), new Vector3(-164, -289, 0), new Vector3(-25, -137, 0), new Vector3(-53, -282, 0) };
                    elType = "image";
                    ishas = true;
                    currAnimalSound = "熊猫";
                    danYuanSoundName = "scene_1_1";
                    break;
                case 9:
                    isRole = true;
                    roleNum = 2;
                    boxsize = new Vector3(200, 300, 0);
                    rolePos = new Vector3(-106, -344, 0);
                    part1Poss = new List<Vector3>() { new Vector3(-502, 167, 0), new Vector3(-384, 185, 0), new Vector3(-317, 98, 0), new Vector3(-280, 260, 0), new Vector3(-445, 271, 0), new Vector3(-546, 55, 0), new Vector3(-208, 155, 0), new Vector3(-225, 48, 0) };
                    part2Poss = new List<Vector3>() { new Vector3(-112, -287, 0), new Vector3(-50, -294, 0), new Vector3(-189, -294, 0), new Vector3(-167, -354, 0), new Vector3(-103, -356, 0), new Vector3(-28, -356, 0), new Vector3(-245, -334, 0), new Vector3(42, -313, 0) };
                    elType = "image";
                    ishas = true;
                    currAnimalSound = "煮";
                    danYuanSoundName = "scene_7_1";
                    break;
                case 10:
                    animalPrefab = animalPrefab2 = "scene_10";
                    part1Animation = "Idle";
                    part2Animation = "Idle";
                    scaleNum = 0.2f;
                    boxsize = new Vector3(800, 500);
                    prefabCenter = new Vector3(0, 300, 0);
                    shaderScaleNum = 0.8f;
                    prefabShader = "scene_10_animal_shader";
                    prefabShader2 = "scene_10_glass_shader";
                    isShowShader1 = true;
                    isShowShader2 = true;
                    shaderPos = new Vector3(0, 6, 0);
                    part1Poss = new List<Vector3>() { new Vector3(-371, -114, 0), new Vector3(-315, -185, 0), new Vector3(-241, -84, 0), new Vector3(-491, -76, 0), new Vector3(-127, -126, 0), new Vector3(-177, -204, 0), new Vector3(-485, -171, 0), new Vector3(-412, -224, 0) };
                    part2Poss = new List<Vector3>() { new Vector3(-129, 81, 0), new Vector3(31, -11, 0), new Vector3(38, -310, 0), new Vector3(0, -379, 0), new Vector3(-141, -379, 0), new Vector3(-338, -379, 0), new Vector3(-337, -393, 0), new Vector3(-482, 60, 0) };
                    danYuanSoundName = "scene_10_1";
                    ishas = true;
                    currAnimalSound = "马";
                    break;
                case 11:
                    elType = "image";
                    scaleNum = 1f;
                    isRole = true;
                    roleNum = 1;
                    //boxsize = new Vector3(100, 80);
                    boxsize = new Vector3(200, 300, 0);
                    rolePos = new Vector3(-287, -206, 0);
                    rolecenter = new Vector3(-170, 200, 0);
                    rolescaleNum = 1f;
                    isBomdBone = true;
                    part1Poss = new List<Vector3>() { new Vector3(-145, 21, 0f), new Vector3(-239, 9, 0f), new Vector3(-250, -35, 0f), new Vector3(-78, 19, 0f), new Vector3(-6, 28, 0f), new Vector3(-57, -3, 0f), new Vector3(-112, -17, 0f), new Vector3(-169, -20, 0f) };
                    part2Poss = new List<Vector3>() { new Vector3(-174, -198, 0), new Vector3(-43, -168, 0), new Vector3(-282, -174, 0), new Vector3(-421, -212, 0), new Vector3(-318, -275, 0), new Vector3(-106, -301, 0), new Vector3(-212, -311, 0), new Vector3(-527, -291, 0) };
                    ishas = true;
                    currAnimalSound = "牛";
                    danYuanSoundName = "scene_7_1";
                    break;
               
                case 12:
                    //stempPart2s = new List<GameObject>();
                    animalPrefab = animalPrefab2= "scene_12";
                    //animalPrefab2 = "scene_12_2";
                    part1Animation = part2Animation = "Idle";
                    //part2Animation = part1Animation "animation";
                    scaleNum = 0.9f;
                    boxsize = new Vector3(100, 180);
                    prefabCenter = new Vector3(-100, 80, 0);
                    //-33 76
                    part1Poss = new List<Vector3>() { new Vector3(-76 + 43, 61 + 15, 0), new Vector3(113 + 43, 51 + 15, 0), new Vector3(12 + 43, 1 + 15, 0), new Vector3(-413 + 43, -16 + 15, 0), new Vector3(-483 + 43, -70 + 15, 0), new Vector3(-349 + 43, -63 + 15, 0), new Vector3(-271 + 43, -265 + 15, 0), new Vector3(-375 + 43, -332 + 15, 0), new Vector3(-169 + 43, -318 + 15, 0) };
                    part2Poss = new List<Vector3>() { new Vector3(-169 + 43, -318 + 15, 0), new Vector3(-375 + 43, -332 + 15, 0), new Vector3(-271 + 43, -265 + 15, 0), new Vector3(-349 + 43, -63 + 15, 0), new Vector3(-483 + 43, -70 + 15, 0), new Vector3(-413 + 43, -16 + 15, 0), new Vector3(12 + 43, 1 + 15, 0), new Vector3(113 + 43, 51 + 15, 0), new Vector3(-76 + 43, 61 + 15, 0) };
                    //part2IsLoop = false;
                    //part2Isdangban = true;
                    ishas = true;
                    currAnimalSound = "青蛙";
                    danYuanSoundName = "scene_1_1";
                    break;
               
               
            }
            scene = "scene_" + sceneIndex;
        }
    }
    void Awake()
    {
        mSound = gameObject.AddComponent<SoundManager>();
    }
    void Start () {
        initGame();
    }
    // Update is called once per frame
    private float speed = 0;
    private float scaleTime = 0;
	void Update () {

        if (mGuanka.isMoveElBg)
        {
            speed += 0.002f;
            if (null != elbg && null != elbg.transform.Find("image"))
            {
                RawImage child = elbg.transform.Find("image").gameObject.GetComponent<RawImage>();
                child.uvRect = new Rect(speed, 0, 1, 0.99f);
            }
            if (null != elmid && null != elmid.transform.Find("image"))
            {
                RawImage child = elmid.transform.Find("image").gameObject.GetComponent<RawImage>();
                child.uvRect = new Rect(-(speed + 0.05f), 0, 1, 0.99f);
            }
            if (null != elpop && null != elpop.transform.Find("image"))
            {
                RawImage child = elpop.transform.Find("image").gameObject.GetComponent<RawImage>();
                child.uvRect = new Rect(-speed, 0, 1, 0.99f);
            }
        }
        if(null != currGo && currGo.active)
        {
            scaleTime += 0.1f;
            if(null != currGo.transform.Find("maximg/wen"))
            {
                Image maxwen = currGo.transform.Find("maximg/wen").gameObject.GetComponent<Image>();
                if (maxwen.enabled)
                {
                    maxwen.rectTransform.localScale = Vector3.one * (1 + 0.2f * Mathf.Sin(scaleTime));
                }
            }
            
            Image num1wen = currGo.transform.Find("num1img/wen").gameObject.GetComponent<Image>();
            if (num1wen.enabled)
            {
                num1wen.rectTransform.localScale = Vector3.one * (1 + 0.2f * Mathf.Sin(scaleTime));
            }
            Image num2wen = currGo.transform.Find("num2img/wen").gameObject.GetComponent<Image>();
            if (num2wen.enabled)
            {
                num2wen.rectTransform.localScale = Vector3.one * (1 + 0.2f * Mathf.Sin(scaleTime));
            }

        }
        if (inpass || (null != inputobj && inputobj.gameObject.active)) return;

        if (Input.GetMouseButtonDown(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            //currchildImage = null;
            if (null != hits)
            {
                foreach (RaycastHit hit in hits)
                {
                    GameObject go = hit.collider.gameObject;
                    //Debug.Log("GetMouseButtonDown go.name:" + go.name);
                    EquationSpine spin = go.GetComponent<EquationSpine>();
                    if(null != spin)
                    {
                        
                        if (mGuanka.ishas)
                        {
                            mSound.PlayShort("aa_animal_effect_sound", mGuanka.currAnimalSound + "yes");
                        }else
                        {
                            //string soundn = mGuanka.animalSounds[Common.GetMutexValue(0, 3)];
                            //mSound.PlayShort("animalequation_sound", soundn);
                        }
                        /*
                        if(Common.GetMutexValue(1,10) % 2 == 0)
                        {
                            spin.PlaySpineEffect("Mistake");
                        }
                        else
                        {
                            spin.PlaySpineEffect("Correct");
                        }
                        */
                        spin.PlaySpineEffect("Correct");
                        break;
                    }

                    Image com = hit.collider.gameObject.GetComponent<Image>();
                    if (null != com && (com.name == "num1img" || com.name == "num2img" || com.name == "maximg"))
                    {
                        currchildImage = com;
                        showInput();
                        break;
                    }

                    
                }
            }
        }

    }
    //显示输入框
    private void showInput()
    {
        if (inpass || (null != inputobj && inputobj.gameObject.active)) return;

        if (null == inputobj)
        {
            GameObject inputgo = UguiMaker.newGameObject("inputgo", transform);
            inputobj = inputgo.AddComponent<EquationInput>();
            /*
            Canvas canvas = inputobj.gameObject.AddComponent<Canvas>();
            inputobj.gameObject.layer = LayerMask.NameToLayer("UI");
            canvas.overrideSorting = true;
            canvas.sortingOrder = 11;
            */
            inputobj.init("animalequation");
            inputobj.SetInputNumberCallBack(getNumfromInputNumObj);
            inputobj.transform.localPosition = new Vector3(353, -186, 0);
        }
        if(currGo == ques1Go)
        {
            inputobj.transform.localPosition = new Vector3(353, currGo.transform.localPosition.y - 380, 0);
        }
        else{
            inputobj.transform.localPosition = new Vector3(353, currGo.transform.localPosition.y - 210, 0);
        }
        mSound.PlayShort("animalequation_sound", "button_down");
        inputobj.transform.SetAsLastSibling();
        inputobj.ShowEffect();
    }
    ////结束输入
    //private void FinishInputNum()
    //{
    //    //Debug.Log("FinishInputNum inputobj.strInputNum : " + inputobj.strInputNum);
    //    string setNum = inputobj.strInputNum;
    //}
    //输入框清除数据
    private void CleanInputNum()
    {
        if (null != currchildImage)
        {
            Image child = currchildImage.transform.Find("num").gameObject.GetComponent<Image>();
            //child.sprite = ResManager.GetSprite("animalequation_sprite", setNum);
            child.enabled = false;
        }
    }
    //获取选择的数字
    private void getNumfromInputNumObj()
    {
        string setNum = inputobj.strInputNum;
        inputobj.HideEffect();
        if (null != currchildImage)
        {
            Image child = currchildImage.transform.Find("num").gameObject.GetComponent<Image>();
            child.sprite = ResManager.GetSprite("animalequation_sprite", setNum);
            child.enabled = true;
            Image wen = currchildImage.transform.Find("wen").gameObject.GetComponent<Image>();
            wen.enabled = false;
        }
        Debug.Log("currchildImage.name : " + currchildImage.name);
        Image maxchild = currGo.transform.Find("maximg/num").gameObject.GetComponent<Image>();
        Image num1child = currGo.transform.Find("num1img/num").gameObject.GetComponent<Image>();
        num1child.SetNativeSize();
        Image num2child = currGo.transform.Find("num2img/num").gameObject.GetComponent<Image>();
        num2child.SetNativeSize();
        if(currchildImage.name == "maximg")
        {
            if(setNum != mGuanka.maxNum.ToString())
            {
                StartCoroutine(TScale(currchildImage));
            }else
            {
                checkFinishCurr(maxchild, num1child, num2child);
            }
        }else if(currchildImage.name == "num1img")
        {
            if(setNum == mGuanka.num1.ToString() || setNum == mGuanka.num2.ToString())
            {
                Debug.Log("= 1或者2");
                if (num2child.enabled)
                {
                    if(((int.Parse(num2child.sprite.name) + int.Parse(setNum)) == mGuanka.maxNum))
                    {
                        if (isMark(num1child, num2child,1))
                        {
                            StartCoroutine(TScale(currchildImage));
                        }
                        else
                        {
                            checkFinishCurr(maxchild, num1child, num2child);
                        } 
                    }
                    else
                    {
                        StartCoroutine(TScale(currchildImage));
                    }
                }else
                {
                    if (isMark(num1child, num2child, 1))
                    {
                        StartCoroutine(TScale(currchildImage));
                    }
                    else
                    {
                        checkFinishCurr(maxchild, num1child, num2child);
                    }
                }
            }else
            {
                StartCoroutine(TScale(currchildImage));
            }
        }else if (currchildImage.name == "num2img")
        {
            if (setNum == mGuanka.num1.ToString() || setNum == mGuanka.num2.ToString())
            {
                if (num1child.enabled)
                {
                    if((int.Parse(num1child.sprite.name) + int.Parse(setNum)) == mGuanka.maxNum)
                    {
                        if (isMark(num1child, num2child,2))
                        {
                            StartCoroutine(TScale(currchildImage));
                        }
                        else
                        {
                            checkFinishCurr(maxchild, num1child, num2child);
                        }
                    }
                    else
                    {
                        StartCoroutine(TScale(currchildImage));
                    }
                }else
                {
                    if (isMark(num1child, num2child, 2))
                    {
                        StartCoroutine(TScale(currchildImage));
                    }
                    else
                    {
                        checkFinishCurr(maxchild, num1child, num2child);
                    }
                    //checkFinishCurr(maxchild, num1child, num2child);
                }
            }
            else
            {
                StartCoroutine(TScale(currchildImage));
            }
        }
       
    }

    private void checkFinishCurr(Image maxchild,Image num1child,Image num2child)
    {
        playPrefabSuccse();
        mSound.PlayShort("animalequation_sound", "数字放入正确");
        ///*
        if (int.Parse(maxchild.sprite.name) == mGuanka.maxNum &&(int.Parse(num1child.sprite.name) + int.Parse(num2child.sprite.name) == mGuanka.maxNum) && ((int.Parse(num1child.sprite.name) == mGuanka.num1 || int.Parse(num1child.sprite.name) == mGuanka.num2) && (int.Parse(num2child.sprite.name) == mGuanka.num1 || int.Parse(num2child.sprite.name) == mGuanka.num2)) && num1child.enabled && num2child.enabled && maxchild.enabled)
       {
           if(mGuanka.timus.Count <= 1)
           {
               finishQues1(currGo);
           }
           else
           {
               for (int i = 1; i < mGuanka.timus.Count; i++)
               {
                   Image olsChild2 = mGuanka.timus[i].transform.Find("num2img/num").gameObject.GetComponent<Image>();
                   Image olsChild1 = mGuanka.timus[i].transform.Find("num1img/num").gameObject.GetComponent<Image>();
                    if (olsChild2.sprite.name == num2child.sprite.name)
                   {
                       //TODO 错误逻辑
                       playErrsound();
                        break;
                   }
                   else
                   {
                       finishQues1(currGo);
                   }
               }
           }
       }else
       {
           //TODO 错误逻辑
           playErrsound();
       }
       //*/
    }
    private bool isMark(Image num1child, Image num2child,int index)
    {
        bool state = false;
        for (int i = 1; i < mGuanka.timus.Count; i++)
        {
            Image olsChild2 = mGuanka.timus[i].transform.Find("num2img/num").gameObject.GetComponent<Image>();
            Image olsChild1 = mGuanka.timus[i].transform.Find("num1img/num").gameObject.GetComponent<Image>();
            if(index == 1)
            {
                Debug.Log("num1child.sprite.name : " + num1child.sprite.name + ";olsChild1.sprite.name : " + olsChild1.sprite.name);
                if(olsChild1.sprite.name == num1child.sprite.name)
                {
                    state = true;
                    break;
                }
            }else
            {
                if (olsChild2.sprite.name == num2child.sprite.name)
                {
                    state = true;
                    break;
                }
            }
        }
        return state;
    }
    private void playErrsound()
    {
        Image maxchild = currGo.transform.Find("maximg/num").gameObject.GetComponent<Image>();
        Image num1child = currGo.transform.Find("num1img/num").gameObject.GetComponent<Image>();
        Image num2child = currGo.transform.Find("num2img/num").gameObject.GetComponent<Image>();
        
        if (num2child.enabled && num1child.enabled && maxchild.enabled)
        {
            playPrefabFail();
            mSound.PlayOnly("animalequation_sound", "sound_fail2",5);
            if(currGo != ques1Go)
            {
                StartCoroutine(TScale(currGo.transform.Find("maximg").gameObject.GetComponent<Image>()));
            }
            StartCoroutine(TScale(currGo.transform.Find("num1img").gameObject.GetComponent<Image>()));
            StartCoroutine(TScale(currGo.transform.Find("num2img").gameObject.GetComponent<Image>()));
        }
    }
    private void finishQues1(GameObject go)
    {
        //Debug.Log("完成第一个题目");
        mSound.StopOnly();
        go.transform.Find("num1img").gameObject.GetComponent<BoxCollider>().enabled = false;
        go.transform.Find("num2img").gameObject.GetComponent<BoxCollider>().enabled = false;
        if (go.transform.Find("maximg").gameObject.GetComponent<BoxCollider>())
        {
            go.transform.Find("maximg").gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        StartCoroutine(TSetQue1(go));
    }
    private void initGame()
    {
        mGuanka.Set(1);
        if (null == bg)
        {
            bg = UguiMaker.newRawImage("bg", transform, "animalequation_texture", mGuanka.scene, false);
        }else
        {
            bg.texture = ResManager.GetTexture("animalequation_texture", mGuanka.scene);
        }
        bg.SetNativeSize();
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);
        StartCoroutine(TwaitInit());

    }
    IEnumerator TwaitInit()
    {
        yield return new WaitForSeconds(0.5f);
        mSound.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.1f);
        setGameData(true);
    }
    //重置游戏
    private void reGame()
    {
        mGuanka.guanka = 1;
        if(null != demo2)
        {
            GameObject.Destroy(demo1);
            GameObject.Destroy(demo2);
        }
        setGameData();
    }
    //设置底图
    private void setElbg()
    {
        if(null == elbg)
        {
            if (mGuanka.iselbg)
            {
                elbg = UguiMaker.newGameObject("elbg", transform);
            }
            
        }else
        {
            if (null != elbg.transform.Find("image"))
            {
                GameObject child = elbg.transform.Find("image").gameObject;
                elbg.transform.DetachChildren();
                GameObject.Destroy(child);
            }
            
        }
        if (!mGuanka.iselbg) return;

        RawImage image = UguiMaker.newRawImage("image", elbg.transform, "animalequation_texture", mGuanka.scene + "_1");
        image.uvRect = new Rect(0, 0, 1, 0.99f);
        elbg.transform.localPosition = mGuanka.elbgPos;// new Vector3(-412, 29,0);
    }
    //设置中间层
    private void setElMid()
    {
        if (null == elmid)
        {
            if (mGuanka.iselmid)
            {
                elmid = UguiMaker.newGameObject("elmid", transform);
            }
        }
        else
        {
            if (null != elmid.transform.Find("image"))
            {
                GameObject child = elmid.transform.Find("image").gameObject;
                elmid.transform.DetachChildren();
                GameObject.Destroy(child);
            }
           
        }
        if (!mGuanka.iselmid) return;
        RawImage image = UguiMaker.newRawImage("image", elmid.transform, "animalequation_texture", mGuanka.scene + "_2");
        image.uvRect = new Rect(0, 0, 1, 0.99f);
        elmid.transform.localPosition = mGuanka.elmidPos;//new Vector3(-188.3f, 34.4f, 0);
    }

    //设置最前层
    private void setElPop()
    {
        if (null == elpop)
        {
            if (mGuanka.iselpop)
            {
                elpop = UguiMaker.newGameObject("elpop", transform);
            }
        }
        else
        {
            if(null != elpop.transform.Find("image"))
            {
                GameObject child = elpop.transform.Find("image").gameObject;
                elpop.transform.DetachChildren();
                GameObject.Destroy(child);
            }
            
        }
        if (!mGuanka.iselpop) return;
        RawImage image = UguiMaker.newRawImage("image", elpop.transform, "animalequation_texture", mGuanka.scene + "_3");
        image.uvRect = new Rect(0, 0, 1, 0.99f);
        elpop.transform.localPosition = mGuanka.elpopPos;

        Image buc = UguiMaker.newImage("buchong", elpop.transform, "public", "white");
        buc.rectTransform.sizeDelta = new Vector2(1423, 185);
        buc.rectTransform.localPosition = new Vector3(0, -155, 0);
        buc.color = new Color32(60, 200, 255, 255);
    }

    private GameObject createPrefabEl(string animalPrefab, List<Vector3> posList, int i,string animation,bool isShow, string prefabShader,bool isshader,float alhp,bool isLoop,bool isdangban)
    {
        GameObject elGo = UguiMaker.newGameObject("spine_" + i, transform);
        //Debug.Log("mGuanka.prefabShader : " + mGuanka.prefabShader);
        if (isshader && prefabShader != null)
        {
            Image shader = UguiMaker.newImage("shader", elGo.transform, "animalequation_sprite", prefabShader);
            shader.transform.localPosition = mGuanka.shaderPos;
            shader.transform.localScale = Vector3.one * mGuanka.shaderScaleNum;
        }

        EquationSpine spine = ResManager.GetPrefab("animalequation_prefab", animalPrefab).GetComponent<EquationSpine>();
        spine.name = "spine";
        spine.transform.parent = elGo.transform;
        spine.transform.localPosition = Vector3.zero;
        elGo.transform.localPosition = posList[i];// startPos + new Vector3((i % 3) * 150, -(int)(i / 3) * 150, 0);
        spine.transform.localScale = Vector3.one * mGuanka.scaleNum;
        spine.PlaySpine(animation, isLoop, alhp);
        mGuanka.spins.Add(elGo);
        BoxCollider box = spine.gameObject.AddComponent<BoxCollider>();
        box.size = mGuanka.boxsize;
        box.center = mGuanka.prefabCenter;// new Vector3(0, 150, 0);
        elGo.active = isShow;
        return elGo;
    }
    private GameObject createJumpoPrefab(Vector3 pos)
    {
        EquationSpine spine = ResManager.GetPrefab("animalequation_prefab", "scene_12_2").GetComponent<EquationSpine>();
        spine.transform.parent = transform;
        spine.transform.localPosition = pos;
        spine.transform.localScale = Vector3.one * mGuanka.scaleNum;
        spine.PlaySpine("animation", false, 1);
        //Image dangban = UguiMaker.newImage("dangban", spine.gameObject.transform, "public", "white");
        //dangban.rectTransform.sizeDelta = new Vector2(130, 200);
        //dangban.rectTransform.localPosition = new Vector3(107, -109, 0);
        //dangban.color = new Color32(38, 170, 219, 255);
        return spine.gameObject;
    }
    private Image PopDangban { get; set; }
    IEnumerator Tjump()
    {
        //yield return new WaitForSeconds(1f);
        Vector3 endPos = new Vector3(-137, -163, 0);
        List<GameObject> stemp = new List<GameObject>();
        if(null == PopDangban)
        {
            PopDangban = UguiMaker.newImage("dangban", transform, "public", "white");
            PopDangban.rectTransform.sizeDelta = new Vector2(130, 100);
            PopDangban.rectTransform.localPosition =  new Vector3(-86, -196, 0);
            PopDangban.color = new Color32(38, 170, 219, 255);
        }
        PopDangban.enabled = true;
        for (int i = 0;i < mGuanka.stempPart2s.Count; i++)
        {
            yield return new WaitForSeconds(2f);
            GameObject go = mGuanka.stempPart2s[i];
            
            Vector3 pos = go.transform.localPosition;
            GameObject jumpGo = createJumpoPrefab(pos);
            jumpGo.transform.localScale = Vector3.one * 0.3f;
            stemp.Add(jumpGo);
            GameObject.Destroy(go);
            Vector3 startPos = pos;
            for (float j = 0; j < 1f; j += 0.2f)
            {
                jumpGo.transform.localPosition = Vector3.Lerp(startPos, endPos, j);
                yield return new WaitForSeconds(0.01f);
            }
            jumpGo.transform.localPosition = endPos;
            PopDangban.transform.SetAsLastSibling();
            mSound.PlayShort("animalequation_sound", "入水",0.5f);
        }
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < stemp.Count; i++)
        {
            GameObject.Destroy(stemp[i]);
        }
            
    }
    //设置游戏数据
    private void setGameData(bool isInit = false)
    {
        if (mGuanka.guanka == 1)
        {
            mSound.PlayTip("animalequation_sound", "看图编算式  game-tips2-6-1", 1, true);
            mGuanka.currenType = "game-tips2-7-4";
            TopTitleCtl.instance.Reset();
        }
        else
        {
            mSound.PlayTip("animalequation_sound", "看图编算式  game-tips2-6-2", 1, true);
            mGuanka.currenType = "game-tips2-7-5";
            TopTitleCtl.instance.AddStar();
        }
        if (!isInit)
        {
            mGuanka.Set(mGuanka.guanka);
        }
        
        bg.texture = ResManager.GetTexture("animalequation_texture", mGuanka.scene);
        bg.SetNativeSize();
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);

        if(null != PopDangban)
        {
            PopDangban.enabled = false;
        }
        inpass = false;
        //Debug.Log("mGuanka.scene : " + mGuanka.scene);
        if (mGuanka.isRole && mGuanka.scene != "scene_11")
        {
            for(int i = 0; i < mGuanka.roleNum; i++)
            {
                int index = i + 1;
                EquationSpine role = ResManager.GetPrefab("animalequation_prefab", mGuanka.scene + "_role_" + index).GetComponent<EquationSpine>();
                role.name = "role";
                role.transform.parent = transform;
                role.transform.localPosition = mGuanka.rolePos + new Vector3(i * 1,0,0);
                role.transform.localScale = Vector3.one * mGuanka.rolescaleNum;
                //role.PlaySpine("Idle", true);
                BoxCollider box = role.gameObject.AddComponent<BoxCollider>();
                box.size = mGuanka.boxsize;
                box.center = mGuanka.rolecenter;// new Vector3(0, 150, 0);
                mGuanka.spins.Add(role.gameObject);
            }
        }

        setElbg();
        //创建动物
        //Vector3 startPos = mGuanka.part1Poss;
        if(mGuanka.elType == "prefab")
        {
            for (int i = 0; i < mGuanka.num1; i++)
            {
                GameObject go = createPrefabEl(mGuanka.animalPrefab, mGuanka.part1Poss, i, mGuanka.part1Animation, mGuanka.part1State,mGuanka.prefabShader, mGuanka.isShowShader1, mGuanka.part1alph,mGuanka.part1IsLoop,mGuanka.part1Isdangban);
                mGuanka.stempPart1s.Add(go);
                StartCoroutine(TToBigger(go));
            }
            setElMid();

            setElPop();
            for (int i = 0; i < mGuanka.num2; i++)
            {
                GameObject go = createPrefabEl(mGuanka.animalPrefab2, mGuanka.part2Poss, i, mGuanka.part2Animation,mGuanka.part2State, mGuanka.prefabShader2, mGuanka.isShowShader2,mGuanka.part2alph, mGuanka.part2IsLoop, mGuanka.part2Isdangban);
               
                if (mGuanka.stempPart2s != null)
                {
                    mGuanka.stempPart2s.Add(go);
                    StartCoroutine(TToBigger(go));
                    if (mGuanka.scene == "scene_12")
                    {
                        jump();
                    }
                    
                }
            }
        }
        else
        {
            for (int i = 0; i < mGuanka.num1; i++)
            {
                Image spine = UguiMaker.newImage("el", transform, "animalequation_sprite", mGuanka.scene + "_el");
                spine.transform.parent = transform;
                spine.transform.localPosition = mGuanka.part1Poss[i];// startPos + new Vector3((i % 3) * 150, -(int)(i / 3) * 150, 0);
                spine.transform.localScale = Vector3.one * mGuanka.scaleNum;
                mGuanka.spins.Add(spine.gameObject);
            }
            setElMid();

            setElPop();
            ///*
            /// Spine.Bone bone_r = mSpine.Skeleton.FindBone("L");
            if (mGuanka.scene == "scene_11")
            {
                EquationSpine role = ResManager.GetPrefab("animalequation_prefab", mGuanka.scene + "_role_1").GetComponent<EquationSpine>();
                role.name = "role";
                role.transform.parent = transform;
                role.transform.localPosition = mGuanka.rolePos;
                role.transform.localScale = Vector3.one * mGuanka.rolescaleNum;
                role.PlaySpine("Idle", true);
                BoxCollider box = role.gameObject.AddComponent<BoxCollider>();
                box.size = mGuanka.boxsize;
                box.center = mGuanka.rolecenter;
                mGuanka.spins.Add(role.gameObject);
            }
            //*/
            //startPos = mGuanka.part2Pos;
            for (int i = 0; i < mGuanka.num2; i++)
            {
                Image spine = UguiMaker.newImage("el", transform, "animalequation_sprite", mGuanka.scene + "_el");
                spine.transform.parent = transform;
                spine.transform.localPosition = mGuanka.part2Poss[i];// startPos + new Vector3((i % 3) * 150, -(int)(i / 3) * 150, 0);
                spine.transform.localScale = Vector3.one * mGuanka.scaleNum;
                mGuanka.spins.Add(spine.gameObject);
            }
            
        }
        /*
        if (null == maxdemoimg)
        {
            Debug.Log("mGuanka.maxNum : " + mGuanka.maxNum);
            maxdemoimg = UguiMaker.newImage("maxdemoimg", transform, "animalequation_sprite", mGuanka.maxNum.ToString());
        }
        else
        {
            maxdemoimg.sprite = ResManager.GetSprite("animalequation_sprite", mGuanka.maxNum.ToString());
        }
        maxdemoimg.transform.localPosition = new Vector3(-464, 266, 0);
        //maxdemoimg.color = Color.black;
        maxdemoimg.rectTransform.localScale = Vector3.one * 0.5f;
        */
        if(null != ques1Go)
        {
            GameObject.Destroy(ques1Go);
        }
        //出题第一道题目
        ques1Go = UguiMaker.newGameObject("ques1Go", transform);

        Image maximg = UguiMaker.newImage("maximg", ques1Go.transform, "animalequation_sprite", "maxNum");
        //BoxCollider maxbox = maximg.gameObject.AddComponent<BoxCollider>();
        //maxbox.size = new Vector3(131, 131, 0);
        Image maxNumimg = UguiMaker.newImage("num", maximg.transform, "animalequation_sprite", mGuanka.maxNum.ToString());
        maxNumimg.color = Color.black;
        maxNumimg.rectTransform.localScale = Vector3.one * 0.5f;
        maximg.enabled = false;

        Image fenimg = UguiMaker.newImage("fenimg", ques1Go.transform, "animalequation_sprite", "fen");
        fenimg.transform.localPosition = new Vector3(0, -96, 0);
        float numX = 77;
        float numY = -189;
        ///*
        if(mGuanka.guanka == 1)
        {
            numX = 77;
            numY = 0;
            fenimg.transform.localEulerAngles = new Vector3(0, 0, 180);
            maximg.transform.localPosition = new Vector3(0, -189, 0);//-189
        }
        else
        {
            fenimg.transform.localEulerAngles = new Vector3(0, 0, 0);
            maximg.transform.localPosition = new Vector3(0, 0, 0);//-189
            numX = 77;
            numY = -189;
        }
        //*/

        Image num1img = UguiMaker.newImage("num1img", ques1Go.transform, "animalequation_sprite", "childNum");
        num1img.transform.localPosition = new Vector3(-numX, numY, 0);//-189
        BoxCollider num1box = num1img.gameObject.AddComponent<BoxCollider>();
        num1box.size = new Vector3(131, 131, 0);

        Image num1Numimg = UguiMaker.newImage("num", num1img.transform, "animalequation_sprite", mGuanka.num1.ToString());
        num1Numimg.rectTransform.localScale = Vector3.one * 0.5f;
        num1Numimg.color = Color.black;
        num1Numimg.enabled = false;

        Image wen1 = UguiMaker.newImage("wen", num1img.transform, "animalequation_sprite", "wen_red");
        wen1.rectTransform.localScale = Vector3.one * 1.2f;

        Image num2img = UguiMaker.newImage("num2img", ques1Go.transform, "animalequation_sprite", "childNum");
        num2img.transform.localPosition = new Vector3(numX, numY, 0);//-189
        BoxCollider num2box = num2img.gameObject.AddComponent<BoxCollider>();
        num2box.size = new Vector3(131, 131, 0);

        Image num2Numimg = UguiMaker.newImage("num", num2img.transform, "animalequation_sprite", mGuanka.num2.ToString());
        num2Numimg.rectTransform.localScale = Vector3.one * 0.5f;
        num2Numimg.color = Color.black;
        num2Numimg.enabled = false;

        Image wen2 = UguiMaker.newImage("wen", num2img.transform, "animalequation_sprite", "wen_red");
        wen2.rectTransform.localScale = Vector3.one * 1.2f;
        ques1Go.transform.localPosition = new Vector3(350, 156, 0);
        if (mGuanka.guanka == 1)
        {
            ques1Go.transform.localPosition = new Vector3(350, 156, 0);
        }
        else
        {
            //ques1Go.transform.localPosition = new Vector3(350, 206, 0);
        }
        currGo = ques1Go;
        if (null != ques2Go)
        {
            GameObject.Destroy(ques2Go);
        }
        //出题第二道题目
        ques2Go = UguiMaker.newGameObject("ques2Go", transform);
        setOneQue(ques2Go, new Vector3(365, -29, 0));
        if (null != ques3Go)
        {
            GameObject.Destroy(ques3Go);
        }
        //出题第二道题目
        ques3Go = UguiMaker.newGameObject("ques3Go", transform);
        setOneQue(ques3Go, new Vector3(365, -50, 0));//-70

        mSound.PlayShort("animalequation_sound", "素材出现通用音效");

    }
    private void jump()
    {
        StartCoroutine(Tjump());
    }
    //播放成功后动物的动画
    private void playPrefabSuccse()
    {
        mSound.PlayShort("aa_animal_effect_sound", mGuanka.currAnimalSound + "yes");
        // Debug.Log("playPrefabSuccse mGuanka.spins.Count:" + mGuanka.spins.Count);
        for (int i = 0;i < mGuanka.spins.Count; i++)
        {
            GameObject paren = mGuanka.spins[i];
            if (null != paren && paren.name != "role")
            {
                EquationSpine spine;
                if (null != paren.transform.Find("spine"))
                {
                    spine = paren.transform.Find("spine").gameObject.GetComponent<EquationSpine>();
                }else
                {
                    spine = paren.GetComponent<EquationSpine>();
                }
                
                if (null != spine)
                {
                    spine.PlaySpineEffect("Correct");
                }
            }else
            {
                if (null != paren)
                {
                    EquationSpine spine = paren.GetComponent<EquationSpine>();
                    if (null != spine)
                    {
                        spine.PlaySpineEffect("Correct");
                    }
                }
            }
            
        }
        //
    }
    //播放失败后动物的动画
    private void playPrefabFail()
    {
        mSound.PlayShort("aa_animal_effect_sound", mGuanka.currAnimalSound + "no");
        //Debug.Log("playPrefabFail mGuanka.spins.Count:" + mGuanka.spins.Count);
        for (int i = 0; i < mGuanka.spins.Count; i++)
        {
            GameObject paren = mGuanka.spins[i];
            if(null != paren && paren.name != "role")
            {
                EquationSpine spine;
                if (null != paren.transform.Find("spine"))
                {
                    spine = paren.transform.Find("spine").gameObject.GetComponent<EquationSpine>();
                }
                else
                {
                    spine = paren.GetComponent<EquationSpine>();
                }
                if (null != spine)
                {
                    spine.PlaySpineEffect("Mistake");
                }
            }
            else
            {
                if (null != paren)
                {
                    EquationSpine spine = paren.GetComponent<EquationSpine>();
                    if (null != spine)
                    {
                        spine.PlaySpineEffect("Mistake");
                    }
                }
                
            }

        }
    }
    private void setOneQue(GameObject go,Vector3 pos)
    {
        float starX = -200;
        Image num1img = UguiMaker.newImage("num1img", go.transform, "animalequation_sprite", "numbg");
        num1img.transform.localPosition = new Vector3(starX + 90 * 0, 0, 0);
        BoxCollider num1box = num1img.gameObject.AddComponent<BoxCollider>();
        num1box.size = new Vector3(131, 131, 0);

        Image num1Numimg = UguiMaker.newImage("num", num1img.transform, "animalequation_sprite", mGuanka.num1.ToString());
        num1Numimg.rectTransform.localScale = Vector3.one * 0.5f;
        num1Numimg.color = Color.black;
        num1Numimg.enabled = false;

        Image wen1 = UguiMaker.newImage("wen", num1img.transform, "animalequation_sprite", "wen_red");
        wen1.rectTransform.localScale = Vector3.one * 1.2f;

        Image fenimg = UguiMaker.newImage("type", go.transform, "animalequation_sprite", "type_" + mGuanka.type);
        fenimg.transform.localPosition = new Vector3(starX + 93 * 1, 0, 0);
        
        Image num2img = UguiMaker.newImage("num2img", go.transform, "animalequation_sprite", "numbg");
        num2img.transform.localPosition = new Vector3(starX + 90 * 2, 0, 0);
        BoxCollider num2box = num2img.gameObject.AddComponent<BoxCollider>();
        num2box.size = new Vector3(131, 131, 0);

        Image num2Numimg = UguiMaker.newImage("num", num2img.transform, "animalequation_sprite", mGuanka.num2.ToString());
        num2Numimg.rectTransform.localScale = Vector3.one * 0.5f;
        num2Numimg.color = Color.black;
        num2Numimg.enabled = false;

        Image wen2 = UguiMaker.newImage("wen", num2img.transform, "animalequation_sprite", "wen_red");
        wen2.rectTransform.localScale = Vector3.one * 1.2f;

        Image deng = UguiMaker.newImage("deng", go.transform, "animalequation_sprite", "type_" + mGuanka.type + "_deng");
        deng.transform.localPosition = new Vector3(starX + 90 * 3, 0, 0);

        Image maximg = UguiMaker.newImage("maximg", go.transform, "animalequation_sprite", "numbg");
        maximg.transform.localPosition = new Vector3(starX + 90 * 4, 0, 0);
        BoxCollider maxbox = maximg.gameObject.AddComponent<BoxCollider>();
        maxbox.size = new Vector3(131, 131, 0);
        Image maxNumimg = UguiMaker.newImage("num", maximg.transform, "animalequation_sprite", mGuanka.maxNum.ToString());
        maxNumimg.color = Color.black;
        maxNumimg.rectTransform.localScale = Vector3.one * 0.5f;
        maxNumimg.enabled = false;

        Image wen3 = UguiMaker.newImage("wen", maximg.transform, "animalequation_sprite", "wen_red");
        wen3.rectTransform.localScale = Vector3.one * 1.2f;

        go.transform.localPosition = pos;

        if(mGuanka.type != "add")
        {
            maximg.transform.localPosition = new Vector3(starX + 90 * 0, 0, 0);
            num1img.transform.localPosition = new Vector3(starX + 90 * 4, 0, 0);
        }
        go.active = false;
    }
    private GameObject demo1 { get; set; }
    private GameObject demo2 { get; set; }
    private void createFinishDemo(GameObject go,Vector3 pos,string type)
    {
        float starX = -200;
        Image num1img = UguiMaker.newImage("num1img", go.transform, "animalequation_sprite", "numbg");
        num1img.transform.localPosition = new Vector3(starX + 90 * 0, 0, 0);

        Image num1Numimg = UguiMaker.newImage("num", num1img.transform, "animalequation_sprite", mGuanka.num1.ToString());
        num1Numimg.rectTransform.localScale = Vector3.one * 0.5f;
        num1Numimg.color = Color.black;

        Image fenimg = UguiMaker.newImage("type", go.transform, "animalequation_sprite", "type_" + type);
        fenimg.transform.localPosition = new Vector3(starX + 93 * 1, 0, 0);

        Image num2img = UguiMaker.newImage("num2img", go.transform, "animalequation_sprite", "numbg");
        num2img.transform.localPosition = new Vector3(starX + 90 * 2, 0, 0);

        Image num2Numimg = UguiMaker.newImage("num", num2img.transform, "animalequation_sprite", mGuanka.num2.ToString());
        num2Numimg.rectTransform.localScale = Vector3.one * 0.5f;
        num2Numimg.color = Color.black;

        Image deng = UguiMaker.newImage("deng", go.transform, "animalequation_sprite", "type_" + type + "_deng");
        deng.transform.localPosition = new Vector3(starX + 90 * 3, 0, 0);

        Image maximg = UguiMaker.newImage("maximg", go.transform, "animalequation_sprite", "numbg");
        maximg.transform.localPosition = new Vector3(starX + 90 * 4, 0, 0);
        BoxCollider maxbox = maximg.gameObject.AddComponent<BoxCollider>();
        maxbox.size = new Vector3(131, 131, 0);
        Image maxNumimg = UguiMaker.newImage("num", maximg.transform, "animalequation_sprite", mGuanka.maxNum.ToString());
        maxNumimg.color = Color.black;
        maxNumimg.rectTransform.localScale = Vector3.one * 0.5f;

        go.transform.localPosition = pos;
        go.transform.localScale = Vector3.zero;

        if (type != "add")
        {
            maximg.transform.localPosition = new Vector3(starX + 90 * 0, 0, 0);
            num1img.transform.localPosition = new Vector3(starX + 90 * 4, 0, 0);
        }
    }
    //抖动某个框
    IEnumerator TScale(Image go)
    {
        mSound.PlayShort("animalequation_sound", "choose_error");
        playPrefabFail();
        for (float j = 0; j < 1f; j += 0.05f)
        {
            go.transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * 6 * j) * 10);
            yield return new WaitForSeconds(0.01f);
        }
        go.transform.localEulerAngles = Vector3.zero;

        Image child = go.transform.Find("num").gameObject.GetComponent<Image>();
        child.enabled = false;
        Image wen = go.transform.Find("wen").gameObject.GetComponent<Image>();
        wen.enabled = true;
    }

    IEnumerator TToBigger(GameObject go)
    {
        Vector3 startScale = Vector3.one * 0.5f;
        Vector3 endScale = Vector3.one;

        for (float j = 0; j < 1f; j += 0.2f)
        {
            go.transform.localScale = Vector3.Lerp(startScale, endScale, j);
            yield return new WaitForSeconds(0.01f);
        }
        go.transform.localScale = endScale;
        EquationSpine spine = go.transform.Find("spine").gameObject.GetComponent<EquationSpine>();
        float num = Common.GetRandValue(1, 8) / 10f;
        yield return new WaitForSeconds(num);
        spine.PlaySpine("Idle", true);
    }
    
    IEnumerator TNextGame(float delay)
    {
        inpass = true;
        //TODO 做出正确的响应，算式位移并放大,动物播放成功动画
        Vector3 startPos1 = ques1Go.transform.localPosition ;
        float ofsetX = 10;
        Vector3 endPos1 = new Vector3(startPos1.x - ofsetX, startPos1.y - 70, 0);
        float ofsetY = 130;
        if (mGuanka.guanka == 2)
        {
            ofsetY = 160;
        }
        Vector3 startPos2 = ques2Go.transform.localPosition;
        Vector3 endPos2 = new Vector3(startPos2.x - ofsetX, startPos2.y - ofsetY, 0);

        Vector3 startPos3 = ques3Go.transform.localPosition;
        Vector3 endPos3 = new Vector3(startPos3.x - ofsetX, startPos3.y - 270, 0);//-200

        Vector3 startScale = Vector3.one * 0.5f;
        Vector3 endScale = Vector3.one * 0.8f;
        for (float j = 0; j < 1f; j += 0.05f)
        {
            ques1Go.transform.localScale = Vector3.Lerp(startScale, endScale, j);
            ques1Go.transform.localPosition = Vector3.Lerp(startPos1, endPos1, j);

            ques2Go.transform.localScale = Vector3.Lerp(startScale, endScale, j);
            ques2Go.transform.localPosition = Vector3.Lerp(startPos2, endPos2, j);

            ques3Go.transform.localScale = Vector3.Lerp(startScale, endScale, j);
            ques3Go.transform.localPosition = Vector3.Lerp(startPos3, endPos3, j);
            yield return new WaitForSeconds(0.01f);
        }
        ques1Go.transform.localScale = endScale;
        ques1Go.transform.localPosition = endPos1;

        ques2Go.transform.localScale = endScale;
        ques2Go.transform.localPosition = endPos2;

        ques3Go.transform.localScale = endScale;
        ques3Go.transform.localPosition = endPos3;

        yield return new WaitForSeconds(2f);

        mGuanka.guanka++;
        if (mGuanka.guanka > mGuanka.guanka_last)
        {
            TopTitleCtl.instance.AddStar();
            if (null != ques1Go)
            {
                GameObject.Destroy(ques1Go);
                GameObject.Destroy(ques2Go);
                GameObject.Destroy(ques3Go);
            }
            demo1 = UguiMaker.newGameObject("demo1", transform);
            createFinishDemo(demo1, new Vector3(381, 52, 0), "add");
            demo2 = UguiMaker.newGameObject("demo2", transform);
            createFinishDemo(demo2, new Vector3(381, -120, 0), mGuanka.type);
            startScale = Vector3.zero;
            endScale = Vector3.one;
            mSound.PlayShort("animalequation_sound", "数字放入正确");
            mSound.PlayTip("animalequation_sound", "additional-113（看图编算式）-new");
            for (float j = 0; j < 1f; j += 0.05f)
            {
                demo1.transform.localScale = Vector3.Lerp(startScale, endScale, j);
                demo2.transform.localScale = Vector3.Lerp(startScale, endScale, j);
                yield return new WaitForSeconds(0.01f);
            }
            demo1.transform.localScale = endScale;
            demo2.transform.localScale = endScale;
            yield return new WaitForSeconds(10f);
            GameOverCtl.GetInstance().Show(mGuanka.guanka_last, reGame);
        }
        else
        {
            //Debug.Log("过关……");
            setGameData();
        }
    }
   
    //设置一题最终的显示
    IEnumerator TSetQue1(GameObject go)
    {
        yield return new WaitForSeconds(1f);
        go.transform.Find("num1img").gameObject.GetComponent<BoxCollider>().enabled = false;
        go.transform.Find("num2img").gameObject.GetComponent<BoxCollider>().enabled = false;
        if (go.transform.Find("maximg").gameObject.GetComponent<BoxCollider>())
        {
            go.transform.Find("maximg").gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        mGuanka.timus.Add(go);
        
        Vector3 startPos = go.transform.localPosition; 
        Vector3 endPos = new Vector3(350, startPos.y + 120, 0);


        Vector3 startScale = Vector3.one;
        Vector3 endScale = Vector3.one * 1.2f;
        if (go == ques1Go)
        {
            mSound.PlayTipList(
               GetABnames(5),
               getOneGoSoundList(ques1Go),
               getOneSoundValList(ques1Go)
               );
            for (float j = 0; j < 1f; j += 0.02f)
            {
                go.transform.localScale = Vector3.Lerp(startScale, endScale, j);
                yield return new WaitForSeconds(0.01f);
            }
            go.transform.localScale = endScale;

            for (float j = 0; j < 1f; j += 0.01f)
            {
                go.transform.localScale = Vector3.Lerp(endScale, startScale, j);
                yield return new WaitForSeconds(0.01f);
            }
            go.transform.localScale = startScale;
            startScale = Vector3.one;
            endScale = Vector3.one * 0.5f;
            yield return new WaitForSeconds(2f);
            mSound.PlayShort("animalequation_sound", "指针对位正确", 1);
            //mSound.PlayOnly("animalequation_sound", "欢呼0", 0.3f);
            for (float j = 0; j < 1f; j += 0.2f)
            {
                go.transform.localScale = Vector3.Lerp(startScale, endScale, j);
                go.transform.localPosition = Vector3.Lerp(startPos, endPos, j);
                yield return new WaitForSeconds(0.01f);
            }
            go.transform.localScale = endScale;
            go.transform.localPosition = endPos;
            yield return new WaitForSeconds(1f);
            StartCoroutine(TShowQue(ques2Go));
        }else if (go == ques2Go)
        {
            
            mSound.PlayTipList(
               GetABnames(11),
               getOneGoSoundList(ques2Go),
               getOneSoundValList(ques2Go)
               );
            for (float j = 0; j < 1f; j += 0.02f)
            {
                go.transform.localScale = Vector3.Lerp(startScale, endScale, j);
                yield return new WaitForSeconds(0.01f);
            }
            go.transform.localScale = endScale;

            for (float j = 0; j < 1f; j += 0.01f)
            {
                go.transform.localScale = Vector3.Lerp(endScale, startScale, j);
                yield return new WaitForSeconds(0.01f);
            }
            go.transform.localScale = startScale;
            startScale = Vector3.one;
            endScale = Vector3.one * 0.5f;
            yield return new WaitForSeconds(10f);
            mSound.PlayShort("animalequation_sound", "指针对位正确", 1);
            for (float j = 0; j < 1f; j += 0.2f)
            {
                go.transform.localScale = Vector3.Lerp(startScale, endScale, j);
                go.transform.localPosition = Vector3.Lerp(startPos, endPos, j);
                yield return new WaitForSeconds(0.01f);
            }
            go.transform.localScale = endScale;
            go.transform.localPosition = endPos;
            
            yield return new WaitForSeconds(1f);

            StartCoroutine(TShowQue(ques3Go));
        }
        else
        {
            mSound.PlayTipList(
               GetABnames(11),
               getOneGoSoundList(ques3Go),
               getOneSoundValList(ques3Go)
               );
            for (float j = 0; j < 1f; j += 0.02f)
            {
                go.transform.localScale = Vector3.Lerp(startScale, endScale, j);
                yield return new WaitForSeconds(0.01f);
            }
            go.transform.localScale = endScale;

            for (float j = 0; j < 1f; j += 0.01f)
            {
                go.transform.localScale = Vector3.Lerp(endScale, startScale, j);
                yield return new WaitForSeconds(0.01f);
            }
            go.transform.localScale = startScale;
            startScale = Vector3.one;
            endScale = Vector3.one * 0.5f;
            yield return new WaitForSeconds(10f);
            mSound.PlayShort("animalequation_sound", "指针对位正确", 1);
            for (float j = 0; j < 1f; j += 0.2f)
            {
                go.transform.localScale = Vector3.Lerp(startScale, endScale, j);
                go.transform.localPosition = Vector3.Lerp(startPos, endPos, j);
                yield return new WaitForSeconds(0.01f);
            }
            go.transform.localScale = endScale;
            go.transform.localPosition = endPos;
            StartCoroutine(TNextGame(30));
        }
    }
    private List<string> GetABnames(int leng = 27)
    {
        List<string> list = new List<string>();
        for(int i = 0;i < leng; i++)
        {
            list.Add("animalequation_sound");
        }
        return list;
    }
    private List<float> getSoundValList()
    {
        float numval = 0.6f;
        List<float> vals = new List<float>()
        {
            1,
            numval,
            1,
            numval,
            1,

            1,
            numval,
            numval,
            numval,
            1,
            numval,
            numval,
            numval,
            1,
            numval,
            numval,
            1,
            numval,
            numval,
            numval,
            1,
            numval,
            numval,
            numval,
            1,
            numval,
            numval
        };
        return vals;
    }
    private List<float> getOneSoundValList(GameObject go)
    {
        float numval = 0.6f;
        List<float> vals;
        if (go == ques1Go)
        {
            vals = new List<float>()
            {
            1,
            numval,
            1,
            numval,
            1,
            };
        }
        else
        {
            vals = new List<float>()
        {
            1,
            numval,
            numval,
            numval,
            1,
            numval,
            numval,
            numval,
            1,
            numval,
            numval
        };
        }

        return vals;
    }
    private List<string> getOneGoSoundList(GameObject go)
    {
        List<string> list;
        Image maxchild = go.transform.Find("maximg/num").gameObject.GetComponent<Image>();
        string nummaxstr = maxchild.sprite.name;

        Image num1child = go.transform.Find("num1img/num").gameObject.GetComponent<Image>();
        string num1str = num1child.sprite.name;

        Image num2child = go.transform.Find("num2img/num").gameObject.GetComponent<Image>();
        string num2str = num2child.sprite.name;
        int index1 = 3;
        int index2 = 2;
        if (num1str == mGuanka.num1.ToString())
        {
            index1 = 2;
            index2 = 3;
        }
        if (mGuanka.guanka == 1)
        {
            if(go == ques1Go)
            {
                list = new List<string>()
                {
                        num1str,
                        "he",
                        num2str,
                        "hecheng",
                        nummaxstr,
                };

            }
            else
            {
                list = new List<string>() {
                        num1str,
                        mGuanka.danYuanSoundName,
                        mGuanka.scene + "_" + index1,
                        mGuanka.currenType,
                        num2str,
                        mGuanka.danYuanSoundName,
                        mGuanka.scene + "_" + index2,
                        "game-tips2-7-6",
                        nummaxstr,
                        mGuanka.danYuanSoundName,
                        "end_" + mGuanka.scene,
                    };
            }
            
        }
        else
        {
            if (go == ques1Go)
            {
                list = new List<string>()
                {
                        nummaxstr,
                        "fencheng",
                        num1str,
                        "he",
                        num2str,
                };

            }else
            {
                list = new List<string>() {
                        nummaxstr,
                        mGuanka.danYuanSoundName,
                        "end_" + mGuanka.scene,
                        mGuanka.currenType,
                        num2str,
                        mGuanka.danYuanSoundName,
                        mGuanka.scene + "_" + index2,
                        "game-tips2-7-6",
                        num1str,
                        mGuanka.danYuanSoundName,
                        mGuanka.scene + "_" + index1,
                    };
            }
            
        }
        return list;
    }
    private List<string> getGoSoundList(GameObject go1,GameObject go2)
    {
        List<string> list;
        Image maxchild = go1.transform.Find("maximg/num").gameObject.GetComponent<Image>();
        string nummaxstr = maxchild.sprite.name;

        Image num1child = go1.transform.Find("num1img/num").gameObject.GetComponent<Image>();
        string num1str = num1child.sprite.name;

        Image num2child = go1.transform.Find("num2img/num").gameObject.GetComponent<Image>();
        string num2str = num2child.sprite.name;
        int index1 = 3;
        int index2 = 2;
        if (num1str == mGuanka.num1.ToString())
        {
            index1 = 2;
            index2 = 3;
        }
        if (mGuanka.guanka == 1)
        {
            
            list = new List<string>() {
                        num1str,
                        "he",
                        num2str,
                        "hecheng",
                        nummaxstr,

                        num1str,
                        mGuanka.danYuanSoundName,
                        mGuanka.scene + "_" + index1,
                        mGuanka.currenType,
                        num2str,
                        mGuanka.danYuanSoundName,
                        mGuanka.scene + "_" + index2,
                        "game-tips2-7-6",
                        nummaxstr,
                        mGuanka.danYuanSoundName,
                        "end_" + mGuanka.scene,
                        //或者是
                        num2str,
                        mGuanka.danYuanSoundName,
                        mGuanka.scene + "_" + index2,
                        mGuanka.currenType,
                        num1str,
                        mGuanka.danYuanSoundName,
                        mGuanka.scene + "_" + index1,
                        "game-tips2-7-6",
                        nummaxstr,
                        mGuanka.danYuanSoundName,
                        "end_" + mGuanka.scene

                    };
        }
        else
        {
            list = new List<string>() {
                        nummaxstr,
                        "fencheng",
                        num2str,
                        "he",
                        num1str,

                        nummaxstr,
                        mGuanka.danYuanSoundName,
                        "end_" + mGuanka.scene,
                        mGuanka.currenType,
                        num2str,
                        mGuanka.danYuanSoundName,
                        mGuanka.scene + "_" + index2,
                        "game-tips2-7-6",
                        num1str,
                        mGuanka.danYuanSoundName,
                        mGuanka.scene + "_" + index1,
                        //或者
                        nummaxstr,
                        mGuanka.danYuanSoundName,
                        "end_" + mGuanka.scene,
                        mGuanka.currenType,
                        num1str,
                        mGuanka.danYuanSoundName,
                       mGuanka.scene + "_" + index1,
                        "game-tips2-7-6",
                         num2str,
                        mGuanka.danYuanSoundName,
                        mGuanka.scene + "_" + index2
                    };
        }

        return list;
    }
    //显示某一道算式题目
    IEnumerator TShowQue(GameObject go)
    {
        mSound.PlayShort("animalequation_sound", "素材出现通用音效");
        go.active = true;
        Vector3 endScale = Vector3.one;
        Vector3 startScale = Vector3.one * 0.5f; //image.rectTransform.localScale +
        for (float j = 0; j < 1f; j += 0.2f)
        {
            go.transform.localScale = Vector3.Lerp(startScale, endScale, j);
            yield return new WaitForSeconds(0.01f);
        }
        go.transform.localScale = endScale;
        currGo = go;
    }
}