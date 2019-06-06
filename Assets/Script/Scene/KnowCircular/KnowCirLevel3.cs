using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class KnowCirLevel3 : MonoBehaviour
{

    KnowCircularCtrl mCtrl;
    public int nGameTimes = 0;//累积次数
    public int nToCount = 0;//达成拖对目标
    public int nCount = 0;//拖对次数

    public GameObject mgo = null;
    public GameObject checkpos;
    public Image[] checkImgs = null;
    private List<KCirObjLv3> checkKCList = new List<KCirObjLv3>();
    private Image ImgCheckWrong;


    public KCYinhuaLv3 yanhua0;
    public KCYinhuaLv3 yanhua1;
    public KCYinhuaLv3 yanhua2;

    public void InitAwake(KnowCircularCtrl _mctrl)
    {
        mCtrl = _mctrl;
        SetDate();
    }

    public void ResetInfos()
    {
        nToCount = 0;
        nCount = 0;
        Common.DestroyChilds(transform);
        checkKCList.Clear();
    }

    public void SetDate()
    {
        ResetInfos();
        StartCoroutine(ieSetDate());
    }
    IEnumerator ieSetDate()
    {
        

        if (nGameTimes <= 0)
        {
            if (UnityEngine.Random.value < 0.65f)
            { mgo = ResManager.GetPrefab("knowcircular_prefab", "game0", transform); }
            else
            { mgo = ResManager.GetPrefab("knowcircular_prefab", "game1", transform); }
        }
        else
        {
            mgo = ResManager.GetPrefab("knowcircular_prefab", "game2", transform);
            RawImage rawImage = mgo.GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.texture = ResManager.GetTexture("knowcircular_texture", "beijing1");
            }           
        }

        yanhua0 = ResManager.GetPrefab("knowcircular_prefab", "myinhua0", mgo.transform).AddComponent<KCYinhuaLv3>();
        yanhua1 = ResManager.GetPrefab("knowcircular_prefab", "myinhua1", mgo.transform).AddComponent<KCYinhuaLv3>();
        yanhua2 = ResManager.GetPrefab("knowcircular_prefab", "myinhua2", mgo.transform).AddComponent<KCYinhuaLv3>();
        yanhua0.transform.localPosition = new Vector3(19f, 12f, 0f);
        yanhua1.transform.localPosition = new Vector3(333f, -70f, 0f);
        yanhua2.transform.localPosition = new Vector3(-348f, -52f, 0f);
        yanhua0.InitAwake(0);
        yanhua1.InitAwake(1);
        yanhua2.InitAwake(2);

        ImgCheckWrong = UguiMaker.newGameObject("ImgCheckWrong", mgo.transform).AddComponent<Image>();
        ImgCheckWrong.rectTransform.sizeDelta = new Vector2(1280f, 800f);
        ImgCheckWrong.transform.SetSiblingIndex(0);
        ImgCheckWrong.color = new Color(1f, 1f, 1f, 0f);
        Button btnWrong = ImgCheckWrong.gameObject.AddComponent<Button>();
        btnWrong.transition = Selectable.Transition.None;
        btnWrong.onClick.AddListener(MWrongClick);

        Image[] imgs = mgo.transform.GetComponentsInChildren<Image>();
        for (int i = 0; i < imgs.Length; i++)
        {
            if (imgs[i] != null && imgs[i].gameObject.name.Contains("t_obj"))
            {
                imgs[i].sprite = ResManager.GetSprite("knowcircular_sprite", imgs[i].gameObject.name);
            }
        }

        checkpos = mgo.transform.Find("checkpos").gameObject;
        checkImgs = checkpos.transform.GetComponentsInChildren<Image>();
        nToCount = checkImgs.Length;
        for (int i = 0; i < checkImgs.Length; i++)
        {
            if (checkImgs[i] != null)
            {
                KCirObjLv3 kcCtrl = checkImgs[i].gameObject.AddComponent<KCirObjLv3>();
                kcCtrl.InitAwake();
                kcCtrl.SetClickCall(MClickCall);
                checkKCList.Add(kcCtrl);
            }
        }

        mgo.transform.localPosition = new Vector3(0f, -800f, 0f);
        mgo.transform.DOLocalMove(Vector3.zero, 1f);
        yield return new WaitForSeconds(1.1f);

        mCtrl.PlayTipSound();
    }


    private void MClickCall(KCirObjLv3 _obj)
    {
        _obj.ShowRang();
        CheckLevelPass();
        mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "setok"));
    }

    private void MWrongClick()
    {
        mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("knowcircular_sound", "setwrong"));

        //yanhua0.PlayYinhua();
        //yanhua1.PlayYinhua();
    }

    private void CheckLevelPass()
    {
        nCount++;
        if (nCount >= nToCount && nToCount > 0)
        {
            if (ImgCheckWrong != null)
            {
                ImgCheckWrong.enabled = false;
            }
            nGameTimes++;
            mCtrl.StartCoroutine(ieCheckLevelPass());
        }
    }
    IEnumerator ieCheckLevelPass()
    {
        yield return new WaitForSeconds(1f);
        PlayYanhua();
        mCtrl.PlaySucSound();
        yield return new WaitForSeconds(5f);
        if (nGameTimes < 2)
        {
            //元素消失
            for (int i = 0; i < checkKCList.Count; i++)
            {
                checkKCList[i].HideRang();
            }
            yield return new WaitForSeconds(0.5f);
            mgo.transform.DOLocalMoveY(-800f, 1f);
            yield return new WaitForSeconds(1.1f);
            //replay
            SetDate();
        }
        else
        {
            mCtrl.LevelCheckNext();
        }
    }

    public void PlayYanhua()
    {
        StartCoroutine(iePlayYanhua());
    }
    IEnumerator iePlayYanhua()
    {
        yanhua0.PlayYinhua();
        yield return new WaitForSeconds(0.25f);
        yanhua1.PlayYinhua();
        yield return new WaitForSeconds(0.25f);
        yanhua2.PlayYinhua();
    }

}
