using UnityEngine;

[RequireComponent(typeof(HurtBox))]
public class HurtLaunch : MonoBehaviour
{
    HurtBox HurtBox => GetComponent<HurtBox>();
    [SerializeField][Range(5, 100)] float launchForce = 10;

    [SerializeField] Vector3 launchDirection = Vector3.up;

    public Vector3 LaunchDirection => transform.rotation * launchDirection.normalized;

    [SerializeField][Range(0, 1)] float launchDirectionWeight = 0.5f;
    [SerializeField][Range(0, 10)] float GizmosSize = 0.5f;

    private void Start()
    {
        HurtBox.OnHurt += Launch;
    }

    void Launch(Health health)
    {
        var launchable = health.GetComponent<Launchable>();
        if (launchable)
        {
            var direction = health.transform.position - transform.position;
            direction.Normalize();

            direction = Vector3.Lerp(direction, LaunchDirection, launchDirectionWeight);

            launchable.Launch(direction * launchForce);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + LaunchDirection * GizmosSize);
        Gizmos.DrawSphere(transform.position + LaunchDirection * GizmosSize, GizmosSize * 0.1f);
    }

}
