using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(CapsuleCollider))]

public class WingedSpiritController : MonoBehaviourPun, IPunObservable
{
    [Header("Stats")]
    public float health = 100;
    public float speed = 5;
    public float slowSpeed = 3;
    public float dashSpeed = 10;
    public float dashCooldown = 1;
    public float accelaration = 1;

    private float dashCooldownTimer = 0;
    private bool dashOnCooldown = false;

    [Header("Invincible")]
    public float invincibleTime = 2.0f;
    private float invincibleCurrentTime = 0;
    [SerializeField]private bool invincible = false;
    
    [Header("Winged Spirit Colors")]
    public SkinnedMeshRenderer _renderer;
    public Material normalMat;
    public Material normalInvMat;
    public Material dashingMat;
    public Material dashingInvMat;


    [Header("Attack")]
    public SpiritAttack orbAttack;

    private Rigidbody rb;
    private CapsuleCollider capCol;
    private Vector3 moveVelocity;
    private Vector2 moveInput;

    public WingedSpiritAI wingedSpiritAI;
    public SteeringAgent agent;

    private void Awake()
    {
        name = "WingedSpirit";
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capCol = GetComponent<CapsuleCollider>();
        GameManager.Instance.wingedSpirit = this.gameObject;
    }

    private void OnDestroy()
    {
        GameManager.Instance.wingedSpirit = null;
        GameManager.Instance.EndGame();
    }

    // Update is called once per frame
    void Update()
    {

        // WingedSpiritControls();
        if(NetworkManager.Instance.IsViewMine(photonView))
        {
            float moveSpeed = speed;
            if (agent.getSpeed() < 0.1f)
            {
                moveSpeed = 0;
            }
            // moveVelocity = Vector3.Lerp(moveVelocity, agent.gameObject.transform.forward * moveSpeed, accelaration * Time.deltaTime);
            moveVelocity = agent.gameObject.transform.forward * moveSpeed;
            rb.MovePosition(rb.position + moveVelocity * Time.deltaTime);
        }


        if (dashOnCooldown)
        {
            DashTime();
        }

        if (invincible)
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

    private void DashTime()
    {
        dashCooldownTimer += Time.deltaTime;
        if (dashCooldownTimer >= dashCooldown)
        {
            _renderer.material = normalMat;
            dashOnCooldown = false;
            dashCooldownTimer = 0.0f;
        }
        else
        {
            _renderer.material = dashingMat;
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

        if(health <= 0)
        {
            health = 0;
            if(NetworkManager.Instance.IsViewMine(photonView))
            {
                Die();
            }
        }

        invincible = true;
    }

    private void Die()
    {
        NetworkManager.Instance.DestroyGameObject(this.gameObject);
    }

    private void WingedSpiritControls()
    {
        if(wingedSpiritAI.enabled == true)
        {
            return;
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

        if(Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.A))
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("Attack", RpcTarget.All);
        }
    }


    [PunRPC]
    void Attack()
    {
        orbAttack.execute();
    }


    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
            stream.SendNext(invincibleCurrentTime);
            stream.SendNext(invincible);
            stream.SendNext(dashCooldownTimer);
            stream.SendNext(dashOnCooldown);
        }
        else
        {
            health = (float)stream.ReceiveNext();
            invincibleCurrentTime = (float)stream.ReceiveNext();
            invincible = (bool)stream.ReceiveNext();
            dashCooldownTimer = (float)stream.ReceiveNext();
            dashOnCooldown = (bool)stream.ReceiveNext();
        }
    }
}
