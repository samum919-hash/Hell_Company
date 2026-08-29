using System;
using UnityEngine;
using GameData;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    // 시간이 변경되었을 때 UI나 다른 매니저들에게 알리는 이벤트
    public static event Action OnTimeChanged;

    [Header("Time Data")]
    [SerializeField] private int currentWeek = 1;
    [SerializeField] private int currentDay = 1;
    [SerializeField] private int currentHour = 8;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// 현재 시간 모드 반환
    /// </summary>
    public TimeMode Get_time_mode() // 아침 밤 각 8시간으로 정했는데 아침 10시간, 밤 6시간이라 수정했음
    {
        if (currentHour >= 8 && currentHour < 16)
            return TimeMode.Day;

        if (currentHour >= 16 && currentHour <= 24)
            return TimeMode.Overtime;

        return TimeMode.Rest;
    }

    /// <summary>
    /// 현재 시간 반환
    /// </summary>
    public int Current_Time() => currentHour;

    /// <summary>
    /// 현재 일차 반환
    /// </summary>
    public int Today_cnt() => currentDay;

    /// <summary>
    /// 현재 주차 반환
    /// </summary>
    public int Week_cnt() => currentWeek;

    /// <summary>
    /// 상환일(일요일 밤/야근 단계) 도달 여부 확인
    /// 7일차이면서 현재 시간대가 Overtime(야근) 상태일 때 true 반환
    /// </summary>

    public bool CheckDebtDay() 
    {
        return currentDay == 7 && Get_time_mode() == TimeMode.Overtime;
    }

    /// <summary>
    /// 카드 사용 시 시간 진행
    /// </summary>
    public void UseCardTime(int timeCost)
    {
        currentHour += timeCost;

        // 24시를 초과하면 다음 날 08시 시작
        if (currentHour > 24)
        {
            currentHour = 8;
            currentDay++;

            if (currentDay > 7)
            {
                currentDay = 1;
                currentWeek++;

                // 목표 자산 갱신
                AssetManager.Instance?.UpdateGoalAsset();
            }
        }

        OnTimeChanged?.Invoke();
    }

    /// <summary>
    /// 테스트용: 시간을 직접 지정
    /// </summary>
    public void SetTimeForTest(int hour)
    {
        currentHour = hour;
        OnTimeChanged?.Invoke();
    }
}