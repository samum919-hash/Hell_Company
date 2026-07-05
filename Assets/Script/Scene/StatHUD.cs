using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 체력/정신력 UI(Slider, Text)를 표시한다.
/// SceneManager로부터 값을 전달받아 화면에 반영만 한다.
/// </summary>
public class StatHUD : MonoBehaviour
{
    [Header("Stat UI")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider mpSlider;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text mpText;

    /// <summary>
    /// 체력/정신력 UI 갱신.
    /// </summary>
    public void UpdateStatUI(int hp, int maxHp, int mp, int maxMp)
    {
        if (hpSlider != null && maxHp > 0)
        {
            hpSlider.maxValue = maxHp;
            hpSlider.value = hp;
        }

        if (mpSlider != null && maxMp > 0)
        {
            mpSlider.maxValue = maxMp;
            mpSlider.value = mp;
        }

        if (hpText != null)
        {
            hpText.text = hp + " / " + maxHp;
        }

        if (mpText != null)
        {
            mpText.text = mp + " / " + maxMp;
        }
    }
}
