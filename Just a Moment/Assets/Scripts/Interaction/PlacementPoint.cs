using UnityEngine;

public class PlacementPoint : MonoBehaviour
{
    public int timeLine;
    public Interactable startingObject;
    [SerializeField] private Collider[] placementColliders = new Collider[0];
    public string[] placementTags = new string[0];

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
}
