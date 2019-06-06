using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimalPartyHand : MonoBehaviour
{
    public static ParticleSystem mEffectPaopao { get; set; }

    public int mId { get; set; }
    public int mSide { get; set; }
    public bool mSelect { get; set; }
    public Image mImage { get; set; }
    public RectTransform mRtran { get; set; }
    

    public void SetData(int hand_id, int side)
    {
        if(null == mEffectPaopao)
        {
            mEffectPaopao = UguiMaker.newParticle("effect_paopao", AnimalPartyCtl.instance.mGuankaCtl3.transform, Vector3.zero, Vector3.one, "effect_paopao0", "effect_paopao0");
        }
        if(null == mRtran)
        {
            mRtran = gameObject.GetComponent<RectTransform>();

            mImage = UguiMaker.newImage("hand", transform, "animalparty_sprite", "hand_clean" + hand_id);
            Button btn = mImage.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.onClick.AddListener(OnClkHand);

        }

        mId = hand_id;
        mSide = side;
        mSelect = false;
        SetImage(mSelect);
        SetButtonEnable(true);
    }

    public void SetImage(bool clean)
    {
        if(clean)
        {
            mImage.sprite = ResManager.GetSprite("animalparty_sprite", "hand_clean" + mId);
            mImage.transform.localScale = Vector3.one;
        }
        else
        {
            mImage.sprite = ResManager.GetSprite("animalparty_sprite", "hand_dirty" + mId);
            mImage.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        }
    }
    public void SetButtonEnable(bool _enable)
    {
        mImage.GetComponent<Button>().enabled = _enable;
    }

    public void OnClkHand()
    {
        mSelect = !mSelect;
        Global.instance.PlayBtnClickAnimation(transform);
        SetImage(mSelect);
        if (mSelect)
        {
            mEffectPaopao.GetComponent<RectTransform>().anchoredPosition3D = mRtran.anchoredPosition3D;
            mEffectPaopao.Play();
            AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "泡泡");
        }
        else
        {
            AnimalPartyCtl.instance.mSound.PlayShort("animalparty_sound", "变肮脏音效");
        }
    }

}
