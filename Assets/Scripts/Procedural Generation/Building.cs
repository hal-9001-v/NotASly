using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] Transform[] corners;
    [SerializeField] WallGenerator wallGenerator;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (var corner in corners)
        {
            wallGenerator.AddCorner(corner.position);
        }

        wallGenerator.Walls();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
