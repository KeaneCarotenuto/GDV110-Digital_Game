﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Object")]
    public GameObject PrefabToSpawn;
    public GameObject Parent;
    [Header("Spawn Settings")]
    public bool IsSpawning = false;
    public float SpawnDelay = 0.0f;
    public bool Repeat = false;
    public int MaxInstances = 1;
    public bool DestroyOld = true;
    [Header("Water")]
    public bool SinksInWater = false;
    [Range(0.0f, 20.0f)] public float TimeBeforeSink = 1.0f;
    [Range(0.0f, 20.0f)] public float SinkDuration = 1.0f;
    [Range(0.0f, 200.0f)] public float SinkForce = 1.0f;
    [Header("Spawn Position")]
    public float XJitter = 0;
    public float YJitter = 0;
    [Header("Current Instances")]
    public List<GameObject> Instances;
    float SpawnCountdown = 0.0f;
    TimeManager timeManager;
    public ParticleSystem main;
    public ParticleSystem rune;
    public AudioSource audioSauce;
    public bool playerNear;

    // Start is called before the first frame update
    void Start()
    {
        timeManager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSpawning && !timeManager.timeReversed)
        {
            SpawnCountdown += Time.deltaTime;
            if (SpawnCountdown > SpawnDelay)
            {
                SpawnCountdown -= SpawnDelay;
                Spawn();
                IsSpawning = Repeat;

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Jumper"))
        {
            playerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Jumper"))
        {
            playerNear = false;
        }
    }

    public void Spawn()
    {
        if (Instances.Count < MaxInstances)
        {
            GameObject NewInstance = Instantiate(PrefabToSpawn, new Vector3(this.transform.position.x + Random.Range(-1 * XJitter, XJitter), this.transform.position.y + Random.Range(-1 * YJitter, YJitter), this.transform.position.z), this.transform.rotation, Parent.transform);
            main.Play();
            rune.Play();

            if (playerNear == true)
            {
            audioSauce.Play();
            }

            if(SinksInWater)
            {
                NewInstance.layer = 10;
            }
            Instances.Add(NewInstance);
            
        }
        else if(DestroyOld)
        {
            GameObject.Destroy(Instances[0]);
            Instances.RemoveAt(0);
            GameObject NewInstance = Instantiate(PrefabToSpawn, new Vector3(this.transform.position.x + Random.Range(-1 * XJitter, XJitter), this.transform.position.y + Random.Range(-1 * YJitter, YJitter), this.transform.position.z), this.transform.rotation, Parent.transform);
            main.Play();
            rune.Play();

            if (playerNear == true)
            {
            audioSauce.Play();
            }

            if (SinksInWater)
            {
                NewInstance.layer = 10;
            }
            Instances.Add(NewInstance);
        }
    }
}
