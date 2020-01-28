using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(CapsuleCollider))]

public class WingedSpiritController : MonoBehaviour
{
    [Header("Stats")]
    public float health = 100;
    public float speed = 5;
    public float slowSpeed = 3;
    public float dashSpeed = 10;
    public float dashCooldown = 3;
    public float accelaration = 1;

    private float dashCooldownTimer = 0;
    private bool dashOnCooldown = false;
    private bool dashing = false;

    [Header("Invincible")]
    public float invincibleTime = 2.0f;
    private float invincibleCurrentTime = 0;
    [SerializeField]private bool invincible = false;
    
    [Header("Winged Spirit Colors")]
    public Material normalMat;
    public Material normalInvMat;
    public Material dashingMat;
    public Material dashingInvMat;

    [Header("UI Info")]
    public Text healthTxt;


    private Rigidbody rb;
    private Renderer _renderer;
    private CapsuleCollider capCol;
    private Vector3 moveVelocity;
    private Vector2 moveInput;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        capCol = GetComponent<CapsuleCollider>();
        healthTxt.text = health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        WingedSpiritControls();

        if (invincible == true)
        {
            InvincibleTime();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Death")
        {
            TakeDamage(10);
        }
    }

    private void InvincibleTime()
    {
        invincibleCurrentTime += Time.deltaTime;

        if(invincibleCurrentTime >= invincibleTime)
        {
            invincible = false;
            invincibleCurrentTime = 0.0f;
            if (dashOnCooldown)
            {
                _renderer.material = dashingMat;
            }
            else
            {
                _renderer.material = normalMat;
            }
            return;

        }

        if (dashOnCooldown)
        {
            _renderer.material = dashingInvMat;
        }
        else
        {
            _renderer.material = normalInvMat;
        }
    }

    public void TakeDamage(float damage)
    {
        if(invincible)
        {
            return;
        }

        health -= 10;
        healthTxt.text = health.ToString();

        if(health <= 0)
        {
            Die();
        }

        invincible = true;
    }

    private void Die()
    {
        healthTxt.text = "Dead";
        Destroy(this.gameObject);
    }

    private void WingedSpiritControls()
    {
        if (dashOnCooldown)
        {
            dashCooldownTimer += Time.deltaTime;
            if (dashCooldownTimer >= dashCooldown)
            {

                _renderer.material = normalMat;
                
                dashOnCooldown = false;
                dashCooldownTimer = 0.0f;
            }
        }

        if (Input.GetAxis("Horizontal") > 0.1f  || Input.GetAxis("Horizontal") < -0.1f || Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f)
        {
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if(Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Space))
            {
                if(!dashOnCooldown)
                {
                    moveVelocity = moveInput.normalized * dashSpeed;
                    dashOnCooldown = true;
                    _renderer.material = dashingMat;

                }
            }

            if(Input.GetKey("joystick button 5"))
            {
                moveVelocity = Vector3.Lerp(moveVelocity, moveInput.normalized * slowSpeed, accelaration * Time.deltaTime);
            }
            else
            {
                moveVelocity = Vector3.Lerp(moveVelocity, moveInput.normalized * speed, accelaration * Time.deltaTime);

            }
        }
        else
        {
            moveVelocity = Vector3.Lerp(moveVelocity, Vector3.zero, accelaration * Time.deltaTime);
        }

        rb.MovePosition(rb.position + moveVelocity * Time.deltaTime);
    }
}
