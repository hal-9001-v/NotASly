using UnityEngine;

public class Roper : MonoBehaviour, IPathFollower
{
    [SerializeField][Range(1, 10)] float speed;
    [SerializeField][Range(0, 5)] float checkDistance;

    public Vector3 RopePosition => currentRope.Path.GetPosition(t);

    Rope[] ropes => FindObjectsOfType<Rope>();

    public bool Attatched => currentRope != null;

    public float CheckDistance => checkDistance;

    Rope currentRope;
    float t;

    public bool Check()
    {
        var closest = GetClosestPath();
        if (closest != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Move(Vector2 input, bool pressing, Vector3 direction)
    {
        if (!Attatched)
            return;

        if (direction.magnitude > 0.1f)
            transform.position = currentRope.Path.GetPosition(t, out t, direction, speed * Time.deltaTime);
        else
            transform.position = currentRope.Path.GetPosition(t);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, checkDistance);
    }

    public void Attach()
    {
        currentRope = (Rope)GetClosestPath();
        t = currentRope.Path.GetClosestT(transform.position);
    }

    public void Dettach()
    {
        currentRope = null;
    }

    public IPathInteractable GetClosestPath()
    {
        Rope closest = null;
        float closesDistance = float.MaxValue;
        foreach (var rope in ropes)
        {
            var distance = Vector3.Distance(transform.position, rope.GetClosestPoint(transform.position));
            if (distance < checkDistance && distance < closesDistance)
            {
                closesDistance = distance;
                closest = rope;
            }
        }

        return closest;
    }

    public Vector3 GetClosestPoint()
    {
        return currentRope.GetClosestPoint(transform.position);
    }
}
