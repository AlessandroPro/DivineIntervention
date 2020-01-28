using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AbilityEvent : UnityEvent { }

public class HinderanceAbility : MonoBehaviour
{
    public AbilityEvent ability;

    public void ExecuteAbility()
    {
        if(ability != null)
        {
            ability.Invoke();
        }
               
    }
}
