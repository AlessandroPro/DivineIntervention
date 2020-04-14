using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WingedSpiritAI : MonoBehaviour
{
    private Rigidbody rb;
    public LayerMask avoidMask;
    public float speed;
    public float sceneWidth;

    private float decisionTimer = 0;
    private Vector3 moveVelocity;

    public SpiritAttack orbAttack;
    private float attackTimer = 0;
    private float attackTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveVelocity = new Vector3();
        attackTime = Random.Range(1f, 6f);
    }

    // Update is called once per frame
    void Update()
    {
        // Constraint z position in local space
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        localVelocity.z = 0;
        rb.velocity = transform.TransformDirection(localVelocity);


        decisionTimer += Time.deltaTime;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.up, out hit, 10, avoidMask))
        {
            float end = sceneWidth * 0.5f;
            float start = -end;
            float leftGapSize = (hit.collider.transform.localPosition.x - hit.collider.transform.localScale.x * 0.5f) - start;
            float rightGapSize = end - (hit.collider.transform.localPosition.x + hit.collider.transform.localScale.x * 0.5f);

            Vector3 leftGapMid = new Vector3(start + leftGapSize * 0.5f, hit.collider.transform.localPosition.y, 0);
            Vector3 rightGapMid = new Vector3(end - rightGapSize * 0.5f, hit.collider.transform.localPosition.y, 0);

            if (Vector3.Distance(transform.localPosition, leftGapMid) < Vector3.Distance(transform.localPosition, rightGapMid))
            {
                if(leftGapSize < 3)
                {
                    moveVelocity = rightGapMid - transform.localPosition;
                }
                else if(rightGapSize > 3)
                {
                    moveVelocity = leftGapMid - transform.localPosition;
                }
                else
                {
                    randomizeDirection();
                }
            }
            else
            {
                if (rightGapSize < 3)
                {
                    moveVelocity = leftGapMid - transform.localPosition;
                }
                else if(leftGapSize > 3)
                {
                    moveVelocity = rightGapMid - transform.localPosition;
                }
                else
                {
                    randomizeDirection();
                }
            }
        }
        else
        {
            randomizeDirection();
        }

        moveVelocity = moveVelocity.normalized * speed;
        transform.localPosition = transform.localPosition + moveVelocity * Time.deltaTime;


        // Random attack
        if(attackTimer > attackTime)
        {
            attackTime = Random.Range(1f, 6f);
            orbAttack.execute();
            attackTimer = 0;
        }
        attackTimer += Time.deltaTime;
    }

    void randomizeDirection()
    {
        if (decisionTimer > Random.Range(1.0f, 2.5f))
        {
            if (transform.localPosition.x > sceneWidth * 0.4f)
            {
                moveVelocity.x = Random.Range(-1.0f, 0.0f);
            }
            else if (transform.localPosition.x < -sceneWidth * 0.4f)
            {
                moveVelocity.x = Random.Range(0.0f, 1.0f);
            }
            else
            {
                moveVelocity.x = Random.Range(-1.0f, 1.0f);
            }

            if (transform.localPosition.y > 25)
            {
                moveVelocity.y = Random.Range(-1.0f, 0.0f);
            }
            else if(transform.localPosition.y < 7)
            {
                moveVelocity.y = Random.Range(0.0f, 1.0f);
            }
            else
            {
                moveVelocity.y = Random.Range(-1.0f, 1.0f);
            }
            
            decisionTimer = 0;
        }
    }
}
