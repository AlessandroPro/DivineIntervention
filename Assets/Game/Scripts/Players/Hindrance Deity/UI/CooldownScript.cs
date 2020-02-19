using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CooldownScript : MonoBehaviour
{
    public HinderanceAbility hinderanceAbility;
    private Text cooldownText;

    // Start is called before the first frame update
    void Start()
    {
        cooldownText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Countdown();
    }

    private void Countdown()
    {
        if(hinderanceAbility.currentCooldown > 0)
        {
            cooldownText.text = ((int)hinderanceAbility.currentCooldown + 1).ToString();
        }
        else
        {
            cooldownText.text = "";
        }
    }
}
