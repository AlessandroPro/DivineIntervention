using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AbilityEvent : UnityEvent <Vector2, Transform> { }

[System.Serializable]
public class ActiveSkillEvent : UnityEvent  { }

public class HinderanceAbility : MonoBehaviour
{
    public AbilityEvent ability;
    public ActiveSkillEvent skill;
    public bool hasCooldown;
    public float cooldownTimer = 3.0f;
    public float currentCooldown = 0.0f;
    public string abilityName;
    private bool onCooldown = false;
    private void Update()
    {
        if(onCooldown)
        {
            currentCooldown -= Time.deltaTime;
            if(currentCooldown <= 0)
            {
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
                currentCooldown = cooldownTimer;
            }
        }
    }

    public void ExecuteSkill()
    {
        if (skill != null && onCooldown == false)
        {
            skill.Invoke();
            if (hasCooldown == true)
            {
                onCooldown = true;
                currentCooldown = cooldownTimer;
            }
        }
    }

}
