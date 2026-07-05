using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스케줄러를 관리한다.
/// 손패에서 선택된 카드를 스케줄러에 올리고,
/// UseCard() 실행 시 카드 효과를 적용한 뒤 무덤으로 보낸다.
/// </summary>
public class SchedulerManager : MonoBehaviour
{
    [Header("Card Managers")]
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private CardEffectManager cardEffectManager;

    [Header("Scheduler UI")]
    [SerializeField] private Transform schedulerArea;
    [SerializeField] private GameObject cardPrefab;

    private readonly List<CardData> schedulerCards = new List<CardData>();
    private readonly List<GameObject> schedulerCardObjects = new List<GameObject>();

    /// <summary>
    /// 스케줄러에 카드를 올린다.
    /// </summary>
    public void AddCardToScheduler(CardData cardData)
    {
        if (cardData == null)
        {
            return;
        }

        schedulerCards.Add(cardData);

        if (cardPrefab != null && schedulerArea != null)
        {
            GameObject cardObject = Instantiate(cardPrefab, schedulerArea);
            schedulerCardObjects.Add(cardObject);

            Card card = cardObject.GetComponent<Card>();
            if (card != null)
            {
                card.SetCard(cardData, null);
            }
        }

        UpdateScheduler();
    }

    /// <summary>
    /// 스케줄러에 올라간 카드 사용.
    /// 가장 먼저 올라간 카드부터 사용한다.
    /// </summary>
    public bool UseCard()
    {
        if (schedulerCards.Count <= 0)
        {
            Debug.Log("[스케줄러] 사용할 카드가 없습니다.");
            return false;
        }

        if (cardEffectManager == null)
        {
            Debug.Log("[스케줄러] CardEffectManager가 연결되지 않았습니다.");
            return false;
        }

        CardData cardData = schedulerCards[0];

        bool canUse = cardEffectManager.UseCard(cardData);

        if (!canUse)
        {
            Debug.Log("[카드 사용 실패] 체력 또는 정신력이 부족합니다.");
            return false;
        }

        RemoveCardFromScheduler(0);

        if (deckManager != null)
        {
            deckManager.AddCardToGraveyard(cardData);
        }
        else
        {
            Debug.Log("[스케줄러] DeckManager가 연결되지 않아 사용한 카드를 무덤에 넣지 못했습니다.");
        }

        UpdateScheduler();
        return true;
    }

    /// <summary>
    /// 스케줄러에서 카드를 제거한다.
    /// </summary>
    public void RemoveCardFromScheduler(int index)
    {
        if (index < 0 || index >= schedulerCards.Count)
        {
            return;
        }

        schedulerCards.RemoveAt(index);

        if (index < schedulerCardObjects.Count)
        {
            GameObject cardObject = schedulerCardObjects[index];
            schedulerCardObjects.RemoveAt(index);
            Destroy(cardObject);
        }
    }

    /// <summary>
    /// 현재 스케줄러 카드 수 반환.
    /// </summary>
    public int GetSchedulerCnt()
    {
        return schedulerCards.Count;
    }

    /// <summary>
    /// 스케줄러 갱신.
    /// Horizontal Layout Group을 사용하면 자동 정렬된다.
    /// </summary>
    public void UpdateScheduler()
    {
        // 스케줄러 위치를 직접 계산해야 할 경우 이곳에 작성한다.
    }
}
