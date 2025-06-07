using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target to follow
    public Vector3 offset;
    public int speed = 5;
    void Start()
    {
        target = FindObjectOfType<Player>().transform; // Find the player controller in the 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
    }
}
