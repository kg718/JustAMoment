using UnityEngine;

public class DestroyOnTimer : MonoBehaviour
{
    public float destroyTime = 1f;
    [SerializeField] private bool useUnscaledTime;
    private float unscaledTime;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        if (!useUnscaledTime) return;
        unscaledTime += Time.unscaledDeltaTime;

        if(unscaledTime >= destroyTime) Destroy(gameObject);
    }
}
