using UnityEngine;

public class config_game
{

    public string m_game_enum;

    public string m_game_name;

    public string m_班级;

    public string m_module;

    public int m_module_index;

    public int m_all_guanka;

    static public config_game Convert(string txtStr)
    {

        config_game result = new config_game();

        string[] datas = txtStr.Split('，');
        for (int i = 0; i < datas.Length; i++ )
        {
            switch (i)
            {
                case 0:
                    {
                        result.m_game_enum = datas[i];

                    }
                    break;
                case 1:
                    {
                        result.m_game_name = datas[i];

                    }
                    break;
                case 2:
                    {
                        result.m_班级 = datas[i];

                    }
                    break;
                case 3:
                    {
                        result.m_module = datas[i];

                    }
                    break;
                case 4:
                    {
                        int temp;
                       if(int.TryParse(datas[i], out temp))
                        {
                            result.m_module_index = temp;
                        }

                    }
                    break;
                case 5:
                    {
                        int temp;
                       if(int.TryParse(datas[i], out temp))
                        {
                            result.m_all_guanka = temp;
                        }

                    }
                    break;
                default:
                    break;
            }

        }
        return result;
    }
}
