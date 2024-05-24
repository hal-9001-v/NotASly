using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public DestroyType destroyType;

    public enum DestroyType
    {
        Destroy,
        Disable,
        SetActiveFalse,
        Respawn
    }

    public void Destroy()
    {
        switch (destroyType)
        {
            case DestroyType.Destroy:
                Destroy(gameObject);
                break;
            case DestroyType.Disable:
                foreach (var renderer in GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = false;
                }

                foreach (var collider in GetComponentsInChildren<Collider>())
                {
                    collider.enabled = false;
                }

                foreach (var rigidbody in GetComponentsInChildren<Rigidbody>())
                {
                    rigidbody.isKinematic = true;
                }

                foreach (var animator in GetComponentsInChildren<Animator>())
                {
                    animator.enabled = false;
                }

                foreach (var script in GetComponents<MonoBehaviour>())
                {
                    script.enabled = false;
                }

                break;

            case DestroyType.SetActiveFalse:
                gameObject.SetActive(false);
                break;

            case DestroyType.Respawn:
                var respawnables = GetComponents<Component>();
                foreach (var respawnable in respawnables)
                {
                    if (respawnable is IRespawnable)
                    {
                        (respawnable as IRespawnable).Respawn();
                    }

                }
                break;
        }

    }

}
