using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform camPos;

    void Update()
    {
        transform.position = camPos.position;
    }
}
