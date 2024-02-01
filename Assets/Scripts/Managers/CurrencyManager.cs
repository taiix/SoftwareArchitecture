using UnityEngine;

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

    public void UpdateGold(int amount)
    {
        gold += amount;
        UIManager.OnGoldChanged?.Invoke(gold);
    }

    public int GetAvailableGold() => gold;

    public bool HasEnoughCurrency(int amount)
    {
        return gold >= amount;
    }

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
