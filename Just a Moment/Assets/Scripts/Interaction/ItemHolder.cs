using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private Transform rayCastOrigin;
    [SerializeField] private float interactionRange;

    protected bool holdingItem = false;
    protected Interactable heldItem;

    private Interactable interactionTarget;
    private Interactable emptyInteraction;

    void Start()
    {
        emptyInteraction = new Interactable();
        interactionTarget = emptyInteraction;
    }

    void Update()
    {
        //Debug.DrawRay(rayCastOrigin.position, transform.forward, Color.red, interactionRange);

        if (Physics.Raycast(rayCastOrigin.position, transform.forward, out RaycastHit _hit, interactionRange, interactableLayers))
        {
            if(_hit.collider.gameObject.layer == LayerMask.NameToLayer("PickUp"))
            {
                if(_hit.collider.TryGetComponent<Interactable>(out Interactable _object))
                {
                    UpdateInteractionTarget(_object);
                } 

            }
            else if (_hit.collider.gameObject.layer == LayerMask.NameToLayer("Placement") && holdingItem)
            {
                heldItem.SetPlacementGhost(_hit.collider.transform);
                UpdateInteractionTarget(emptyInteraction);
                //if (_hit.collider.TryGetComponent<PlacementPoint>(out PlacementPoint _placement))
                //{

                //}
            }
            else
            {
                UpdateInteractionTarget(emptyInteraction);
                if(holdingItem)
                {
                    heldItem.ResetPlacementGhost();
                }
            }

        }
        else
        {
            UpdateInteractionTarget(emptyInteraction);
            if(holdingItem)
            {
                heldItem.ResetPlacementGhost();
            }
        }
    }

    public void OnInteract()
    {
        if(Physics.Raycast(rayCastOrigin.position, transform.forward, out RaycastHit _hit, interactionRange, interactableLayers))
        {
            if(!holdingItem && _hit.collider.gameObject.layer == LayerMask.NameToLayer("PickUp"))
            {
                if (_hit.collider.gameObject.TryGetComponent<Interactable>(out Interactable _object))
                {
                    _object.SetHoldPoint(transform);
                    _object.SetCollision(false);
                    heldItem = _object;
                    heldItem.transform.parent = null;
                    holdingItem = true;
                }
            }
            else if (holdingItem && _hit.collider.gameObject.layer == LayerMask.NameToLayer("Placement"))
            {
                if (_hit.collider.gameObject.TryGetComponent<PlacementPoint>(out PlacementPoint _placement))
                {
                    heldItem.SetHoldPoint(_placement.transform);
                    //_placement.SetCollision(false);
                    heldItem.SetCollision(true);
                    heldItem.transform.parent = TimeLineManager.instance.timelines[TimeLineManager.instance.currentTimeline].transform;
                    heldItem = null;
                    holdingItem = false;
                }
            }
        }
    }

    public void OnDrop()
    {
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit _hit);
        Vector3 _dir = _hit.point - rayCastOrigin.position;
        _dir = new Vector3(_dir.x, _hit.point.y, _dir.z).normalized;


        heldItem.SetCollision(true);
        heldItem.transform.parent = TimeLineManager.instance.timelines[TimeLineManager.instance.currentTimeline].transform;
        heldItem.ResetHoldPoint();
        if (Physics.Raycast(rayCastOrigin.position, _dir, out RaycastHit _adjustedHit, heldItem.size + 1f))
        {
            heldItem.transform.position = _adjustedHit.point;
        }
        else
        {
            heldItem.transform.position = new Vector3(_hit.point.x, _hit.point.y + heldItem.size / 2f, _hit.point.z) ;
        }
        heldItem.transform.localRotation = Quaternion.identity;
        heldItem = null;
        holdingItem = false;
    }

    private void UpdateInteractionTarget(Interactable _interaction)
    {
        interactionTarget.SetOutline(false);
        interactionTarget = _interaction;
        interactionTarget.SetOutline(true);
    }
}
