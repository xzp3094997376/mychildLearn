using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PickPeachGuanka1 : MonoBehaviour {

    public bool mdata_can_control = false;
    public PickPeachAnimal[] mAnimal = new PickPeachAnimal[3];
    public List<PickPeachTao> mGroundTaozi = new List<PickPeachTao>();

    Button mBtnOK { get; set; }
    public ParticleSystem mEffectOK { get; set; }

    public int mdata_animal_id1 = 0;
    public int mdata_animal_id2 = 0;
    public int mdata_animal_id3 = 0;
    

    Vector3 temp_clickdown_mark = Vector3.zero;
    Vector3 temp_clickdown_pos = Vector3.zero;
	void Update ()
    {
        if (1 != PickPeachCtl.instance.mdata_guanka || !mdata_can_control)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            temp_clickdown_pos = transform.worldToLocalMatrix.MultiplyPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            temp_clickdown_mark = PickPeachCtl.instance.mScene.transform.localPosition;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100))
            {
                PickPeachTreeCell.gSelect = hit.collider.gameObject.GetComponent<PickPeachTreeCell>();
                PickPeachLanzi.gSelect = hit.collider.gameObject.GetComponent<PickPeachLanzi>();

                if(null == PickPeachLanzi.gSelect)
                {
                    PickPeachTao.gSelect = hit.collider.gameObject.GetComponent<PickPeachTao>();
                    if (null != PickPeachTao.gSelect)
                    {
                        PickPeachTao.gSelect.Select();
                    }
                }

            }

        }
        else if(Input.GetMouseButton(0))
        {
            if(null == PickPeachTao.gSelect)
            {
                //拖拽场景
                float y = transform.worldToLocalMatrix.MultiplyPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).y - temp_clickdown_pos.y;
                Vector3 pos = temp_clickdown_mark + new Vector3(0, y, 0);
                if (pos.y < -633)
                    pos.y = -633;
                else if (pos.y > 0)
                    pos.y = 0;
                PickPeachCtl.instance.mScene.transform.localPosition = pos;
            }
            else
            {
                //拖拽桃子
                Vector3 temp_pos = transform.worldToLocalMatrix.MultiplyPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)); ;
                temp_pos.z = 0;
                PickPeachTao.gSelect.mRtran.anchoredPosition3D = temp_pos;
            }



        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (null != PickPeachTreeCell.gSelect)
            {
                //摇树
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if(PickPeachTreeCell.gSelect == hit.collider.gameObject.GetComponent<PickPeachTreeCell>())
                    {
                        PickPeachTreeCell.gSelect.Shake();
                    }

                }

                PickPeachTreeCell.gSelect = null;

            }
            else if(null != PickPeachTao.gSelect)
            {
                PickPeachLanzi.gSelect = null;
                RaycastHit[] hits = Common.getMouseRayHits();
                if(null != hits)
                {
                    foreach(RaycastHit hit in hits)
                    {
                        PickPeachLanzi.gSelect = hit.collider.gameObject.GetComponent<PickPeachLanzi>();
                        if (null != PickPeachLanzi.gSelect)
                            break;
                    }
                }
                if(null != PickPeachLanzi.gSelect)
                {
                    if(PickPeachLanzi.gSelect.PushTaozi(PickPeachTao.gSelect))
                    {
                        //放进篮子
                        PickPeachCtl.instance.mSound.PlayShort("进去");
                        if(mGroundTaozi.Contains(PickPeachTao.gSelect))
                        {
                            mGroundTaozi.Remove(PickPeachTao.gSelect);
                        }
                        switch (PickPeachLanzi.gSelect.GetResult())
                        {
                            case -1:
                                PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip我想要更多的桃子" + Random.Range(0, 1000) % 2, 1);
                                break;
                            case 0:
                                if(PickPeachCtl.instance.GetFlowerNumBig() == PickPeachLanzi.gSelect.mdata_flower_number)
                                {
                                    PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip我的桃子是最多的", 1);
                                }
                                else if (PickPeachCtl.instance.GetFlowerNumSmall() == PickPeachLanzi.gSelect.mdata_flower_number)
                                {
                                    PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip我的桃子最少", 1);
                                }
                                else
                                {
                                    PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip这是我想要的桃子", 1);
                                }
                                break;
                            case 1:
                                PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip太多了我有点吃不完", 1);
                                break;

                        }
                    }
                    else
                    {
                        //篮子已满
                        PickPeachTao.gSelect.FallDown();
                    }

                }
                else //if(PickPeachTao.gSelect.mRtran.anchoredPosition.y > -300)
                {
                    //掉下来
                    PickPeachTao.gSelect.FallDown();
                }

                PickPeachTao.gSelect = null;

            }
            else if(null != PickPeachLanzi.gSelect)
            {
                //弹出桃子
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if(PickPeachLanzi.gSelect == hit.collider.gameObject.GetComponent<PickPeachLanzi>())
                    {
                        PickPeachCtl.instance.mSound.PlayShort("进去");
                        PickPeachLanzi.gSelect.PopTaozi();


                        switch (PickPeachLanzi.gSelect.GetResult())
                        {
                            case -1:
                                PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip我想要更多的桃子" + Random.Range(0, 1000) % 2, 1);
                                break;
                            case 0:
                                if (PickPeachCtl.instance.GetFlowerNumBig() == PickPeachLanzi.gSelect.mdata_flower_number)
                                {
                                    PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip我的桃子是最多的", 1);
                                }
                                else if (PickPeachCtl.instance.GetFlowerNumSmall() == PickPeachLanzi.gSelect.mdata_flower_number)
                                {
                                    PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip我的桃子最少", 1);
                                }
                                else
                                {
                                    PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip这是我想要的桃子", 1);
                                }
                                break;
                            case 1:
                                PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip太多了我有点吃不完", 1);
                                break;

                        }

                    }
                    
                }

            }

        }

	
	}

    public Button GetBtnOK()
    {
        return mBtnOK;
    }

    public void BeginGame()
    {


        StartCoroutine(TBeginGame());

    }
    bool isFirstTime = true;
    IEnumerator TBeginGame()
    {
        yield return new WaitForSeconds(0.1f);
        if (null != mAnimal[0])
        {
            Destroy(mAnimal[0].gameObject);
            mAnimal[0] = null;
        }
        if (null != mAnimal[2])
        {
            Destroy(mAnimal[1].gameObject);
            mAnimal[1] = null;
        }
        if (null != mAnimal[2])
        {
            Destroy(mAnimal[2].gameObject);
            mAnimal[2] = null;
        }

        List<int> ids = Common.GetMutexValue(1, 14, 3);
        mdata_animal_id1 = ids[0];
        mdata_animal_id2 = ids[1];
        mdata_animal_id3 = ids[2];

        mAnimal[0] = UguiMaker.newGameObject("mAnimal1", PickPeachCtl.instance.mScene.transform).AddComponent<PickPeachAnimal>();
        mAnimal[1] = UguiMaker.newGameObject("mAnimal2", PickPeachCtl.instance.mScene.transform).AddComponent<PickPeachAnimal>();
        mAnimal[2] = UguiMaker.newGameObject("mAnimal3", PickPeachCtl.instance.mScene.transform).AddComponent<PickPeachAnimal>();
        mAnimal[0].Init(mdata_animal_id1, 0);
        mAnimal[1].Init(mdata_animal_id2, 1);
        mAnimal[2].Init(mdata_animal_id3, 2);

        mAnimal[0].MoveIn();
        mAnimal[1].MoveIn();
        mAnimal[2].MoveIn();

        yield return new WaitForSeconds(3);
        if (null != mGroundTaozi && 0 < mGroundTaozi.Count)
        {
            while(0 < mGroundTaozi.Count)
            {
                if(null != mGroundTaozi[0])
                {
                    Destroy(mGroundTaozi[0].gameObject);
                }
                mGroundTaozi.RemoveAt(0);
            }
        }
        mGroundTaozi.Clear();

        if (null == mBtnOK)
        {
            mBtnOK = UguiMaker.newButton("mBtnOK", transform, "pickpeach_sprite", "btn_up");
            mBtnOK.transition = Selectable.Transition.None;
            

            mEffectOK = ResManager.GetPrefab("effect_okbtn", "okbtn_effect").GetComponent<ParticleSystem>();
            UguiMaker.InitGameObj(mEffectOK.gameObject, mBtnOK.transform, "mEffectOK", new Vector3(0, 0, 0), Vector3.one);

        }
        else
        {
            ShowBtnEnd(true);
        }
        mBtnOK.transform.SetAsLastSibling();
        mBtnOK.onClick.RemoveAllListeners();
        mBtnOK.onClick.AddListener(OnClkBtn);
        mBtnOK.image.sprite = ResManager.GetSprite("pickpeach_sprite", "btn_up");

        Vector2 pos0 = new Vector2(561, -500);
        Vector2 pos1 = new Vector2(561, -348);
        for(float i = 0; i < 1f; i += 0.07f)
        {
            mBtnOK.image.rectTransform.anchoredPosition = Vector2.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mBtnOK.image.rectTransform.anchoredPosition = pos1;

        mdata_can_control = true;

        PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip关卡1", 1, true);


        yield return new WaitForSeconds(1);
        if (isFirstTime)
        {
            SoundManager.instance.PlayBgAsync("bgmusic_loop3", "bgmusic_loop3", 0.4f);
            isFirstTime = false;
        }

    }

    public void EndGame()
    {
        mdata_can_control = false;
        mEffectOK.Play();
        mAnimal[0].PlaySayYes();
        mAnimal[1].PlaySayYes();
        mAnimal[2].PlaySayYes();
        

    }
    public void ShowBtnEnd(bool is_show)
    {
        mBtnOK.gameObject.SetActive(is_show);
    }


    public void PushInGround_Taozi(PickPeachTao tao)
    {
        if(!mGroundTaozi.Contains(tao))
        {
            mGroundTaozi.Add(tao);
        }
        mGroundTaozi.Sort(
            delegate(PickPeachTao x, PickPeachTao y)
            {
                if (x.mRtran.anchoredPosition.y > y.mRtran.anchoredPosition.y)
                    return -1;
                else if (x.mRtran.anchoredPosition.y < y.mRtran.anchoredPosition.y)
                    return 1;
                else
                    return 0;

            }
            );
        for(int i = 0; i < mGroundTaozi.Count; i++)
        {
            mGroundTaozi[i].mRtran.SetAsLastSibling();
        }

    }
    public void OnClkBtn()
    {
        if (!mdata_can_control)
            return;

        mBtnOK.image.sprite = ResManager.GetSprite("pickpeach_sprite", "btn_down");
        CancelInvoke();
        Invoke("InvokeOnClkBtn", 0.3f);
        PickPeachCtl.instance.mSound.PlayShort("button_up");
    }
    void InvokeOnClkBtn()
    {
        if (!PickPeachCtl.instance.callbackGuanka1_btnOk())
        {
            PickPeachCtl.instance.mSound.PlayShort("button_down");
            mBtnOK.image.sprite = ResManager.GetSprite("pickpeach_sprite", "btn_up");
        }

    }

}
