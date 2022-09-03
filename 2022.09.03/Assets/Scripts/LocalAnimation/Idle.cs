using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : MonoBehaviour
{
    // アニメーションの速度
    [SerializeField]
    private float animationSpeed = 1.0f;
    // idle アニメーションの大きさ
    [SerializeField]
    private float idleScale = 0.1f;
    // アニメーションを開始させるフラグ
    [SerializeField]
    private bool startAnimation = false;
    // アニメーションをループさせるか
    [SerializeField]
    private bool loop = true;

    private float animationTime = 0.0f;

    // Update is called once per frame
    // デフォルトの設定で、秒間６０回、Updateが走る
    void Update()
    {
        if (startAnimation == false) return;
        // 以下のコードが同じ動きするよ
        //if (!startAnimation) return;

        //前回のUpdate()から、今回のUpdate()の間までにかかった時間
        float deltaTime = Time.deltaTime;

        //アニメーション開始時からの時間を記録しておく
        animationTime = animationTime + deltaTime;

        // アニメーションが一巡したら、フラグにあわせて、進行を停止・駆動の判断をするロジック
        if (animationTime >= animationSpeed)
        {
            if (loop)
            {   // ループしているアニメーションの時間設定を巻き戻す
                animationTime -= animationSpeed;
            }
            else
            {   // ループのフラグが入っていないとき アニメーションを１巡で止める
                startAnimation = false;
                animationTime = 0;
                // アニメーションがとまったら、マリオの位置、大きさを初期値に戻す
                this.transform.localPosition = Vector3.zero;
                this.transform.localScale = Vector3.one;
            }
            return;
        }

        //テストコード ゲーム開始からの時間をちゃんと出せているか
        //Debug.Log(animationTime);

        // SinWave（Sin波）の値を使って、大きさを上下させる
        float modifyY = Mathf.Sin(2 * Mathf.PI * animationTime * animationSpeed);
        // アニメーションの大きさを指定の大きさに変更
        modifyY = modifyY * idleScale * 0.5f;
        // アニメーションのデフォルトを指定
        float modifyScaleY = modifyY + 1.0f;
        // 現在あるべき、大きさをTransformに反映させる
        this.transform.localScale = new Vector3(1.0f
                                        , modifyScaleY
                                        , 1.0f);

        float modifyPositionY = modifyY * 0.5f;
        // 足が床を突き抜けないように、YのPositionを調整する
        this.transform.localPosition = new Vector3(this.transform.localPosition.x
                                                    , modifyPositionY
                                                    , this.transform.localPosition.z);
    }
}
