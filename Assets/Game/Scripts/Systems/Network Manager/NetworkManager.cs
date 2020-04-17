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
    public bool useNetwork = true;
    string gameVersion = "1";

    public enum EventCode
    {
        LoadGameSceneEvent = 1,
        GameSceneLoadedEvent,
        setupGameSceneEvent
    }

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

    public bool IsViewMine(PhotonView photonView)
    {
        return photonView.IsMine;
    }


    public bool IsDeviceViewMine(GameDevice device, PhotonView photonView)
    {
        if(DeviceManager.Instance.IsThisDevice(device) && IsViewMine(photonView))
        {
            return true;
        }

        return false;
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

    public void DestroyGameObject(GameObject gameObject)
    {
        PhotonView photonView = gameObject.GetComponent<PhotonView>();

        if(photonView == null)
        {
            Debug.LogError("Can't Network-Destroy a game object without a Photon View.");
            return;
        }

        PhotonNetwork.Destroy(photonView);
    }

    public int GetNumPlayersInRoom()
    {
        return PhotonNetwork.PlayerList.Length;
    }

    public void RaiseEventAll(object content, EventCode evCode)
    {
        RaiseEvent(content, evCode, ReceiverGroup.All);
    }

    public void RaiseEventOthers(object content, EventCode evCode)
    {
        RaiseEvent(content, evCode, ReceiverGroup.Others);
    }

    public void RaiseEventMaster(object content, EventCode evCode)
    {
        RaiseEvent(content, evCode, ReceiverGroup.MasterClient);
    }

    private void RaiseEvent(object content, EventCode evCode, ReceiverGroup recGroup)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = recGroup };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent((byte)evCode, content, raiseEventOptions, sendOptions);
    }
}
