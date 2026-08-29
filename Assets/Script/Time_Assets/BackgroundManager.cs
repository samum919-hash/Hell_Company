using UnityEngine;
using UnityEngine.UI;
using GameData;

public class BackgroundManager : MonoBehaviour
{
    [Header("Background Image")]
    [SerializeField] private Image backgroundImage;

    [Header("Sprites")]
    [SerializeField] private Sprite dayBackground;
    [SerializeField] private Sprite nightBackground;

    private void OnEnable()
    {
        TimeManager.OnTimeChanged += UpdateBackground;
    }

    private void OnDisable()
    {
        TimeManager.OnTimeChanged -= UpdateBackground;
    }

    private void Start()
    {
        UpdateBackground();
    }

    private void UpdateBackground()
    {
        TimeMode mode = TimeManager.Instance.Get_time_mode();

        switch (mode)
        {
            case TimeMode.Day:
                backgroundImage.sprite = dayBackground;
                break;

            case TimeMode.Overtime:
                backgroundImage.sprite = nightBackground;
                break;

            case TimeMode.Rest:
                // Rest 모드 배경 처리 필요 여부 확인 필요
                break;
        }
    }
}