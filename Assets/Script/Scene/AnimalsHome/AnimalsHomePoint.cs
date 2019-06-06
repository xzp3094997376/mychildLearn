using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimalsHomePoint : MonoBehaviour
{

    private Button btn;
    private System.Action<GameObject> clickCall = null;

    public void InitAwake()
    {
        btn = gameObject.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        EventTriggerListener.Get(gameObject).onClick = ClickBtn;
    }

    private void ClickBtn(GameObject _go)
    {
        if (clickCall != null)
        {
            clickCall(gameObject);
        }
    }

    public void SetClickCall(System.Action<GameObject> _call)
    {
        clickCall = _call;
    }

    public void ButtonActive(bool _active)
    {
        gameObject.GetComponent<Image>().raycastTarget = _active;
    }

}
