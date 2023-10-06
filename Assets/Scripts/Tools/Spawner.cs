using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Spawn[] spawns;

    public void SpawnObjects(int ammount)
    {
        for (int i = 0; i < ammount; i++)
        {
            SpawnObject();
        }
    }

    public void SpawnObject()
    {
        int totalTickets = 0;
        foreach (Spawn spawn in spawns)
        {
            totalTickets += spawn.tickets;
        }

        int spawnIndex = UnityEngine.Random.Range(0, totalTickets);

        for (int j = 0; j < spawns.Length; j++)
        {
            spawnIndex -= spawns[j].tickets;
            if (spawnIndex <= 0)
            {
                var spawnable = Instantiate(spawns[j].prefab, transform.position, Quaternion.identity);
                spawnable.Spawn();
                return;
            }
        }
    }

    [Serializable]
    class Spawn
    {
        public Spawnable prefab;
        public int tickets;
    }
}
