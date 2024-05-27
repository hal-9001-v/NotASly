using UnityEngine;

public class WallSticker : MonoBehaviour, IPathFollower
{
    public bool Attatched => currentWall != null;

    public float CheckDistance => checkDistance;

    [SerializeField][Range(0, 5)] float checkDistance = 1.5f;
    [SerializeField][Range(0, 5)] float offset;

    [SerializeField][Range(0, 10)] float speed;

    WallStick currentWall;

    WallStick[] Walls => FindObjectsOfType<WallStick>();

    float t;

    public void Attach()
    {
        currentWall = GetClosestPath() as WallStick;
    }

    public bool Check()
    {
        return GetClosestPath() != null;
    }

    public void Dettach()
    {
        currentWall = null;
    }

    public IPathInteractable GetClosestPath()
    {
        WallStick closest = null;
        float closestDistance = float.PositiveInfinity;
        foreach (var wall in Walls)
        {
            var distance = Vector3.Distance(wall.GetClosestPoint(transform.position), transform.position);
            if (distance < checkDistance && distance < closestDistance)
            {
                closest = wall;
                closestDistance = distance;
            }
        }

        return closest;
    }

    public Vector3 GetClosestPoint()
    {
        return currentWall.GetClosestPoint(transform.position);
    }

    public void Move(Vector2 input, bool pressing, Vector3 direction)
    {
        if (!Attatched)
            return;

        if (pressing == false)
        {
            Dettach();
            return;
        }

        if (direction.magnitude > 0.1f)
            transform.position = currentWall.Path.GetPosition(t, out t, direction, speed * Time.deltaTime) + Vector3.up * offset;
        else
            transform.position = currentWall.Path.GetPosition(t) + Vector3.up * offset;
    }
}
