using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BirthdayCakeCake : MonoBehaviour {

    public List<BirthdayCakeFlower> mFlowers = new List<BirthdayCakeFlower>();
    public List<BirthdayCakeLazhu> mLazhus = new List<BirthdayCakeLazhu>();
    


	void Start () {
	
	}
	
    public void Init(BirthdayCakeCtl.Guanka guanka)
    {
        if(0 == mFlowers.Count)
        {
            GameObject flower_parent = UguiMaker.newGameObject("flower", transform);
            for (int i = 0; i < 24; i++)
            {
                BirthdayCakeFlower com = UguiMaker.newGameObject(i.ToString(), flower_parent.transform).AddComponent<BirthdayCakeFlower>();
                com.transform.localPosition = guanka.flower_poss[i];
                mFlowers.Add(com);

            }


            GameObject lazhu_parent = UguiMaker.newGameObject("lazhu", transform);
            for (int i = 0; i < BirthdayCakeCtl.instance.la_zhu_num; i++)
            {
                BirthdayCakeLazhu lazhu = UguiMaker.newGameObject(i.ToString(), lazhu_parent.transform).AddComponent<BirthdayCakeLazhu>();
                BoxCollider mBox = lazhu.gameObject.AddComponent<BoxCollider>();
                mBox.size = new Vector3(55, 45, 1);
                mBox.center = new Vector3(0, 10, 0);
                lazhu.transform.localPosition = guanka.lazhu_box_poss[i];
                mLazhus.Add(lazhu);

            }


        }
        else
        {
            for(int i = 0; i < mFlowers.Count; i++)
            {
                mFlowers[i].Reset();
            }
            for(int i = 0; i < mLazhus.Count; i++)
            {
                mLazhus[i].Reset();
            }
        }

    }
    public void Reset()
    {
        for(int i = 0; i < mFlowers.Count; i++)
        {
            mFlowers[i].Reset();
        }
        for (int i = 0; i < mLazhus.Count; i++)
        {
            mLazhus[i].Reset();
        }

    }

    public bool HaveRuleFlower()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return true;
        }

        //i表示挑多少个为一组作为模板
        int[] muban = new int[mFlowers.Count];// { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3};
        for(int i = 1; i <= mFlowers.Count / 2; i++)
        {
            if (0 != mFlowers.Count % i)
            {
                //不能完成呈现规律
                continue;
            }
            bool is_success = true;
            //生成模板数据
            for(int j = 0; j < muban.Length; j++)
            {
                muban[j] = mFlowers[j % i].mColor;
                if (muban[j] == 3)//不允许有白色
                    return false;
            }
            //用模板数据区匹配
            for(int j = 0; j < muban.Length; j++)
            {
                if(muban[j] != mFlowers[j].mColor)
                {
                    is_success = false;
                    break;
                }
            }
            if(is_success)
            {
                return true;
            }
        }
        return false;
    }
    public bool HaveRuleLazhu()
    {
        if(Application.platform != RuntimePlatform.Android)
        {
            //return true;
        }

        //i表示挑多少个为一组作为模板
        int[] muban = new int[mLazhus.Count];// { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3};
        for (int i = 1; i <= mLazhus.Count / 2; i++)
        {
            if(0 != mLazhus.Count % i)
            {
                //不能完成呈现规律
                continue;
            }
            bool is_success = true;
            //生成模板数据
            for (int j = 0; j < muban.Length; j++)
            {
                muban[j] = mLazhus[j % i].mColor;
                if (muban[j] == 3)
                    return false;
            }
            //用模板数据区匹配
            for (int j = 0; j < muban.Length; j++)
            {
                if (muban[j] != mLazhus[j].mColor)
                {
                    is_success = false;
                    break;
                }
            }
            if (is_success)
            {
                string str = "";
                for (int z = 0; z < muban.Length; z++)
                    str += muban[z];
                //Debug.Log("i=" + i);
                //Debug.Log(str);
                str = "";
                for (int z = 0; z < mLazhus.Count; z++)
                    str += mLazhus[z].mColor;
                //Debug.Log(str);

                return true;
            }
        }
        return false;
    }

    public void EnabelBtn_AllFlower(bool can_click)
    {
        for (int i = 0; i < mFlowers.Count; i++)
        {
            //mFlowers[i].mBtn.enabled = can_click;
            mFlowers[i].mBox.enabled = can_click;
        }
    }
    public void ShowAllFire(bool is_show)
    {
        for (int i = 0; i < mLazhus.Count; i++)
        {
            mLazhus[i].ShowFire(is_show);
        }
    }

    public bool AllFlowerHaveColor()
    {
        bool result = true;
        for (int i = 0; i < mFlowers.Count; i++)
        {
            if(0 > mFlowers[i].mColor || 2 < mFlowers[i].mColor)
            {
                result = false;
                break;
            }
        }
        return result;
    }

    public bool AllLazhuInCake()
    {
        bool result = true;
        for (int i = 0; i < mLazhus.Count; i++)
        {
            if (0 > mLazhus[i].mColor || 2 < mLazhus[i].mColor)
            {
                result = false;
                break;
            }
        }
        return result;
    }

    public void FlushLazhuEffect()
    {
        for (int i = 0; i < mLazhus.Count; i++)
        {
            if (mLazhus[i].mColor == 3)
                mLazhus[i].ShowEffect(true);
            else
                mLazhus[i].ShowEffect(false);
        }
    }
}
