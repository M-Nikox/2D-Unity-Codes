using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpGrav : MonoBehaviour
{
    private float fallMult = 2.5f;
    private float lowThreshold = 2f;
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * (Physics2D.gravity.y * (fallMult - 1) * Time.deltaTime);
        } else if (rb.velocity.y > 0 && Input.GetButtonDown("jump"))
        {
            rb.velocity += Vector2.up * (Physics2D.gravity.y * (lowThreshold - 1) * Time.deltaTime);
        }
    }
}
