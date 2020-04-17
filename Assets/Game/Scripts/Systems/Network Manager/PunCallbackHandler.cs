using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;
using ExitGames.Client.Photon;

public class PunCallbackHandler : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [System.Serializable]
    public class RaiseEvent : UnityEvent<byte, object[]> { }

    [HideInInspector] public UnityEvent OnJoinedLobbyEvent = new UnityEvent();
    [HideInInspector] public UnityEvent OnConnectedToMasterEvent = new UnityEvent();
    [HideInInspector] public UnityEvent OnDisconnectedEvent = new UnityEvent();
    [HideInInspector] public UnityEvent OnJoinedRoomEvent = new UnityEvent();
    [HideInInspector] public UnityEvent OnCreatedRoomEvent = new UnityEvent();
    [HideInInspector] public UnityEvent OnCreatedRoomFailedEvent = new UnityEvent();
    [HideInInspector] public UnityEvent OnPlayerLeftRoomEvent = new UnityEvent();
    [HideInInspector] public UnityEvent OnPlayerEnteredRoomEvent = new UnityEvent();
    [HideInInspector] public RaiseEvent OnRaiseEvent = new RaiseEvent();


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

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("A Player has left the room. Disconnecting...");
        base.OnPlayerLeftRoom(otherPlayer);
        OnPlayerLeftRoomEvent.Invoke();
        NetworkManager.Instance.Disconnect();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A Player has joined the room.");
        base.OnPlayerEnteredRoom(newPlayer);
        OnPlayerEnteredRoomEvent.Invoke();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        object[] data = new object[] { };

        OnRaiseEvent.Invoke(eventCode, data);
    }

    public void resetAll()
    {
        OnJoinedLobbyEvent.RemoveAllListeners();
        OnConnectedToMasterEvent.RemoveAllListeners();
        OnDisconnectedEvent.RemoveAllListeners();
        OnJoinedRoomEvent.RemoveAllListeners();
        OnCreatedRoomEvent.RemoveAllListeners();
        OnCreatedRoomFailedEvent.RemoveAllListeners();
        OnRaiseEvent.RemoveAllListeners();
        OnPlayerLeftRoomEvent.RemoveAllListeners();
        OnPlayerEnteredRoomEvent.RemoveAllListeners();
    }
}
