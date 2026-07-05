using UnityEngine;
using UnityEngine.UI;
using GameData;

/// <summary>
/// 시간대(낮/야근/휴식)에 맞춰 배경 이미지를 전환한다.
/// Rest 상태는 밤 배경을 그대로 사용한다 (임시 처리, 별도 배경 필요 시 조건 추가).
/// </summary>
public class BackgroundController : MonoBehaviour
{
    [Header("Background")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite dayBackground;
    [SerializeField] private Sprite nightBackground;

    /// <summary>
    /// 시간대에 맞는 배경으로 전환한다.
    /// </summary>
    public void UpdateBackground(TimeMode mode)
    {
        if (backgroundImage == null)
        {
            return;
        }

        backgroundImage.sprite = (mode == TimeMode.Day) ? dayBackground : nightBackground;
    }
}
