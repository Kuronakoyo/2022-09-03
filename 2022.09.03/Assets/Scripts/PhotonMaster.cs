using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonMaster : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _parentOfMario;

    void Start()
    {
        // PhotonのSettingアセットの情報をもとに、マスターサーバに接続
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {

        GameObject obj = PhotonNetwork.Instantiate("Mario", Vector3.zero, Quaternion.identity);
        if (obj == null) return;
        obj.transform.SetParent(_parentOfMario);
    }
}
