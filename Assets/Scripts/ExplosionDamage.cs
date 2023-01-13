using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    public GameObject bomb;
    public float power = 10.0f;
    public float radius = 5.0f;
    public float upForce = 1.0f;
    public GameObject bigExplosionPrefab;
    public int health = 20;

    void Start()
    {

    }

    void FixedUpdate()
    {
        /*if (bomb == enabled)
        {
            Invoke("Detonate", 5.0f);
        }*/
        if (health == 0)
        {
            Animator colorChange = GetComponent<Animator>();
            colorChange.enabled = true;
        }
    }

    /* 

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            Detonate();
        }
    } */

    public void Detonate()
    {
        Instantiate(bigExplosionPrefab, transform.position, transform.rotation);
        Vector3 explosionPosition = bomb.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPosition, radius, upForce, ForceMode.Impulse);
            }
        }
        Destroy(gameObject);
    }
}
