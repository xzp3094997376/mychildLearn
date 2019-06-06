using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BirthdayCakePaopao : MonoBehaviour
{
    public static BirthdayCakePaopao gSelect { get; set; }
    public static ParticleSystem gEffect { get; set; }

    public bool mIsControl { get; set; }
    public int mColor { get; set; }
    public Image mImage { get; set; }
    public BoxCollider mBox { get; set; }

    void OnDestroy()
    {
        gEffect = null;
    }
    public void Shoot(int color, Vector3 world_pos)
    {
        if(null == mBox)
        {
            mBox = gameObject.AddComponent<BoxCollider>();
            mBox.size = new Vector3(64, 64, 1);
        }
        if(null == mImage)
        {
            mImage = UguiMaker.newImage("image", transform, "birthdaycake_sprite", "paopao0");
        }

        mColor = color;
        mImage.sprite = ResManager.GetSprite("birthdaycake_sprite", "paopao" + color.ToString());
        transform.position = world_pos;

        StopAllCoroutines();
        StartCoroutine("TPhysics");

    }
    public void Kill()
    {
        if(null == gEffect)
        {
            gEffect = ResManager.GetPrefab("birthdaycake_prefab", "paopao_effect").GetComponent<ParticleSystem>();
            UguiMaker.InitGameObj(gEffect.gameObject, BirthdayCakeCtl.instance.transform, "effect", Vector3.zero, Vector3.one);
        }
        gEffect.transform.position = transform.position;
        gEffect.Emit(10);
        gameObject.SetActive(false);

        BirthdayCakeCtl.instance.mSound.PlayShort("birthdaycake_sound", "click_paopao");
    }

    float f = 0.99f;//0.007f;//阻力
    int a_num = 0;//加速度的次数
    Vector3 a = Vector3.zero;//加速度
    Vector3 speed = Vector3.zero;
    IEnumerator TPhysics()
    {
        //float timecount = 0;
        Vector3 sub_angle = new Vector3(0, 0, Random.Range(-4, 4));
        Vector3 pos0 = transform.localPosition;
        Vector3 pos1 = pos0 + new Vector3(Random.Range(-50, 50), Random.Range(120, 200), 0);
        for(float i = 0; i < 1f; i += 0.025f)
        {
            transform.localPosition = Vector3.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * i));
            transform.localScale = Vector3.Lerp(new Vector3(0.5f, 0.5f, 1), Vector3.one, i);
            //transform.localEulerAngles += sub_angle;
            if (mIsControl)
                break;
            yield return new WaitForSeconds(0.01f);
        }
        transform.SetParent(BirthdayCakeCtl.instance.mPaopaoParentFront.transform);

        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            if (mIsControl) continue;

            //加速
            if (a_num > 0)
            {
                speed = speed + a;
                a_num--;
            }
            else
            {
                float ax, ay;
                if (transform.localPosition.x < 0)
                    ax = Random.Range(-0.5f, 1f);
                else
                    ax = Random.Range(-1f, 0.5f);

                if (transform.localPosition.y < 0)
                    ay = Random.Range(-0.5f, 1f);
                else
                    ay = Random.Range(-1f, 0.5f);

                a = new Vector3(ax, ay, 0).normalized * 0.02f;

                //从新生成a
                //a = new Vector3(
                //       Random.Range(-1f, 1f),
                //       Random.Range(-1f, 1f),
                //       0
                //       ).normalized * 0.03f;

                a_num = Random.Range(10, 80);
            }

            //阻力
            speed = speed * f;
            transform.localPosition += speed;
            //transform.localEulerAngles += sub_angle;

            if(transform.localPosition.y > 350 || transform.localPosition.y < -278 || transform.localPosition.x < -511 || transform.localPosition.x > 462)
            {
                gameObject.SetActive(false);
            }
        }

    }

}
