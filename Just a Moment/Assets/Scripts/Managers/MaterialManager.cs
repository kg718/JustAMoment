using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager Instance;

    public Material ghostMat;
    public Material wrongGhostMat;

    private void Start()
    {
        Instance = this;
    }
}
