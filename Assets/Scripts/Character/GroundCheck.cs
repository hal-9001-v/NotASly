using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] LayerMask groundMask;
    [SerializeField][Range(0, 10)] float raycastCheckDistance = 0.5f;

    [SerializeField][Range(0, 10)] float sphereCheckDistance = 0.5f;
    [SerializeField][Range(0, 10)] float checkRadius = 0.5f;
    public float YSpeed { get; private set; }
    Collider[] colliders;

    private void Awake()
    {
        colliders = new Collider[1];
    }

    public bool IsGrounded
    {
        get
        {
            if (YSpeed <= 0)
            {
                if (Physics.Raycast(transform.position, Vector3.down, out var hit, raycastCheckDistance, groundMask))
                    return true;

                if (Physics.OverlapSphereNonAlloc(transform.position + Vector3.down * sphereCheckDistance, checkRadius, colliders, groundMask) > 0)
                    return true;

            }

            return false;
        }
    }

    public void SetYSpeed(float speed)
    {
        YSpeed = speed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastCheckDistance);
        Gizmos.DrawWireSphere(transform.position + Vector3.down * sphereCheckDistance, checkRadius);
    }

}
