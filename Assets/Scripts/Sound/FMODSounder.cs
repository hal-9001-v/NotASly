using UnityEngine;
using FMODUnity;

public class FMODSounder : MonoBehaviour
{
    [SerializeField]EventReference soundReference;

    public void PlaySound()
    {
        SoundManager.Instance.PlaySound(soundReference, transform.position);
    }

}
