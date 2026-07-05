using UnityEditor.U2D.Animation;
using UnityEngine;

/// <summary>
/// 캐릭터의 체력과 정신력 계산을 담당한다.
/// CharacterData의 HP, MP 데이터를 기준으로 현재 상태를 확인하고,
/// 카드 시스템, 아이템 시스템, 게임 시스템에서 전달받은 값으로 HP와 MP를 변경한다.
/// </summary>
public class CharacterStat : MonoBehaviour
{
    [SerializeField]
    private CharacterData characterData;

    private const float LowStatRate = 0.25f;

    public bool IsHPLow { get; private set; }
    public bool IsMPLow { get; private set; }

    private void Awake()
    {
        if (characterData == null)
        {
            characterData = GetComponent<CharacterData>();
        }

        CntStat();
    }

    /// <summary>
    /// 현재 체력 확인
    /// 캐릭터 스탯 데이터 CharacterData에서 현재 HP를 가져온다.
    /// 외부 StatManager.GetHP()와 연결될 수 있는 현재 HP 반환 구조이다.
    /// </summary>
    public int CheckHP()
    {
        if (characterData == null)
        {
            return 0;
        }

        return characterData.currentHP;
    }

    /// <summary>
    /// 현재 정신력 확인
    /// 캐릭터 스탯 데이터 CharacterData에서 현재 MP를 가져온다.
    /// 외부 StatManager.GetMP()와 연결될 수 있는 현재 MP 반환 구조이다.
    /// </summary>
    public int CheckMP()
    {
        if (characterData == null)
        {
            return 0;
        }

        return characterData.currentMP;
    }

    /// <summary>
    /// 체력 감소
    /// 카드 시스템의 UseCard()에서 카드의 HP 소모량을 전달받는다고 가정한다.
    /// 전달받은 HP 소모량만큼 currentHP를 감소시킨다.
    /// </summary>
    public void UseHP(int useHPCnt)
    {
        if (characterData == null)
        {
            return;
        }

        int safeUseHPCnt = Mathf.Max(0, useHPCnt);
        characterData.currentHP = Mathf.Max(0, characterData.currentHP - safeUseHPCnt);
    }

    /// <summary>
    /// 정신력 감소
    /// 카드 시스템의 UseCard()에서 카드의 MP 소모량을 전달받는다고 가정한다.
    /// 전달받은 MP 소모량만큼 currentMP를 감소시킨다.
    /// </summary>
    public void UseMP(int useMPCnt)
    {
        if (characterData == null)
        {
            return;
        }

        int safeUseMPCnt = Mathf.Max(0, useMPCnt);
        characterData.currentMP = Mathf.Max(0, characterData.currentMP - safeUseMPCnt);
    }

    /// <summary>
    /// 체력 회복
    /// 아이템 시스템에서 아이템의 HP 회복량을 전달받는다고 가정한다.
    /// 전달받은 HP 회복량만큼 currentHP를 회복한다.
    /// </summary>
    public void HealHP(int healHPCnt)
    {
        if (characterData == null)
        {
            return;
        }

        int safeHealHPCnt = Mathf.Max(0, healHPCnt);
        characterData.currentHP = Mathf.Min(characterData.maxHP, characterData.currentHP + safeHealHPCnt);
    }

    /// <summary>
    /// 정신력 회복
    /// 아이템 시스템에서 아이템의 MP 회복량을 전달받는다고 가정한다.
    /// 전달받은 MP 회복량만큼 currentMP를 회복한다.
    /// </summary>
    public void HealMP(int healMPCnt)
    {
        if (characterData == null)
        {
            return;
        }

        int safeHealMPCnt = Mathf.Max(0, healMPCnt);
        characterData.currentMP = Mathf.Min(characterData.maxMP, characterData.currentMP + safeHealMPCnt);
    }

    /// <summary>
    /// 휴식 시 체력·정신력 전체 회복
    /// 게임 시스템의 SelectRest()에서 호출된다고 가정한다.
    /// currentHP를 maxHP로, currentMP를 maxMP로 회복한다.
    /// </summary>
    public void RestCh()
    {
        if (characterData == null)
        {
            return;
        }

        characterData.currentHP = characterData.maxHP;
        characterData.currentMP = characterData.maxMP;
    }

    /// <summary>
    /// 야근 시 체력·정신력 회복
    /// 게임 시스템의 SelectOT()에서 호출된다고 가정한다.
    /// 임시값으로 HP와 MP를 각각 +1 회복한다.
    /// </summary>
    public void OTCh()
    {
        if (characterData == null)
        {
            return;
        }

        characterData.currentHP = Mathf.Min(characterData.maxHP, characterData.currentHP + 1);
        characterData.currentMP = Mathf.Min(characterData.maxMP, characterData.currentMP + 1);
    }

    /// <summary>
    /// 현재 스탯 계산
    /// 캐릭터 파트의 HP·MP 소모 및 회복 결과를 기준으로 현재 HP와 MP 상태를 계산한다.
    /// HP와 MP가 각각 최대값의 25% 미만인지 확인한다.
    /// </summary>
    public void CntStat()
    {
        if (characterData == null)
        {
            IsHPLow = false;
            IsMPLow = false;
            return;
        }

        characterData.maxHP = Mathf.Max(1, characterData.maxHP);
        characterData.maxMP = Mathf.Max(1, characterData.maxMP);

        characterData.currentHP = Mathf.Clamp(characterData.currentHP, 0, characterData.maxHP);
        characterData.currentMP = Mathf.Clamp(characterData.currentMP, 0, characterData.maxMP);

        IsHPLow = (float)characterData.currentHP / characterData.maxHP < LowStatRate;
        IsMPLow = (float)characterData.currentMP / characterData.maxMP < LowStatRate;
    }
}