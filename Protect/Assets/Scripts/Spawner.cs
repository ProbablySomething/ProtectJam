using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float rateOfSpawns = 2f;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private float maxDistanceToSpawn = 1;

    private List<GameObject> spawns;
    private List<GameObject> spawnsToDestroy;
    private float currentSpawnTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        spawns = new List<GameObject>();
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        currentSpawnTime += Time.deltaTime;
        if(currentSpawnTime > rateOfSpawns)
        {
            currentSpawnTime = 0;
            Spawn();
        }
    }

    private void Spawn()
    {
        spawns.Add(Instantiate(objectToSpawn, (Vector2)transform.position + GetRandomVector(), Quaternion.identity));
    }

    private Vector2 GetRandomVector()
    {
        return Random.insideUnitCircle.normalized * Random.Range(0,maxDistanceToSpawn);
    }

    public void Die()
    {
        spawnsToDestroy = new List<GameObject>();
        foreach(GameObject spawn in spawns)
        {
            spawnsToDestroy.Add(spawn);
            
            Destroy(spawn);
        }
        foreach(GameObject destroyable in spawnsToDestroy)
        {
            spawns.Remove(destroyable);
        }
    }
}
