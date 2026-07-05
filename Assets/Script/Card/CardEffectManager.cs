using UnityEngine;

/// <summary>
/// 카드 효과를 처리한다.
/// 카드 사용 시 캐릭터 행동, 시간 진행, 자산 획득을 다른 매니저에게 전달한다.
/// 카드 이동 처리는 SchedulerManager와 DeckManager에서 담당한다.
/// </summary>
public class CardEffectManager : MonoBehaviour
{
    [Header("Other Managers")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CharacterStat characterStat;
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private AssetManager assetManager;

    private void Awake()
    {
        if (characterController == null)
        {
            characterController = FindObjectOfType<CharacterController>();
        }

        if (characterStat == null && characterController != null)
        {
            characterStat = characterController.GetComponent<CharacterStat>();
        }

        if (timeManager == null)
        {
            timeManager = TimeManager.Instance;
        }

        if (assetManager == null)
        {
            assetManager = AssetManager.Instance;
        }
    }

    /// <summary>
    /// 카드 사용.
    /// 사용할 수 있는 카드인지 확인한 뒤 캐릭터 행동, 시간 진행, 자산 획득을 처리한다.
    /// </summary>
    public bool UseCard(CardData cardData)
    {
        if (cardData == null)
        {
            return false;
        }

        if (!CanUseCard(cardData))
        {
            return false;
        }

        ActCard(cardData.useHPCnt, cardData.useMPCnt);
        UseCardTime(cardData.useTimeCnt);
        asset_plus(cardData.plusAssetCnt);

        return true;
    }

    /// <summary>
    /// 카드 비용 확인.
    /// 현재 HP와 MP가 카드 비용보다 부족하면 사용할 수 없다.
    /// </summary>
    public bool CanUseCard(CardData cardData)
    {
        if (characterStat == null || cardData == null)
        {
            return false;
        }

        if (CheckHP() < cardData.useHPCnt)
        {
            return false;
        }

        if (CheckMP() < cardData.useMPCnt)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 현재 체력 확인.
    /// CharacterStat의 CheckHP()와 연결한다.
    /// </summary>
    public int CheckHP()
    {
        if (characterStat == null)
        {
            return 0;
        }

        return characterStat.CheckHP();
    }

    /// <summary>
    /// 현재 정신력 확인.
    /// CharacterStat의 CheckMP()와 연결한다.
    /// </summary>
    public int CheckMP()
    {
        if (characterStat == null)
        {
            return 0;
        }

        return characterStat.CheckMP();
    }

    /// <summary>
    /// 카드 행동 처리.
    /// CharacterController의 ActCard()와 연결한다.
    /// HP/MP 감소와 캐릭터 모션 갱신은 캐릭터 파트에서 처리한다.
    /// </summary>
    public void ActCard(int useHPCnt, int useMPCnt)
    {
        if (characterController == null)
        {
            return;
        }

        characterController.ActCard(useHPCnt, useMPCnt);
    }

    /// <summary>
    /// 카드 사용 시 시간 진행.
    /// TimeManager의 UseCardTime()과 연결한다.
    /// </summary>
    public void UseCardTime(int useTimeCnt)
    {
        if (timeManager == null)
        {
            timeManager = TimeManager.Instance;
        }

        if (timeManager == null)
        {
            return;
        }

        timeManager.UseCardTime(useTimeCnt);
    }

    /// <summary>
    /// 자산 획득.
    /// AssetManager의 Asset_plus()와 연결한다.
    /// </summary>
    public void asset_plus(int plusAssetCnt)
    {
        if (assetManager == null)
        {
            assetManager = AssetManager.Instance;
        }

        if (assetManager == null)
        {
            return;
        }

        assetManager.Asset_plus(plusAssetCnt);
    }
}
