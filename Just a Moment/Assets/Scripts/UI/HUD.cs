using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI percentageText;
    private float fillTarget = 0f;
    private float previousFill = 0f;
    private int percentageTarget = 0;
    private int previousPercentage = 0;
    private bool lerping = false;


    private float lerpValue = 0f;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if(lerping)
        {
            int _p = 0;
            lerpValue += Time.deltaTime;
            fill.fillAmount = Mathf.Lerp(previousFill, fillTarget, lerpValue);
            _p = Mathf.RoundToInt(Mathf.Lerp(previousPercentage, percentageTarget, lerpValue));
            percentageText.text = _p.ToString() + "%";
            if(lerpValue >= 1f)
            {
                fill.fillAmount = fillTarget;
                percentageText.text = percentageTarget.ToString() + "%";
                previousPercentage = _p;
                lerping = false;
            }
        }
    }

    public void UpdateCompletionDisplay(int _completion, int _total)
    {
        previousFill = fill.fillAmount;
        fillTarget = ((float)_completion / (float)_total);
        percentageTarget = Mathf.RoundToInt(fillTarget * 100);
        lerpValue = 0f;
        lerping = true;
    }
}
