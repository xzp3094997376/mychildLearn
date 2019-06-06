using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Spine.Unity;

public class logicEq : MonoBehaviour {
    public static GameObject gSlect {get;set; }
    public string mResult = "";
    public List<int> mNums = null;
    public List<int> mIds = null;
    public List<int> remID = new List<int>();
    public string mtype { get; set; }
    private List<GameObject> inputs = new List<GameObject>();
    private SoundManager masound { get; set; }
    public void setData(List<int> ids,List<int> nums,int _index,int guanka)
    {
        remID.Clear();
        if (null == masound)
        {
            masound = gameObject.AddComponent<SoundManager>();
        }
        if(guanka == 2)
        {
            setGuanka2Data(ids, nums, _index);

        }else if (guanka == 1)
        {
            setGuanka1Data(ids, nums, _index);
        }else
        {
            setGuanka2Data(ids, nums, _index);
        }
    }
    public void showDemoNum()
    {

    }
    private void setGuanka1Data(List<int> ids, List<int> nums, int _index)
    {
        mtype = "jian";
        
        if (nums[1] > nums[0])
        {
            int stempNum = 0;
            stempNum = nums[0];
            nums[0] = nums[1];
            nums[1] = stempNum;

            int stempId = 0;
            stempId = ids[0];
            ids[0] = ids[1];
            ids[1] = stempId;
        }
        
        if (_index == 0)
        {
            mtype = "jian";
            mResult = (nums[0] - nums[1]).ToString();
            nums[0] = nums[1] + int.Parse(mResult);
            nums[1] = nums[1];
           
        }
        else
        {
            mtype = "jia";
            ids[0] = ids[1];
            nums[2] = nums[1];
            ids[2] = ids[1];
            ids[1] = ids[2];
            mResult = (nums[1] + nums[1]).ToString();
        }
        mNums = nums;
        mIds = ids;
        //Debug.Log("setGuanka1Data 0 : " + mNums[0] + ";1 : " + mNums[1]);
        Image type1 = UguiMaker.newImage("type1", transform.transform, "animalnumberlogic_sprite", mtype);
        type1.rectTransform.localPosition = new Vector3(196, 78, 0);
        Image dengyu = UguiMaker.newImage("deng", transform.transform, "animalnumberlogic_sprite", "deng");
        dengyu.rectTransform.localPosition = new Vector3(543, 78, 0);
        Image result = UguiMaker.newImage("result", transform.transform, "animalnumberlogic_sprite", mResult);
        result.rectTransform.localPosition = new Vector3(637, 78, 0);
        result.rectTransform.localScale = Vector3.one * 0.5f;
        Color color = new Color(107 / 255f, 24 / 255f, 4 / 255f, 1);
        result.color = color;

        int index = 0;
        float dis = 365;
        Color colornu = new Color(107 / 255f, 24 / 255f, 4 / 255f, 1);

        for (int i = 0; i < 2; i++)
        {
            GameObject go = UguiMaker.newGameObject("animal_" + index, transform);
            remID.Add(ids[i]);
            GameObject prefabGo = ResManager.GetPrefab("aa_animal_person_prefab", MDefine.GetAnimalNameByID_EN(ids[i]));
            prefabGo.name = "prefab";
            prefabGo.transform.parent = go.transform;
            prefabGo.transform.localScale = Vector3.one;
            prefabGo.transform.localPosition = Vector3.one;
            SkeletonGraphic spine = prefabGo.transform.Find("spine").gameObject.GetComponent<SkeletonGraphic>();
            spine.AnimationState.SetAnimation(1, "face_idle", true);
            go.transform.localPosition = new Vector3(index * dis, 0, 0);
            Image imput = UguiMaker.newImage("imput", go.transform, "animalnumberlogic_sprite", "sx_kuang");
            imput.rectTransform.localScale = Vector3.one * 0.8f;
            imput.rectTransform.localPosition = new Vector3(0, 40, 0);
            BoxCollider box = imput.gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(80, 80, 0);
            Image numimage = UguiMaker.newImage("num", imput.transform, "animalnumberlogic_sprite", nums[i].ToString());
            numimage.rectTransform.localScale = Vector3.one * 0.3f;
            numimage.color = colornu;
            numimage.enabled = false;
            Image wen = UguiMaker.newImage("wen", imput.transform, "animalnumberlogic_sprite", "wen_red");
            wen.rectTransform.localScale = Vector3.one * 0.8f;
            inputs.Add(imput.gameObject);
            index++;
        }
    }
    //设置第二关数据
    private void setGuanka2Data(List<int> ids, List<int> nums, int _index)
    {
        Debug.Log("setGuanka2Data");
        string typeSprite = "jia";

        if (nums[1] >= nums[2])
        {
        }
        else
        {
            int stempNum = 0;
            stempNum = nums[1];
            nums[1] = nums[2];
            nums[2] = stempNum;

            int stempId = 0;
            stempId = ids[1];
            ids[1] = ids[2];
            ids[2] = stempId;

        }
        mNums = nums;
        //remID = ids;
        string resultSprite = (nums[0] + nums[1]).ToString();
        int leng = 2;
        int startIndex = 0;
        float dis = 365;
        if (_index == 2)
        {
            leng = 3;
            dis = 250;
            resultSprite = (nums[0] + nums[1] + nums[2]).ToString();
        }
        else if (_index == 1)
        {
            typeSprite = "jian";
            startIndex = 1;
            leng = 3;
            resultSprite = (nums[1] - nums[2]).ToString();
        }

        Image type1 = UguiMaker.newImage("type1", transform.transform, "animalnumberlogic_sprite", typeSprite);
        type1.rectTransform.localPosition = new Vector3(196, 78, 0);
        if (_index == 2)
        {
            type1.rectTransform.localPosition = new Vector3(125, 78, 0);
            Image type2 = UguiMaker.newImage("type2", transform.transform, "animalnumberlogic_sprite", typeSprite);
            type2.rectTransform.localPosition = new Vector3(373, 78, 0);
        }
        Image dengyu = UguiMaker.newImage("deng", transform.transform, "animalnumberlogic_sprite", "deng");
        dengyu.rectTransform.localPosition = new Vector3(543, 78, 0);
        float rx = 637;
        if (_index == 2)
        {
            dengyu.rectTransform.localPosition = new Vector3(617, 78, 0);
            rx = 705;
        }
        Image result = UguiMaker.newImage("result", transform.transform, "animalnumberlogic_sprite", resultSprite);
        result.rectTransform.localPosition = new Vector3(rx, 78, 0);
        result.rectTransform.localScale = Vector3.one * 0.5f;
        Color color = new Color(107 / 255f, 24 / 255f, 4 / 255f, 1);
        result.color = color;

        int index = 0;
        Color colornu = new Color(107 / 255f, 24 / 255f, 4 / 255f, 1);
        for (int i = startIndex; i < leng; i++)
        {
            GameObject go = UguiMaker.newGameObject("animal_" + index, transform);
            GameObject prefabGo = ResManager.GetPrefab("aa_animal_person_prefab", MDefine.GetAnimalNameByID_EN(ids[i]));
            remID.Add(ids[i]);
            prefabGo.name = "prefab";
            prefabGo.transform.parent = go.transform;
            prefabGo.transform.localScale = Vector3.one;
            prefabGo.transform.localPosition = Vector3.one;
            SkeletonGraphic spine = prefabGo.transform.Find("spine").gameObject.GetComponent<SkeletonGraphic>();
            spine.AnimationState.SetAnimation(1, "face_idle", true);
            go.transform.localPosition = new Vector3(index * dis, 0, 0);
            Image imput = UguiMaker.newImage("imput", go.transform, "animalnumberlogic_sprite", "sx_kuang");
            imput.rectTransform.localScale = Vector3.one * 0.8f;
            imput.rectTransform.localPosition = new Vector3(0, 40, 0);
            BoxCollider box = imput.gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(80, 80, 0);
            //box.center = new Vector3(0, 60, 0);
            Image numimage = UguiMaker.newImage("num", imput.transform, "animalnumberlogic_sprite", nums[i].ToString());
            numimage.rectTransform.localScale = Vector3.one * 0.3f;
            numimage.color = colornu;
            numimage.enabled = false;
            Image wen = UguiMaker.newImage("wen", imput.transform, "animalnumberlogic_sprite", "wen_red");
            wen.rectTransform.localScale = Vector3.one * 0.8f;
            inputs.Add(imput.gameObject);
            index++;
        }
    }
    public bool setNum(int num,int guanka)
    {
        bool state = false;
        Image input = logicEq.gSlect.GetComponent<Image>();
        Image inputNum = input.transform.Find("num").gameObject.GetComponent<Image>();
        inputNum.sprite = ResManager.GetSprite("animalnumberlogic_sprite", num.ToString());
        inputNum.SetNativeSize();
        inputNum.rectTransform.localScale = Vector3.one * 0.3f;
        inputNum.color = new Color(107 / 255f, 24 / 255f, 4 / 255f, 1); ;
        Image wen = input.transform.Find("wen").gameObject.GetComponent<Image>();
        wen.enabled = false;
        inputNum.enabled = true;
        int eqIndex = int.Parse(input.transform.parent.parent.name.Split('_')[1]);

        int numIndex = int.Parse(input.transform.parent.name.Split('_')[1]);
        int spineIndex = numIndex;
        if (eqIndex == 1)
        {
            numIndex = int.Parse(input.transform.parent.name.Split('_')[1]) + 1;
        }
        SkeletonGraphic spine = input.transform.parent.Find("prefab/spine").GetComponent<SkeletonGraphic>();
        string soundNameyes = MDefine.GetAnimalEffectSoundNameByID_CH(remID[spineIndex], "yes");
        string soundNameno = MDefine.GetAnimalEffectSoundNameByID_CH(remID[spineIndex], "no");
        masound.StopTip();
        for (int i = 0;i < inputs.Count; i++)
        {
            Image numimage = inputs[i].GetComponent<Image>();
            if(input == numimage)
            {
                if (num == mNums[numIndex])
                {
                    state = true;
                    setOkstate(input);
                    PlaySpine(spine, "face_sayyes", false, 1);
                    masound.PlayTipList(new List<string>() { "aa_animal_effect_sound"}, new List<string>() { soundNameyes});
                    //masound.PlayTipList(new List<string>() { "aa_animal_effect_sound", "animalnumberlogic_sound" }, new List<string>() { soundNameyes, "game-tips3-3-15-10" });
                    //masound.PlayTip("animalnumberlogic_sound", "game-tips3-3-15-10");
                }
                else
                {
                    PlaySpine(spine, "face_sayno", false,2);
                    string errname = "";
                    if(Common.GetRandValue(0,10) % 2 == 0)
                    {
                        errname = "game-tips3-3-15-8";
                    }else
                    {
                        errname = "game-tips3-3-15-9";
                    }
                    masound.PlayTipList(new List<string>() { "aa_animal_effect_sound", "animalnumberlogic_sound" }, new List<string>() { soundNameno, errname});
                   
                    StartCoroutine(TCloceNum(inputNum, wen));

                }
                break;
            }
        }
        return state;
    }
    public void setOkstate(Image input)
    {
        int numIndex = int.Parse(input.transform.parent.name.Split('_')[1]);
        int eqIndex = int.Parse(input.transform.parent.parent.name.Split('_')[1]);
        if (eqIndex == 1)
        {
            numIndex = int.Parse(input.transform.parent.name.Split('_')[1]) + 1;
        }
        int num = mNums[numIndex];
        Debug.Log("num : " + num);
        Image inputNum = input.transform.Find("num").gameObject.GetComponent<Image>();
        inputNum.sprite = ResManager.GetSprite("animalnumberlogic_sprite", num.ToString());
        inputNum.enabled = true;
        Image wen = input.transform.Find("wen").gameObject.GetComponent<Image>();
        wen.enabled = false;
        BoxCollider box = input.gameObject.GetComponent<BoxCollider>();
        box.enabled = false;
    }
    public void scleWen(Vector3 wenscle)
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            Image numimage = inputs[i].GetComponent<Image>();
            Image wen = numimage.transform.Find("wen").gameObject.GetComponent<Image>();
            if (wen.enabled)
            {
                wen.rectTransform.localScale = wenscle;
            }
        }
    }
    public bool checkFinish()
    {
        bool boo = true;
        for (int i = 0; i < inputs.Count; i++)
        {
            Image numimage = inputs[i].GetComponent<Image>();
            Image wen = numimage.transform.Find("wen").gameObject.GetComponent<Image>();
            if (wen.enabled)
            {
                boo = false;
                break;
            }
        }
        return boo;
    }
    public void PlaySpine(SkeletonGraphic spine,string name, bool isloop,int times = 1)
    {
        StartCoroutine(TplaySpine(spine, name, isloop, times));
    }
    IEnumerator TplaySpine(SkeletonGraphic spine, string name, bool isloop, int times = 1)
    {
        for(int i = 0;i < times; i++)
        {
            spine.AnimationState.ClearTracks();
            spine.AnimationState.SetAnimation(1, name, isloop);
            yield return new WaitForSeconds(0.5f);
        }

        if (!isloop)
        {
            StartCoroutine(TdelayRestState(spine));
        }
        
    }
    IEnumerator TdelayRestState(SkeletonGraphic spine)
    {
        yield return new WaitForSeconds(0.5f);
        spine.AnimationState.ClearTracks();
        spine.AnimationState.SetAnimation(1, "face_idle", true);
    }
    IEnumerator TCloceNum(Image numimage,Image wen)
    {
        for (float j = 0; j < 1f; j += 0.05f)
        {
            float p = Mathf.Sin(Mathf.PI * j) * 0.8f;
            //img.rectTransform.localScale = Vector2.Lerp(Vector2.zero, Vector3.one * temp_scale, j) + new Vector2(p, p);

            numimage.transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Mathf.PI * 6 * j) * 10);
            yield return new WaitForSeconds(0.01f);
        }
        numimage.transform.localEulerAngles = Vector3.zero;
        numimage.enabled = false;
        wen.enabled = true;
    }
}
