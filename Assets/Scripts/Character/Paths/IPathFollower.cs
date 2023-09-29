using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathFollower
{
    public bool Attatched { get; }
    public float CheckDistance { get; }
    public void Attach();
    public void Dettach();
    public bool Check();
    public void Move(Vector2 input, Vector3 direction);

    /// <summary>
    /// Returns ow far along the path the follower is. If it is out of range of the follower, it returns null
    /// </summary>
    /// <returns></returns>
    public IPathInteractable GetClosestPath();

    public Vector3 GetClosestPoint();
}
