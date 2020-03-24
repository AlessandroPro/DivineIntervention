using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    // Scroll main texture based on time

    public float scrollSpeed = 0.5f;
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
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, -offset));

        timer += Time.deltaTime;

    }
}
