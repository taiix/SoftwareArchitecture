using TMPro;
using UnityEngine;

/// <summary>
/// Class responsible for controlling button texts and updating them based on tower information.
/// </summary>
public class ButtonsController : MonoBehaviour
{
    public string updageTextUpdater;
    public string sellTextUpdater;

    [SerializeField] TextMeshProUGUI upgradeText;
    [SerializeField] TextMeshProUGUI sellText;

    private Tower selectedTower;

    private void Update()
    {
        upgradeText.text = updageTextUpdater;
        sellText.text = sellTextUpdater;
    }

    /// <summary>
    /// Updates the text for upgrade and sell buttons based on the provided tower's information.
    /// </summary>
    /// <param name="currentTower"></param>
    public void TextButtonUpdater(Tower currentTower)
    {
        selectedTower = currentTower;
        if (selectedTower != null)
        {
            float sellCost = currentTower.type.cost;
            float upgradeCost = currentTower.upgradeCost;

            updageTextUpdater = "-$" + upgradeCost.ToString();
            sellTextUpdater = "+$" + sellCost.ToString();
        }
    }
}
