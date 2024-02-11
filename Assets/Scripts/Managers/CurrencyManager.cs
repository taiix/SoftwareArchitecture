using UnityEngine;

/// <summary>
/// Manages the player's currency in the game.
/// </summary>
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance { get; private set; }


    [SerializeField] private int gold;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }

    private void Start() => UIManager.OnGoldChanged?.Invoke(gold);

    /// <summary>
    /// Add gold and update the UI
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateGold(int amount)
    {
        gold += amount;
        UIManager.OnGoldChanged?.Invoke(gold);
    }

    public int GetAvailableGold() => gold;

    /// <summary>
    /// Check if the player has enough gold
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool HasEnoughCurrency(int amount)
    {
        return gold >= amount;
    }

    /// <summary>
    /// Deducts the specified amount of gold from the player's balance if enough gold is available.
    /// Updates the UI with the new gold amount.
    /// </summary>
    /// <param name="amount"></param>
    public void DeductCurrency(int amount)
    {
        if (HasEnoughCurrency(amount))
        {
            gold -= amount;
            UIManager.OnGoldChanged?.Invoke(gold);
        }
        else
        {
            Debug.LogError("Not enough gold to deduct.");
        }
    }
}
