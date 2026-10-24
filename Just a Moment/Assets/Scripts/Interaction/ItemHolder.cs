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

    [SerializeField] private AudioClip placeSound;
    [SerializeField] private GameObject placeEffects;

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
                    if(_object.interactable)
                    {
                        UpdateInteractionTarget(_object);
                    }
                } 

            }
            else if (_hit.collider.gameObject.layer == LayerMask.NameToLayer("Placement") && holdingItem)
            {
                heldItem.SetPlacementGhost(_hit.collider.transform);
                UpdateInteractionTarget(emptyInteraction);
                if (_hit.collider.TryGetComponent<PlacementPoint>(out PlacementPoint _placement))
                {
                    if(_placement.CheckPlacementAbility(heldItem))
                    {
                        heldItem.SetGhostMaterial(MaterialManager.Instance.ghostMat);
                    }
                    else
                    {
                        heldItem.SetGhostMaterial(MaterialManager.Instance.wrongGhostMat);
                    }
                }
            }
            else if (_hit.collider.gameObject.layer == LayerMask.NameToLayer("Changeable") && holdingItem)
            {
                if (_hit.collider.TryGetComponent<ChangeInteraction>(out ChangeInteraction _changer))
                {

                }
            }
            else
            {
                UpdateInteractionTarget(emptyInteraction);
                if (holdingItem)
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
                    if (_object.interactable)
                    {
                        _object.SetHoldPoint(transform);
                        _object.SetCollision(false);
                        heldItem = _object;
                        if (heldItem.pickUpSound != null)
                        {
                            SoundEffectManager.Instance.PlaySoundEffect(heldItem.pickUpSound, 0.8f, 1.1f);
                        }
                        heldItem.transform.parent = null;
                        holdingItem = true;
                        if(_object.currentPlacement != null)
                        {
                            _object.currentPlacement.CompletePoint(false);
                            TimeLineManager.instance.CheckLevelCompletion();
                        }
                    }
                }
            }
            else if (holdingItem && _hit.collider.gameObject.layer == LayerMask.NameToLayer("Placement"))
            {
                if (_hit.collider.gameObject.TryGetComponent<PlacementPoint>(out PlacementPoint _placement))
                {
                    if(_placement.CheckPlacementAbility(heldItem))
                    {
                        SoundEffectManager.Instance.PlaySoundEffect(placeSound);
                        Instantiate(placeEffects, _hit.collider.transform.position, Quaternion.identity);
                        heldItem.SetHoldPoint(_placement.transform);
                        //_placement.SetCollision(false);
                        heldItem.SetCollision(true);
                        heldItem.transform.parent = TimeLineManager.instance.timelines[TimeLineManager.instance.currentTimeline].transform;
                        heldItem.ResetPlacementGhost();
                        heldItem.currentPlacement = _placement;
                        _placement.CompletePoint(true);
                        if (heldItem.placeSound != null)
                        {
                            SoundEffectManager.Instance.PlaySoundEffect(heldItem.placeSound, 0.8f, 1.1f);
                        }
                        TimeLineManager.instance.CheckLevelCompletion();
                        if (_placement.lockObjectIn)
                        {
                            heldItem.interactable = false;
                        }
                        heldItem = null;
                        holdingItem = false;
                    }
                }
            }
            else if (holdingItem && _hit.collider.gameObject.layer == LayerMask.NameToLayer("Changeable"))
            {
                if (_hit.collider.gameObject.TryGetComponent<ChangeInteraction>(out ChangeInteraction _changer))
                {
                    _changer.Change(heldItem);
                }
            }
        }
    }

    public void OnDrop()
    {
        if (!holdingItem) return;
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
            heldItem.transform.position = new Vector3(_hit.point.x, _hit.point.y + heldItem.size / 2f, _hit.point.z);
        }
        heldItem.transform.localRotation = Quaternion.identity;
        heldItem.ResetPlacementGhost();
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
