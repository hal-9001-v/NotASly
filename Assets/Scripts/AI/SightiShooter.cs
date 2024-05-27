using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightiShooter : MonoBehaviour
{
    [SerializeField] Spawner spawner;
    [SerializeField] SightSensor sightSensor;

    // Start is called before the first frame update
    void Start()
    {
        sightSensor.OnSense += Shoot;
    }


    private void Shoot(BaseSenseTrigger trigger)
    {
        spawner.SpawnObject();
    }
}
