using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HinderanceDietyController : MonoBehaviour
{
    public HinderanceAbility tapAbility;
    public HinderanceAbility swipeAbility;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckEvents();
    }

    private void CheckEvents()
    {
        if (Input.GetMouseButtonDown(0))
        {
            tapAbility.ExecuteAbility();
        }
    }
}
