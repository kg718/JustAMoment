using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    private Transform holdPoint;

    public float size;
    public bool interactable = true;
    [SerializeField] private Collider[] objectColliders = new Collider[0];
    [SerializeField] private GameObject[] outlineObjects = new GameObject[0];
    [SerializeField] private MeshRenderer[] ghostObjects = new MeshRenderer[0];
    public AudioClip pickUpSound;
    public AudioClip placeSound;
    public string placementTag;
    public string changeTag = "None";
    public UnityEvent OnChange;

    [HideInInspector] public PlacementPoint currentPlacement;

    void Start()
    {
        ResetPlacementGhost();
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
        if (ghostObjects.Length <= 0) return;
        foreach (Renderer _renderer in ghostObjects)
        {
            _renderer.enabled = true;
        }
        ghostObjects[0].transform.position = _ghostPoint.position;
        ghostObjects[0].transform.rotation = _ghostPoint.rotation;
    }

    public void ResetPlacementGhost()
    {
        if(ghostObjects.Length <= 0) return;
        foreach (Renderer _renderer in ghostObjects)
        {
            _renderer.enabled = false;
        }
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

    public void SetGhostMaterial(Material _material)
    {
        if(ghostObjects.Length <= 0) return;
        foreach(Renderer _renderer in ghostObjects)
        {
            _renderer.material = _material;
        }
    }
}
