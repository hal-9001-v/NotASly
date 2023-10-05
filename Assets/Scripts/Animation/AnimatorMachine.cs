using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorMachine : MonoBehaviour
{
    Animator Animator => GetComponent<Animator>();

    public string CurrentState { get; private set; }

    public const float DefaultTransitionTime = 0.2f;
    public const int DefaultLayer = 0;

    public void ChangeState(string changeState, float transitionTime = DefaultTransitionTime, int layer = DefaultLayer)
    {
        if (CurrentState == changeState) return;

        Animator.CrossFadeInFixedTime(changeState, transitionTime);
        Animator.CrossFadeInFixedTime(changeState, transitionTime, layer);

        CurrentState = changeState;
    }

    public void ChangeFloat(string changeFloat, float value)
    {
        Animator.SetFloat(changeFloat, value);
    }

    public void ChangeLayerWeight(int layer, float weight)
    {
        Animator.SetLayerWeight(layer, weight);
    }
}
