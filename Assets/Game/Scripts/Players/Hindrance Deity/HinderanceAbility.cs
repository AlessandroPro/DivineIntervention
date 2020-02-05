using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AbilityEvent : UnityEvent <Vector2> { }

public class HinderanceAbility : MonoBehaviour
{
    public AbilityEvent ability;

    public void ExecuteAbility(Vector2 position)
    {
        if(ability != null)
        {
            ability.Invoke(position);
        }
               
    }
}
