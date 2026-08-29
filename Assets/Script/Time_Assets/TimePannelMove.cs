using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class TimePannelMove : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private RectTransform morningPanel;
    [SerializeField] private RectTransform nightPanel;

    [Header("Position")]
    [SerializeField] private float hiddenY = 900f;
    [SerializeField] private float visibleY = 133f;

    [Header("Timing")]
    [SerializeField] private float moveDuration = 3f;
    [SerializeField] private float stayDuration = 2f;

    private TimeMode lastMode;
    private Coroutine morningRoutine;
    private Coroutine nightRoutine;

    private void OnEnable() // 이벤트 확인
    {
        TimeManager.OnTimeChanged += HandleTimeChanged;
        lastMode = TimeManager.Instance.Get_time_mode();
    }

    private void OnDisable()
    {
        TimeManager.OnTimeChanged -= HandleTimeChanged;
    }

    private void HandleTimeChanged() // 모드 변화 감지
    {
        TimeMode currentMode = TimeManager.Instance.Get_time_mode();

        if (currentMode == lastMode)
        {
            lastMode = currentMode;
            return;
        }

        if (currentMode == TimeMode.Day && lastMode == TimeMode.Overtime)
        {
            PlayPanel(morningPanel, ref morningRoutine);
        }
        else if (currentMode == TimeMode.Overtime && lastMode == TimeMode.Day)
        {
            PlayPanel(nightPanel, ref nightRoutine);
        }

        lastMode = currentMode;
    }

    // Animation Action
    private void PlayPanel(RectTransform panel, ref Coroutine routine)
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(PanelSequence(panel));
    }

        private IEnumerator PanelSequence(RectTransform panel)
    {
        yield return MoveY(panel, hiddenY, visibleY);
        yield return new WaitForSeconds(stayDuration);
        yield return MoveY(panel, visibleY, hiddenY);
    }

    // move
    private IEnumerator MoveY(RectTransform panel, float fromY, float toY)
    {
        float elapsed = 0f;
        Vector2 pos = panel.anchoredPosition;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            pos.y = Mathf.Lerp(fromY, toY, t);
            panel.anchoredPosition = pos;
            yield return null;
        }

        pos.y = toY;
        panel.anchoredPosition = pos;
    }
}