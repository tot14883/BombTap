using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterExplosion : MonoBehaviour
{
    public GameObject bomb;
    public float power = 10.0f;
    public float radius = 5.0f;
    public float upForce = 1.0f;
    public GameObject bigExplosionPrefab;

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (bomb == enabled)
        {
            Invoke("Detonate", 5);
        }
    }

    void Detonate()
    {
        Instantiate(bigExplosionPrefab, transform.position, transform.rotation);
        Instantiate(bomb, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z + 1.0f), transform.rotation);
        Instantiate(bomb, new Vector3(transform.position.x + 1.0f, transform.position.y + 1.0f, transform.position.z - 0.5f), transform.rotation);
        Instantiate(bomb, new Vector3(transform.position.x - 1.0f, transform.position.y + 1.0f, transform.position.z - 0.5f), transform.rotation);

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
