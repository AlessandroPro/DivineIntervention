using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : Singleton<NetworkManager>
{
    public PunCallbackHandler punHandler;

    string gameVersion = "1";

    public void Connect()
    {
        // Photon conenct to lobby

        //Debug.Log("Connecting to Photon Network");
        //connectionStatus.text = "Connecting...";
        //PhotonNetwork.GameVersion = gameVersion;
        //PhotonNetwork.ConnectUsingSettings();

        // then in onConenctedToLobby join or create a room

    }

    public void ShowLobby()
    {

    }

    //add action to callback event

    //remove action from callback event

}
