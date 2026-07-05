using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameData;

/// <summary>
/// 덱과 카드뭉치(무덤)를 관리한다.
/// 덱 생성, 카드 뽑기, 덱 카드 수 계산, 카드 종류별 계산,
/// 사용된 카드 또는 버린 카드를 무덤에 추가하는 기능을 담당한다.
/// </summary>
public class DeckManager : MonoBehaviour
{
    [Header("Deck UI")]
    [SerializeField] private TMP_Text deckCountText;
    [SerializeField] private TMP_Text discardCountText;

    [Header("Deck Setting")]
    [SerializeField] private int guideCardCnt = 3;
    [SerializeField] private int deliveryCardCnt = 3;
    [SerializeField] private int convenienceStoreCardCnt = 3;

    private readonly List<CardData> deck = new List<CardData>();
    private readonly List<CardData> discardPile = new List<CardData>();

    /// <summary>
    /// 카드 정보 생성.
    /// 카드 이름, 카드 종류, HP 소모, MP 소모, 시간 소모, 자산 획득량을 설정한다.
    /// </summary>
    public CardData CreateCard(string cardName, CardType cardType, int useHPCnt, int useMPCnt, int useTimeCnt, int plusAssetCnt)
    {
        return new CardData(cardName, cardType, useHPCnt, useMPCnt, useTimeCnt, plusAssetCnt);
    }

    /// <summary>
    /// 덱 생성.
    /// 사용할 카드들을 덱에 추가한다.
    /// 이 함수는 GameManager에서 게임 시작 시 호출하는 것을 권장한다.
    /// </summary>
    public void CreateDeck()
    {
        deck.Clear();
        discardPile.Clear();

        // 안내 가이드: 체력과 정신력을 똑같이 사용하는 일
        AddCardCopies("안내 가이드", CardType.Guide, 8, 8, 2, 3000, guideCardCnt);

        // 택배 상하차: 체력을 많이 사용하고 정신력은 적게 사용하는 일
        AddCardCopies("택배 상하차", CardType.Delivery, 20, 5, 3, 6000, deliveryCardCnt);

        // 편의점 알바: 체력은 적게 사용하고 정신력을 많이 사용하는 일
        AddCardCopies("편의점 알바", CardType.ConvenienceStore, 5, 20, 3, 5000, convenienceStoreCardCnt);

        ShuffleDeck();
        UpdateDeck();
    }

    /// <summary>
    /// 같은 종류의 카드를 여러 장 생성해서 덱에 추가한다.
    /// 카드 한 장마다 새로운 CardData 객체를 생성한다.
    /// </summary>
    private void AddCardCopies(string cardName, CardType cardType, int useHPCnt, int useMPCnt, int useTimeCnt, int plusAssetCnt, int count)
    {
        for (int i = 0; i < count; i++)
        {
            deck.Add(CreateCard(cardName, cardType, useHPCnt, useMPCnt, useTimeCnt, plusAssetCnt));
        }
    }

    /// <summary>
    /// 카드 한 장 반환.
    /// 덱이 섞인 상태라고 가정하고 맨 위 카드 한 장을 뽑은 뒤 덱에서 제거한다.
    /// </summary>
    public CardData DrawCard()
    {
        if (deck.Count <= 0)
        {
            Debug.Log("[덱] 뽑을 카드가 없습니다.");
            return null;
        }

        CardData cardData = deck[0];
        deck.RemoveAt(0);

        UpdateDeck();

        return cardData;
    }

    /// <summary>
    /// 덱에 카드가 있는지 확인한다.
    /// </summary>
    public bool CheckDeck()
    {
        return deck.Count > 0;
    }

    /// <summary>
    /// 현재 덱 카드 수 계산.
    /// </summary>
    public int CntDeck()
    {
        return deck.Count;
    }

    /// <summary>
    /// 덱 남은 수 반환.
    /// </summary>
    public int GetDeckCnt()
    {
        return CntDeck();
    }

    /// <summary>
    /// 카드뭉치(무덤) 카드 수 반환.
    /// </summary>
    public int GetDiscardCnt()
    {
        return discardPile.Count;
    }

    /// <summary>
    /// 특정 카드를 덱에서 제거한다.
    /// 일반적인 카드 뽑기에서는 DrawCard() 안의 RemoveAt(0)을 사용한다.
    /// </summary>
    public void RemoveCardToDeck(CardData cardData)
    {
        if (cardData == null)
        {
            return;
        }

        deck.Remove(cardData);
        UpdateDeck();
    }

    /// <summary>
    /// 덱 안에 어떤 카드가 몇 장 있는지 계산한다.
    /// </summary>
    public Dictionary<string, int> CntCards()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();

        for (int i = 0; i < deck.Count; i++)
        {
            string cardName = deck[i].cardName;

            if (!result.ContainsKey(cardName))
            {
                result.Add(cardName, 0);
            }

            result[cardName]++;
        }

        return result;
    }

    /// <summary>
    /// 사용된 카드나 버린 카드를 무덤에 추가한다.
    /// </summary>
    public void AddCardToGraveyard(CardData cardData)
    {
        if (cardData == null)
        {
            return;
        }

        discardPile.Add(cardData);
        UpdateGraveyard();
    }

    /// <summary>
    /// 덱을 무작위로 섞는다.
    /// </summary>
    public void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int randomIndex = Random.Range(i, deck.Count);

            CardData temp = deck[i];
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    /// <summary>
    /// 덱 UI 갱신.
    /// </summary>
    public void UpdateDeck()
    {
        if (deckCountText != null)
        {
            deckCountText.text = deck.Count.ToString();
        }

        UpdateGraveyard();
    }

    /// <summary>
    /// 카드뭉치(무덤) UI 갱신.
    /// </summary>
    public void UpdateGraveyard()
    {
        if (discardCountText != null)
        {
            discardCountText.text = discardPile.Count.ToString();
        }
    }
}
