using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour
{
    RagdollCharacter ragdollCharacter;

    [SerializeField] Limb[] childLimb;

    [SerializeField] GameObject limbPrefab;
    [SerializeField] GameObject woundHole;

    // Start is called before the first frame update
    void Start()
    {
        ragdollCharacter = transform.root.GetChild(0).GetComponent<RagdollCharacter>();

        if (woundHole != null)
        {
            woundHole.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHit()
    {
        if(childLimb.Length > 0)
        {
            foreach(Limb limb in childLimb)
            {
                if (limb != null)
                {
                    limb.GetHit();
                }
            }
        }

        if(woundHole != null)
        {
            woundHole.SetActive(true);
        }

        /*if(limbPrefab != null)
        {
            Instantiate(limbPrefab, transform.position, transform.rotation);
        }

        transform.localScale = Vector3.zero;*/

        ragdollCharacter.GetKilled();

        Destroy(this);
    }
}
