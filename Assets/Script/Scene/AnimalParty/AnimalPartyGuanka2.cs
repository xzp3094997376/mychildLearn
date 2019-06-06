using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AnimalPartyGuanka2 : MonoBehaviour
{
    public RectTransform mRtran { get; set; }

    Image mBg2 { get; set; }
    Image mTable { get; set; }
    Image mToy0 { get; set; }
    Image mToy1 { get; set; }
    Image mToy2 { get; set; }

    List<AnimalPartyHead> mHeads = new List<AnimalPartyHead>();
    List<Image> mFruit = new List<Image>();

    //数据
    List<int> animal_ids { get; set; }//14种中挑,从背对我们的动物起逆时针数
    List<int> fruit_ids { get; set; }//6种中挑,从背对我们的动物起逆时针数
    List<int> find_seq { get; set; }//要找的动物的顺序,存的是座位索引
    int current_find_num = 0;//当前找到第几个动物
    int current_find_type = 0;//语音提示类型:-1前面的水果，0对面，1左边，2右边

    Vector3 temp_drag_pos = Vector3.zero;
    public void Update()
    {
        if (2 != AnimalPartyCtl.instance.mGuanka.guanka)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100))
            {
                AnimalPartyHead.gSelect = hit.collider.gameObject.GetComponent<AnimalPartyHead>();
                if(null != AnimalPartyHead.gSelect)
                {
                    AnimalPartyHead.gSelect.transform.SetAsLastSibling();
                    temp_drag_pos = hit.collider.gameObject.transform.position - Common.getMouseWorldPos();
                    AnimalPartyHead.gSelect.Select();
                    AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "星星闪烁2");
                }
            }
        }
        else if(Input.GetMouseButton(0))
        {
            if(null != AnimalPartyHead.gSelect)
            {
                Vector3 pos = transform.worldToLocalMatrix.MultiplyPoint( temp_drag_pos + Common.getMouseWorldPos());
                pos.z = 0;
                AnimalPartyHead.gSelect.mRtran.anchoredPosition3D = pos;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if(null != AnimalPartyHead.gSelect)
            {
                bool correct = false;
                int animal_index = find_seq[current_find_num];
                int animal_id = animal_ids[animal_index];

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray, 100);
                if (AnimalPartyHead.gSelect.mAnimalId == animal_id && null != hits && 0 < hits.Length)
                {
                    string p = "sit" + find_seq[current_find_num];
                    Debug.Log(p);

                    foreach(RaycastHit hit in hits)
                    {
                        if(hit.collider.gameObject.name.Equals(p))
                        {
                            AnimalPartyHead.gSelect.mRtran.anchoredPosition = hit.collider.gameObject.GetComponent<RectTransform>().anchoredPosition3D;
                            correct = true;
                            break;
                        }
                    }
                }

                if(correct)
                {
                    AnimalPartyHead.gSelect.SetBoxEnable(false);
                    AnimalPartyHead.gSelect.Select2Place();
                    InitNext();
                    AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "吸附");
                }
                else
                {
                    AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "选错飞回去音效");
                    AnimalPartyHead.gSelect.Select2Reset();
                }


                AnimalPartyHead.gSelect = null;
            }

        }

    }


    public void Init()
    {
        
        if (null == mBg2)
        {
            mRtran = GetComponent<RectTransform>();

            mBg2 = UguiMaker.newImage("mBg2", transform, "animalparty_sprite", "bg2", false);
            mBg2.rectTransform.sizeDelta = new Vector2(1423, 800);

            mTable = UguiMaker.newImage("mTable", transform, "animalparty_sprite", "table", false);
            mTable.rectTransform.anchoredPosition3D = new Vector3(0, -115, 0);

            mToy0 = UguiMaker.newImage("mToy0", transform, "animalparty_sprite", "toy0", false);
            mToy1 = UguiMaker.newImage("mToy1", transform, "animalparty_sprite", "toy1", false);
            mToy2 = UguiMaker.newImage("mToy2", transform, "animalparty_sprite", "toy2", false);

            mToy0.rectTransform.anchoredPosition = new Vector2(-627, -12);
            mToy1.rectTransform.anchoredPosition = new Vector2(533, -321);
            mToy2.rectTransform.anchoredPosition = new Vector2(683, -228);

            Vector3[] fruit_poss = new Vector3[] { new Vector3(3.3f, -116.5f, 0), new Vector3(209, -70.75f, 0), new Vector3(3.3f, 6.3f, 0), new Vector3(-198.2f, -70.75f, 0)};

            for(int i = 0; i < 4f; i++)
            {
                mHeads.Add(UguiMaker.newGameObject("head" + i, transform).AddComponent<AnimalPartyHead>());
                mFruit.Add(UguiMaker.newImage("fruit" + i, transform, "animalparty_sprite", "fruit0", false));
                mFruit[i].rectTransform.anchoredPosition3D = fruit_poss[i];
            }

        }

        animal_ids = Common.GetMutexValue(1, 14, 4);
        fruit_ids = Common.GetMutexValue(0, 5, 4);

        List<Vector3> poss = Common.PosSortByWidth(1280, 4, 233);
        poss = Common.BreakRank<Vector3>(poss);
        for (int i = 0; i < 4; i++)
        {
            mHeads[i].SetData(animal_ids[i], fruit_ids[i], poss[i]);
            mFruit[i].sprite = ResManager.GetSprite("animalparty_sprite", "fruit" + fruit_ids[i]);
        }

        Vector3[] sit_poss = new Vector3[] { new Vector3(4.8f, -228.45f, 0), new Vector3(398.7f, 2.84f, 0), new Vector3(14.94f, 103.7f, 0), new Vector3(-362.6f, -11.5f, 0)};
        for(int i = 0; i < 4; i++)
        {
            GameObject obj = UguiMaker.newGameObject("sit" + i, transform);
            obj.GetComponent<RectTransform>().anchoredPosition3D = sit_poss[i];
            obj.AddComponent<BoxCollider>().size = new Vector3(130, 130, 1);

        }

        //要从背对我们或者正对我们开始选
        find_seq = new List<int>() { 0, 1, 2, 3};
        find_seq = Common.BreakRank<int>(find_seq);
        if(0 == Random.Range(0, 1000) % 2)
        {
            find_seq.Remove(0);
            find_seq.Insert(0, 0);
        }
        else
        {
            find_seq.Remove(2);
            find_seq.Insert(0, 2);
        }
        string msg = "";
        for (int i = 0; i < find_seq.Count; i++)
            msg += find_seq[i] + "-";
        Debug.Log("find_seq=" + msg);

        current_find_num = 0;
        current_find_type = -1;
        

    }
    public void InitNext()
    {
        current_find_num++;
        if(4 == current_find_num)
        {
            Debug.LogError("游戏结束");
            StartCoroutine("TGuankaOver");
            return;
        }

        //随机参考对象
        int random_num = Random.Range(0, 1000) % current_find_num;//current_find_num - 1;//
        int random_index = find_seq[random_num];
        int current_index = find_seq[current_find_num];
        int sub = current_index - random_index;
        //语音提示类型:-1前面的水果，0对面，1左边，2右边
        switch (sub)
        {
            case 2:
            case -2:
                    current_find_type = 0;//在参考对象对面
                break;
            case 1:
            case -3:
                current_find_type = 2;//在参考对象右边
                break;
            case -1:
            case 3:
                current_find_type = 1;//在参考对象左边
                break;
                
        }

        PlayTip();

    }
    public void PlayTip()
    {
        int animal_index = find_seq[current_find_num];
        int animal_id = animal_ids[animal_index];
        int fruit_id = fruit_ids[animal_index];

        string msg = "";
        string animal_name = MDefine.GetAnimalNameByID_CH(animal_id);
        string fruit_name = "";
        string fruit_name_sound = "";

        switch (fruit_id)
        {
            case 0:
                fruit_name = "一盘苹果";
                fruit_name_sound = "game-tips6-2-30";
                break;
            case 1:
                fruit_name = "一盘香蕉";
                fruit_name_sound = "game-tips6-2-28";
                break;
            case 2:
                fruit_name = "一盘葡萄";
                fruit_name_sound = "game-tips6-2-29";
                break;
            case 3:
                fruit_name = "一盘火龙果";
                fruit_name_sound = "game-tips6-2-32";
                break;
            case 4:
                fruit_name = "一盘西瓜";
                fruit_name_sound = "game-tips6-2-27";
                break;
            case 5:
                fruit_name = "一盘橘子";
                fruit_name_sound = "game-tips6-2-31";
                break;

        }


        //语音提示类型:-1前面的水果，0对面，1左边，2右边
        switch (current_find_type)
        {
            case -1:
                {
                    msg = animal_name + "的前面放着" + fruit_name;
                    AnimalPartyCtl.instance.mSound.PlayTipList(
                        new List<string>() { "aa_animal_name", "animalparty_sound", "animalparty_sound" },
                        new List<string>() { animal_name, "game-tips6-2-26", fruit_name_sound },
                        true );
                }
                break;
            case 0:
                {
                    int other_index = animal_index + 2;
                    if (other_index > 3)
                        other_index -= 4;
                    int other_id = animal_ids[other_index];
                    msg = animal_name + "坐在" + MDefine.GetAnimalNameByID_CH(other_id) + "的对面";
                    AnimalPartyCtl.instance.mSound.PlayTipList(
                        new List<string>() { "aa_animal_name", "animalparty_sound", "aa_animal_name", "animalparty_sound" },
                        new List<string>() { animal_name, "game-tips6-2-33", MDefine.GetAnimalNameByID_CH(other_id), "game-tips6-2-34" },
                        true);
                }
                break;
            case 1:
                {
                    int other_index = animal_index + 1;
                    if (other_index > 3)
                        other_index = 0;
                    int other_id = animal_ids[other_index];
                    msg = animal_name + "坐在" + MDefine.GetAnimalNameByID_CH(other_id) + "的左边";
                    AnimalPartyCtl.instance.mSound.PlayTipList(
                        new List<string>() { "aa_animal_name", "animalparty_sound", "aa_animal_name", "animalparty_sound" },
                        new List<string>() { animal_name, "game-tips6-2-33", MDefine.GetAnimalNameByID_CH(other_id), "game-tips6-2-35"},
                        true);

                }
                break;
            case 2:
                {
                    int other_index = animal_index - 1;
                    if (other_index < 0)
                        other_index = 3;
                    int other_id = animal_ids[other_index];
                    msg = animal_name + "坐在" + MDefine.GetAnimalNameByID_CH(other_id) + "的右边";
                    AnimalPartyCtl.instance.mSound.PlayTipList(
                        new List<string>() { "aa_animal_name", "animalparty_sound", "aa_animal_name", "animalparty_sound" },
                        new List<string>() { animal_name, "game-tips6-2-33", MDefine.GetAnimalNameByID_CH(other_id), "game-tips6-2-36" },
                        true);
                }
                break;
        }

        Debug.Log(msg);

    }
    
    IEnumerator TGuankaOver()
    {
        TopTitleCtl.instance.mSoundTipData.Clean();
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < mHeads.Count; i++)
        {
            mHeads[i].PlaySound();
        }
        yield return new WaitForSeconds(2f);
        AnimalPartyCtl.instance.NextGuanka();

    }


}
