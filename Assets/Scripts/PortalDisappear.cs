using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalDisappear : MonoBehaviour
{
    public float timer;
    public float timerLimitFade = 1f;
    public float timerLimitDestroy = 2f;
    public bool fading;
    public ParticleSystem main;
    public ParticleSystem rune;
    public AudioSource audioSauce;
    public AudioClip fadeSound;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timerLimitFade)
        {
            main.Stop();
            rune.Stop();
            if(fading != true)
            {
                audioSauce.PlayOneShot(fadeSound);
            }
            fading = true;
            
        }

        if (timer >= timerLimitDestroy)
        {
            Destroy(gameObject);
        }
    }
}
