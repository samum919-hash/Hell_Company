using TMPro;
using UnityEngine;

public class AssetPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI assetValue;

    private void OnEnable()
    {
        AssetManager.OnAssetChanged += UpdateAssetValue;

        UpdateAssetValue();
    }

    private void OnDisable()
    {
        AssetManager.OnAssetChanged -= UpdateAssetValue;
    }

    private void UpdateAssetValue()
    {
        if (AssetManager.Instance == null)
            return;

        assetValue.text = $"${AssetManager.Instance.Asset_Cnt():N0}";
    }
}