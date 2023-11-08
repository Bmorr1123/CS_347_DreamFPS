using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{   
    private Rigidbody weaponBody;
    // Start is called before the first frame update
    void Start()
    {
        weaponBody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetButtonDown("mouse 0")) {
        //     weaponBody.AddForce(Vector3.forward * 10);
        // }
    }
}
