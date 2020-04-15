using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiritUI : Menu
{
    public Text healthValueText;
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
        }
        else
        {
            healthValueText.text = "0";
        }
    }
}
