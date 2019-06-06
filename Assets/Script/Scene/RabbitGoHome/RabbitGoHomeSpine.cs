using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class RabbitGoHomeSpine : MonoBehaviour {
    public SkeletonGraphic mSpine { get; set; }
    public RectTransform mrtran { get; set; }
    private GameObject luoboContain { get; set; }
    private Image warkZA { get; set; }
    private Image idleZA { get; set; }
    public int luoboNum = 0;
    public bool isWalk = false;
    public class MoveData
    {
        public List<Vector3> endPoss { get; set; }
        public bool isGoHome { get; set; }
        public MoveData(List<Vector3> _endPoss, bool _isGoHome)
        {
            endPoss = _endPoss;
            isGoHome = _isGoHome;
        }
    }
    // Use this for initialization
    void Start () {
        CreateContain();
    }
	private void CreateContain()
    {
        if (null == luoboContain)
        {
            luoboContain = UguiMaker.newGameObject("luoboContain", transform);
            
            idleZA = UguiMaker.newImage("idleZA", transform, "rabbitgohome_sprite", "idleza");
            idleZA.transform.localPosition = new Vector3(-210, 210, 0);//
            idleZA.enabled = false;

            warkZA = UguiMaker.newImage("warkZA", transform, "rabbitgohome_sprite", "warkza");
            warkZA.transform.localPosition = new Vector3(-48, 241, 0);
            warkZA.enabled = false;
        }
    }
    // Update is called once per frame
    private Vector3 luoboContainWalkPos = Vector3.zero;
    private Vector3 luoboContainIdlePos = Vector3.zero;
    private Vector3 luoboPos = Vector3.zero;
    float times = 0;
    void Update () {

        if(null != luoboContainWalkPos)
        {
            times += 0.1f;
            luoboContain.transform.localPosition = luoboPos + new Vector3(0, 5f, 0) * Mathf.Sin(times * 1.2f);
        }
        if (null != mSpine)
        {

            //Spine.BoneData bone_r = mSpine.SkeletonData.FindBone("R");//mSpine.AnimationState.Data.SkeletonData.FindBone("R");
            Spine.Bone bang_a = mSpine.Skeleton.FindBone("bang_a");//正面
            //Vector3 pos1 = new Vector3(bang_a.WorldX, bang_a.WorldY, 0);
            //Debug.Log(pos1);
            //return;
            if (null != bang_a)
            {
                //Debug.Log("bang_a : " + bang_a);
                Vector3 pos = new Vector3(bang_a.WorldX, bang_a.WorldY, 0);
                Vector3 pos1 = (transform.worldToLocalMatrix.MultiplyPoint(pos) + new Vector3(1600, -3450, 0)) * 0.1f - new Vector3(300,50,0);
                //pos = mSpine.transform.worldToLocalMatrix.MultiplyPoint(pos);
                pos.z = 0;
                //Debug.Log(pos + " " + pos1);
                idleZA.transform.localPosition = pos * 100 + new Vector3(8, 10, 0);
                
                /*
                if (luoboContainWalkPos == Vector3.zero)
                {
                    luoboContainWalkPos = pos * 80 + new Vector3(250, -150, 0);
                }
                if (!isWalk)
                {
                    luoboContain.transform.localPosition = luoboContainWalkPos;
                }
                */
            }
            

            Spine.Bone bang_b = mSpine.Skeleton.FindBone("bang_b");//侧面
            if (null != bang_b)
            {
                Vector3 pos = new Vector3(bang_b.WorldX, bang_b.WorldY, 0);
                //pos = mSpine.transform.worldToLocalMatrix.MultiplyPoint(pos);
                pos.z = 0;
                warkZA.rectTransform.anchoredPosition3D = pos * 100 + new Vector3(8, 20, 0);
                
                if (luoboContainWalkPos == Vector3.zero)
                {
                    Vector3 locPos = pos * 800 + new Vector3(120, -150, 0);
                    luoboContainIdlePos = locPos;
                }
                /*
                if (isWalk)
                {
                    Debug.Log("luoboContainIdlePos : " + luoboContainIdlePos);
                    luoboContain.transform.localPosition = luoboContainIdlePos;
                }
                */
            }


        }
    }
    
    public void PlaySpine(string name, bool isloop)
    {
        if (null == mSpine)
        {
            mrtran = GetComponent<RectTransform>();
            mSpine = gameObject.transform.Find("spine").gameObject.GetComponent<SkeletonGraphic>();
            CreateContain();
        }
        if(name == "Idle")
        {
            idleZA.enabled = true;
            warkZA.enabled = false;
            luoboPos = Vector3.zero;
            
        }
        else if(name == "Walk")
        {
            idleZA.enabled = false;
            warkZA.enabled = true;
            luoboPos = new Vector3(150, 0, 0); 
        }
        luoboContain.transform.localPosition = luoboPos;
        mSpine.AnimationState.ClearTracks();
        mSpine.AnimationState.SetAnimation(1, name, isloop);
    }
    public void MoveToEnd(Vector3 pos)
    {
        StopAllCoroutines();
        StartCoroutine(TMoveToEnd(pos));
    }
    public void MoveTo(List<Vector3> endPoss,bool isGoHome)
    {
        StopAllCoroutines();
        StartCoroutine("TMoveToPositions", new MoveData(endPoss, isGoHome));
    }
    IEnumerator TMoveToEnd(Vector3 _endPos)
    {
        KengRabbitGoHomeCtr.isWarking = true;
        isWalk = true;
        PlaySpine("Walk", true);
        List<Vector3> endPoss = new List<Vector3>();
        endPoss.Add(_endPos);
        for (int i = 0; i < endPoss.Count; i++)
        {
            Vector3 endPos = endPoss[i] - new Vector3(0, 20, 0);
            if (endPos.x > transform.localPosition.x)
            {
                transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                transform.localEulerAngles = new Vector3(0, 180, 0);
            }
            while (transform.localPosition != endPos)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPos, 80 * Time.deltaTime);
                yield return 0;
            }
            yield return new WaitForSeconds(0.0f);
        }
        isWalk = false;
        PlaySpine("Idle", true);
        KengRabbitGoHomeCtr.isWarking = false;
    }
    //列表坐标
    IEnumerator TMoveToPositions(MoveData data)
    {
        //Debug.Log("startMove");
        List<Vector3> endPoss = data.endPoss;
        bool isGoHome = data.isGoHome;
        KengRabbitGoHomeCtr.isWarking = true;
        //KengRabbitGoHomeCtr.mtransform.playClickWalkSound();
        PlaySpine("Walk", true);

        if (isGoHome)
        {
            //float fblockTime = 0.4f;
            //transform.DOScale(Vector3.one * 0.06f, endPoss.Count * fblockTime).SetEase(Ease.Linear);
        }
        float dis = 0.1f / endPoss.Count;
        for (int i = 0;i < endPoss.Count; i++)
        {
            KengRabbitGoHomeCtr.mtransform.playClickWalkSound();
            Vector3 endPos = endPoss[i] - new Vector3(0,20,0);
            if(endPos.x > transform.localPosition.x)
            {
                transform.localEulerAngles = new Vector3(0, 0, 0);
            }else
            {
                transform.localEulerAngles = new Vector3(0, 180, 0);
            }
            if (isGoHome && endPoss[i] == new Vector3(511, -362, 0))
            {
                float fblockTime = 0.1f;
                transform.DOScale(Vector3.one * 0.06f, endPoss.Count * fblockTime).SetEase(Ease.Linear);
            }
            KengRabbitGoHomeCtr.mtransform.setPassId(endPoss[i]);
            while (transform.localPosition != endPos)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPos, 200 * Time.deltaTime);
                yield return 0;
            }
            yield return new WaitForSeconds(0.01f);
        }
        KengRabbitGoHomeCtr.mtransform.playStopWalkSound();
       

        if (!KengRabbitGoHomeCtr.isBacking)
        {
            KengRabbitGoHomeCtr.mtransform.finishCurrMove(luoboNum);
        }
        PlaySpine("Idle", true);
        //luoboContain.transform.localPosition = 
        yield return new WaitForSeconds(0.5f);
        KengRabbitGoHomeCtr.isWarking = false;
        if (isGoHome)
        {
            KengRabbitGoHomeCtr.mtransform.CloseDoor(true);
            yield return new WaitForSeconds(0.5f);
            KengRabbitGoHomeCtr.mtransform.NextGame();
            //StartCoroutine(TIntoHome());
        }
    }
    IEnumerator TIntoHome()
    {
        Vector3 endPos = new Vector3(567, -378, 0);
        Vector3 startscale = Vector3.one * 0.2f;
        Vector3 endscale = Vector3.one * 0.1f;
        for (float j = 0; j < 1f; j += 0.005f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, endPos, j);
            transform.localScale = Vector3.Lerp(startscale, endscale, j + 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localScale = endscale;
        KengRabbitGoHomeCtr.isWarking = false;
        PlaySpine("Idle", true);
        KengRabbitGoHomeCtr.mtransform.NextGame();
    }
    private void OnTriggerExit(Collider other)
    {
        string[] arr = other.name.Split('_');
        if (arr[0] == "luobo")
        {
            KengItem keng = other.gameObject.GetComponent<KengItem>();
            if (keng.isP && KengRabbitGoHomeCtr.isBacking)
            {
                removeOneChild();
                KengRabbitGoHomeCtr.mtransform.playPushLuoboSound();
                keng.showLuobo();
            }
        }
    }
    public int passId = 0;
    private void OnTriggerEnter(Collider other)
    {
        string[] arr = other.name.Split('_');
        //Debug.Log("other.name : " + other.name);
        if(arr[0] == "luobo")
        {
            KengItem keng = other.gameObject.GetComponent<KengItem>();
            if (keng.isP && KengRabbitGoHomeCtr.isBacking)
            {
                //removeOneChild();
                //keng.showLuobo();
            }else
            {
                addLuobo(keng);
               
            }
            

        }else if(other.name == "endPoint")
        {
            /*
            BoxCollider startBox = other.gameObject.GetComponent<BoxCollider>();
            startBox.enabled = false ;
            StopAllCoroutines();
            if (luoboNum == 7)
            {
                removeContainChild();
                KengRabbitGoHomeCtr.mtransform.NextGame();
            }else
            {
                //removeContainChild();
                PlaySpine("Idle", true);
                KengRabbitGoHomeCtr.mtransform.ErrRoad();
            }
            */
        }else if (arr[0] == "shizilu")
        {
            KengRabbitGoHomeCtr.mtransform.setOpenRoadList(other.gameObject);
        }
        if(arr.Length > 2 && arr[1] == "gezi" && int.Parse(arr[0]) == KengRabbitGoHomeCtr.mtransform.currRoad)
        {
            RaGrid grid = other.gameObject.GetComponent<RaGrid>();
            passId = grid.mID;
            //Debug.Log("兔子通过的节点：" + passId);
        }
    }
    private void addLuobo(KengItem keng)
    {
        CreateContain();
        luoboNum++;
        if (luoboNum > 7)
        {
            luoboNum--;
            passId--;
            if(luoboContain.transform.childCount > 7)
            {
                GameObject.Destroy(luoboContain.transform.GetChild(0).gameObject);
            }
            StopCoroutine("TMoveToPositions");
            PlaySpine("Idle", true);
            KengRabbitGoHomeCtr.isWarking = false;
            KengRabbitGoHomeCtr.mtransform.Full();
            KengRabbitGoHomeCtr.mtransform.ErrRoad();
        }
        else
        {
            Image luobo = UguiMaker.newImage("luobo", luoboContain.transform, "rabbitgohome_sprite", "luobo");
            Vector3 EndPos = new Vector3(-190, 270, 0) + new Vector3((luoboNum % 3) * 25, -(int)(luoboNum / 3) * 10, 0);
            Vector3 endScale = Vector3.one;
            luobo.transform.localScale = Vector3.one * 2.5f;
            StartCoroutine(TIntoPack(luobo, EndPos, endScale));
            KengRabbitGoHomeCtr.mtransform.addShowNum(luoboNum);
            KengRabbitGoHomeCtr.mtransform.noFull();

            KengRabbitGoHomeCtr.mtransform.playGetLuoboSound();
            keng.closeLuobo();
        }
        
    }
    IEnumerator TIntoPack(Image luobo,Vector3 endPos,Vector3 endScale)
    {
        Vector3 startPos = new Vector3(10,0,0);
        Vector3 endPos1 = new Vector3(0, 500, 0);
        for (float j = 0; j < 1f; j += 0.2f)
        {
            luobo.transform.localPosition = Vector3.Lerp(startPos, endPos1, j);
            yield return new WaitForSeconds(0.001f);
        }
        Vector3 startScale = Vector3.one * 3;
        for (float j = 0; j < 1f; j += 0.1f)
        {
            luobo.transform.localPosition = Vector3.Lerp(endPos1, endPos, j);
            luobo.transform.localScale = Vector3.Lerp(startScale, endScale, j);
            yield return new WaitForSeconds(0.001f);
        }
        luobo.transform.localPosition = endPos;
        luobo.transform.localScale = endScale;
    }
    private void removeOneChild()
    {
        luoboNum--;
        KengRabbitGoHomeCtr.mtransform.addShowNum(luoboNum);
        if(luoboContain.transform.childCount > 0)
        {
            GameObject.Destroy(luoboContain.transform.GetChild(0).gameObject);
        }
        if(luoboNum < 0)
        {
            luoboNum = 0;
        }
    }
    public void removeContainChild()
    {
        luoboNum = 0;
        foreach (Transform go in luoboContain.transform)
        {
            GameObject.Destroy(go.gameObject);
        }
    }
}
