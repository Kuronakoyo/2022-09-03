using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 操作キャラクタのPositionを移動する
/// </summary>
public class MoveCharacterRoot : MonoBehaviour
{
    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private GroundHight groundHight;

    // 変数 移動速度 
    /// <summary>
    /// Unity の １秒間での画面内距離
    /// </summary>
    public float MoveDistancePerSec = 0.06125f;
    //変数 ジャンプする高さ

    // 仮の重力の強さ
    public float GravityPower = 0.05f;

    // 左右に動いてよいかのフラグ
    private bool canMoveLeft, canMoveRight;

    #region DEBUG 変数

    // Debug 用フラグ ― 左右移動 ジャンプ
    public bool IsMoveLeft, IsMoveRight;

    #endregion


    // 左右移動

    // 指定された速度から移動距離を割り出し、移動させる

    private float CalMoveDistancePerFrame(float distancePerSec)
    {
        // ゲーム内の時間をFPSを0にして止める場合があったら、移動させない
        if (Application.targetFrameRate == 0) return 0;
        // １フレームで進む距離を割り出す
        float distancePerFrame
            = distancePerSec / Application.targetFrameRate;

        return -distancePerFrame;
    }

    public void MoveLeft()
    {
        float distance = CalMoveDistancePerFrame(MoveDistancePerSec);
        if (canMoveLeft) MoveLeft(distance);
    }

    public void MoveRight()
    {
        float distance = CalMoveDistancePerFrame(MoveDistancePerSec);
        if (canMoveRight) MoveRight(distance);
    }

    public void Jump()
    {
        // Jump スクリプト
        
    }

    // 距離を定めての左右移動
    private void MoveLeft(float distance)
    {
        targetTransform.localPosition
            = new Vector2(targetTransform.localPosition.x - distance
                            , targetTransform.localPosition.y);
    }

    private void MoveRight(float distance)
    {
        targetTransform.localPosition
            = new Vector2(targetTransform.localPosition.x + distance
                            , targetTransform.localPosition.y);
    }

    // 重力で落ちる
    private void UpdateFalldownByGravity()
    {


        // 移動しようとする距離
        float falldownDistancePerFrame
             = CalMoveDistancePerFrame(GravityPower);
        // もし、地面以下にめり込もうとしていたら、
        // ちょうど地面のところで止まるようにする

        // キャラクタの足までの距離
        float halfMarioSizeY = targetTransform.lossyScale.y / 2.0f;
        // これから移動しようとするライン（Y座標）
        float purposeY = (targetTransform.position.y - halfMarioSizeY)
                             - falldownDistancePerFrame;

        (float gh, bool leftCol, bool rightCol) = groundHight.ReturnGroundHight(targetTransform.position.x
                                                , targetTransform.lossyScale.x);
        // 地面にめり込みそうなら その場で止める
        if (gh > purposeY)
        {
            // 今いる自分の高さが、載せたい位置とで、大きく違うなら
            // 載せなくする(X軸にたいして、一つ前のUpdate時の位置に固定)
            float jimenUeY = gh + halfMarioSizeY;
            float moveYkyori = jimenUeY - targetTransform.localPosition.y;
            // ちょっとの差なら、乗り越えられるようにする
            bool norioeOK = moveYkyori < 1.25f;

            if (norioeOK)
            {
                //地面の位置で止める( 地面の上に乗せる)
                targetTransform.localPosition
                = new Vector2(targetTransform.localPosition.x
                    , jimenUeY);

                return;
            }
            else
            {
                // 乗り越えちゃだめなら、左右移動がだめらフラグを立てて、
                // 操作オブジェクトにそこで止まれと教える
                canMoveLeft = leftCol == false;
                canMoveRight = rightCol == false;
            }
        }

        // 重力移動
        targetTransform.localPosition
            = new Vector2(targetTransform.localPosition.x
                , targetTransform.localPosition.y
                 - falldownDistancePerFrame);

        // もしGroundZero（地球の中心）より下にいこうとしたら 落下をとめる
        if (targetTransform.localPosition.y <= gh)
        {
            float jimenUeY = gh + halfMarioSizeY;
            float moveYkyori = jimenUeY - targetTransform.localPosition.y;
            // ちょっとの差なら、乗り越えられるようにする
            bool norioeOK = moveYkyori < 1.25f;

            if (norioeOK == false)
            {
                jimenUeY = groundHight.GroundZero;
            }
            targetTransform.localPosition
                = new Vector2(targetTransform.localPosition.x
                    , jimenUeY);
        }
    }

    // ジャンプ移動



    void Update()
    {
        canMoveLeft = true;
        canMoveRight = true;
        UpdateFalldownByGravity();

        // FIXME DebugManagerを作ったら、処理を移行して、こちらは消す
        DebugUpdate();
    }

    #region DEBUG
    private void DebugUpdate()
    {
        if (IsMoveLeft)
        {
            MoveLeft();
        }

        if (IsMoveRight)
        {
            MoveRight();
        }
    }

    #endregion
}
