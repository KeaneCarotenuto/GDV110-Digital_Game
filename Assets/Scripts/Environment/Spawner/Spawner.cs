using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject PrefabToSpawn;
    public GameObject Parent;
    public bool IsSpawning = false;
    public float SpawnDelay = 0.0f;
    public bool Repeat = false;
    public int MaxInstances = 1;
    public bool DestroyOld = true;
    public float XJitter = 0;
    public float YJitter = 0;
    public List<GameObject> Instances;
    float SpawnCountdown = 0.0f;
    TimeManager timeManager;

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
    public void Spawn()
    {
        if (Instances.Count < MaxInstances)
        {
            GameObject NewInstance = Instantiate(PrefabToSpawn, new Vector3(this.transform.position.x + Random.Range(-1 * XJitter, XJitter), this.transform.position.y + Random.Range(-1 * YJitter, YJitter), this.transform.position.z), this.transform.rotation, Parent.transform);
            Instances.Add(NewInstance);
            
        }
        else if(DestroyOld)
        {
            GameObject.Destroy(Instances[0]);
            Instances.RemoveAt(0);
            GameObject NewInstance = Instantiate(PrefabToSpawn, new Vector3(this.transform.position.x + Random.Range(-1 * XJitter, XJitter), this.transform.position.y + Random.Range(-1 * YJitter, YJitter), this.transform.position.z), this.transform.rotation, Parent.transform);
            Instances.Add(NewInstance);
        }
    }
}
