using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Transform holdPoint;

    public float size;
    [SerializeField] private Collider[] objectColliders = new Collider[0];
    [SerializeField] private GameObject[] outlineObjects = new GameObject[0];
    [SerializeField] private GameObject ghostObject;
    public string[] placementTags = new string[0];

    void Start()
    {
        
    }

    void Update()
    {
        if (holdPoint == null) return;
        transform.position = holdPoint.position;
        transform.rotation = holdPoint.rotation;
    }

    public void SetHoldPoint(Transform _holdPoint)
    {
        holdPoint = _holdPoint;
    }

    public void SetPlacementGhost(Transform _ghostPoint)
    {
        if (ghostObject == null) return;
        ghostObject.SetActive(true);
        ghostObject.transform.position = _ghostPoint.position;
        ghostObject.transform.rotation = _ghostPoint.rotation;
    }

    public void ResetPlacementGhost()
    {
        if(ghostObject == null) return;
        ghostObject.SetActive(false);
    }

    public void ResetHoldPoint()
    {
        holdPoint = null;
    }

    public void SetCollision(bool _collisionOn)
    {
        if (objectColliders.Length <= 0) return;
        foreach (Collider _collider in objectColliders)
        {
            _collider.enabled = _collisionOn;
        }
    }

    public void SetOutline(bool _outlineOn)
    {
        if (outlineObjects.Length <= 0) return;
        foreach (GameObject _ouline in outlineObjects)
        {
            _ouline.SetActive(_outlineOn);
        }
    }
}
