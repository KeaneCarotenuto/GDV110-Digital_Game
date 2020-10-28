using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    
    public PlayerControls ctrl;
    [Header("References")]
    public AudioSource music;
    public AudioSource ambience;
    [Header("Events")]
    public UnityEvent OnTimeReverse;
    public UnityEvent OnTimeNormalise;

    [Header("Time Configuration")]
    [Tooltip("The current speed of time between 1 (normal) and -1 (reversed)")] [Range(-1.0f, 1.0f)] public float timeScale = 1.0f;
    [Tooltip("The speed time reverses at while ability is active")] [Range(10.0f, 10.0f)] public float timeReverseSpeed = 10;
    [Tooltip("The speed at which time returns to normal when the ability is not active")] [Range(10.0f, 10.0f)] public float timeNormaliseSpeed = 10;
    [Header("Player Ability Configuration")]
    [Range(0f, 100f)] public float abilityCost = 10;
    [Range(0f, 100f)] public float sandRegenRate = 1;
    [Range(0f, 100f)] public float currentSand;
    private float timeScale_MIN = -1f, timeScale_MAX = 1f, sand_MIN = 0f, sand_MAX = 100f;

    [Header("Is")]
    public bool timeReversed, powerActive, sandRemaining;
    





    // Start is called before the first frame update
    void Awake()
    {
        ctrl = new PlayerControls();
        timeReversed = false;
        currentSand = sand_MAX;
        ctrl.Player.ReverseTime.started += ctx => enablePower();
        ctrl.Player.ReverseTime.canceled += ctx => disablePower();
        ctrl.Enable();
        
    }

    private void Start()
    {
        if (!music && GameObject.Find("MusicManager")) {
            music = GameObject.Find("MusicManager").GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!music && GameObject.Find("MusicManager"))
        {
            Debug.Log("Finding music");
            music = GameObject.Find("MusicManager").GetComponent<AudioSource>();
        }

        if (timeScale - 0.001f*timeReverseSpeed > timeScale_MIN && powerActive) { timeScale -= 0.001f * timeReverseSpeed; }
        else if (timeScale + 0.001f * timeNormaliseSpeed < timeScale_MAX && !powerActive) { timeScale += 0.001f * timeNormaliseSpeed; }
        if (timeScale < 0 && (!timeReversed)) 
        { 
            timeReversed = true;
            OnTimeReverse.Invoke();
        }
        else if(timeScale > 0 && timeReversed)
        { 
            timeReversed = false;
            OnTimeNormalise.Invoke();
        }
        if (music != null) music.pitch = timeScale;
        ambience.pitch = timeScale;
        Time.timeScale = System.Math.Abs(timeScale);
    }

    private void FixedUpdate()
    {
        if (!powerActive && currentSand < 100f)
        {
            currentSand += sandRegenRate/100f;
        }
        else if(powerActive)
        {
            currentSand -= abilityCost/100f;
        }
        if (currentSand <= 0)
        {
            sandRemaining = false;
            currentSand = 0;
            disablePower();
        }
        else
        {
            sandRemaining = true;
            
            if(currentSand > 100f)
            {
                currentSand = 100f;
            }
        }
    }

    private void enablePower()
    {
        if (sandRemaining)
        {
            powerActive = true;
        }
    }
    private void disablePower()
    {
        powerActive = false;
    }
}
