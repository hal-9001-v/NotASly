using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Venter : MonoBehaviour
{
    public bool IsInside;
    public bool IsInEntry;

    Mover mover => GetComponent<Mover>();
    
    public bool Check()
    {
        if (IsInEntry)
        {
            return true;
        }

        return false;
    }

    public void Move(Vector3 direction)
    {
        mover.Move(direction);
    }
}
