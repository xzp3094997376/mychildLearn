using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 截屏控制器
/// </summary>
public class GetScreenPicController : MonoBehaviour
{
    public bool bState = false;

    private string strPicPath = "";

    //private Image hidebg;
    private RawImage img;
    private Button sharebtn;
    private Button closebtn;

    private Text pathText;

    private int nWidth = Screen.width;
    private int nHeight = Screen.height;

    public static GetScreenPicController Create(Transform _parent)
    {
        GameObject mload = Resources.Load("prefab/GetScreenTexture/GetScreenTexture") as GameObject;
        GameObject obj = GameObject.Instantiate(mload);
        obj.transform.SetParent(_parent);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        GetScreenPicController result = obj.AddComponent<GetScreenPicController>();
        result.InitAwake();
        return result;
    }
    private void InitAwake()
    {
        //hidebg = transform.FindChild("hidebg").GetComponent<Image>();
        img = transform.Find("RawImage").GetComponent<RawImage>();

        pathText = transform.Find("pathText").GetComponent<Text>();
        pathText.gameObject.SetActive(false);

        sharebtn = transform.Find("sharebtn").GetComponent<Button>();
        sharebtn.gameObject.SetActive(false);
        closebtn = transform.Find("closebtn").GetComponent<Button>();
        EventTriggerListener.Get(sharebtn.gameObject).onClick = ClickBtn;
        EventTriggerListener.Get(closebtn.gameObject).onClick = ClickBtn;
        gameObject.SetActive(false);
    }

    private System.Action mReadyAction = null;
    private System.Action mFinishAction = null;
    public void SetReadyAction(System.Action _action)
    { mReadyAction = _action; } 
    public void SetFinishAction(System.Action _action)
    { mFinishAction = _action; }



    /// <summary>
    /// 截屏保存
    /// </summary>
    public void OnClickShot()
    {
        img.gameObject.SetActive(false);
        sharebtn.gameObject.SetActive(false);
        closebtn.gameObject.SetActive(false);
        //pathText.text = "Save:";

        //隐藏top title
        TopTitleCtl.instance.gameObject.SetActive(false);

        bState = true;
        gameObject.SetActive(true);
        gameObject.transform.localScale = Vector3.one;

        StartCoroutine(getTexture2d());
    }
    IEnumerator getTexture2d()
    {
        if (mReadyAction != null)
        {
            mReadyAction();
        }

        //截图操作
        yield return new WaitForEndOfFrame();
        Texture2D t = new Texture2D(nWidth, nHeight, TextureFormat.RGB24, false);
       
        img.transform.localScale = Vector3.one * 0.001f;
        img.transform.gameObject.SetActive(true);
        img.transform.DOScale(Vector3.one,0.3f);
        //sharebtn.gameObject.SetActive(true);
        closebtn.gameObject.SetActive(true);
        //显示top title
        TopTitleCtl.instance.gameObject.SetActive(true);

        t.ReadPixels(new Rect(0, 0, nWidth, nHeight), 0, 0, true);
        byte[] bytes = t.EncodeToPNG();
        t.Compress(true);
        t.Apply();
        img.texture = t;

        System.DateTime now = new System.DateTime();
        now = System.DateTime.Now;
        string filename = "image" + now.Day + now.Hour + now.Minute + now.Second + ".png";

        string Path_save = "";

        if (Application.platform == RuntimePlatform.Android)
        {
            //Pictures
            string[] src = new string[1] { "Android" };
            string[] srcs = Application.persistentDataPath.Split(src, System.StringSplitOptions.None);
            Path_save = srcs[0] + "Pictures";
            if (!Directory.Exists(Path_save))
            {
                Directory.CreateDirectory(Path_save);
            }
            //何秋光数学思维
            string strPath1 = Path_save + "/何秋光数学思维";
            if (!Directory.Exists(strPath1))
            {
                Directory.CreateDirectory(strPath1);
            }
            //game name
            string strGameName = strPath1 + "/" + ApkInfo.GetSceenName(SceneEnum.None);
            BaseScene baseScene = SceneMgr.Instance.GetNowScene();
            if (baseScene != null)
                strGameName = strPath1 + "/" + ApkInfo.GetSceenName(baseScene.mSceneType);
            if (!Directory.Exists(strGameName))
            {
                Directory.CreateDirectory(strGameName);
            }
            //save
            strPicPath = strGameName + "/" + filename;           
            File.WriteAllBytes(strPicPath, bytes);
        }
        else
        {
            Path_save = ApkConfig.GetScreenTextureSavePath();
            strPicPath = Path_save + "/" + filename;
            if (!Directory.Exists(Path_save))
            {
                Directory.CreateDirectory(Path_save);
            }           
            File.WriteAllBytes(strPicPath, bytes);
        }

        //2.5秒后自动消失
        yield return new WaitForSeconds(2.5f);
        bState = false;
        transform.DOScale(Vector3.one * 0.001f, 0.3f).OnComplete(() =>
        {
            if (mFinishAction != null)
            {
                mFinishAction();
            }
            gameObject.SetActive(false);
        });
        
        //分享
        AndroidDataCtl.DoAndroidFunc("shareToStroryWorld", strPicPath);
    }


    private void ClickBtn(GameObject _go)
    {
        //if (_go.name.CompareTo(sharebtn.gameObject.name) == 0)
        //{
        //    //分享
        //    AndroidDataCtl.DoAndroidFunc("shareToStroryWorld", strPicPath);
        //}
        //else if (_go.name.CompareTo(closebtn.gameObject.name) == 0)
        //{
        //    bState = false;
        //    transform.DOScale(Vector3.one * 0.001f, 0.3f).OnComplete(()=> 
        //    {             
        //        if (mFinishAction != null)
        //        {
        //            mFinishAction();
        //        }
        //        gameObject.SetActive(false);
        //    });
        //}
    }

}
