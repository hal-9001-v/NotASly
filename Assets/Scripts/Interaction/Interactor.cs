using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField][Range(0, 2)] float directInteractionRange = 1;
    [SerializeField] InteractorTag interactorTag;
    public InteractorTag InteractorTag => interactorTag;

    DirectInteractable[] DirectInteractables => FindObjectsOfType<DirectInteractable>();

    public bool ActiveInteraction()
    {
        foreach (var interactable in DirectInteractables)
        {
            if (Vector3.Distance(transform.position, interactable.transform.position) < directInteractionRange)
            {
                interactable.Interact(this);
                return true;
            }
        }

        return false;
    }

}
