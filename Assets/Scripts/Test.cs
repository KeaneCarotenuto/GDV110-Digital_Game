using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RewindScript;

public class Test : MonoBehaviour
{

    public int Step;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetComponent<RewindScript>().playback == false && collision.gameObject.name.Contains("Jumper") && Step <= 2)
        {
            GetComponent<Explodable>().explode();
            GetComponent<RewindScript>().recordedTrans.Insert(0,new StoredTransform(Time.time, transform.position, transform.rotation, true));
        }
    }
}
