using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RewindScript;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetComponent<RewindScript>().playback == false && collision.gameObject.name.Contains("Jumper"))
        {
            GetComponent<Explodable>().explode();
            GetComponent<RewindScript>().recordedTrans.Add(new StoredTransform(transform.position, transform.rotation, true));
        }
    }
}
