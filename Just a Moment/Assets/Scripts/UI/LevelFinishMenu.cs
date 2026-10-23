using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinishMenu : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnClickReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
