using UnityEngine;

/// <summary>
/// 캐릭터의 현재 상태에 맞는 모션 또는 이미지를 처리한다.
/// 캐릭터 이미지는 총 4개만 사용한다.
///
/// 사용 이미지:
/// 1. Ch_Idle.png    -> idleSprite
/// 2. Ch_Tired.png   -> tiredSprite
/// 3. Ch_Stress.png  -> stressSprite
/// 4. Ch_Exhaust.png -> exhaustSprite
///
/// 중요:
/// - Resources.Load 방식으로 이미지 경로를 직접 불러오지 않는다.
/// - 실제 이미지는 Unity Inspector에서 Sprite 변수에 직접 연결한다.
/// - Animator Controller, Timeline, 추가 애니메이션 시스템은 사용하지 않는다.
/// - 카드 사용 전용 이미지는 만들지 않는다.
/// </summary>
public class CharacterMotion : MonoBehaviour
{
    [Header("Character Stat")]
    [SerializeField]
    private CharacterStat characterStat;

    [Header("Sprite Renderer")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [Header("Character State Sprites")]
    [Tooltip("기본 모습 이미지: Ch_Idle.png")]
    public Sprite idleSprite;

    [Tooltip("체력 부족 이미지: Ch_Tired.png")]
    public Sprite tiredSprite;

    [Tooltip("정신력 부족 이미지: Ch_Stress.png")]
    public Sprite stressSprite;

    [Tooltip("체력·정신력 둘 다 부족 이미지: Ch_Exhaust.png")]
    public Sprite exhaustSprite;

    private void Awake()
    {
        if (characterStat == null)
        {
            characterStat = GetComponent<CharacterStat>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        CheckCh();
    }

    /// <summary>
    /// 기본 대기 모션 또는 기본 모습 이미지 적용
    /// CheckCh()의 정상 상태 결과를 가져온다고 가정한다.
    ///
    /// 실행 조건:
    /// HP가 25% 이상이고 MP가 25% 이상일 때 실행된다.
    ///
    /// 적용 이미지:
    /// Ch_Idle.png
    /// </summary>
    public void IdleAct()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        spriteRenderer.sprite = idleSprite;
    }

    /// <summary>
    /// 카드 사용 모션 처리
    /// 카드 시스템의 UseCard()에서 카드 정보를 가져온다고 가정한다.
    ///
    /// 중요:
    /// 카드 사용 전용 이미지는 적용하지 않는다.
    /// 카드 행동 이후 현재 HP와 MP 상태에 맞는 이미지가 CheckCh()를 통해 다시 적용된다.
    /// </summary>
    public void CardAct()
    {
        CheckCh();
    }

    /// <summary>
    /// 체력 부족 모션 또는 체력 부족 이미지 적용
    /// CheckCh()의 HP 부족 상태 결과를 가져온다고 가정한다.
    ///
    /// 실행 조건:
    /// HP가 25% 미만이고 MP는 25% 이상일 때 실행된다.
    ///
    /// 적용 이미지:
    /// Ch_Tired.png
    /// </summary>
    public void TiredAct()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        spriteRenderer.sprite = tiredSprite;
    }

    /// <summary>
    /// 정신력 부족 모션 또는 정신력 부족 이미지 적용
    /// CheckCh()의 MP 부족 상태 결과를 가져온다고 가정한다.
    ///
    /// 실행 조건:
    /// MP가 25% 미만이고 HP는 25% 이상일 때 실행된다.
    ///
    /// 적용 이미지:
    /// Ch_Stress.png
    /// </summary>
    public void StressAct()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        spriteRenderer.sprite = stressSprite;
    }

    /// <summary>
    /// 체력·정신력 모두 부족 모션 또는 이미지 적용
    /// CheckCh()의 HP·MP 모두 부족 상태 결과를 가져온다고 가정한다.
    ///
    /// 실행 조건:
    /// HP와 MP가 모두 25% 미만일 때 실행된다.
    ///
    /// 적용 이미지:
    /// Ch_Exhaust.png
    /// </summary>
    public void ExhaustAct()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        spriteRenderer.sprite = exhaustSprite;
    }

    /// <summary>
    /// 현재 상태에 맞는 모션 또는 이미지 적용
    /// 캐릭터 스탯 파트의 HP·MP 상태를 가져온다.
    ///
    /// 상태 판단 기준:
    /// 1. HP 25% 이상, MP 25% 이상 -> IdleAct()
    /// 2. HP 25% 미만, MP 25% 이상 -> TiredAct()
    /// 3. HP 25% 이상, MP 25% 미만 -> StressAct()
    /// 4. HP 25% 미만, MP 25% 미만 -> ExhaustAct()
    /// </summary>
    public void CheckCh()
    {
        if (characterStat == null)
        {
            return;
        }

        characterStat.CntStat();

        bool isHPLow = characterStat.IsHPLow;
        bool isMPLow = characterStat.IsMPLow;

        if (isHPLow && isMPLow)
        {
            ExhaustAct();
        }
        else if (isHPLow)
        {
            TiredAct();
        }
        else if (isMPLow)
        {
            StressAct();
        }
        else
        {
            IdleAct();
        }
    }
}