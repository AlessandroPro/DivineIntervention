using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiritUI : Menu
{
    public Text healthValueText;
    public Text timeValueText;
    private WingedSpiritController spirit;

    private void Update()
    {

        if (GameManager.Instance.wingedSpirit != null)
        {
            if(spirit == null)
            {
                spirit = GameManager.Instance.wingedSpirit.GetComponent<WingedSpiritController>();
            }
        }
        else
        {
            spirit = null;
        }

        if(spirit != null)
        {
            healthValueText.text = spirit.health.ToString();
            timeValueText.text = spirit.secondsAlive.ToString("0.00") + " Seconds";
        }
        else
        {
            healthValueText.text = "0";
        }
    }
}
