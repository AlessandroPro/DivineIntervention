using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HinderanceDeityAI : MonoBehaviour
{
    public HinderanceAbility tapAbility;
    public HinderanceAbility swipeAbility;
    public GameObject gameWindow2D;

    public float tapIntervalMin;
    public float tapIntervalMax;
    private float tapInterval;
    private float tapTimer;

    public float swipeIntervalMin;
    public float swipeIntervalMax;
    private float swipeInterval;
    private float swipeTimer;

    // Start is called before the first frame update
    void Start()
    {
        tapInterval = Random.Range(tapIntervalMin, tapIntervalMax);
        swipeInterval = Random.Range(swipeIntervalMin, swipeIntervalMax);

    }

    // Update is called once per frame
    void Update()
    {
        if(tapTimer > tapInterval)
        {
            tapAbility.ExecuteAbility(getRandomScenePoint());
            //tapAbility.ExecuteAbility(getRandomScreenPoint());
            tapTimer = 0;
            tapInterval = Random.Range(tapIntervalMin, tapIntervalMax);
        }

        if (swipeTimer > swipeInterval)
        {
            swipeAbility.ExecuteAbility(getRandomScenePoint());
            //swipeAbility.ExecuteAbility(getRandomScreenPoint());
            swipeTimer = 0;
            swipeInterval = Random.Range(swipeIntervalMin, swipeIntervalMax);
        }

        tapTimer += Time.deltaTime;
        swipeTimer += Time.deltaTime;
    }

    Vector2 getRandomScreenPoint()
    {
        float randomX = Random.Range(0, Screen.width);
        float randomY = Random.Range(0, Screen.height);

        return Camera.main.ScreenToWorldPoint(new Vector2(randomX, randomY));
    }

    Vector2 getRandomScenePoint()
    {
        float randomX = 0;
        float randomY = 0;

        if (gameWindow2D)
        {
            float gameWindowWidth = gameWindow2D.transform.localScale.x;
            randomX = Random.Range(-gameWindowWidth * 0.5f, gameWindowWidth * 0.5f);
            randomY = Random.Range(0, gameWindow2D.transform.localScale.y);
        }
        return new Vector2(randomX, randomY);

    }
}
