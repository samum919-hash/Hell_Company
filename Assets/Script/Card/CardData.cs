namespace GameData
{
    public enum CardType
    {
        Guide,              // 안내 가이드
        Delivery,           // 택배 상하차
        ConvenienceStore    // 편의점 알바
    }
}

/// <summary>
/// 카드의 기본 정보를 관리한다.
/// 카드 이름, 카드 종류, HP/MP/시간 소모량, 자산 획득량을 저장한다.
/// 실제 카드 효과 실행은 CardEffectManager에서 처리한다.
/// </summary>
public class CardData
{
    public string cardName;
    public GameData.CardType cardType;

    public int useHPCnt;
    public int useMPCnt;
    public int useTimeCnt;

    public int plusAssetCnt;

    public CardData(string cardName, GameData.CardType cardType, int useHPCnt, int useMPCnt, int useTimeCnt, int plusAssetCnt)
    {
        this.cardName = cardName;
        this.cardType = cardType;
        this.useHPCnt = useHPCnt;
        this.useMPCnt = useMPCnt;
        this.useTimeCnt = useTimeCnt;
        this.plusAssetCnt = plusAssetCnt;
    }
}
