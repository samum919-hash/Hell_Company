using UnityEngine;

/// <summary>
/// StatHUD, AssetHUD, TimeHUD, DeckHandHUD, BackgroundController를 모아
/// 한 번에 갱신을 지시하는 상위 컨트롤러.
/// SceneManager의 조회 함수 결과를 각 하위 HUD에 전달만 한다.
/// TimeManager, AssetManager의 이벤트를 구독해 자동 갱신도 처리한다.
/// </summary>
public class HUDController : MonoBehaviour
{
    [Header("Scene Manager")]
    [SerializeField] private SceneManager sceneManager;

    [Header("HUD Parts")]
    [SerializeField] private StatHUD statHUD;
    [SerializeField] private AssetHUD assetHUD;
    [SerializeField] private TimeHUD timeHUD;
    [SerializeField] private DeckHandHUD deckHandHUD;
    [SerializeField] private BackgroundController backgroundController;

    private void Awake()
    {
        if (statHUD == null)
        {
            statHUD = FindObjectOfType<StatHUD>();
        }

        if (assetHUD == null)
        {
            assetHUD = FindObjectOfType<AssetHUD>();
        }

        if (timeHUD == null)
        {
            timeHUD = FindObjectOfType<TimeHUD>();
        }

        if (deckHandHUD == null)
        {
            deckHandHUD = FindObjectOfType<DeckHandHUD>();
        }

        if (backgroundController == null)
        {
            backgroundController = FindObjectOfType<BackgroundController>();
        }
    }

    private void OnEnable()
    {
        TimeManager.OnTimeChanged += HandleTimeChanged;
        AssetManager.OnAssetChanged += HandleAssetChanged;
    }

    private void OnDisable()
    {
        TimeManager.OnTimeChanged -= HandleTimeChanged;
        AssetManager.OnAssetChanged -= HandleAssetChanged;
    }

    private void HandleTimeChanged()
    {
        UpdateTime();
        UpdateBackground();
    }

    private void HandleAssetChanged()
    {
        UpdateAsset();
    }

    /// <summary>
    /// HUD 전체 갱신.
    /// SceneManager 조회 함수들을 호출해 각 하위 HUD에 값을 전달한다.
    /// </summary>
    public void UpdateHUD()
    {
        if (sceneManager == null)
        {
            return;
        }

        UpdateStat();
        UpdateAsset();
        UpdateTime();
        UpdateDeckHand();
    }

    private void UpdateStat()
    {
        if (statHUD == null)
        {
            return;
        }

        statHUD.UpdateStatUI(
            sceneManager.GetHP(),
            sceneManager.GetMaxHP(),
            sceneManager.GetMP(),
            sceneManager.GetMaxMP());
    }

    private void UpdateAsset()
    {
        if (assetHUD == null || sceneManager == null)
        {
            return;
        }

        assetHUD.UpdateAssetUI(
            sceneManager.GetAsset(),
            sceneManager.GetGoalAsset());
    }

    private void UpdateTime()
    {
        if (timeHUD == null || sceneManager == null)
        {
            return;
        }

        timeHUD.UpdateTimeUI(
            sceneManager.GetTimeStr(),
            sceneManager.GetDayCnt(),
            sceneManager.GetWeekCnt(),
            sceneManager.GetTimeMode());
    }

    private void UpdateDeckHand()
    {
        if (deckHandHUD == null || sceneManager == null)
        {
            return;
        }

        deckHandHUD.UpdateDeckHandUI(
            sceneManager.GetDeckCnt(),
            sceneManager.GetDiscardCnt(),
            sceneManager.GetHandCnt(),
            sceneManager.GetSchedulerCnt());
    }

    /// <summary>
    /// 배경 갱신을 BackgroundController에 위임한다.
    /// </summary>
    public void UpdateBackground()
    {
        if (backgroundController == null || sceneManager == null)
        {
            return;
        }

        backgroundController.UpdateBackground(sceneManager.GetTimeMode());
    }
}
