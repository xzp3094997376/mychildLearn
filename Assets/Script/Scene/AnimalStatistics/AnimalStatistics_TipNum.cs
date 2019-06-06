using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimalStatistics_TipNum : MonoBehaviour
{

    public int nNum = 0;

    private Image bg;
    private Image num1;
    private Image num2;

    public RectTransform rt;
    private BoxCollider2D box2D;

    public void InitAwake()
    {
        rt = gameObject.GetComponent<RectTransform>();
        box2D = gameObject.GetComponent<BoxCollider2D>();
        bg = gameObject.GetComponent<Image>();
        bg.sprite = ResManager.GetSprite("animalstatistics_sprite", "xuxianqian");

        bg.rectTransform.sizeDelta = new Vector2(83f, 83f);
        box2D.size = new Vector2(83f, 83f);

        num1 = transform.Find("num1").GetComponent<Image>();
        num2 = transform.Find("num2").GetComponent<Image>();
        num1.color = Color.white;
        num2.color = Color.white;
        HideNumber();
    }

    public void SetBlueBGActive(bool _active)
    {
        bg.enabled = _active;
    }

    public void SetScale(float _scale)
    {
        transform.localScale = Vector3.one * _scale;
    }

    public void ResetInfos()
    {
        bg.sprite = ResManager.GetSprite("animalstatistics_sprite", "xuxianqian");
    }

    public void SetBGSprite(Sprite _sp)
    {
        bg.sprite = _sp;
    }

    /// <summary>
    /// 设置数字
    /// </summary>
    /// <param name="_num"></param>
    public void SetNumber(int _num)
    {
        nNum = _num;
        num1.enabled = false;
        num2.enabled = false;
        if (_num < 10)
        {
            num1.enabled = true;
            num1.rectTransform.anchoredPosition = Vector2.zero;
            int nA = _num;
            num1.sprite = ResManager.GetSprite("animalstatistics_sprite", nA.ToString());
        }
        else
        {
            num1.enabled = true;
            num2.enabled = true;
            num1.rectTransform.anchoredPosition = new Vector2(-15f, 0f);
            num2.rectTransform.anchoredPosition = new Vector2(15f, 0f);
            int nA = _num / 10;
            int nB = _num % 10;
            num1.sprite = ResManager.GetSprite("animalstatistics_sprite", nA.ToString());
            num2.sprite = ResManager.GetSprite("animalstatistics_sprite", nB.ToString());
        }

        num1.SetNativeSize();
        num2.SetNativeSize();
    }

    /// <summary>
    /// 显示数字
    /// </summary>
    public void ShowNumber()
    {
        num1.enabled = false;
        num2.enabled = false;
        if (nNum < 10)
        {
            num1.enabled = true;
            num1.rectTransform.anchoredPosition = Vector2.zero;
        }
        else
        {
            num1.enabled = true;
            num2.enabled = true;
            num1.rectTransform.anchoredPosition = new Vector2(-15f, 0f);
            num2.rectTransform.anchoredPosition = new Vector2(15f, 0f);
        }
    }

    /// <summary>
    /// 隐藏数字
    /// </summary>
    public void HideNumber()
    {
        num1.enabled = false;
        num2.enabled = false;
    }

    public void SetNumColor(Color _color)
    {
        num1.color = _color;
        num2.color = _color;
    }
}
