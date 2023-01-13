using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
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
            Invoke("Detonate", 5.0f);
        }
    }

    void Detonate()
    {
        Instantiate(bigExplosionPrefab, transform.position, transform.rotation);
        Vector3 explosionPosition = bomb.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);
        foreach (Collider hit in colliders)
        {

            Limb limb = hit.transform.GetComponent<Limb>();
            if (limb != null)
            {
                limb.GetHit();
            }

            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPosition, radius, upForce, ForceMode.Impulse);
            }

            ExplosionDamage explosionDamage = hit.GetComponent<ExplosionDamage>();
            if (explosionDamage != null)
            {
                explosionDamage.Detonate();
            }
        }

        Destroy(gameObject);
    }
}
