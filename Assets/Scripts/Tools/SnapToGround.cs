using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToGround : MonoBehaviour, IRespawnable
{
    [SerializeField] Collider Collider;

    Vector3 respawnPosition;
    Quaternion respawnRotation;

    private void Awake()
    {
        respawnPosition = transform.position;
        respawnRotation = transform.rotation;

        Respawn();
    }

    [ContextMenu("Snap")]
    public void Snap()
    {
        if (Collider == null)
        {
            Collider = GetComponent<Collider>();
        }

        if (Collider == null)
        {
            return;
        }

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100f))
        {
            var snap = hit.transform.GetComponent<SnapToGround>();
            if (snap != null)
            {
                snap.Snap();
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f))
                    transform.position = hit.point + Collider.bounds.size.y * 0.5f * Vector3.up;
            }
            else
            {
                transform.position = hit.point + Collider.bounds.size.y * 0.5f * Vector3.up;
            }

        }
    }

    public void Respawn()
    {
        transform.position = respawnPosition;
        transform.rotation = respawnRotation;

        Snap();
    }
}
