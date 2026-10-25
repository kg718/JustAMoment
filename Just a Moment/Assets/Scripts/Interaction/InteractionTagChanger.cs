using UnityEngine;

public class InteractionTagChanger : MonoBehaviour
{
    [SerializeField] private string newTag;
    private Interactable interactableObject;

    private void Start()
    {
        interactableObject = GetComponent<Interactable>();
    }

    public void ChangeTag()
    {
        interactableObject.placementTag = newTag;
    }
}
