using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float speed;
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
