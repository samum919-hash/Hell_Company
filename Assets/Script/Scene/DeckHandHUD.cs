using UnityEngine;
using TMPro;

/// <summary>
/// 덱, 무덤, 손패, 스케줄러의 카드 수 UI를 표시한다.
/// SceneManager로부터 값을 전달받아 화면에 반영만 한다.
/// </summary>
public class DeckHandHUD : MonoBehaviour
{
    [Header("Deck / Hand UI")]
    [SerializeField] private TMP_Text deckCountText;
    [SerializeField] private TMP_Text discardCountText;
    [SerializeField] private TMP_Text handCountText;
    [SerializeField] private TMP_Text schedulerCountText;

    /// <summary>
    /// 덱/무덤/손패/스케줄러 카드 수 UI 갱신.
    /// </summary>
    public void UpdateDeckHandUI(int deckCnt, int discardCnt, int handCnt, int schedulerCnt)
    {
        if (deckCountText != null)
        {
            deckCountText.text = deckCnt.ToString();
        }

        if (discardCountText != null)
        {
            discardCountText.text = discardCnt.ToString();
        }

        if (handCountText != null)
        {
            handCountText.text = handCnt.ToString();
        }

        if (schedulerCountText != null)
        {
            schedulerCountText.text = schedulerCnt.ToString();
        }
    }
}
