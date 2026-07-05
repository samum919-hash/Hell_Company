using UnityEngine;

/// <summary>
/// 캐릭터 시스템 전체를 제어한다.
/// CharacterData, CharacterStat, CharacterMotion을 연결한다.
/// Unity 기본 CharacterController 컴포넌트 기능은 사용하지 않는다.
/// 캐릭터 이동 기능은 구현하지 않는다.
/// </summary>
public class CharacterController : MonoBehaviour
{
    [Header("Character Parts")]
    [SerializeField]
    private CharacterData characterData;

    [SerializeField]
    private CharacterStat characterStat;

    [SerializeField]
    private CharacterMotion characterMotion;

    private void Awake()
    {
        if (characterData == null)
        {
            characterData = GetComponent<CharacterData>();
        }

        if (characterStat == null)
        {
            characterStat = GetComponent<CharacterStat>();
        }

        if (characterMotion == null)
        {
            characterMotion = GetComponent<CharacterMotion>();
        }

        CreateCh();
    }

    /// <summary>
    /// 캐릭터 기본 정보 설정
    /// 캐릭터 내부의 초기 설정값을 사용한다.
    /// CharacterData의 초기값을 기준으로 캐릭터를 생성한다.
    /// </summary>
    public void CreateCh()
    {
        if (characterStat != null)
        {
            characterStat.CntStat();
        }

        if (characterMotion != null)
        {
            characterMotion.CheckCh();
        }
    }

    /// <summary>
    /// 현재 캐릭터 상태 확인
    /// 캐릭터 스탯 파트의 HP·MP 상태를 가져온다.
    /// CharacterMotion의 CheckCh()와 연결되어 현재 상태에 맞는 모션 또는 이미지를 적용한다.
    /// </summary>
    public void CheckCh()
    {
        if (characterMotion == null)
        {
            return;
        }

        characterMotion.CheckCh();
    }

    /// <summary>
    /// 캐릭터 상태 갱신
    /// 캐릭터 스탯 파트의 계산 결과를 가져온다.
    /// CharacterStat의 CntStat()을 호출한다.
    /// 계산된 상태를 바탕으로 현재 캐릭터 상태를 갱신한다.
    /// </summary>
    public void CntStat()
    {
        if (characterStat == null)
        {
            return;
        }

        characterStat.CntStat();
    }

    /// <summary>
    /// 휴식 행동 처리
    /// 게임 시스템의 SelectRest()에서 호출된다고 가정한다.
    /// CharacterStat의 RestCh()를 호출한다.
    /// 이후 CntStat()과 CheckCh()를 통해 상태와 모션 또는 이미지를 갱신한다.
    /// </summary>
    public void RestAct()
    {
        if (characterStat == null)
        {
            return;
        }

        characterStat.RestCh();
        CntStat();
        CheckCh();
    }

    /// <summary>
    /// 야근 행동 처리
    /// 게임 시스템의 SelectOT()에서 호출된다고 가정한다.
    /// CharacterStat의 OTCh()를 호출한다.
    /// 이후 CntStat()과 CheckCh()를 통해 상태와 모션 또는 이미지를 갱신한다.
    /// </summary>
    public void OTAct()
    {
        if (characterStat == null)
        {
            return;
        }

        characterStat.OTCh();
        CntStat();
        CheckCh();
    }

    /// <summary>
    /// 카드 행동 처리
    /// 카드 시스템의 UseCard()에서 사용 카드 정보를 가져온다고 가정한다.
    /// 카드의 HP 소모량과 MP 소모량을 전달받아 CharacterStat의 UseHP(), UseMP()를 호출한다.
    /// CharacterMotion의 CardAct()를 호출한다.
    /// 이후 CntStat()과 CheckCh()를 통해 상태와 모션 또는 이미지를 갱신한다.
    ///
    /// 시간 소모 UseTime(), 자산 획득 Asset_plus(), 덱 갱신 UpdateDeck(),
    /// 손패 갱신 UpdateHand(), 무덤 추가 AddCardToGraveyard()는
    /// 다른 파트에서 처리하므로 여기서 구현하지 않는다.
    /// </summary>
    public void ActCard(int useHPCnt, int useMPCnt)
    {
        if (characterStat == null)
        {
            return;
        }

        characterStat.UseHP(useHPCnt);
        characterStat.UseMP(useMPCnt);

        if (characterMotion != null)
        {
            characterMotion.CardAct();
        }

        CntStat();
        CheckCh();
    }
}