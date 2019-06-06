using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class AS_PaopaoNum : MonoBehaviour
{
    public int nNum = 0;

    private Button btn;
    private miniImageNumber imgNumber;

    private Image img;

    private System.Action<AS_PaopaoNum> mClickCall = null;

	public void InitStart (int _num, System.Action<AS_PaopaoNum> _clickCall = null)
    {
        nNum = _num;
        mClickCall = _clickCall;

        img = UguiMaker.newImage("img", transform, "animalstatistics_sprite", "mflower0");
        img.rectTransform.sizeDelta = Vector2.one * 160f;
        btn = img.gameObject.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        btn.onClick.AddListener(ClickCall);

        imgNumber = UguiMaker.newGameObject("miniNum", transform).AddComponent<miniImageNumber>();
        imgNumber.strABName = "animalstatistics_sprite";
        imgNumber.strFirstPicName = "";
        imgNumber.SetNumber(_num);
        imgNumber.SetNumColor(Color.black);       
    }

    public void SetPos(Vector3 vLocalPos)
    {
        transform.localPosition = vLocalPos;
        transform.localScale = Vector3.one * 0.001f;
        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
    }

    private void ClickCall()
    {
        if (mClickCall != null)
        {
            mClickCall(this);
        }
    }

    public void BtnActive(bool _active)
    {
        img.raycastTarget = _active;
    }
}
