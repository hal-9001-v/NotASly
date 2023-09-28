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

    Vector3 direction;

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

    public void Move(Vector3 direction)
    {
        this.direction = direction;
    }

    private void FixedUpdate()
    {
        if (currentRope == null)
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

    public void RawMove(Vector2 input)
    {
        throw new System.NotImplementedException();
    }
}
