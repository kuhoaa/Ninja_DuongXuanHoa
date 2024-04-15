using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targer;
    public Vector3 offset;
    public float speed = 20;
    // Start is called before the first frame update
    void Start()
    {
        targer = FindAnyObjectByType<Player>().transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targer.position + offset, Time.deltaTime * speed);
    }
}
