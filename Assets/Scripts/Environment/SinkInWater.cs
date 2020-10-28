using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkInWater : MonoBehaviour
{
    public Spawner spawner;
    private Rigidbody2D body;
    private bool IsSinking = false;
    [Header("Settings")]
    [Range(0.0f, 20.0f)]public float TimeBeforeSink = 1.0f;
    [Range(0.0f, 20.0f)] public float SinkDuration = 1.0f;
    [Range(0.0f, 200.0f)] public float SinkForce = 1.0f;

    private void Awake()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsSinking)
        {
            body.AddForce(Vector2.down*SinkForce);
        }
    }

    IEnumerator Sink()
    {
        IsSinking = true;
        transform.position = transform.up * -10;
        yield return new WaitForSeconds(SinkDuration);
        //spawner.Instances.Remove(this.gameObject);
        //Destroy(this.gameObject);
    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            TimeBeforeSink -= Time.deltaTime;
            if (TimeBeforeSink < 0)
            {
                StartCoroutine(Sink());
            }
        }
    }

}
