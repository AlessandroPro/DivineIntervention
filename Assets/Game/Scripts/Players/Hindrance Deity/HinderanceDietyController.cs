using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HinderanceDietyController : MonoBehaviour
{
    public HinderanceAbility tapAbility;
    public HinderanceAbility swipeAbility;
    public LayerMask mask;
    public Transform tapAllowance;
    public float disallowedRadius = 4.0f;
    public float errorTime = 1.0f;


    private bool tapMoved = false;
    private bool swipeActivated = false;
    private bool shieldTriggered = false;
    private float currentErrorTime = 0.0f;

    private Vector2 touchStartPosition;
    private Transform wingedSpirit;
    private LineRenderer line;
    

    


    // Start is called before the first frame update
    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(wingedSpirit == null)
        {
            if (GameManager.Instance.wingedSpirit == null)
            {
                return;
            }
            else
            {
                wingedSpirit = GameManager.Instance.wingedSpirit.transform;
            }
        }

        CheckEvents();
        DrawDisallowedArea();
    }

    private void CheckEvents()
    {
        if (Input.GetMouseButtonDown(0) && DeviceManager.Instance.devMode)
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //Physics.Raycast(ray, out hit, Mathf.Infinity, mask);

            //if (hit.collider != null)
            //{
            //    return;
            //}


            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            if (mousePosition.y <= tapAllowance.position.y)
            {
                return;
            }


            float dist = Vector3.Distance(mousePosition, wingedSpirit.position);


            if (dist > disallowedRadius)
            {
                tapAbility.ExecuteAbility(mousePosition, wingedSpirit);
            }
            else
            {
                currentErrorTime = 0.0f;
                shieldTriggered = true;
            }
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            if (touchPos.y <= tapAllowance.position.y)
            {
                return;
            }


            if(touch.phase == TouchPhase.Began)
            {
                touchStartPosition = touchPos;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchCurrentPosition = touchPos;
                float diff = Vector2.Distance(touchStartPosition, touchCurrentPosition);

                if (diff > 0.5f)
                {
                    if (swipeActivated == true)
                    {
                        return;
                    }

                    tapMoved = true;
                    swipeAbility.ExecuteAbility(touchPos, wingedSpirit);
                    swipeActivated = true;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (tapMoved)
                {
                    swipeActivated = false;
                }
                else
                {
                    float dist = Vector3.Distance(touchPos, wingedSpirit.position);
                    if(dist > disallowedRadius)
                    {
                        tapAbility.ExecuteAbility(touchPos, wingedSpirit);
                    }
                    else
                    {
                        currentErrorTime = 0.0f;
                        shieldTriggered = true;
                    }
                }
                tapMoved = false;
            }
        }
    } 


    private void DrawDisallowedArea()
    {
        if(wingedSpirit == null)
        {
            return;
        }

        line.positionCount = 51;
        line.transform.position = wingedSpirit.position;

        if (shieldTriggered)
        {
            line.material.color = Color.red;
            currentErrorTime += Time.deltaTime;

            if(currentErrorTime >= errorTime)
            {
                shieldTriggered = false;
            }
        }
        else
        {
            line.material.color = Color.grey;
        }

        CreatePoints();
    }

    void CreatePoints()
    {
        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < (50 + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * disallowedRadius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * disallowedRadius;

            line.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / 50);
        }
    }
}
