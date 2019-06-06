
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#if OPEN_MIC
public class AudioMicrophone : MonoBehaviour
{
    static string[] micArray = null;
    public static AudioMicrophone instance = null;
    public static void Init()
    {
        if (null != instance) return;
        GameObject obj = new GameObject();
        obj.transform.position = Vector3.zero;
        obj.AddComponent<AudioMicrophone>();
    }
    public static void Kill()
    {
        if(null != instance)
        {
            Destroy(instance.gameObject);
        }
        instance = null;

    }

    System.Action m_callback;
    System.Action<float> m_callback_volume;
    public AudioClip mClip { get; set; }
    int RECORD_TIME = 0;

    void Awake()
    {
        instance = this;
        if (null == micArray)
        {
            micArray = Microphone.devices;
            if (micArray.Length == 0)
            {
                Debug.LogError("Microphone.devices is null");
            }
            foreach (string deviceStr in Microphone.devices)
            {
                Debug.Log("device name = " + deviceStr);
            }
            if (micArray.Length == 0)
            {
                Debug.LogError("no mic device");
            }
        }
    }
	void Destroy()
    {
        instance = null;
    }

    //录音
    public void StartRecord(int record_time, System.Action callback)
    {
        if (micArray.Length == 0)
        {
            Debug.Log("No Record Device!");
            return;
        }
        m_callback = callback;
        RECORD_TIME = record_time;
        mClip = Microphone.Start("Built -in Microphone", false, record_time, 44100);
        StartCoroutine(TimeDown());
        //倒计时  

    }
    public void StopRecord()
    {
        if (micArray.Length == 0)
        {
            Debug.Log("No Record Device!");
            return;
        }
        if (!Microphone.IsRecording(null))
        {
            return;
        }
        Microphone.End(null);

        Debug.Log("StopRecord");

    }
    private IEnumerator TimeDown()
    {
        Debug.Log(" IEnumerator TimeDown()");

        int time = 0;
        while (time < RECORD_TIME)
        {
            if (!Microphone.IsRecording(null))
            { //如果没有录制  
                Debug.Log("IsRecording false");
                yield break;
            }
            Debug.Log("yield return new WaitForSeconds " + time);
            yield return new WaitForSeconds(1);
            time++;
        }
        StopRecord();
        m_callback();
        yield return 0;
    }

    //检测音量大小
    public void BeginCheekVolume(System.Action<float> callback_volume)
    {
        if (micArray.Length == 0)
        {
            Debug.Log("No Record Device!");
            return;
        }
        m_callback_volume = callback_volume;
        StopCoroutine("TBeginCheekVolume");
        StartCoroutine("TBeginCheekVolume");
    }
    public void StopCheekVolume()
    {
        StopCoroutine("TBeginCheekVolume");
    }

    IEnumerator TBeginCheekVolume()
    {
        int sub_time = 1;
        float volume = 0;

        mClip = Microphone.Start("Built -in Microphone", true, sub_time, 44100);
        while (true)
        {
            yield return new WaitForSeconds(sub_time);
            if (null == mClip)
            {
                continue;
            }

            int clip_len = AudioMicrophone.instance.mClip.samples * AudioMicrophone.instance.mClip.channels;
            float[] samples = new float[clip_len];
            mClip.GetData(samples, 0);

            volume = 0;
            for (int i = 0; i < samples.Length; i++)
            {
                volume += Mathf.Abs(samples[i]);
            }

            volume = (int)(1000 * volume / samples.Length);

            if (null != m_callback_volume)
                m_callback_volume(volume);
            
            
        }

    }
    

}
#endif

