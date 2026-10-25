using UnityEngine;

public class ChangeInteraction : MonoBehaviour
{
    [SerializeField] private string changeTag;
    public GameObject[] outlineObjects = new GameObject[0];

    public void Change(Interactable _object)
    {
        if(_object.changeTag == changeTag)
        {
            _object.OnChange?.Invoke();
        }
    }
}
