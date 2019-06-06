using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AnimalPartyGuanka3 : MonoBehaviour
{
    public RectTransform mRtran { get; set; }

    Image mBg3 { get; set; }
    Image mBg4 { get; set; }

    List<AnimalPartyHand> mHands = null;
    Image mOKBtn { get; set; }
    ParticleSystem mEffectOK { get; set; }

    public int guanka_find_side = -1;

    public void Init()
    {

        if (null == mBg3)
        {
            mRtran = GetComponent<RectTransform>();

            mBg3 = UguiMaker.newImage("mBg3", transform, "animalparty_sprite", "bg3", false);
            mBg3.rectTransform.sizeDelta = new Vector2(1423, 800);
            mBg3.type = Image.Type.Tiled;

            mBg4 = UguiMaker.newImage("mBg4", transform, "animalparty_sprite", "bg4", false);
            mBg4.rectTransform.sizeDelta = new Vector2(1423, 800);
            mBg4.rectTransform.anchoredPosition = new Vector2(0, -35);
            mBg4.rectTransform.sizeDelta = new Vector2(1164, 681);
            mBg4.type = Image.Type.Sliced;


            mHands = new List<AnimalPartyHand>();
            for(int i = 0; i < 8; i++)
            {
                mHands.Add(UguiMaker.newGameObject("hand", transform).AddComponent<AnimalPartyHand>());
            }

            mOKBtn = UguiMaker.newImage("okbtn", transform, "animalparty_sprite", "btn_ok_up");
            mOKBtn.rectTransform.anchoredPosition = new Vector2(557, -327);
            Button btn = mOKBtn.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.onClick.AddListener(OnClkOk);

            mEffectOK = UguiMaker.newParticle("effect_ok", mOKBtn.transform, Vector3.zero, Vector3.one, "effect_okbtn", "okbtn_effect");

        }

        mOKBtn.sprite = ResManager.GetSprite("animalparty_sprite", "btn_ok_up");
        mOKBtn.GetComponent<Button>().enabled = true;

        List <int> ids = Common.GetMutexValue(0, 9, 8);
        List<Vector3> poss0 = Common.PosSortByWidth(1090, 4, 115);
        List<Vector3> poss1 = Common.PosSortByWidth(1090, 4, -190);
        int[] sides = new int[] { 1, -1, -1, -1, -1, -1, 1, 1, 1, 1 };
        for(int i = 0; i < mHands.Count; i++)
        {
            mHands[i].SetData(ids[i], sides[ids[i]]);
            if (i < 4)
                mHands[i].mRtran.anchoredPosition3D = poss0[i];
            else
                mHands[i].mRtran.anchoredPosition3D = poss1[i - 4];


        }

        guanka_find_side = Random.Range(0, 1000) % 2 == 0 ? -1 : 1;
        if (-1 == guanka_find_side)
        {
            Debug.LogError("请找出所有的左手");

        }
        else
        {
            Debug.LogError("请找出所有的右手");

        }

    }
    public void PlayTip()
    {
        if (-1 == guanka_find_side)
        {
            AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-tips6-2-39", 1, true);

        }
        else
        {
            AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-tips6-2-40", 1, true);

        }

    }
    public void OnClkOk()
    {
        StartCoroutine("TGameOver");
    }
    IEnumerator TGameOver()
    {
        mOKBtn.sprite = ResManager.GetSprite("animalparty_sprite", "btn_ok_down");
        mOKBtn.GetComponent<Button>().enabled = false;
        AnimalPartyCtl.instance.mSound.PlayShort("button_down");
        yield return new WaitForSeconds(0.3f);
        mOKBtn.sprite = ResManager.GetSprite("animalparty_sprite", "btn_ok_up");
        AnimalPartyCtl.instance.mSound.PlayShort("button_up");

        bool correct = true;
        for(int i = 0; i < mHands.Count; i++)
        {
            if(guanka_find_side == mHands[i].mSide)
            {
                if(!mHands[i].mSelect)
                {
                    correct = false;
                    break;
                }
            }
            else
            {
                if(mHands[i].mSelect)
                {
                    correct = false;
                    break;
                }
            }
        }

        if(correct)
        {
            for (int i = 0; i < mHands.Count; i++)
            {
                mHands[i].SetButtonEnable(false);
            }

            TopTitleCtl.instance.AddStar();
            mEffectOK.Play();
            AnimalPartyCtl.instance.mSound.PlayShort("gameover_show");
            yield return new WaitForSeconds(2);
            GameOverCtl.GetInstance().Show(4, AnimalPartyCtl.instance.Reset);
            Debug.LogError("嗯嗯，你做的很好！");
            AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-right6-1-2");
        }
        else
        {
            mOKBtn.GetComponent<Button>().enabled = true;
            Debug.LogError("再照着这些手势学一学？有一些好像不对呢！");
            AnimalPartyCtl.instance.mSound.PlayTip("animalparty_sound", "game-right6-1-4");
        }

    }



}
