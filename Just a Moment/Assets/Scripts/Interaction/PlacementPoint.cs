using UnityEngine;

public class PlacementPoint : ItemHolder
{
    public int timeLine;
    public Interactable startingObject;

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
}
