using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HinderanceDeityAI : MonoBehaviour
{
    public HinderanceAbility tapAbility;
    public HinderanceAbility swipeAbility;
    public BlockGenerator blockGen;
    public GameObject gameWindow2D;
    

    public float tapIntervalMin;
    public float tapIntervalMax;
    private float tapInterval;
    private float tapTimer;

    public float swipeIntervalMin;
    public float swipeIntervalMax;
    private float swipeInterval;
    private float swipeTimer;

    private Transform wingedSpirit;

    // Start is called before the first frame update
    void Start()
    {
        tapInterval = Random.Range(tapIntervalMin, tapIntervalMax);
        swipeInterval = Random.Range(swipeIntervalMin, swipeIntervalMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (wingedSpirit == null)
        {
            if(GameManager.Instance.wingedSpirit == null)
            {
                return;
            }
            else
            {
                wingedSpirit = GameManager.Instance.wingedSpirit.transform;
            }
        }

        if (tapTimer > tapInterval)
        {
            if(Random.Range(0, 5) == 0)
            {
                blockGen.swapPlanes();
            }
            else
            {
                tapAbility.ExecuteAbility(getRandomScenePoint(), wingedSpirit);
            }
            
            //tapAbility.ExecuteAbility(getRandomScreenPoint());
            tapTimer = 0;
            tapInterval = Random.Range(tapIntervalMin, tapIntervalMax);
        }

        if (swipeTimer > swipeInterval)
        {
            swipeAbility.ExecuteAbility(getRandomScenePoint(), wingedSpirit);
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
