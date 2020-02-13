using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceManager : Singleton<DeviceManager>
{

    public enum Devices
    {
        IPhoneAR,
        AndroidTablet,
        PC
    };

    public Devices device;

    public bool devMode = true;

    public GameObject AR;
    public GameObject mainCamera;
    public GameObject interaction;
    public GameObject plane2D;
    public GameObject wingedSpirit;
    public GameObject hindranceDeity;
    public GameObject blockGenerator;


    private void Awake()
    {
        switch(device)
        {
            case Devices.AndroidTablet:
                AndroidSetup();
                break;
            case Devices.IPhoneAR:
                IPhoneARSetup();
                break;
            case Devices.PC:
                PCSetup();
                break;
        }
    }

    private void PCSetup()
    {

        Destroy(AR);
        Destroy(interaction);
        
        plane2D.GetComponent<MeshRenderer>().enabled = false;

        wingedSpirit.GetComponent<WingedSpiritAI>().enabled = false;
        wingedSpirit.GetComponent<WingedSpiritController>().enabled = true;

        hindranceDeity.GetComponent<HinderanceDietyController>().enabled = false;
        hindranceDeity.GetComponent<HinderanceDeityAI>().enabled = true;

        BlockGenerator blockGenScript = blockGenerator.GetComponent<BlockGenerator>();
        blockGenScript.enableBlockAI = true;
        blockGenScript.only2D = true;
    }

    private void IPhoneARSetup()
    {
        if(!devMode)
        {
            Destroy(mainCamera);
            AR.SetActive(true);
        }
        else
        {
            Destroy(AR);
        }

        plane2D.GetComponent<MeshRenderer>().enabled = true;

        wingedSpirit.GetComponent<WingedSpiritAI>().enabled = true;
        wingedSpirit.GetComponent<WingedSpiritController>().enabled = false;

        hindranceDeity.GetComponent<HinderanceDietyController>().enabled = false;
        hindranceDeity.GetComponent<HinderanceDeityAI>().enabled = true;

        BlockGenerator blockGenScript = blockGenerator.GetComponent<BlockGenerator>();
        blockGenScript.enableBlockAI = false;
        blockGenScript.only2D = false;
    }

    private void AndroidSetup()
    {
        Destroy(AR);
        Destroy(interaction);
        plane2D.GetComponent<MeshRenderer>().enabled = false;

        wingedSpirit.GetComponent<WingedSpiritAI>().enabled = true;
        wingedSpirit.GetComponent<WingedSpiritController>().enabled = false;

        hindranceDeity.GetComponent<HinderanceDietyController>().enabled = true;
        hindranceDeity.GetComponent<HinderanceDeityAI>().enabled = false;

        BlockGenerator blockGenScript = blockGenerator.GetComponent<BlockGenerator>();
        blockGenScript.enableBlockAI = true;
        blockGenScript.only2D = true;
    }
}
