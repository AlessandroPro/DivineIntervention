using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class NetworkManager : Singleton<NetworkManager>
{
    public PunCallbackHandler punHandler;
    public string roomName = "GameRoom";
    string gameVersion = "1";

    void Awake()
    {
        PlayerPrefs.DeleteAll();
        //PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void SetPlayerName(string name)
    {
        PhotonNetwork.LocalPlayer.NickName = name;
    }

    public string GetPlayerName()
    {
        return PhotonNetwork.LocalPlayer.NickName;
    }

    public bool IsConnected()
    {
        return PhotonNetwork.IsConnected;
    }

    public bool IsMasterClient()
    {
        return PhotonNetwork.IsMasterClient;
    }

    public void Connect()
    {
        // Photon conenct to lobby
        Debug.Log("Connecting to Photon Network");
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("PhotonNetwork.IsConnected! | Trying to Create/Join Room " + roomName);
            RoomOptions roomOptions = new RoomOptions();

            roomOptions.PublishUserId = true;
            roomOptions.MaxPlayers = 3;
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public GameObject InstantiateGameObject(string prefabName, Vector3 position, Quaternion rotation)
    {
        GameObject newObject = PhotonNetwork.Instantiate(prefabName, position, rotation);
        return newObject;
    }

    public void RaiseEventAll(object content, byte evCode)
    {
        RaiseEvent(content, evCode, ReceiverGroup.All);
    }

    public void RaiseEventOthers(object content, byte evCode)
    {
        RaiseEvent(content, evCode, ReceiverGroup.Others);
    }

    public void RaiseEventMaster(object content, byte evCode)
    {
        RaiseEvent(content, evCode, ReceiverGroup.MasterClient);
    }

    private void RaiseEvent(object content, byte evCode, ReceiverGroup recGroup)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = recGroup };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }
}
