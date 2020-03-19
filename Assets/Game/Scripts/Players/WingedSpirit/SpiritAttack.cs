using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritAttack : MonoBehaviour
{
    public float duration = 1;
    private float rotateAngle = 0;
    private bool canExecute = true;

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.SetActive(false);
        if (duration <= 0)
        {
            duration = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //attackOrb.transform.RotateAround(transform.position, transform.forward, (360 / duration) * Time.fixedDeltaTime);

        if(!canExecute)
        {
            if (rotateAngle < 360)
            {
                float rotateBy = (360 / duration) * Time.deltaTime;
                rotateAngle += rotateBy;
                transform.Rotate(0, 0, rotateBy);
            }
            else
            {
                canExecute = true;
                gameObject.SetActive(false);
            }
        }

    }

    public void execute()
    {
        if(canExecute)
        {
            canExecute = false;
            transform.rotation = Quaternion.identity;
            rotateAngle = 0;

            gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<TurretLogic>())
        {
            Destroy(other.gameObject);
        }
    }
}
