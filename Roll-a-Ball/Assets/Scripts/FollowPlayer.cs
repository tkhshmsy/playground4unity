using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;

    void Start()
    {
        // calc distance from own to target
        offset = GetComponent<Transform>().position - target.position;
    }
    void Update()
    {
        // input target position into own position
        GetComponent<Transform>().position = target.position + offset;
    }
}
