using UnityEngine;
using TMPro;

public class UIElements : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI SpeedTime;

    private float timeElapsedYears = 0f;
    private int speedtime = 1;

    public void UpdateTime(float dt)
    {
        timeElapsedYears += dt;

        int years = Mathf.FloorToInt(timeElapsedYears);
        int days = Mathf.FloorToInt((timeElapsedYears - years) * 365.25f);

        timeText.text = $"Time: {years} years {days} days";
    }
    public void UpdateSpeedTime(float newSpeed)
    {
        speedtime = (int)newSpeed;
        SpeedTime.text = $"Time Scale: x{speedtime}";
    }
}