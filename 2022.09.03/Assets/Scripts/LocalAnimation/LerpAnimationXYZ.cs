using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpAnimationXYZ : MonoBehaviour
{
    //スクアットアニメーションの開始からの時間
    float animationTime = 0.0f;

    [SerializeField]
    private bool startAnimation
     = false;

    private Vector3 startTransformPosition
     = default;
    private Quaternion startTransformRotation
     = default;
    private Vector3 startTransformScale
     = default;

    [System.Serializable] // Unity側が理解できるようにしてほしいというおまじない
    struct AnimationData // 独自の構造体を用意する
    {
        public float time;
        public Vector3 purposePosition;
        public Vector3 purposeRotation;
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

        startTransformPosition = this.transform.localPosition;
        startTransformRotation = this.transform.localRotation;
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

            Vector3 purposePositon = ListAnimation[drivingIndex].purposePosition;
            Vector3 beforePosition;
            Vector3 purposeRotation = ListAnimation[drivingIndex].purposeRotation;
            Vector3 beforeRotation;
            Vector3 purposeScale = ListAnimation[drivingIndex].purposeScale;
            Vector3 beforeScale;

            if (drivingIndex == 0)
            {
                beforePosition = startTransformPosition;
                beforeRotation = startTransformRotation.eulerAngles;
                beforeScale = startTransformScale;
            }
            else
            {
                beforePosition = ListAnimation[drivingIndex - 1].purposePosition;
                beforeRotation = ListAnimation[drivingIndex - 1].purposeRotation;
                beforeScale = ListAnimation[drivingIndex - 1].purposeScale;
            }

            Debug.Log($" Percentage: {per}");
            if (per >= 1.0f)
            {
                Debug.Log("drivingIndex++");
                drivingIndex++;
                this.transform.localPosition = purposePositon;
                this.transform.localRotation = Quaternion.Euler(purposeRotation);
                this.transform.localScale = purposeScale;
            }
            else
            {
                this.transform.localPosition = beforePosition * (1.0f - per) + purposePositon * per;
                this.transform.localRotation = Quaternion.Euler(beforeRotation * (1.0f - per) + purposeRotation * per);
                this.transform.localScale = beforeScale * (1.0f - per) + purposeScale * per;
            }
        }
        else
        {
            Debug.Log("Finish Animation");
            drivingIndex = 0;
            startAnimation = false;
            animationTime = 0;
            this.transform.localPosition = startTransformPosition;
            this.transform.localRotation = startTransformRotation;
            this.transform.localScale = startTransformScale;
        }

        // 現在のTransformのスケールをいじる

    }
}
