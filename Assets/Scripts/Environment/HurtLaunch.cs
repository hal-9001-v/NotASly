using UnityEngine;

[RequireComponent(typeof(HurtBox))]
public class HurtLaunch : MonoBehaviour
{
    HurtBox HurtBox => GetComponent<HurtBox>();
    [SerializeField] float launchForce;

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

            launchable.Launch(direction * launchForce);
        }
    }

}
