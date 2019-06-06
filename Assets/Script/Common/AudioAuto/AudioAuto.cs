using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;


#if OPEN_MIC
public class AudioAuto : MonoBehaviour
{
    public static AudioAuto instance = null;
    public static void Init()
    {
        if(null == instance)
        {
            GameObject obj = new GameObject();
            obj.name = "AudioAuto";
            instance = obj.AddComponent<AudioAuto>();

            obj.AddComponent<AudioSource>().loop = false;

        }
    }

    List<AudioClip> clips;
    List<float> volumes;

    //录音的采样率
    const int samplingRate = 44100;
    const int audio_lenth = 1;
    float sound_mid = 4;


    void Start()
    {
        string[] micDevices = Microphone.devices;
        if (micDevices.Length == 0)
        {
            Debug.Log("没有找到录音组件");
        }
        else
        {
            StartCoroutine("TAuto");
        }

        if(Application.platform == RuntimePlatform.Android)
        {
            sound_mid = 80;
        }

    }
    //void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Z))
    //    {
    //        Debug.Log("开始录音");
    //        Recording();
    //    }

    //    if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        Debug.Log("停止录音");
    //        StopRecord();
    //    }

    //    if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        Debug.Log("播放录音");
    //        PlayRecord();
    //    }

    //    if (Input.GetKeyDown(KeyCode.V))
    //    {
    //        Debug.Log("保存录音");
    //        //SaveRecord();
    //    }

    //}

    void ListCreate()
    {
        clips = new List<AudioClip>();
        volumes = new List<float>();
    }
    void ListRemoveAt(int index)
    {
        clips.RemoveAt(index);
        volumes.RemoveAt(index);
    }
    void ListAdd(float volume, AudioClip clip)
    {
        volumes.Add(volume);
        clips.Add(clip);
        //Debug.Log("ListAdd,volume=" + volume + " clip=" + clip);
    }
    void ListReplace(float volume, AudioClip clip, int index)
    {
        volumes[index] = volume;
        clips[index] = clip;
    }
    void ListClean()
    {
        if(volumes != null)
        {
            volumes.Clear();
        }
        if(null != clips)
        {
            for (int i = 0; i < clips.Count; i++)
            {
                Destroy(clips[i]);
            }
            clips.Clear();
        }
    }

    public void PlayClip(AudioClip c)
    {
        gameObject.GetComponent<AudioSource>().clip = c;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void Callback_Wav(string msg)
    {
        ScreenDebug.Log("安卓反馈语音识别：" + msg);
    }

    
    IEnumerator TAuto()
    {
        //Debug.LogError(DateTime.Now.ToString("HH:mm:ss  M月d日", System.Globalization.DateTimeFormatInfo.InvariantInfo) + ".wav");
        ListClean();
        ListCreate();
        List<AudioClip> file_clip = new List<AudioClip>();
        float volume = 0;
        while (true)
        {
            AudioClip clip = Recording();
            yield return new WaitForSeconds(1f);
            
            volume = 0;
            int clip_len = clip.samples * clip.channels;
            float[] samples = new float[clip_len];
            if(clip.GetData(samples, 0))
            {
                for (int i = 0; i < samples.Length; i++)
                {
                    volume += Mathf.Abs(samples[i]);
                }
                volume = (int)(1000 * volume / samples.Length);
                //Debug.LogError(volume);
                ScreenDebug.Log(volume.ToString());

                /*
                if(volume > sound_mid)
                {

                    string wav_name = DateTime.Now.ToString("HH-mm-ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + ".wav";
                    SaveRecord(wav_name, clip);
                    yield return new WaitForSeconds(0.5f);
                    PlayClip(clip);

                }
                continue;
                */





                if(clips.Count == 0)
                {
                    //原本是空的
                    ListAdd(volume, clip);
                }
                else
                {
                    int state = 0;//1-还没开始，2-开始中，3-终结
                    if(1 == volumes.Count)
                    {
                        state = 1;
                    }
                    else
                    {
                        state = 2;
                    }

                    if (volume < sound_mid)
                    {
                        if(1 == state)
                        {
                            //替换第一个无声开头
                            ListReplace(volume, clip, 0);
                        }
                        else if(2 == state)
                        {
                            //结束录音,标记
                            ListAdd(volume, clip);
                        }
                    }
                    else
                    {
                        ListAdd(volume, clip);
                    }

                    int clip_process = 0;//0在匹配开头，1在匹配录音，2在匹配结束
                    file_clip.Clear();
                    //Debug.Log("state=" + state + " clips.count=" + clips.Count + " volumes.count=" + volumes.Count + " file_clip.count=" + file_clip.Count);
                    for (int i = 0; i < volumes.Count; i++)
                    {
                        switch (clip_process)
                        {
                            case 0:
                                {
                                    if (volumes[i] < sound_mid)
                                    {
                                        file_clip.Insert(0, clips[i]);
                                    }
                                    else
                                    {
                                        file_clip.Add(clips[i]);
                                        clip_process = 1;
                                    }
                                }
                                break;
                            case 1:
                                {
                                    //Debug.Log("clip_process=1");
                                    if (volumes[i] < sound_mid)
                                    {
                                        file_clip.Add(clips[i]);
                                        clip_process = 2;
                                    }
                                    else
                                    {
                                        file_clip.Add(clips[i]);
                                    }
                                }
                                break;
                            case 2:
                                {
                                    //Debug.Log("clip_process=2");
                                    if (file_clip.Count == 3 || file_clip.Count == 4)
                                    {
                                        string wav_name = DateTime.Now.ToString("HH-mm-ss_M-d", System.Globalization.DateTimeFormatInfo.InvariantInfo) + ".wav";
                                        AudioClip temp_clip = Combine(file_clip);
                                        
                                        SaveRecord(wav_name, temp_clip);
                                        ListClean();
                                        yield return new WaitForSeconds(0.5f);
                                        string path = PathTool.GetAudioAutoWavPath() + wav_name;
                                        if(Application.platform == RuntimePlatform.Android)
                                        {

                                            AndroidDataCtl.DoAndroidFunc("recognizeRecord", path);
                                        }

                                        ScreenDebug.Log("发送给安卓->保存路径=" + path);
                                        //Debug.Log("保存路径:" + path);

                                        PlayClip(temp_clip);

                                    }
                                    else if(file_clip.Count > 4)
                                    {
                                        ListClean();
                                    }
                                }
                                break;

                        }


                    }
                }
                
            }
            else
            {
                Debug.LogError("错误！：获取录音数据音量错误！AudioAuto");
            }


        }

    }








    //合成音频
    public static AudioClip Combine(List<AudioClip> clips)
    {
        if (clips == null || clips.Count == 0)
            return null;

        int channels = clips[0].channels;
        int frequency = clips[0].frequency;
        for (int i = 1; i < clips.Count; i++)
        {
            if (clips[i].channels != channels || clips[i].frequency != frequency)
            {
                Debug.LogError("错误:clips[i].channels != channels || clips[i].frequency != frequency");
                return null;
            }
        }

        try
        {
            List<float> list = new List<float>();
            for (int i = 0; i < clips.Count; i++)
            {
                if (clips[i] == null)
                    continue;


                int clip_len = clips[i].samples * clips[i].channels;
                float[] buffer = new float[clip_len];

                //clips[i].LoadAudioData();
                
                clips[i].GetData(buffer, 0);
                for(int j = 0; j < buffer.Length; j++)
                {
                    list.Add(buffer[j]);
                }
                //memoryStream.Write(buffer, 0, buffer.Length);
            }

            float[] bytes = list.ToArray();//memoryStream.ToArray();
            AudioClip result = AudioClip.Create("Combine", bytes.Length / 4 / channels * clips.Count, channels, frequency, false);
            result.SetData(bytes, 0);
            return result;
        }
        catch
        {
            Debug.LogError("合成音频失败");
            return null;
        }
    }

    



    /// <summary>
    /// 开始录音
    /// </summary>
    public AudioClip Recording()
    {
        //Debug.Log("录音时长为" + audio_lenth + "秒");
        Microphone.End(null);//录音前先停掉录音，录音参数为null时采用的是默认的录音驱动
        return Microphone.Start(null, false, audio_lenth, samplingRate);
    }

    /// <summary>
    /// 停止录音
    /// </summary>
    public void StopRecord()
    {
        int audioLength;
        int lastPos = Microphone.GetPosition(null);
        if (Microphone.IsRecording(null))
        {
            audioLength = lastPos / samplingRate;
        }
        else
        {
            audioLength = audio_lenth;
        }

        Microphone.End(null);

        if (audioLength < 1.0f)
        {
            Debug.Log("录音时长短");
        }
        Debug.Log("录音时长=" + audioLength);
    }

    /// <summary>
    /// 播放录音
    /// </summary>
    public void PlayRecord()
    {
        StopRecord();
        if(null != clips && 0 < clips.Count)
            AudioSource.PlayClipAtPoint(clips[0], Vector3.zero);
    }

    /// <summary>
    /// 保存录音, wav_name需要带后缀
    /// </summary>
    public void SaveRecord(string wav_name, AudioClip clip)
    {
        string path = PathTool.GetAudioAutoWavPath() + wav_name;

        try
        {
            Save(clip, path);
            ScreenDebug.Log("保存完毕");
        }
        catch (Exception ex)
        {
            ScreenDebug.Log(ex.Message + ex.StackTrace);
        }
    }

    public static void Save(AudioClip clip, string path)
    {
        string filePath = Path.GetDirectoryName(path);
        Debug.LogError("filePath=" + filePath);
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        using (FileStream fileStream = CreateEmpty(path))
        {
            ConvertAndWrite(fileStream, clip);
            WriteHeader(fileStream, clip);
        }

    }

    private static void ConvertAndWrite(FileStream fileStream, AudioClip clip)
    {

        float[] samples = new float[clip.samples];

        clip.GetData(samples, 0);

        Int16[] intData = new Int16[samples.Length];

        Byte[] bytesData = new Byte[samples.Length * 2];

        int rescaleFactor = 32767; //to convert float to Int16  

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            Byte[] byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }
        fileStream.Write(bytesData, 0, bytesData.Length);
    }
    /*
    private static void ConvertAndWrite(FileStream fileStream, List<AudioClip> clips)
    {
        List<float[]> samples_array = new List<float[]>();
        for(int i = 0; i < clips.Count; i++)
        {
            samples_array.Add(new float[clips[i].samples]);
            clips[i].GetData(samples_array[i], 0);


            Int16[] intData = new Int16[samples_array[i].Length];
            Byte[] bytesData = new Byte[samples_array[i].Length * 2];

            int rescaleFactor = 32767; //to convert float to Int16  

            for (int j = 0; j < samples_array[i].Length; j++)
            {
                intData[i] = (short)(samples_array[i][j] * rescaleFactor);
                Byte[] byteArr = new Byte[2];
                byteArr = BitConverter.GetBytes(intData[i]);
                byteArr.CopyTo(bytesData, i * 2);
                
            }

        }
        //fileStream.Write(bytesData, 0, bytesData.Length);



        //float[] samples = new float[clips[0].samples];
        //clip.GetData(samples, 0);

        //Int16[] intData = new Int16[samples.Length];
        //Byte[] bytesData = new Byte[samples.Length * 2];

        //int rescaleFactor = 32767; //to convert float to Int16  

        //for (int i = 0; i < samples.Length; i++)
        //{
        //    intData[i] = (short)(samples[i] * rescaleFactor);
        //    Byte[] byteArr = new Byte[2];
        //    byteArr = BitConverter.GetBytes(intData[i]);
        //    byteArr.CopyTo(bytesData, i * 2);
        //}
        //fileStream.Write(bytesData, 0, bytesData.Length);
    }
    */

    private static FileStream CreateEmpty(string filepath)
    {
        FileStream fileStream = new FileStream(filepath, FileMode.Create);
        byte emptyByte = new byte();

        for (int i = 0; i < 44; i++) //preparing the header  
        {
            fileStream.WriteByte(emptyByte);
        }

        return fileStream;
    }
    private static void WriteHeader(FileStream stream, AudioClip clip)
    {
        int hz = clip.frequency;
        int channels = clip.channels;
        int samples = clip.samples;

        stream.Seek(0, SeekOrigin.Begin);

        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        stream.Write(riff, 0, 4);

        Byte[] chunkSize = BitConverter.GetBytes(stream.Length - 8);
        stream.Write(chunkSize, 0, 4);

        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        stream.Write(wave, 0, 4);

        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        stream.Write(fmt, 0, 4);

        Byte[] subChunk1 = BitConverter.GetBytes(16);
        stream.Write(subChunk1, 0, 4);

        UInt16 two = 2;
        UInt16 one = 1;

        Byte[] audioFormat = BitConverter.GetBytes(one);
        stream.Write(audioFormat, 0, 2);

        Byte[] numChannels = BitConverter.GetBytes(channels);
        stream.Write(numChannels, 0, 2);

        Byte[] sampleRate = BitConverter.GetBytes(hz);
        stream.Write(sampleRate, 0, 4);

        Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2  
        stream.Write(byteRate, 0, 4);

        UInt16 blockAlign = (ushort)(channels * 2);
        stream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

        UInt16 bps = 16;
        Byte[] bitsPerSample = BitConverter.GetBytes(bps);
        stream.Write(bitsPerSample, 0, 2);

        Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
        stream.Write(datastring, 0, 4);

        Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
        stream.Write(subChunk2, 0, 4);

    }

}
#endif

