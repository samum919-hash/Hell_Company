using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameData;

/// <summary>
/// 시간, 일차, 주차 텍스트 및 낮/야근/휴식 아이콘 UI를 표시한다.
/// SceneManager로부터 값을 전달받아 화면에 반영만 한다.
/// </summary>
public class TimeHUD : MonoBehaviour
{
    [Header("Time UI")]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text weekText;

    [Header("Time Mode Icon")]
    [SerializeField] private Image timeModeIcon;
    [SerializeField] private Sprite dayIcon;
    [SerializeField] private Sprite overtimeIcon;
    [SerializeField] private Sprite restIcon;

    /// <summary>
    /// 시간/일차/주차 텍스트 갱신.
    /// </summary>
    public void UpdateTimeUI(int hour, int day, int week, TimeMode mode)
    {
        if (timeText != null)
        {
            timeText.text = hour + ":00";
        }

        if (dayText != null)
        {
            dayText.text = day + "일차";
        }

        if (weekText != null)
        {
            weekText.text = week + "주차";
        }

        UpdateTimeModeIcon(mode);
    }

    /// <summary>
    /// 현재 시간대(낮/야근/휴식)에 맞는 아이콘을 표시한다.
    /// </summary>
    private void UpdateTimeModeIcon(TimeMode mode)
    {
        if (timeModeIcon == null)
        {
            return;
        }

        switch (mode)
        {
            case TimeMode.Day:
                timeModeIcon.sprite = dayIcon;
                break;

            case TimeMode.Overtime:
                timeModeIcon.sprite = overtimeIcon;
                break;

            case TimeMode.Rest:
                timeModeIcon.sprite = restIcon;
                break;
        }
    }
}
