using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10;
    void FixedUpdate()
    {
        //input x and z
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // get rigidbody
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        // add force to x,y,z
        rigidbody.AddForce(x * speed, 0, z * speed);
    }
}
