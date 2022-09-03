using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundHight : MonoBehaviour
{
    private float groundZero = -10;
    public float GroundZero => groundZero;

    [SerializeField]
    private List<GroundTrap> groundTrapList
                     = new List<GroundTrap>();
    

    // Groundはどこにあるのか
    public (float groundY, bool leftColed, bool rightColed)
         ReturnGroundHight(float positionX, float bodySize)
    {
        // トラップに相当する位置であれば、Groundはものすごく低く
        // ものすごく というのは 見えなくて死ぬぐらい（ いったん画面外となる ―10 と設定)
        // ToDo : マップ全体の下限に合わせる   
        float gamengaiY = groundZero;

        // BodySizeが0だったら 演算の必要なく、幽霊なので すりぬける(地面を遠くする)
        if (bodySize <= 0) return (gamengaiY, false, false);

        // サイズの両端がGroundTrapのすべての範囲内に入ったら
        // すりぬけて（地面を遠くする）
        bool on = false;
        float halfBodySize = bodySize * 0.5f;
        float trapleft = positionX - halfBodySize;
        float trapright = positionX + halfBodySize;
        // 左右にぶつかっているのかどうかを保持
        bool leftCol = false;
        bool rightCol = false;
        foreach (var trap in groundTrapList)
        {
            (float bodyleft, float bodyRight) v
                         = trap.ParseLeftAndRightX();
            leftCol = v.bodyleft >= trapleft;//左側でぶつかってる
            rightCol = v.bodyRight <= trapright;//右側でぶつかってる
            if (leftCol == false && rightCol == false)
            {
                on = true;
                break;
            }

        }
        if (on)
        {
            // すりぬけて
            return (gamengaiY, leftCol, rightCol);
        }

        return (this.transform.position.y, leftCol, rightCol);
    }
}
