using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TvRoomControl : MonoBehaviour {

    public RectTransform mRtran { get; set; }
    Image mBg0 { get; set; }
    Image mBg1 { get; set; }
    Image mBtnOK { get; set; }
    Image mBtnStop { get; set; }
    ParticleSystem mOKBtn_Effect { get; set; }

    List<Image> mBtnTypes = new List<Image>();
    Dictionary<int, bool> mBtnPress{ get; set; }//index-记录是否按下对应按钮
    Dictionary<int, GameObject> mBtnEffect { get; set; }//index-按钮对应特效



    public void Init()
    {
        if (null == mRtran)
        {
            mBtnPress = new Dictionary<int, bool>();
            mBtnPress.Add(0, false);
            mBtnPress.Add(1, false);
            mBtnPress.Add(2, false);
            mBtnPress.Add(3, false);
            mBtnPress.Add(4, false);



            mRtran = gameObject.GetComponent<RectTransform>();
            mRtran.sizeDelta = new Vector2(1423, 0);

            mBg0 = UguiMaker.newImage("bg0", transform, "tvroom_sprite", "control_bg0", false);
            mBg0.rectTransform.sizeDelta = new Vector2(1423, 124);
            mBg0.rectTransform.pivot = new Vector2(0.5f, 0);
            mBg0.rectTransform.anchoredPosition3D = Vector3.zero;

            mBg1 = UguiMaker.newImage("bg1", transform, "tvroom_sprite", "control_bg1", false);
            mBg1.rectTransform.sizeDelta = new Vector2(1076, 116);
            mBg1.rectTransform.pivot = new Vector2(0.5f, 0);
            mBg1.rectTransform.anchoredPosition3D = Vector3.zero;
            mBg1.gameObject.SetActive(false);

            List<Vector3> poss = new List<Vector3>(){
                new Vector3( -401.5f, 7.76f, 0f),
                new Vector3( -225.5f, 7.76f, 0f),
                new Vector3( -49.5f, 7.76f, 0f),
                new Vector3( 126.5f, 7.76f, 0f),
                new Vector3( 302.5f, 7.76f, 0f),
            };

            for (int i = 0; i < 5; i++)
            {
                Button b = UguiMaker.newButton("btn" + i, mBg1.transform, "tvroom_sprite", "btn1up");
                Image img = b.GetComponent<Image>();
                img.rectTransform.anchoredPosition3D = poss[i];
                mBtnTypes.Add(img);
            }
            mBtnTypes[0].GetComponent<Button>().onClick.AddListener(OnClkBtn0);
            mBtnTypes[1].GetComponent<Button>().onClick.AddListener(OnClkBtn1);
            mBtnTypes[2].GetComponent<Button>().onClick.AddListener(OnClkBtn2);
            mBtnTypes[3].GetComponent<Button>().onClick.AddListener(OnClkBtn3);
            mBtnTypes[4].GetComponent<Button>().onClick.AddListener(OnClkBtn4);

            mBtnOK = UguiMaker.newImage("ok", mBg1.transform, "tvroom_sprite", "btnokup");
            mBtnOK.rectTransform.anchoredPosition3D = new Vector3(457, 12.7f, 0);
            Button btnOK = mBtnOK.gameObject.AddComponent<Button>();
            btnOK.transition = Selectable.Transition.None;
            btnOK.onClick.AddListener(OnClkBtnOK);

            mBtnStop = UguiMaker.newImage("ok", mBg1.transform, "tvroom_sprite", "btnokup");
            mBtnStop.rectTransform.anchoredPosition3D = new Vector3(0, 12.7f, 0);
            Button btnStop = mBtnStop.gameObject.AddComponent<Button>();
            btnStop.transition = Selectable.Transition.None;
            btnStop.onClick.AddListener(OnClkBtnStop);


            mBtnEffect = new Dictionary<int, GameObject>();
            Color32[] colors = new Color32[] {
                new Color32(255, 111, 0, 255),
                new Color32(255, 0, 206, 255),
                new Color32(135, 131, 0, 255),
                new Color32(0, 255, 213, 255),
                new Color32(255, 0, 216, 255)
            };
            for (int i = 0; i < 5; i++)
            {
                GameObject effect = ResManager.GetPrefab("tvroom_prefab", "button_effect");
                UguiMaker.InitGameObj(effect, mBtnTypes[i].transform, "effect", Vector3.zero, Vector3.one);
                effect.GetComponent<ParticleSystem>().startColor = colors[i];
                effect.gameObject.SetActive(false);
                mBtnEffect.Add(i, effect);
            }

        }
    }
    public void ResetData()
    {
        for (int i = 0; i < 5; i++)
        {
            mBtnTypes[i].sprite = ResManager.GetSprite("tvroom_sprite", "btn" + (i + 1) + "up");
            mBtnEffect[i].gameObject.SetActive(false);
        }

        mBtnPress[0] = false;
        mBtnPress[1] = false;
        mBtnPress[2] = false;
        mBtnPress[3] = false;
        mBtnPress[4] = false;
    }
         
    public bool GetTypeIsSelect(int type)
    {
        return mBtnPress[type-1];
    }
    public void SetAllBtnUp()
    {
        for(int i = 0; i < 5; i++)
        {
            mBtnTypes[i].sprite = ResManager.GetSprite("tvroom_sprite", "btn" + (i + 1) + "up");
            mBtnEffect[i].gameObject.SetActive(false);
        }
    }
         

	
    public void OnClkBtn0()
    {
        OnClkBtnCall(0);

    }
    public void OnClkBtn1()
    {
        OnClkBtnCall(1);

    }
    public void OnClkBtn2()
    {
        OnClkBtnCall(2);

    }
    public void OnClkBtn3()
    {
        OnClkBtnCall(3);

    }
    public void OnClkBtn4()
    {
        OnClkBtnCall(4);

    }
    public void OnClkBtnCall(int index)
    {
        mBtnPress[index] = !mBtnPress[index];
        if (mBtnPress[index])
        {
            mBtnTypes[index].sprite = ResManager.GetSprite("tvroom_sprite", "btn" + (index+1) + "down");
            mBtnEffect[index].gameObject.SetActive(true);
            TvRoomCtl.instance.mSound.PlayShort("tvroom_sound", "01-按钮按下");
            switch (index)
            {
                case 0:
                    TvRoomCtl.instance.mSound.PlayOnly("tvroom_sound", "game-tips2-2-2");
                    break;
                case 1:
                    TvRoomCtl.instance.mSound.PlayOnly("tvroom_sound", "game-tips2-2-6");
                    break;
                case 2:
                    TvRoomCtl.instance.mSound.PlayOnly("tvroom_sound", "game-tips2-2-5");
                    break;
                case 3:
                    TvRoomCtl.instance.mSound.PlayOnly("tvroom_sound", "game-tips2-2-3");
                    break;
                case 4:
                    TvRoomCtl.instance.mSound.PlayOnly("tvroom_sound", "game-tips2-2-4");
                    break;
            }

        }
        else
        {
            mBtnTypes[index].sprite = ResManager.GetSprite("tvroom_sprite", "btn" + (index+1) + "up");
            mBtnEffect[index].gameObject.SetActive(false);
            TvRoomCtl.instance.mSound.PlayShort("tvroom_sound", "02-按钮弹起-01");
        }
    }

    public void OnClkBtnOK()
    {
        TvRoomCtl.instance.mSound.StopTip();
        mBtnOK.raycastTarget = false;
        mBtnOK.sprite = ResManager.GetSprite("tvroom_sprite", "btnokdown");

        if(TvRoomCtl.instance.Callback_ClickOK())
        {
            //Invoke("InvokeOnClkBtnOK", 3);

            TvRoomCtl.instance.mSound.StopOnly();
            if (null == mOKBtn_Effect)
            {
                mOKBtn_Effect = ResManager.GetPrefab("tvroom_prefab", "okbtn_effect").GetComponent<ParticleSystem>();
                UguiMaker.InitGameObj(mOKBtn_Effect.gameObject, mBtnOK.transform, "okbtn_effect", Vector3.zero, Vector3.one);
            }
            TvRoomCtl.instance.mSound.PlayShort("tvroom_sound", "04-星星-02");
            mOKBtn_Effect.Play();
        }
        else
        {
            Invoke("InvokeOnClkBtnOK", 2);

            if (Random.Range(0, 1000) % 2 == 0)
                TvRoomCtl.instance.mSound.PlayOnly("tvroom_sound", "game-tips2-2-8");
            else
                TvRoomCtl.instance.mSound.PlayOnly("tvroom_sound", "game-tips2-2-8A");

        }

        TvRoomCtl.instance.mSound.PlayShort("tvroom_sound", "01-按钮按下");

    }
    public void InvokeOnClkBtnOK()
    {
        mBtnOK.raycastTarget = true;
        mBtnOK.sprite = ResManager.GetSprite("tvroom_sprite", "btnokup");

        TvRoomCtl.instance.mSound.PlayShort("tvroom_sound", "02-按钮弹起-01");

    }
    
    public void OnClkBtnStop()
    {
        mBtnStop.raycastTarget = false;
        mBtnStop.sprite = ResManager.GetSprite("tvroom_sprite", "btnokdown");
        Invoke("InvokeOnClkBtnStop", 2);
        TvRoomCtl.instance.Callback_ClickStop();
        PushOutStop();
    }
    public void InvokeOnClkBtnStop()
    {
        mBtnStop.raycastTarget = true;
        mBtnStop.sprite = ResManager.GetSprite("tvroom_sprite", "btnokup");
    }

    //控制台
    public void PushInBg1()
    {
        mBtnStop.gameObject.SetActive(false);
        mBtnOK.gameObject.SetActive(true);
        for (int i = 0; i < mBtnTypes.Count; i++)
        {
            mBtnTypes[i].gameObject.SetActive(true);
        }

        ResetData();
        StartCoroutine(TPushInBg1());
    }
    public void PushOutBg1()
    {
        SetAllBtnUp();
        StartCoroutine(TPushOutBg1());
    }
    IEnumerator TPushInBg1()
    {
        InvokeOnClkBtnOK();
        mBg1.gameObject.SetActive(true);
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mBg1.rectTransform.anchoredPosition = Vector2.Lerp(
                new Vector2(0, -116), new Vector2(0, 0), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mBg1.rectTransform.anchoredPosition = new Vector2(0, 0);
    }
    IEnumerator TPushOutBg1()
    {
        Vector3 pos0 = mBg1.rectTransform.anchoredPosition;
        Vector3 pos1 = new Vector3(0, -116);
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mBg1.rectTransform.anchoredPosition = Vector2.Lerp(pos0, pos1, i);
            yield return new WaitForSeconds(0.01f);
        }
        mBg1.rectTransform.anchoredPosition = pos1;

    }

    //停止随机
    public void PushInStop()
    {
        mBtnStop.gameObject.SetActive(true);
        mBtnOK.gameObject.SetActive(false);
        for(int i = 0; i < mBtnTypes.Count; i++)
        {
            mBtnTypes[i].gameObject.SetActive(false);
        }
        
        StartCoroutine(TPushInBg1());
    }
    public void PushOutStop()
    {
        StartCoroutine(TPushOutStop());
    }
    IEnumerator TPushInStop()
    {
        mBg1.gameObject.SetActive(true);
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mBg1.rectTransform.anchoredPosition = Vector2.Lerp(
                new Vector2(0, -116), new Vector2(0, 0), Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }

    }
    IEnumerator TPushOutStop()
    {
        yield return new WaitForSeconds(0.7f);
        Vector3 pos0 = mBg1.rectTransform.anchoredPosition;
        Vector3 pos1 = new Vector3(0, -116);
        for (float i = 0; i < 1f; i += 0.05f)
        {
            mBg1.rectTransform.anchoredPosition = Vector2.Lerp(pos0, pos1, i);
            yield return new WaitForSeconds(0.01f);
        }
        mBg1.rectTransform.anchoredPosition = pos1;
        PushInBg1();
    }

}
