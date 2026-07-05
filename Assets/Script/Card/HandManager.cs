using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 손패를 관리한다.
/// 덱에서 카드 5장을 뽑아 손패에 추가하고,
/// 선택된 카드를 스케줄러로 이동시킨다.
/// </summary>
public class HandManager : MonoBehaviour
{
    [Header("Card Managers")]
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private SchedulerManager schedulerManager;

    [Header("Hand UI")]
    [SerializeField] private Transform handArea;
    [SerializeField] private GameObject cardPrefab;

    private readonly List<CardData> handCards = new List<CardData>();
    private readonly List<GameObject> handCardObjects = new List<GameObject>();

    /// <summary>
    /// 기본 카드 뽑기.
    /// 덱에서 카드 5장을 뽑는다.
    /// </summary>
    public void DrawCards()
    {
        DrawCards(5);
    }

    /// <summary>
    /// 덱에서 지정한 수만큼 카드를 뽑아 손패에 추가한다.
    /// </summary>
    public void DrawCards(int drawCnt)
    {
        if (deckManager == null)
        {
            Debug.Log("[손패] DeckManager가 연결되지 않았습니다.");
            return;
        }

        for (int i = 0; i < drawCnt; i++)
        {
            if (!deckManager.CheckDeck())
            {
                break;
            }

            CardData cardData = deckManager.DrawCard();
            AddCardToHand(cardData);
        }

        UpdateHand();
        deckManager.UpdateDeck();
    }

    /// <summary>
    /// 뽑은 카드를 손패에 넣고 카드 UI를 생성한다.
    /// </summary>
    public void AddCardToHand(CardData cardData)
    {
        if (cardData == null)
        {
            return;
        }

        handCards.Add(cardData);

        if (cardPrefab != null && handArea != null)
        {
            GameObject cardObject = Instantiate(cardPrefab, handArea);
            handCardObjects.Add(cardObject);

            Card card = cardObject.GetComponent<Card>();
            if (card != null)
            {
                card.SetCard(cardData, this);
            }
        }

        UpdateHand();
    }

    /// <summary>
    /// 손패 카드 선택.
    /// 선택된 카드를 스케줄러에 올리고 손패에서 제거한다.
    /// </summary>
    public void SelectCard(Card selectedCard)
    {
        if (selectedCard == null)
        {
            return;
        }

        CardData cardData = selectedCard.GetCardData();

        bool success = AddCardToScheduler(cardData);

        if (!success)
        {
            return;
        }

        RemoveCardFromHand(cardData, selectedCard.gameObject);
        UpdateHand();
    }

    /// <summary>
    /// 스케줄러에 카드를 올린다.
    /// 성공 여부를 반환한다.
    /// </summary>
    public bool AddCardToScheduler(CardData cardData)
    {
        if (schedulerManager == null)
        {
            Debug.Log("[손패] SchedulerManager가 연결되지 않았습니다.");
            return false;
        }

        schedulerManager.AddCardToScheduler(cardData);
        return true;
    }

    /// <summary>
    /// 손패에서 카드를 제거한다.
    /// </summary>
    public void RemoveCardFromHand(CardData cardData, GameObject cardObject)
    {
        if (cardData != null)
        {
            handCards.Remove(cardData);
        }

        if (cardObject != null)
        {
            handCardObjects.Remove(cardObject);
            Destroy(cardObject);
        }
    }

    /// <summary>
    /// 현재 손패 카드 수 반환.
    /// </summary>
    public int GetHandCnt()
    {
        return handCards.Count;
    }

    /// <summary>
    /// 손패 갱신.
    /// Horizontal Layout Group을 사용하면 별도 좌표 계산 없이 자동 정렬된다.
    /// </summary>
    public void UpdateHand()
    {
        // 손패 위치를 직접 계산해야 할 경우 이곳에 작성한다.
    }
}
