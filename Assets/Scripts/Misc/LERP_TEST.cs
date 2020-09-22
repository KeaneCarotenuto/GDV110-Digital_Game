using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LERP_TEST : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(7, 0, 0), 0.01f);
    }
}
