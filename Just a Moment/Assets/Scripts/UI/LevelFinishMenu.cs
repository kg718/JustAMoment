using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinishMenu : MonoBehaviour
{
    public static LevelFinishMenu Instance;
    [SerializeField] private GameObject menu;

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        
    }

    public void ActivateFinishPanel()
    {
        menu.SetActive(true);
    }

    public void OnClickReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
