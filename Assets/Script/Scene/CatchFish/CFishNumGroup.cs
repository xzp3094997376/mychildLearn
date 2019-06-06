using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class CFishNumGroup : MonoBehaviour
{

    public CFishNumObj mLeftNum;
    public CFishNumObj mRightNum;

    private BoxCollider2D box2d;
    private Image imgrect;

    public void InitAwake(CFishNumObj _leftNum, CFishNumObj _rightNum)
    {
        transform.localPosition = new Vector3(_leftNum.transform.localPosition.x, 0f, 0f);
        _leftNum.transform.SetParent(transform);
        _rightNum.transform.SetParent(transform);

        box2d = gameObject.AddComponent<BoxCollider2D>();
        box2d.size = new Vector2(_leftNum.GetSize().x, _leftNum.GetSize().y * 2f);

        _leftNum.BoxActive(false);
        _rightNum.BoxActive(false);

        mLeftNum = _leftNum;
        mRightNum = _rightNum;

        imgrect = UguiMaker.newImage("imgrect", transform, "catchfish_sprite", "rect1", false);
        imgrect.enabled = false;

        
    }


    /// <summary>
    /// 光标显示/隐藏
    /// </summary>
    /// <param name="_active"></param>
    public void ImgRectActive(bool _active)
    {
        imgrect.enabled = _active;
    }

    /// <summary>
    /// box2d active
    /// </summary>
    /// <param name="_active"></param>
    public void BoxActive(bool _active)
    {
        box2d.enabled = _active;
    }

    /// <summary>
    /// to改变位置
    /// </summary>
    /// <param name="_vto"></param>
    public void ChangeToPos(Vector3 _vto)
    {
        BoxActive(false);
        transform.DOLocalMove(_vto, 0.5f).SetDelay(0.2f).OnComplete(() =>
        {
            ImgRectActive(false);
            BoxActive(true);
        });
    }




}
