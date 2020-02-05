using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HinderanceDietyController : MonoBehaviour
{
    public HinderanceAbility tapAbility;
    public HinderanceAbility swipeAbility;

    private bool tapMoved = false;
    private bool swipeActivated = false;

    private Vector2 touchStartPosition;

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
        if (Input.GetMouseButtonDown(0) && DeviceManager.Instance.devMode)
        {
           tapAbility.ExecuteAbility(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

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
                    swipeAbility.ExecuteAbility(touchPos);
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
                    tapAbility.ExecuteAbility(touchPos);
                }
                tapMoved = false;
            }
        }
    }
}
