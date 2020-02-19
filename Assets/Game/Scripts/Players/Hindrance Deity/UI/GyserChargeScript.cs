using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyserChargeScript : MonoBehaviour
{
    public float chargeTime = 20.0f;

    public float chargeCost = 0.25f;

    private Vector3 startPosition;
    private Vector3 endPosition = new Vector3(0,0,-1);

    [SerializeField]
    //Between 0 and 1
    private float currentCharge = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
        
    }

    // Update is called once per frame
    void Update()
    {
        ChargeGyser();
    }

    private void ChargeGyser()
    {
        if (currentCharge > 1)
        {
            currentCharge = 1;
        }
        else if(currentCharge < 0)
        {
            currentCharge = 0;
        }

        if (chargeTime != 0)
        {
            currentCharge += Time.deltaTime / chargeTime;
        }

        transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentCharge);
    }



    public int GyserRequest()
    {
        int verdict = 0;

        if(currentCharge >= 1)
        {
            verdict = 2;
            currentCharge = 0;
        }
        else if(currentCharge > chargeCost)
        {
            verdict = 1;
            currentCharge -= chargeCost;
        }

        return verdict;
    }
}
