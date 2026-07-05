using UnityEngine;
using TMPro;

/// <summary>
/// 보유 자산 / 목표 자산 UI를 표시한다.
/// SceneManager로부터 값을 전달받아 화면에 반영만 한다.
/// </summary>
public class AssetHUD : MonoBehaviour
{
    [Header("Asset UI")]
    [SerializeField] private TMP_Text assetText;
    [SerializeField] private TMP_Text goalAssetText;

    /// <summary>
    /// 자산 UI 갱신.
    /// </summary>
    public void UpdateAssetUI(int asset, int goalAsset)
    {
        if (assetText != null)
        {
            assetText.text = asset + "G";
        }

        if (goalAssetText != null)
        {
            goalAssetText.text = "목표 " + goalAsset + "G";
        }
    }
}
