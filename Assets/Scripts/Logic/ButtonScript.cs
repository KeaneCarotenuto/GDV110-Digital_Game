using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    private GameObject b_press;
    private GameObject b_base;

    public bool output;


    // Start is called before the first frame update
    void Start()
    {
        b_press = transform.Find("Press").gameObject;
        b_base = transform.Find("Base").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(b_press.transform.position, b_base.transform.position) <= 0.1f)
        {
            output = true;
            b_base.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            output = false;
            b_base.GetComponent<SpriteRenderer>().color = Color.white;
        }

        
    }
}


[ExecuteInEditMode]
public class DrawLine : MonoBehaviour
{
    
}

