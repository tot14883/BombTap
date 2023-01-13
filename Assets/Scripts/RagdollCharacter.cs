using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollCharacter : MonoBehaviour
{
    Animator myAnim;
    List<Rigidbody> ragdollRigid;

    void Start()
    {
        myAnim = GetComponent<Animator>();

        ragdollRigid = new List<Rigidbody>(transform.GetComponentsInChildren<Rigidbody>());
        ragdollRigid.Remove(GetComponent<Rigidbody>());

        DeactivateRagdoll();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ActivateRagdoll()
    {
        myAnim.enabled = false;
        for (int i = 0; i < ragdollRigid.Count; i++)
        {
            ragdollRigid[i].useGravity = true;
            ragdollRigid[i].isKinematic = false;
        }
    }

    void DeactivateRagdoll()
    {
        myAnim.enabled = true;
        for (int i = 0; i < ragdollRigid.Count; i++)
        {
            ragdollRigid[i].useGravity = false;
            ragdollRigid[i].isKinematic = true;
        }
    }

    public void GetKilled()
    {
        ActivateRagdoll();
    }

    public bool isActiveRagdoll()
    {
        return !myAnim.enabled;
    }
}
