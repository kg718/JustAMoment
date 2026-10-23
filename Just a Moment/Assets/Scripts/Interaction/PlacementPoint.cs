using UnityEngine;

public class PlacementPoint : MonoBehaviour
{
    public int timeLine;
    public Interactable startingObject;
    public bool lockObjectIn;
    [SerializeField] private Collider[] placementColliders = new Collider[0];
    public string[] placementTags = new string[0];

    [HideInInspector] public bool completed = false;

    void Start()
    {
        if(startingObject != null)
        {
            startingObject.SetHoldPoint(transform);
        }
    }

    void Update()
    {

    }

    public void SetCollision(bool _collisionOn)
    {
        if (placementColliders.Length <= 0) return;
        foreach (Collider _collider in placementColliders)
        {
            _collider.enabled = _collisionOn;
        }
    }

    public bool CheckPlacementAbility(Interactable _object)
    {
        if(placementTags.Length <= 0)
        {
            return true;
        }
        foreach (string _tag in placementTags)
        {
            if(_tag == _object.placementTag)
            {
                return true;
            }
        }
        return false;
    }

    public void CompletePoint(bool _completion)
    {
        completed = _completion;
    }
}
