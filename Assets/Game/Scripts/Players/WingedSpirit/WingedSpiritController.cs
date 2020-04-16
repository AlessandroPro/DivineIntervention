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
    public float secondsAlive = 0.0f;

    private float dashCooldownTimer = 0;
    [HideInInspector]
    public bool dashOnCooldown = false;

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



    [Header("Bot AI")]
    public bool playAsBot = false;
    public GameObject steeringBehaviours;
    public SteeringAgent agent;

    [Header("Audio")]
    public AudioClip dashSound;

    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        name = "WingedSpirit";

        if(playAsBot == false)
        {
            Destroy(steeringBehaviours);
        }
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
        if(NetworkManager.Instance.IsViewMine(photonView))
        {
            if(playAsBot)
            {
                float moveSpeed = speed;

                if (agent.getSpeed() < 0.1f)
                {
                    moveSpeed = 0;
                }
                moveInput = agent.gameObject.transform.forward;
                moveVelocity = Vector3.Lerp(moveVelocity, moveInput * moveSpeed, accelaration * Time.deltaTime);
                //moveVelocity = agent.gameObject.transform.forward * moveSpeed;
            }
            else
            {
                WingedSpiritControls();
            }

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

        secondsAlive += Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("DamageZone"))
        {
            if(NetworkManager.Instance.IsViewMine(photonView))
            {
                SpiritTakeDamageCall(10);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Death")
        {
            SpiritTakeDamage(10);
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

    public void SpiritTakeDamageCall(float damage)
    {
        photonView.RPC("SpiritTakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    private void SpiritTakeDamage(float damage)
    {
        if(invincible)
        {
            return;
        }

        audioSource.Play();
        health -= damage;

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
        if (Input.GetAxis("Horizontal") > 0.1f  || Input.GetAxis("Horizontal") < -0.1f || Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f)
        {
            moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if(Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Space))
            {
                dash();
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

        if(Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.A))
        {
            sendAttack();
        }
    }


    [PunRPC]
    void Attack()
    {
        orbAttack.execute();
    }

    public void sendAttack()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("Attack", RpcTarget.All);
    }

    public void dash()
    {
        if (!dashOnCooldown)
        {
            audioSource.PlayOneShot(dashSound);
            moveVelocity = moveInput.normalized * dashSpeed;
            dashOnCooldown = true;
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
            stream.SendNext(secondsAlive);
            stream.SendNext(invincibleCurrentTime);
            stream.SendNext(invincible);
            stream.SendNext(dashCooldownTimer);
            stream.SendNext(dashOnCooldown);
        }
        else
        {
            health = (float)stream.ReceiveNext();
            secondsAlive = (float)stream.ReceiveNext();
            invincibleCurrentTime = (float)stream.ReceiveNext();
            invincible = (bool)stream.ReceiveNext();
            dashCooldownTimer = (float)stream.ReceiveNext();
            dashOnCooldown = (bool)stream.ReceiveNext();
        }
    }
}
