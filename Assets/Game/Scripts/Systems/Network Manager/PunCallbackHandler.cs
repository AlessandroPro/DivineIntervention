using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;

public class PunCallbackHandler : MonoBehaviourPunCallbacks
{
    [HideInInspector] public UnityEvent OnJoinedLobbyEvent = new UnityEvent();
    [HideInInspector] public UnityEvent OnConnectedToMasterEvent = new UnityEvent();
    [HideInInspector] public UnityEvent OnDisconnectedEvent = new UnityEvent();
    [HideInInspector] public UnityEvent OnJoinedRoomEvent = new UnityEvent();
    [HideInInspector] public UnityEvent OnCreatedRoomEvent = new UnityEvent();
    [HideInInspector] public UnityEvent OnCreatedRoomFailedEvent = new UnityEvent();

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby!");
        OnJoinedLobbyEvent.Invoke();
    }

    // Photon Methods
    public override void OnConnectedToMaster()
    {
        base.OnConnected();
        OnConnectedToMasterEvent.Invoke();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Disconnected. Please check your Internet connection.");
        OnDisconnectedEvent.Invoke();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room!");
        OnJoinedRoomEvent.Invoke();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully!");
        OnCreatedRoomEvent.Invoke();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed: " + message);
        OnCreatedRoomFailedEvent.Invoke();
    }
}
