using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiritUI : Menu
{
    public Text healthValueText;

    private void Update()
    {
        if(GameManager.Instance.wingedSpirit != null)
        {
            // Yes, this is inefficient, TODO later
            healthValueText.text = GameManager.Instance.wingedSpirit.GetComponent<WingedSpiritController>().health.ToString();
        }
        else
        {
            healthValueText.text = "0";
        }
    }
}
