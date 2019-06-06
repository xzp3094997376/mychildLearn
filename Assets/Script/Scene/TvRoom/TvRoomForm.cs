using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TvRoomForm : MonoBehaviour {
    Image mBg { get; set; }
    public Image mForm { get; set; }

    //数据，list表示关数队列。dic表示每一行数据，key=type，value=成绩：-1没有数据：0不打勾：1打勾
    List<Dictionary<int, int>> mData { get; set; }
    //打勾
    Dictionary<string, Image> mGou { get; set; }
    GridLayoutGroup group { get; set; }
    RectTransform mBtnForm { get; set; }
    public bool is_pass_game = false;

    void OnDestroy()
    {
        if(null != mForm)
            Destroy(mForm.gameObject);
    }
    void Start ()
    {
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 2;
        gameObject.AddComponent<GraphicRaycaster>();
        gameObject.layer = LayerMask.NameToLayer("UI");

        mGou = new Dictionary<string, Image>();
        mData = new List<Dictionary<int, int>>();
        config_game config = FormManager.config_games[SceneEnum.TvRoom];
        for(int i = 0; i < config.m_all_guanka; i++)
        {
            mData.Add(new Dictionary<int, int>());
        }
        Reset();
        ShowBtnForm();
    }

    //数据表清空
    public void Reset()
    {
        for(int i = 0; i < mData.Count; i++)
        {
            for(int type = 1; type <= 5; type++)
            {
                if(!mData[i].ContainsKey(type))
                {
                    mData[i].Add(type, -1);
                }
                else
                {
                    mData[i][type] = -1;
                }
            }
        }
        foreach( Image img in mGou.Values)
        {
            img.gameObject.SetActive(false);
        }

    }
    void Show()
    {
        is_pass_game = false;
        Show(false);
    }
    public void Show(bool flush_data)
    {
        is_pass_game = flush_data;
        Global.instance.PlayBtnClickAnimation(mBtnForm.transform);

        if (null == mBg)
        {
            mBg = UguiMaker.newImage("bg", transform, "public", "white");
            mBg.rectTransform.sizeDelta = new Vector2(1423, 800);
            mBg.color = new Color(0, 0, 0, 0.5f);
            mBg.gameObject.AddComponent<Button>().onClick.AddListener(HideListen);

            mForm = UguiMaker.newImage("form", transform, "tvroom_sprite", "form_bg", true);

            group = UguiMaker.newGameObject("group", mForm.transform).AddComponent<GridLayoutGroup>();
            group.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            group.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            group.constraintCount = 5;
            group.cellSize = new Vector2(55, 39);
            group.padding = new RectOffset(-198, 0, -89, 0);
            group.spacing = new Vector2(32.07f, 7.3f);

            for (int guanka = 1; guanka <= 5; guanka++)
            {
                for(int type = 1; type <= 5; type++)
                {
                    string key = guanka + "-" + type;
                    Image img = UguiMaker.newImage( key, group.transform, "tvroom_sprite", "form_correct", false);
                    img.gameObject.SetActive(false);
                    mGou.Add(key, img);
                }
            }
        }
        mBg.gameObject.SetActive(true);
        mForm.gameObject.SetActive(true);
        StartCoroutine(TShow(flush_data));

    }
    public void HideListen()
    {
        Hide(false);
    }
    public void Hide(bool dont_play_animation = false)
    {
        if (!dont_play_animation)
        {
            if (is_pass_game)
            {
                TvRoomCtl.instance.Callback_NextGuanka();
            }
            StartCoroutine(THide());
        }
        else
        {
            mBg.gameObject.SetActive(false);
            mForm.gameObject.SetActive(false);
        }

    }
    IEnumerator TShow(bool flush_data)
    {
        TvRoomCtl.instance.mSound.PlayOnly("tvroom_sound", "06-表格收起");
        mForm.rectTransform.anchoredPosition = new Vector2(0, 50);
        for (float i = 0; i < 1f; i += 0.2f)
        {
            mBg.color = Color32.Lerp(new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 200), i);
            mForm.rectTransform.anchoredPosition = Vector2.Lerp(mBtnForm.anchoredPosition, new Vector2(0, 50), i);
            mForm.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, i);
            yield return new WaitForSeconds(0.01f);
        }
        mForm.rectTransform.anchoredPosition = new Vector2(0, 50);


        if (flush_data)
        {
            //获取数据
            for (int type = 1; type <= 5; type++)
            {
                string key = TvRoomCtl.instance.mGuanka.guanka + "-" + type;
                if (TvRoomCtl.instance.mControl.GetTypeIsSelect(type))
                {
                    mData[TvRoomCtl.instance.mGuanka.guanka - 1][type] = 1;
                }
                else
                {
                    mData[TvRoomCtl.instance.mGuanka.guanka - 1][type] = 0;
                }
            }
            //根据数据刷新表格
            float p = 1;
            for (int guanka = 1; guanka <= 5; guanka++)
            {
                for (int type = 1; type <= 5; type++)
                {
                    string key = guanka + "-" + type;
                    Image img = mGou[guanka + "-" + type];
                    if (img.gameObject.activeSelf)
                        continue;
                    switch (mData[guanka - 1][type])
                    {
                        case -1:
                            img.gameObject.SetActive(false);
                            break;
                        case 0:
                            img.gameObject.SetActive(true);
                            img.sprite = ResManager.GetSprite("tvroom_sprite", "form_error");
                            if(0.5f < mForm.transform.localScale.x)
                                TvRoomCtl.instance.mSound.PlayShort("tvroom_sound", "07-表格符号");
                            break;
                        case 1:
                            img.gameObject.SetActive(true);
                            img.sprite = ResManager.GetSprite("tvroom_sprite", "form_correct");
                            if (0.5f < mForm.transform.localScale.x)
                                TvRoomCtl.instance.mSound.PlayShort("tvroom_sound", "07-表格符号");
                            break;
                    }

                    for (float i = 0; i < 1f; i += 0.07f)
                    {
                        p = 1 + Mathf.Sin(Mathf.PI * i) * 3f * 0.2f;
                        img.transform.localScale = new Vector3(p, p, 1);
                        yield return new WaitForSeconds(0.01f);
                    }
                    img.transform.localScale = Vector3.one;
                    //yield return new WaitForSeconds(0.1f);

                }
            }

        }


    }
    IEnumerator THide()
    {
        TvRoomCtl.instance.mSound.PlayOnly("tvroom_sound", "06-表格收起");
        for (float i = 0; i < 1f; i += 0.2f)
        {
            mBg.color = Color32.Lerp(new Color32(0, 0, 0, 200), new Color32(0, 0, 0, 0), i);
            mForm.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(0, 50), mBtnForm.anchoredPosition, i);
            mForm.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i);
            yield return new WaitForSeconds(0.01f);
        }

        mBg.gameObject.SetActive(false);
        mForm.gameObject.SetActive(false);

    }



    //表格按钮
    public void ShowBtnForm()
    {
        Button btn = null;
        if (null == mBtnForm)
        {
            btn = UguiMaker.newButton("btn_Form", transform, "tvroom_sprite", "btn_form");
            mBtnForm = btn.GetComponent<RectTransform>();
            btn.transition = Selectable.Transition.None;
            btn.onClick.AddListener(Show);

            mBtnForm.anchoredPosition3D = new Vector3(-542.8f, -200.9f, 0);

        }
        else
        {
            btn = mBtnForm.GetComponent<Button>();
        }
        
        StartCoroutine(TShowBtnForm());

    }
    IEnumerator TShowBtnForm()
    {
        Vector3 s0 = mBtnForm.localScale;
        Vector3 s1 = 0.5f * s0;

        mBtnForm.gameObject.SetActive(true);
        for (float i = 0; i < 1f; i += 0.1f)
        {
            float p = Mathf.Sin(Mathf.PI * i) * s0.x * 0.3f;
            mBtnForm.localScale = Vector3.Lerp(s1, s0, i) + new Vector3(p, p, 0);
            yield return new WaitForSeconds(0.01f);
        }
        mBtnForm.localScale = s0;
      
    }
    


}
