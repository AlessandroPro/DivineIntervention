using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class PunCallbackHandler : MonoBehaviourPunCallbacks
{
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby!");
    }

    // Photon Methods
    public override void OnConnectedToMaster()
    {

        base.OnConnected();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Disconnected. Please check your Internet connection.");
    }

    public override void OnJoinedRoom()
    {

    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully!");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed: " + message);
    }
}
