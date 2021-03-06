﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    //[HideInInspector] public readonly byte setupGameSceneEvent = 3;

    public GameObject AR;
    public GameObject mainCamera;
    public GameObject devARCamera;
    public GameObject interaction;
    public GameObject plane2D;
    public GameObject hindranceDeity;
    public GameObject blockGenerator;
    public GameObject gyserGauge;
    public GameObject AbilitySwapCanvas;
    public GameObject HudBlocker;
    public GameObject background;

    // Start is called before the first frame update
    void Awake()
    {
        GameManager.Instance.gameSetup = this;
        NetworkManager.Instance.punHandler.OnRaiseEvent.AddListener(setupGameScene);
    }

    private void Start()
    {
        SceneLoader.Instance.SetActiveScene(gameObject.scene);

        switch (DeviceManager.Instance.device)
        {
            case GameDevice.PC:
                PCSetup();
                break;
            case GameDevice.AndroidTablet:
                AndroidSetup();
                break;
            case GameDevice.IPhoneAR:
                IPhoneARSetup();
                break;
        }
    }

    // Event called on all clients when the masterClient's game manager calls StartGame()
    public void setupGameScene(byte eventCode, object[] data)
    {
        if(eventCode != (byte)NetworkManager.EventCode.setupGameSceneEvent)
        {
            return;
        }

        SceneLoader.Instance.SetActiveScene(gameObject.scene);

        switch (DeviceManager.Instance.device)
        {
            case GameDevice.PC:
                PCStartGame();
                break;
            case GameDevice.AndroidTablet:
                AndroidStartGame();
                break;
            case GameDevice.IPhoneAR:
                IPhoneARStartGame();
                break;
        }
    }
    private void OnDestroy()
    {
        GameManager.Instance.gameSetup = null;
        NetworkManager.Instance.punHandler.OnRaiseEvent.RemoveListener(setupGameScene);
    }

    private void PCSetup()
    {
        Destroy(AR);
        Destroy(interaction);
        Destroy(devARCamera);

        plane2D.GetComponent<MeshRenderer>().enabled = false;

        //hindranceDeity.GetComponent<HinderanceDeityAI>().enabled = true;

        gyserGauge.GetComponent<MeshRenderer>().enabled = false;
        gyserGauge.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        AbilitySwapCanvas.SetActive(false);
        HudBlocker.SetActive(false);

        background.SetActive(true);
    }

    private void IPhoneARSetup()
    {
        if (!DeviceManager.Instance.devMode)
        {
            Destroy(mainCamera);
            Destroy(devARCamera);
            AR.SetActive(true);
        }
        else
        {
            Destroy(AR);
            Destroy(mainCamera);
            devARCamera.SetActive(true);
        }

        plane2D.GetComponent<MeshRenderer>().enabled = true;

        hindranceDeity.GetComponent<HinderanceDietyController>().enabled = false;
        //hindranceDeity.GetComponent<HinderanceDeityAI>().enabled = true;

        gyserGauge.GetComponent<MeshRenderer>().enabled = false;
        gyserGauge.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        AbilitySwapCanvas.SetActive(false);
        HudBlocker.SetActive(false);

        background.SetActive(false);
    }

    private void AndroidSetup()
    {
        Destroy(AR);
        Destroy(interaction);
        Destroy(devARCamera);
        plane2D.GetComponent<MeshRenderer>().enabled = false;

        hindranceDeity.GetComponent<HinderanceDietyController>().enabled = true;
        hindranceDeity.GetComponent<HinderanceDeityAI>().enabled = false;

        gyserGauge.GetComponent<MeshRenderer>().enabled = true;
        gyserGauge.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        AbilitySwapCanvas.SetActive(true);
        HudBlocker.SetActive(true);

        background.SetActive(true);
    }


    private void PCStartGame()
    {
        NetworkManager.Instance.InstantiateGameObject("WingedSpirit", new Vector3(0, 10, 0), Quaternion.identity);
    }

    private void IPhoneARStartGame()
    {
        BlockGenerator blockGenScript = blockGenerator.GetComponent<BlockGenerator>();
        blockGenScript.startGeneratingBlocks();
    }

    private void AndroidStartGame()
    {

    }


}
