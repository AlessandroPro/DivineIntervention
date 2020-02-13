using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AbilityEvent : UnityEvent <Vector2, Transform> { }

public class HinderanceAbility : MonoBehaviour
{
    public AbilityEvent ability;
    public bool hasCooldown;
    public float cooldownTimer = 3.0f;
    private float currentCooldown = 0.0f;
    private bool onCooldown = false;
    private void Update()
    {
        if(onCooldown)
        {
            currentCooldown += Time.deltaTime;
            if(currentCooldown >= cooldownTimer)
            {
                currentCooldown = 0.0f;
                onCooldown = false;
            }
        }
    }

    public void ExecuteAbility(Vector2 position, Transform wingedSpirit)
    {
        if(ability != null && onCooldown == false)
        {
            ability.Invoke(position, wingedSpirit);
            if(hasCooldown == true)
            {
                onCooldown = true;
            }
        }
               
    }
}
