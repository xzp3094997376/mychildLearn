using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class AnimalNumberLogicCtr : MonoBehaviour {
    public Guanka mGuanka = new Guanka();
    public SoundManager mSound { get; set; }
    private RawImage bg { get; set; }
    private EquationInput inputobj { get; set; }
    private bool isInPass { get; set; }
    private GameObject qeContain { get; set; }

    public int checkID = 6;
    public class Guanka
    {
        public int guanka { get; set; }
        public int guanka_last { get; set; }
        public List<int> animalIDs = new List<int>();
        public List<int> nums = new List<int>();
        public List<logicEq> logicEqs = new List<logicEq>();
        public int currtimes { get; set; }
        public int alltimes { get; set; }
        public int queNum { get; set; }
        public Guanka()
        {
            guanka_last = 3;
        }
        public void resetCurrTime()
        {
            currtimes = 0;
        }
        public void Set(int _guanka)
        {
            guanka = _guanka;
            logicEqs.Clear();

            animalIDs.Clear();
            animalIDs = Common.GetMutexValue(1, 10, 3);

            nums.Clear();
            int max =  Common.GetMutexValue(6, 10);
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            int length = max;
            List<Vector3> vec3 = new List<Vector3>() ;
            for (int x = 1; x < length; x++)
            {
                for (int y = 1; y < length; y++)
                {
                    for (int z = 1; z < length; z++)
                    {
                        if(x != y && x != z && y != z)
                        {
                            if(x + y + z == max)
                            {
                                vec3.Add(new Vector3(x, y, z));
                            }
                        }
                    }
                }
            }
            Vector3 vect3 = Common.BreakRank(vec3)[0];
            num1 = (int)vect3.x;
            num2 = (int)vect3.y;
            num3 = (int)vect3.z;
            nums.Add(num1);
            nums.Add(num2);
            nums.Add(num3);

            switch (_guanka)
            {
                case 1:
                    queNum = 2;
                    break;
                case 2:
                    queNum = 3;
                    break;
                case 3:
                    queNum = 3;
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
        bg = UguiMaker.newRawImage("bg", transform, "animalnumberlogic_texture", "bg", false);
        bg.rectTransform.sizeDelta = new Vector2(1423, 800);
        StartCoroutine(TInit());
       
    }
    IEnumerator TInit()
    {
        yield return new WaitForSeconds(0.5f);
        setGameData(1);
    }
    // Update is called once per frame
    Vector3 temp_select_offset = Vector3.zero;
    private float scaleTime = 0;
    void Update () {

        if (null != qeContain)
        {
            scaleTime += 0.1f;
            for (int i = 0;i < mGuanka.logicEqs.Count; i++)
            {
                logicEq eq = mGuanka.logicEqs[i];
                Vector3 scelNum = Vector3.one * (0.8f + 0.2f * Mathf.Sin(scaleTime));
                eq.scleWen(scelNum);
            }
        }

        if (isInPass) return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);

            if (null != hits && (null == inputobj || !inputobj.gameObject.activeSelf))
            {
                foreach (RaycastHit hit in hits)
                {
                    GameObject com = hit.collider.gameObject;
                    if(null != com)
                    {
                        logicEq.gSlect = com;
                        showInput();
                        break;
                    }
                }
            }
        }
    }
    private void showInput()
    {
        if (isInPass || (null != inputobj && inputobj.gameObject.active)) return;

        if (null == inputobj)
        {
            InputInfoData data = new InputInfoData();
            data.fNumScale = 3f;
            data.fscale = 0.3f;
            data.vBgSize = new Vector2(665, 745);//575);
            data.vCellSize = new Vector2(200, 166);
            data.vSpacing = new Vector2(10, 10);
            data.bgcolor = new Color32(252, 229, 194, 255);
            data.color_blockBG = new Color32(179, 138, 89, 255);
            data.color_blockNum = new Color32(202, 183, 155, 255);
            data.color_blockSureBG = new Color32(172, 123, 66, 255);
            data.color_blockSureStart = new Color32(202, 183, 155, 255);
            data.strAlatsName = "animalnumberlogic_sprite";

            inputobj = UguiMaker.newGameObject("inputobj",transform).gameObject.AddComponent<EquationInput>();// InputNumObj.Create(transform, data);
            inputobj.init("animalnumberlogic", 4);
            inputobj.SetInputNumberCallBack(getNumfromInputNumObj);
            inputobj.transform.localPosition = new Vector3(-55, -76, 0);
        }
        
        Vector3 mousePos = Common.getMouseLocalPos(transform);
        if(mousePos.y > 100)
        {
            temp_select_offset = new Vector3(0, -200, 0);
        }else if (mousePos.y < 100 && mousePos.y > -100)
        {
            temp_select_offset = new Vector3(-200, 0, 0);
        }else
        {
            temp_select_offset = new Vector3(0, 200, 0);
        }
        inputobj.GetComponent<RectTransform>().anchoredPosition3D = mousePos + temp_select_offset;
        mSound.PlayShort("animalnumberlogic_sound", "button_down");
        inputobj.transform.SetAsLastSibling();
        inputobj.ShowEffect();
    }
    private void CleanInputNum()
    {
        GameObject currGo = logicEq.gSlect;
        Image currchildImage = logicEq.gSlect.GetComponent<Image>();
        if (null != currchildImage)
        {
            Image child = currchildImage.transform.Find("num").gameObject.GetComponent<Image>();
            child.enabled = false;
            Image wen = currchildImage.transform.Find("wen").gameObject.GetComponent<Image>();
            wen.enabled = true;
        }
    }
    //获取选择的数字
    private bool isFirtOk { get; set; }
    private void getNumfromInputNumObj()
    {
        string setNum = inputobj.strInputNum;
        if (setNum == "0")
        {
            setNum = "10";
        }
        inputobj.HideEffect();

        GameObject currGo = logicEq.gSlect;
        logicEq lgeq = logicEq.gSlect.transform.parent.parent.gameObject.GetComponent<logicEq>();
        mSound.StopTip();
        if (setNum == "") return;

        bool boo = lgeq.setNum(int.Parse(setNum),mGuanka.guanka);
        if (boo)
        {
            if (!isFirtOk)
            {
                StartCoroutine(TDelayPlayOk());
            }
            checkeFinish();
        }
    }
    IEnumerator TDelayPlayOk()
    {
        yield return new WaitForSeconds(1.2f);
        mSound.PlayShort("animalnumberlogic_sound", "game-tips3-3-15-10");
        isFirtOk = true;
    }
    //判断是否结束
    private void checkeFinish()
    {
        bool boo = true;
        for (int i = 0; i < mGuanka.logicEqs.Count; i++)
        {
            logicEq eq = mGuanka.logicEqs[i];
            if (!eq.checkFinish())
            {
                boo = false;
                break;
            }
        }
        if (boo)
        {
            StartCoroutine(TDelayPlayOk());

            StartCoroutine(TnextQuestion());
        }
    }
    //重置游戏
    private void reGame()
    {
        mGuanka.guanka = 1;
        mGuanka.resetCurrTime();
        setGameData(mGuanka.guanka);
    }
    private void nextQuestion()
    {
        mGuanka.currtimes++;
        if (mGuanka.currtimes >= mGuanka.alltimes)
        {
            StartCoroutine(TNextGame());
        }else
        {
            StartCoroutine(TNextQue());
        }
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
        isInPass = false;
        isFirtOk = false;
        if (null != qeContain)
        {
            GameObject.Destroy(qeContain);
        }
        qeContain = UguiMaker.newGameObject("qeContain", transform);

        Vector3 startPos = new Vector3(-207, 134, 0);
        for (int i = 0;i < mGuanka.queNum; i++)
        {
            logicEq eq = UguiMaker.newGameObject("eq_" + i, qeContain.transform).AddComponent<logicEq>();
            eq.setData(mGuanka.animalIDs, mGuanka.nums, i,mGuanka.guanka);
            eq.transform.localPosition = startPos + new Vector3(0, -245 * i, 0);
            if (i == 2)
            {
                eq.transform.localPosition =  new Vector3(-281, -340, 0);
            }
            mGuanka.logicEqs.Add(eq);
        }
        List<string> soundNames = new List<string>();
        List<string> soundabNames = new List<string>();
        List<float> vals = new List<float>();
        soundNames.Add("game-tips3-3-15-7");
        soundabNames.Add("animalnumberlogic_sound");
        vals.Add(1);
        if (mGuanka.logicEqs.Count < 3)
        {
            logicEq eq = mGuanka.logicEqs[0];
            string animal1Name = MDefine.GetAnimalNameByID_CH(eq.remID[0]);
            soundNames.Add(animal1Name);
            soundabNames.Add("aa_animal_name");
            vals.Add(1);
            soundNames.Add("window_close");
            soundabNames.Add("animalnumberlogic_sound");
            vals.Add(0);
            string animal2Name = MDefine.GetAnimalNameByID_CH(eq.remID[1]);
            soundNames.Add(animal2Name);
            soundabNames.Add("aa_animal_name");
            vals.Add(1);
        }
        else
        {
            logicEq eq = mGuanka.logicEqs[2];
            string animal1Name = MDefine.GetAnimalNameByID_CH(eq.remID[0]);
            soundNames.Add(animal1Name);
            soundabNames.Add("aa_animal_name");
            vals.Add(1);

            soundNames.Add("window_close");
            soundabNames.Add("animalnumberlogic_sound");
            vals.Add(0);

            string animal2Name = MDefine.GetAnimalNameByID_CH(eq.remID[1]);
            soundNames.Add(animal2Name);
            soundabNames.Add("aa_animal_name");
            vals.Add(1);

            soundNames.Add("window_close");
            soundabNames.Add("animalnumberlogic_sound");
            vals.Add(0);
            string animal3Name = MDefine.GetAnimalNameByID_CH(eq.remID[2]);
            soundNames.Add(animal3Name);
            soundabNames.Add("aa_animal_name");
            vals.Add(1);
        }
        soundNames.Add("game-tips3-3-15-5");
        soundabNames.Add("animalnumberlogic_sound");
        vals.Add(1);

        soundNames.Add("game-tips3-3-15-6");
        soundabNames.Add("animalnumberlogic_sound");
        vals.Add(1);

       // Debug.Log(soundabNames.Count + " ;soundNames.Count : " + soundNames.Count);
        mSound.PlayTipList(soundabNames, soundNames, vals, true);

        if(mGuanka.guanka == 2 || mGuanka.guanka == 3)
        {
            int index = Common.GetRandValue(0, 2);
            int child = Common.GetRandValue(0, 1);
            logicEq eq = mGuanka.logicEqs[index];
            Image input = eq.gameObject.transform.Find("animal_" + child + "/imput").gameObject.GetComponent<Image>();
            eq.setOkstate(input);
        }
        if (!isPlaybg)
        {
            isPlaybg = true;
            mSound.PlayBgAsync("bgmusic_loop0", "bgmusic_loop0", 0.1f);
        }
    }
    private bool isPlaybg = false;
    IEnumerator TnextQuestion()
    {
        yield return new WaitForSeconds(2f);
        nextQuestion();
    }

    IEnumerator TNextQue()
    {
        yield return new WaitForSeconds(2f);
        setGameData(mGuanka.guanka);
    }
    IEnumerator TNextGame()
    {
        isInPass = true;
        yield return new WaitForSeconds(2f);
        mGuanka.guanka++;
        if (mGuanka.guanka > mGuanka.guanka_last)
        {
            TopTitleCtl.instance.AddStar();
            yield return new WaitForSeconds(2f);
            GameOverCtl.GetInstance().Show(mGuanka.guanka_last, reGame);
        }
        else
        {
            setGameData(mGuanka.guanka);
        }
    }
}
