using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class M_Notification
{
    public Component sender;
    public string name;
    public object data;

    public M_Notification(Component _sender,string _name)
    { sender = _sender;name = _name;data = null;}
    public M_Notification(Component _sender,string _name,object _data)
    { sender = _sender;name = _name;data = _data;}
}

public delegate void OnNotificationDelegate(M_Notification note);

public class NotificationCenter : MonoBehaviour
{

    private static NotificationCenter instance = null;

    public static NotificationCenter GetInstance()
    {
        if (instance == null)
        {
            instance = GameObject.FindObjectOfType<NotificationCenter>();
            if (instance == null)
            {
                instance = Global.instance.gameObject.AddComponent<NotificationCenter>();
            }  
        }
        return instance;
    }


    private Dictionary<string, OnNotificationDelegate> eventListerners = new Dictionary<string, OnNotificationDelegate>();


    /// <summary>
    /// 添加监听
    /// </summary>
    public void addEventListerner(string _type, OnNotificationDelegate _listener)
    {
        if (!eventListerners.ContainsKey(_type))
        {
            OnNotificationDelegate deleg = null;
            eventListerners[_type] = deleg;
        }
        eventListerners[_type] += _listener;
    }


    /// <summary>
    /// 移除监听
    /// </summary>
    public void removeEventListerner(string _type, OnNotificationDelegate _listener)
    {
        if (!eventListerners.ContainsKey(_type))
        {
            return;
        }
        eventListerners[_type] -= _listener;
    }
    /// <summary>
    /// 移除监听
    /// </summary>
    public void removeEventListerner(string _type)
    {
        if (eventListerners.ContainsKey(_type))
        {
            eventListerners.Remove(_type);
        }
    }


    /// <summary>
    /// 派发数据
    /// </summary>
    public void dispatchEvent(string _type, M_Notification note)
    {
        if (eventListerners.ContainsKey(_type))
        {
            eventListerners[_type](note);
        }
    }

    /// <summary>
    /// 派发数据
    /// </summary>
    public void dispatchEvent(string _type)
    {
        dispatchEvent(_type, null);
    }


}
