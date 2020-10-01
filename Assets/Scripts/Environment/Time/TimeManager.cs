using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public PlayerControls ctrl;
    public AudioSource music;


    public float timeScale = 1f, timeReverseSpeed = 1;
    private float timeScale_MIN = -1f, timeScale_MAX = 1f;

    public bool timeIsReversed, powerActive;



    // Start is called before the first frame update
    void Awake()
    {
        ctrl = new PlayerControls();
        timeIsReversed = false;
        ctrl.Player.ReverseTime.started += ctx => enablePower();
        ctrl.Player.ReverseTime.canceled += ctx => disablePower();
        ctrl.Enable();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeScale - 0.001f*timeReverseSpeed > timeScale_MIN && powerActive) { timeScale -= 0.001f * timeReverseSpeed; }
        else if (timeScale + 0.001f * timeReverseSpeed < timeScale_MAX && !powerActive) { timeScale += 0.001f * timeReverseSpeed; }
        if (timeScale < 0) { timeIsReversed = true; }
        else { timeIsReversed = false; }
        music.pitch = timeScale;
        Time.timeScale = System.Math.Abs(timeScale);
    }

    private void FixedUpdate()
    {
        
    }

    private void enablePower()
    {
        powerActive = true;
    }
    private void disablePower()
    {
        powerActive = false;
    }
}
