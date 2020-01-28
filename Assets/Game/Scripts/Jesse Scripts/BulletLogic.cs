using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletLogic : MonoBehaviour
{
    public float speed = 5.0f;
    public float damage = 5.0f;

    private Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void Update()
    {
        rigid.MovePosition(transform.position + (transform.up * speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        WingedSpiritController spiritController = other.gameObject.GetComponent<WingedSpiritController>();

        if (spiritController != null)
        {
            spiritController.TakeDamage(damage);
        }

        Destroy(this.gameObject);
    }
}
