using UnityEngine;
using System.Collections;
using Spine.Unity;

public class AnimalNumOnlyAnimal : MonoBehaviour
{
    RectTransform mRtran { get; set; }
    SkeletonGraphic mSpine { get; set; }
    int data_animal_id { get; set; }

    public void Init(int animal_id)
    {
        data_animal_id = animal_id;
        mRtran = gameObject.GetComponent<RectTransform>();
        mSpine = transform.Find("spine").GetComponent<SkeletonGraphic>();
        transform.localScale = new Vector3(0.5f, 0.5f, 1);
    }

    public void Play(string anim)
    {
        mSpine.AnimationState.SetAnimation(0, anim, true);
    }

    public void Jump()
    {
        //Debug.Log("Jump()");
        gameObject.SetActive(true);
        StartCoroutine("TJump");
    }

    IEnumerator TJump()
    {
        yield return new WaitForSeconds(Random.Range(0f, 1f));
        AnimalNumOnlyCtl.instance.mSound.PlayShort("aa_animal_sound", MDefine.GetAnimalNameByID_CH(data_animal_id) + "0");
        AnimalNumOnlyCtl.instance.mSound.PlayShortDefaultAb( "动物上飞");

        
        Vector3 pos0 = new Vector3( Random.Range(-500, 500), -550, 0);
        float speed_p = 0.01f;// * (1 + Random.Range(0, 1000) % 2);
        float speed_x = Random.Range(0, 5) * (pos0.x < 0 ? 1 : -1);
        float hight = Random.Range(400, 700);
        int countx = 0;
        for(float i = 0; i < 1f; i +=  speed_p)
        {
            mRtran.anchoredPosition3D = pos0 + new Vector3(countx * speed_x, Mathf.Sin(Mathf.PI * i) * hight, 0);
            transform.localEulerAngles += new Vector3(0, 0, 2);
            countx++;
            yield return new WaitForSeconds(0.01f);
        }

    }


}
