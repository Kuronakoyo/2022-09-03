using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrap : MonoBehaviour
{
    [SerializeField]
    private float center;
    [SerializeField]
    private float size;

    [SerializeField]
    private SpriteRenderer viewSp;

    void Awake()
    {
        // Trapの位置に見た目のスプライトを拡縮・移動させて見やすくする
        if (viewSp == null)
        {
            Debug.LogError("スプライトがないぞ アタッチしろ");
            return;
        }

        viewSp.transform.position
                 = new Vector3(center, 0, 0);
        viewSp.transform.localScale
                 = new Vector3(size, 10, 1);
    }

    public (float bodyLeft, float bodyRight) ParseLeftAndRightX()
    {
        float halfBodySize = size * 0.5f;
        float left = center - halfBodySize;
        float right = center + halfBodySize;
        return (left,right);
    }
}
