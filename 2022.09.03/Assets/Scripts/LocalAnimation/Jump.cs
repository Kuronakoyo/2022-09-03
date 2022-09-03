using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] // ジャンプするために足をたわませる
    private float squatScale = 0.3f;
    [SerializeField]
    private float squatSpeed = 0.2f;
    //スクアットアニメーションの開始からの時間
    float animationTime = 0.0f;

    [SerializeField] // 体を急激に伸ばす
    private float stretchScale = 1.5f;
    [SerializeField]
    private float strechSpeed = 0.125f;

    [SerializeField] // 伸びた体が戻る
    private float returnSpeed = 0.5f;

    [SerializeField] // 落下時間
    private float fallSpeed = 0.5f;

    [SerializeField] // 着地時にあしがたわむ
    private float squatScale2 = 0.75f;
    [SerializeField]
    private float squatSpeed2 = 0.2f;

    [SerializeField] // たわんだ体が戻る
    private float returnSpeed2 = 0.2f;

    [SerializeField]
    private bool startAnimation
     = false;


    private Vector3 startTransformScale
     = default;

    void Start()
    {
        //スタートしたタイミングで大きさ（TransformのScale)
        //を記録
        startTransformScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // アニメーション開始をするフラグが立っていないときは
        // 以降の処理を行わない
        if (startAnimation == false) return;

        animationTime += Time.deltaTime;

        if (animationTime <= squatSpeed) // しゃがむ
        {
            // この処理の間 0から1で記録される変数
            float per = animationTime / squatSpeed;
            // Debug.Log(per);
            this.transform.localScale = new Vector3(1.0f
                                                    , 1.0f - (per * (1.0f - squatScale))
                                                    , 1.0f);
        }
        else if (animationTime <= squatSpeed + strechSpeed) // のばす
        {
            // この処理の間 0から1で記録される変数
            float per = (animationTime - squatSpeed) / strechSpeed;
            this.transform.localScale = new Vector3(1.0f
                                                    , squatScale * (1.0f - per) + stretchScale * per
                                                    , 1.0f);


        }
        else if (animationTime <= squatSpeed + strechSpeed + returnSpeed) // のばされたからだがもとにもどる
        {
            // この処理の間 0から1で記録される変数
            float per = (animationTime - squatSpeed - strechSpeed) / returnSpeed;
            this.transform.localScale = new Vector3(1.0f
                                                    , stretchScale * (1.0f - per) + startTransformScale.y * per
                                                    , 1.0f);

        }
        else if (animationTime <= squatSpeed + strechSpeed + returnSpeed + fallSpeed) // のばされたからだがもとにもどる
        {
            // この処理の間 0から1で記録される変数

            this.transform.localScale = startTransformScale;

        }
        else if (animationTime <= squatSpeed + strechSpeed + returnSpeed + fallSpeed + squatSpeed2) // のばされたからだがもとにもどる
        {
            // この処理の間 0から1で記録される変数
            float per = (animationTime - squatSpeed - strechSpeed - returnSpeed - fallSpeed) / squatSpeed2;
            this.transform.localScale = new Vector3(1.0f
                                                    , startTransformScale.y * (1.0f - per) + squatScale2 * per
                                                    , 1.0f);

        }
        else if (animationTime <= squatSpeed + strechSpeed + returnSpeed + fallSpeed + squatSpeed2 + returnSpeed2) // のばされたからだがもとにもどる
        {
            // この処理の間 0から1で記録される変数
            float per = (animationTime - squatSpeed - strechSpeed - returnSpeed - fallSpeed - squatSpeed2) / returnSpeed2;
            this.transform.localScale = new Vector3(1.0f
                                                    , squatScale2 * (1.0f - per) + startTransformScale.y * per
                                                    , 1.0f);

        }
        else
        {//アニメーションがおわったら
         //、フラグをおとして、初期状態にする
            startAnimation = false;
            animationTime = 0;
            this.transform.localScale = startTransformScale;
        }
    }
}
