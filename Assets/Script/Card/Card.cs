using UnityEngine;
using TMPro;

/// <summary>
/// 화면에 보이는 카드 한 장을 담당한다.
/// CardData의 정보를 UI에 출력하고, 클릭 시 HandManager에 선택 사실을 전달한다.
/// </summary>
public class Card : MonoBehaviour
{
    [Header("Card UI")]
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text hpCostText;
    [SerializeField] private TMP_Text mpCostText;
    [SerializeField] private TMP_Text timeCostText;
    [SerializeField] private TMP_Text assetText;

    private CardData cardData;
    private HandManager handManager;

    /// <summary>
    /// 카드 UI에 표시할 카드 데이터를 설정한다.
    /// 손패에 있는 카드라면 HandManager를 함께 연결한다.
    /// 스케줄러에 올라간 카드는 클릭 처리가 필요 없으므로 HandManager가 null일 수 있다.
    /// </summary>
    public void SetCard(CardData cardData, HandManager handManager)
    {
        this.cardData = cardData;
        this.handManager = handManager;

        UpdateCard();
    }

    /// <summary>
    /// 현재 카드 데이터를 반환한다.
    /// </summary>
    public CardData GetCardData()
    {
        return cardData;
    }

    /// <summary>
    /// 카드 클릭 처리.
    /// 손패 카드 클릭 시 스케줄러로 올리는 과정은 HandManager에서 처리한다.
    /// </summary>
    public void OnClickCard()
    {
        if (handManager == null)
        {
            return;
        }

        handManager.SelectCard(this);
    }

    /// <summary>
    /// 카드 UI 갱신.
    /// </summary>
    public void UpdateCard()
    {
        if (cardData == null)
        {
            return;
        }

        if (cardNameText != null)
        {
            cardNameText.text = cardData.cardName;
        }

        if (hpCostText != null)
        {
            hpCostText.text = "HP -" + cardData.useHPCnt;
        }

        if (mpCostText != null)
        {
            mpCostText.text = "MP -" + cardData.useMPCnt;
        }

        if (timeCostText != null)
        {
            timeCostText.text = "소요 " + cardData.useTimeCnt + "시간";
        }

        if (assetText != null)
        {
            assetText.text = "+" + cardData.plusAssetCnt + "G";
        }
    }
}
