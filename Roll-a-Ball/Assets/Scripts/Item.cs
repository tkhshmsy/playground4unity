using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // callback from trigger enter
    void OnTriggerEnter(Collider hit)
    {
        // is it Player ?
        if (hit.CompareTag("Player")) {
            // destroy own
            Destroy(gameObject);
        }
    }
}
