using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    // Scroll main texture based on time

    public float scrollSpeed = 0.5f;
    public bool scrollY = true;
    public bool scrollX = false;
    float timer = 0;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if(timer * scrollSpeed > 1)
        {
            timer = 0;
        }

        float offset = Mathf.Lerp(0, 1, timer * scrollSpeed);

        float xOffset = 0;
        float yOffset = 0;

        if(scrollY)
        {
            yOffset = -offset;
        }
        else
        {
            xOffset = -offset;
        }

        rend.material.SetTextureOffset("_MainTex", new Vector2(xOffset, yOffset));


        timer += Time.deltaTime;

    }
}
