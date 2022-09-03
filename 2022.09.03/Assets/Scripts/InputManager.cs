using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 一括してキー入力を分配するクラス


public class InputManager : MonoBehaviour
{
    [SerializeField]
    private MoveCharacterRoot moveCharacterRoot;

    void Update()
    {
        // 上下左右キーの入力
        if (Input.GetKey(KeyCode.W))//上
        {

        }
        else if (Input.GetKey(KeyCode.S))//下
        {

        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))//左
        {
            moveCharacterRoot.MoveLeft();
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))//右
        {
            moveCharacterRoot.MoveRight();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveCharacterRoot.Jump();
        }

    }
}
