using UnityEngine;
using UnityEngine.Events;

public class HeigtChecker : MonoBehaviour
{
    public bool AutoCheck = true;
    [SerializeField][Range(1, 50)] float killingHeight = 10;

    public UnityEvent OnHeight;

    [SerializeField] LayerMask groundMask;

    private void Update()
    {
        if (AutoCheck)
        {
            CheckHeight();
        }
    }

    public void CheckHeight()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, killingHeight, groundMask) == false)
        {
            OnHeight?.Invoke();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * killingHeight);
    }
}
