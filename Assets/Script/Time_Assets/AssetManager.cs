using System;
using UnityEngine;
using GameData;

public class AssetManager : MonoBehaviour
{
    public static AssetManager Instance { get; private set; }

    // 자산 변경 이벤트
    public static event Action OnAssetChanged;

    [Header("현재 자산")]
    [SerializeField] private int currentAsset = 0;

    [Header("주차별 목표 자산")]
    [SerializeField]
    private int[] weekGoals =
    {
        10000,   // 1주차
        25000,   // 2주차
        50000,   // 3주차
        100000   // 4주차
    }; // 임시

    private int goalAsset;

    [Header("야근 수당")]
    [SerializeField] private int overtimeBonusAmount = 50;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        UpdateGoalAsset();
    }

    /// <summary>
    /// 현재 자산 반환
    /// </summary>
    public int Asset_Cnt()
    {
        return currentAsset;
    }

    /// <summary>
    /// 현재 목표 자산 반환
    /// </summary>
    public int Asset_Goal()
    {
        return goalAsset;
    }

    /// <summary>
    /// 자산 증가
    /// </summary>
    public void Asset_plus(int amount)
    {
        currentAsset += amount;
        OnAssetChanged?.Invoke();
    }

    /// <summary>
    /// 자산 감소
    /// </summary>
    public void Asset_use(int amount)
    {
        currentAsset -= amount;
        OnAssetChanged?.Invoke();
    }

    /// <summary>
    /// 현재 주차에 맞는 목표 자산 갱신
    /// </summary>
    public void UpdateGoalAsset()
    {
        if (TimeManager.Instance == null)
            return;

        int week = TimeManager.Instance.Week_cnt();

        if (week <= weekGoals.Length)
            goalAsset = weekGoals[week - 1];
        else
            goalAsset = weekGoals[weekGoals.Length - 1];

        Debug.Log($"[목표 자산 갱신] {week}주차 목표 : {goalAsset}G");

        OnAssetChanged?.Invoke();
    }

    /// <summary>
    /// 시간 모드에 따른 보상
    /// </summary>
    public void ProcessTimeModeReward()
    {
        if (TimeManager.Instance == null)
            return;

        switch (TimeManager.Instance.Get_time_mode())
        {
            case TimeMode.Day:
                // 낮
                break;

            case TimeMode.Overtime:
                Asset_plus(overtimeBonusAmount);
                Debug.Log($"[야근 수당] +{overtimeBonusAmount}G");
                break;

            case TimeMode.Rest:
                // 휴식
                break;
        }
    }
}