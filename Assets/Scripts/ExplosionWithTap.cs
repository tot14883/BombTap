using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ExplosionWithTap : MonoBehaviour
{
    public GameObject bomb;
    public float power = 10.0f;
    public float radius = 5.0f;
    public float upForce = 1.0f;
    public GameObject bigExplosionPrefab;
    public AudioSource explosionAudio;
    public AudioSource bombCountdown;
    //public float mixerPitch;
    //private AudioMixerGroup bombMixerGroup;
    public float distance;
    private GameObject[] playerObjects;

    private void Start()
    {
        //bombMixerGroup = Resources.Load<AudioMixerGroup>("Audio/Bomb Countdown Mixer");
        //bombMixerGroup.audioMixer.SetFloat("pitchBombCountdown", 0.5f);

        playerObjects = GameObject.FindGameObjectsWithTag("Player");
    }

    private void Update()
    {
        foreach (GameObject player in playerObjects)
        {
            distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= 2)
            {

            }
            else if(distance <= 3)
            {

            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Player");
            Detonate();
        }
    }

    void Detonate()
    {
        explosionAudio.Play();
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
