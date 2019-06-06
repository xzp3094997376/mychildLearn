using UnityEngine;

public class config_tujian
{

    public int m_nID;

    public string m_strName;

    public string m_strIcon;

    public string m_strTxt;

    public string m_strSound1;

    static public config_tujian Convert(string txtStr)
    {

        config_tujian result = new config_tujian();

        string[] datas = txtStr.Split('，');
        for (int i = 0; i < datas.Length; i++ )
        {
            switch (i)
            {
                case 0:
                    {
                        int temp;
                       if(int.TryParse(datas[i], out temp))
                        {
                            result.m_nID = temp;
                        }

                    }
                    break;
                case 1:
                    {
                        result.m_strName = datas[i];

                    }
                    break;
                case 2:
                    {
                        result.m_strIcon = datas[i];

                    }
                    break;
                case 3:
                    {
                        result.m_strTxt = datas[i];

                    }
                    break;
                case 4:
                    {
                        result.m_strSound1 = datas[i];

                    }
                    break;
                default:
                    break;
            }

        }
        return result;
    }
}
