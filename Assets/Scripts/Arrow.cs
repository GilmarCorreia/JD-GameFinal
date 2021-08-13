using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider bc;

    bool disableRotation = false;
    public float destroyTime = 10f; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        Destroy(this.gameObject, destroyTime);
    }

    private void Update()
    {
        //if(rb.velocity.sqrMagnitude != 0)
        if(!disableRotation)
            transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag != "Player")
    //    {
    //        disableRotation = true;
    //        rb.isKinematic = true;
    //        bc.isTrigger = true;
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            disableRotation = true;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
        }
    }

}
