using UnityEngine;

public class config_apk 
{

    public int m_apk_define;

    public string m_desc1;

    public string m_sound_name;

    public int m_last_guanka;

    public string m_backup;

    static public config_apk Convert(string txtStr)
    {

        config_apk result = new config_apk();

        string[] datas = txtStr.Split('，');
        for (int i = 0; i < datas.Length; i++)
        {
            switch (i)
            {
                case 0:
                    {
                        int temp;
                        if (int.TryParse(datas[i], out temp))
                        {
                            result.m_apk_define = temp;
                        }

                    }
                    break;
                case 1:
                    {
                        result.m_desc1 = datas[i];

                    }
                    break;
                case 2:
                    {
                        result.m_sound_name = datas[i];

                    }
                    break;
                case 3:
                    {
                        int temp;
                        if (int.TryParse(datas[i], out temp))
                        {
                            result.m_last_guanka = temp;
                        }

                    }
                    break;
                case 4:
                    {
                        result.m_backup = datas[i];

                    }
                    break;
                default:
                    break;
            }

        }
        return result;
    }

}
