using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Splines;

public interface IPathInteractable
{
    public Path Path { get; }

    public Vector3 GetClosestPoint(Vector3 point);

}
