using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpAnimationScaleXYZ : MonoBehaviour
{
    //スクアットアニメーションの開始からの時間
    float animationTime = 0.0f;

    [SerializeField]
    private bool startAnimation
     = false;

    private Vector3 startTransformScale
     = default;

    [System.Serializable] // Unity側が理解できるようにしてほしいというおまじない
    struct AnimationData // 独自の構造体を用意する
    {
        public float time;
        public Vector3 purposeScale;
    }

    [SerializeField] // Listでもっておいて、SerializeField属性をつけておくと、Inspector上からいじれる
    List<AnimationData> ListAnimation = new List<AnimationData>();

    int drivingIndex = default;

    //[SerializeField]
    //List<float> ListAnimation = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        /* テストコード
        ListAnimation.Add(0.1f);
        ListAnimation.Add(0.5f);
        ListAnimation.Add(0.2f);
        ListAnimation.Add(0.1f);
        ListAnimation.Add(0.5f);
        ListAnimation.Add(0.2f);
        ListAnimation.Add(0.1f);
        ListAnimation.Add(0.5f);
        ListAnimation.Add(0.2f);
        */

        startTransformScale = this.transform.localScale;
    }

    /// <summary>
    /// 指定したIndex番号のアニメーションに至るまでの
    /// すべてのListに登録してあるアニメーションの
    /// 指定稼働時間を足し合わせて取得する
    /// </summary>
    /// <returns></returns>
    float GetHistoryTime(int index)
    {
        float ret = 0.0f;
        for (int i = 0; i <= index; i++)
        {
            ret += ListAnimation[i].time;
        }
        Debug.Log($"GetHistory Time Index:{index} Time:{ret}");
        return ret;
    }

    // Update is called once per frame
    void Update()
    {
        // アニメーション開始をするフラグが立っていないときは
        // 以降の処理を行わない
        if (startAnimation == false) return;

        animationTime += Time.deltaTime;

        //Debug.Log(animationTime);
        /* Updateの中で ListAnimationを全部まわそうとすると、
        うまくうごかない
        for (int i = 0; i < ListAnimation.Count; i++)
        {
            ///
        }
        */
        // リストがどれだけの大きさあるか
        //ListAnimation.Count

        // 現在、アニメーションリストのどの部分を動かしている途中なのか
        if (animationTime <= GetHistoryTime(ListAnimation.Count - 1)
           && drivingIndex < ListAnimation.Count)
        {
            float per = (animationTime - GetHistoryTime(drivingIndex - 1)) 
                                                / ListAnimation[drivingIndex].time;
            Vector3 purpose = ListAnimation[drivingIndex].purposeScale;
            Vector3 before;
            if (drivingIndex == 0)
            {
                before = startTransformScale;
            }
            else
            {
                before = ListAnimation[drivingIndex - 1].purposeScale;
            }

            Debug.Log($" Percentage: {per}");
            if (per >= 1.0f)
            {
                Debug.Log("drivingIndex++");
                drivingIndex++;
                this.transform.localScale = purpose;
            }
            else
            {
                this.transform.localScale = before * (1.0f - per) + purpose * per;
            }
        }
        else
        {
            Debug.Log("Finish Animation");
            drivingIndex = 0;
            startAnimation = false;
            animationTime = 0;
            this.transform.localScale = startTransformScale;
        }

        // 現在のTransformのスケールをいじる

    }
}
