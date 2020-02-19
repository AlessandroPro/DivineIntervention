using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SwapAbilityScript : MonoBehaviour
{
    public HinderanceAbility hinderance;
    public HinderanceDietyController dietyController;
    private Image _buttonImage;


    private void Start()
    {
        _buttonImage = GetComponent<Image>();
    }

    private void Update()
    {
        CheckSelection();
    }
    public void SwapAbility()
    {
        if(hinderance != null && dietyController != null)
        {
            dietyController.tapAbility = hinderance;
        }
    }

    private void CheckSelection()
    {
        if(hinderance.abilityName == dietyController.tapAbility.abilityName)
        {
            _buttonImage.color = Color.blue;
        }
        else
        {
            _buttonImage.color = Color.white;
        }
    }
}
