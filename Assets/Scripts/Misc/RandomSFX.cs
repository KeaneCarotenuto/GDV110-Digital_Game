using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSFX : MonoBehaviour
{
    [Header("Audio Clips")]
    public List<AudioClip> Clips;
    [Header("Collision")]
    public bool PlayOnCollision = false;
    public List<string> CollisionTags;
    [Header("Audio Settings")]
    [Range(0.0f, 1.0f)]public float Volume = 1f;
    [Range(-2.0f, 2.0f)] public float Pitch = 1f;

    IEnumerator PlaySound()
    {
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.volume = Volume;
        audio.pitch = Pitch;
        audio.clip = Clips[Random.Range(0, Clips.Count)];
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        Destroy(audio);
    }

    public void Play()
    {
        StartCoroutine(PlaySound());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(CollisionTags.Contains(collision.gameObject.tag))
        {
            Play();
        }
    }
}
