using TMPro;
using UnityEngine;

public class TimeLineManager : MonoBehaviour
{
    public static TimeLineManager instance;

    public GameObject[] timelines = new GameObject[0];
    [HideInInspector] public int currentTimeline = 0;
    [SerializeField] private PlacementPoint[] requiredPlacements = new PlacementPoint[0];
    [SerializeField] private Animator UITransition;

    [SerializeField] private TextMeshProUGUI timelineText;

    private bool onCoolDown = false;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        
    }

    public void OnSwitchTimeline()
    {
        if (onCoolDown) return;
        if (currentTimeline < timelines.Length - 1)
        {
            currentTimeline++;
        }
        else
        {
            currentTimeline = 0;
        }
        Invoke("UpdateTimeLines", 0.5f);
        UITransition.Play("TimelineChange");
        onCoolDown = true;
        Invoke("ResetAnimation", 2.3f);
        if (timelineText != null)
        {
            timelineText.text = "Timeline: " + currentTimeline;
        }
    }

    private void UpdateTimeLines()
    {
        for (int i = 0; i < timelines.Length; i++)
        {
            if (i == currentTimeline)
            {
                timelines[i].SetActive(true);
            }
            else
            {
                timelines[i].SetActive(false);
            }
        }
    }

    private void ResetAnimation()
    {
        UITransition.Play("TimelineDefault");
        onCoolDown = false;
    }

    public int CheckLevelCompletion()
    {
        int _completion = 0;
        foreach (PlacementPoint _placement in requiredPlacements)
        {
            if(_placement.completed)
            {
                _completion++;
            }
        }
        print(_completion + " / " + requiredPlacements.Length);
        HUD.instance.UpdateCompletionDisplay(_completion, requiredPlacements.Length);
        if (_completion >= requiredPlacements.Length)
        {
            LevelFinishMenu.Instance.ActivateFinishPanel();
        }
        return _completion;
    }
}
