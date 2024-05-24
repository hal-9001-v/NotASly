using UnityEngine;

public class Piper : MonoBehaviour, IPathFollower
{
    [SerializeField][Range(1, 10)] float speed;
    [SerializeField][Range(0, 5)] float checkDistance;

    public Vector3 PipePosition => currentPipe.Path.GetPosition(t);

    Pipe[] Pipes => FindObjectsOfType<Pipe>();

    public bool Attatched => currentPipe != null;

    public float CheckDistance => checkDistance;

    Pipe currentPipe;
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

        if (Mathf.Abs(input.y) > 0.1f)
        {
            if (input.y > 0)
                t = currentPipe.Path.GetTWithDisplacement(t, speed * Time.fixedDeltaTime);
            else
                t = currentPipe.Path.GetTWithDisplacement(t, -speed * Time.fixedDeltaTime);
        }

        transform.position = currentPipe.Path.GetPosition(t);

        //TODO: Fix this
        var rotation = Quaternion.LookRotation(transform.forward, currentPipe.Path.GetTangent(t));
        transform.rotation = rotation;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, checkDistance);
    }

    public void Attach()
    {
        currentPipe = (Pipe)GetClosestPath();
        t = currentPipe.Path.GetClosestT(transform.position);
    }

    public void Dettach()
    {
        currentPipe = null;
    }

    public IPathInteractable GetClosestPath()
    {
        IPathInteractable closestPath = null;
        float closestDistance = Mathf.Infinity;
        foreach (var pipe in Pipes)
        {
            var closest = pipe.GetClosestPoint(transform.position);
            Debug.DrawLine(transform.position, closest, Color.red);

            var distance = Vector3.Distance(closest, transform.position);
            if (distance < checkDistance && distance < closestDistance)
            {
                closestPath = pipe;
                closestDistance = distance;
            }

        }

        return closestPath;
    }

    public Vector3 GetClosestPoint(Vector3 point)
    {
        return currentPipe.GetClosestPoint(point);
    }

    public Vector3 GetClosestPoint()
    {
        return currentPipe.GetClosestPoint(transform.position);
    }
}
