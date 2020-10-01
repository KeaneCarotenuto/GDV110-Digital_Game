using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public PlayerControls ctrl;

    public float timeScale;
    public bool timeReversed;



    // Start is called before the first frame update
    void Start()
    {
        timeReversed = false;
        timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
