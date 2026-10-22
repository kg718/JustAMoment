using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSetup : MonoBehaviour
{

    void Start()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    void Update()
    {
        
    }
}
