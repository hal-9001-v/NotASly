using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathFollower
{
    public float CheckDistance { get; }
    public void Attach();
    public void Dettach();
    public void Move();

    /// <summary>
    /// Returns ow far along the path the follower is. If it is out of range of the follower, it returns null
    /// </summary>
    /// <returns></returns>
    public IPathInteractable GetClosestPath();

    public Vector3 GetClosestPoint();
}
