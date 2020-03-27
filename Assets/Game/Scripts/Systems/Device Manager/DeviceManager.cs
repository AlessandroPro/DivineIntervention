using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameDevice
{
    PC,
    IPhoneAR,
    AndroidTablet,
    All
};

public class DeviceManager : Singleton<DeviceManager>
{
    [HideInInspector] 
    public GameDevice device;       // the actual device this is on
    public GameDevice editorDevice; // the device specified in the editor

    public bool useEditorDevice = false;
    public bool devMode = true;

    private void Awake()
    {
        if(useEditorDevice)
        {
            device = editorDevice;
            return;
        }

        switch(Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
                device = GameDevice.PC;
                break;
            case RuntimePlatform.Android:
                device = GameDevice.AndroidTablet;
                break;
            case RuntimePlatform.IPhonePlayer:
                device = GameDevice.IPhoneAR;
                break;
            default:
                device = editorDevice;
                break;
        }
    }

    public bool IsThisDevice(GameDevice _device)
    {
        if (device == _device)
        {
            return true;
        }

        return false;
    } 
}
