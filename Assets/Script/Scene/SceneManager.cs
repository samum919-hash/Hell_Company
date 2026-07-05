using UnityEngine;
using GameData;

/// <summary>
/// Scene 파트 전체를 조율하는 허브 클래스.
/// Character, Time, Asset, Deck, Hand, Scheduler 매니저를 조회하고
/// 턴 진행, 야근/휴식 선택, 상환일 판정, 게임오버 판정을 처리한다.
/// 실제 UI 출력은 HUDController에서 담당하며, 이 클래스는 로직만 담당한다.
/// </summary>
public class SceneManager : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CharacterStat characterStat;

    [Header("Time / Asset")]
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private AssetManager assetManager;

    [Header("Card")]
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private HandManager handManager;
    [SerializeField] private SchedulerManager schedulerManager;

    [Header("UI")]
    [SerializeField] private HUDController hudController;
    [SerializeField] private GameObject gameOverPanel;

    private bool isGameOver = false;

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

        if (deckManager == null)
        {
            deckManager = FindObjectOfType<DeckManager>();
        }

        if (handManager == null)
        {
            handManager = FindObjectOfType<HandManager>();
        }

        if (schedulerManager == null)
        {
            schedulerManager = FindObjectOfType<SchedulerManager>();
        }

        if (hudController == null)
        {
            hudController = FindObjectOfType<HUDController>();
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    // ------------------------------------------------------------
    // 조회(Get) 계열
    // ------------------------------------------------------------

    public int GetHP()
    {
        return characterStat != null ? characterStat.CheckHP() : 0;
    }

    public int GetMaxHP()
    {
        if (characterStat == null)
        {
            return 0;
        }

        CharacterData data = characterStat.GetComponent<CharacterData>();
        return data != null ? data.maxHP : 0;
    }

    public int GetMP()
    {
        return characterStat != null ? characterStat.CheckMP() : 0;
    }

    public int GetMaxMP()
    {
        if (characterStat == null)
        {
            return 0;
        }

        CharacterData data = characterStat.GetComponent<CharacterData>();
        return data != null ? data.maxMP : 0;
    }

    public int GetAsset()
    {
        return assetManager != null ? assetManager.Asset_Cnt() : 0;
    }

    public int GetGoalAsset()
    {
        return assetManager != null ? assetManager.Asset_Goal() : 0;
    }

    public int GetTimeStr()
    {
        return timeManager != null ? timeManager.Current_Time() : 0;
    }

    public int GetDayCnt()
    {
        return timeManager != null ? timeManager.Today_cnt() : 0;
    }

    public int GetWeekCnt()
    {
        return timeManager != null ? timeManager.Week_cnt() : 0;
    }

    public TimeMode GetTimeMode()
    {
        return timeManager != null ? timeManager.Get_time_mode() : TimeMode.Day;
    }

    public int GetDeckCnt()
    {
        return deckManager != null ? deckManager.CntDeck() : 0;
    }

    public int GetDiscardCnt()
    {
        return deckManager != null ? deckManager.GetDiscardCnt() : 0;
    }

    public int GetHandCnt()
    {
        return handManager != null ? handManager.GetHandCnt() : 0;
    }

    public int GetSchedulerCnt()
    {
        return schedulerManager != null ? schedulerManager.GetSchedulerCnt() : 0;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    // ------------------------------------------------------------
    // 진입점(Action) 계열
    // ------------------------------------------------------------

    /// <summary>
    /// 하루 시작 처리.
    /// 게임오버 상태라면 아무 것도 하지 않는다.
    /// 손패에 카드를 뽑는다.
    /// </summary>
    public void StartDay()
    {
        if (isGameOver)
        {
            return;
        }

        if (handManager == null)
        {
            Debug.Log("[Scene] HandManager가 연결되지 않았습니다.");
            return;
        }

        handManager.DrawCards();

        UpdateHUD();
    }

    /// <summary>
    /// 턴 넘기기.
    /// 스케줄러에 올라간 카드를 전부 소진할 때까지 순차 실행한다.
    /// 카드 사용 실패(스탯 부족) 시 즉시 중단하고 게임오버 처리한다.
    /// 카드 사용 성공 직후마다 상환일 여부를 확인한다.
    /// </summary>
    public void NextTurn()
    {
        if (isGameOver)
        {
            return;
        }

        if (schedulerManager == null)
        {
            Debug.Log("[Scene] SchedulerManager가 연결되지 않았습니다.");
            return;
        }

        while (schedulerManager.GetSchedulerCnt() > 0)
        {
            bool success = schedulerManager.UseCard();

            if (!success)
            {
                TriggerGameOver();
                return;
            }

            CheckWeekGoal();

            if (isGameOver)
            {
                return;
            }

            CheckGameOver();

            if (isGameOver)
            {
                return;
            }
        }

        UpdateHUD();
    }

    /// <summary>
    /// 야근 선택 처리.
    /// CharacterController의 OTAct()를 호출한다.
    /// </summary>
    public void SelectOT()
    {
        if (isGameOver)
        {
            return;
        }

        if (characterController == null)
        {
            Debug.Log("[Scene] CharacterController가 연결되지 않았습니다.");
            return;
        }

        characterController.OTAct();

        UpdateHUD();
    }

    /// <summary>
    /// 휴식 선택 처리.
    /// CharacterController의 RestAct()를 호출한다.
    /// </summary>
    public void SelectRest()
    {
        if (isGameOver)
        {
            return;
        }

        if (characterController == null)
        {
            Debug.Log("[Scene] CharacterController가 연결되지 않았습니다.");
            return;
        }

        characterController.RestAct();

        UpdateHUD();
    }

    /// <summary>
    /// 상환일(일요일 밤/야근 단계) 도달 여부를 확인하고,
    /// 목표 자산 미달 시 게임오버 처리한다.
    /// TimeManager.UseCardTime()이 주차/일차를 리셋하기 전 시점에 호출되어야 한다.
    /// </summary>
    public void CheckWeekGoal()
    {
        if (timeManager == null || assetManager == null)
        {
            return;
        }

        if (!timeManager.CheckDebtDay())
        {
            return;
        }

        if (assetManager.Asset_Cnt() < assetManager.Asset_Goal())
        {
            TriggerGameOver();
        }
    }

    /// <summary>
    /// 체력 또는 정신력이 0에 도달했는지 확인하고 게임오버 처리한다.
    /// </summary>
    public void CheckGameOver()
    {
        if (characterStat == null)
        {
            return;
        }

        if (characterStat.CheckHP() <= 0 || characterStat.CheckMP() <= 0)
        {
            TriggerGameOver();
        }
    }

    /// <summary>
    /// 게임오버 상태로 전환한다.
    /// 씬 전환 없이 상태 플래그 세팅과 UI 패널 표시만 담당한다.
    /// 이후 확장(결과 화면, 씬 전환 등)은 추후 추가 예정이다.
    /// </summary>
    private void TriggerGameOver()
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Debug.Log("[Scene] 게임오버 발생.");
    }

    // ------------------------------------------------------------
    // 화면 갱신 계열
    // ------------------------------------------------------------

    /// <summary>
    /// HUD 갱신을 HUDController에 위임한다.
    /// </summary>
    public void UpdateHUD()
    {
        if (hudController == null)
        {
            return;
        }

        hudController.UpdateHUD();
    }

    /// <summary>
    /// 배경 갱신을 HUDController에 위임한다.
    /// </summary>
    public void UpdateBackground()
    {
        if (hudController == null)
        {
            return;
        }

        hudController.UpdateBackground();
    }
}
