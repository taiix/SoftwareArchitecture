using UnityEngine;
using UnityEngine.Events;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance { get; private set; }

   // public static UnityAction<int> OnGoldUpdated;

    [SerializeField]private int gold;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }

    private void Start() => UIManager.OnGoldChanged?.Invoke(gold);

    private void Update()
    {
        if (Input.anyKey)
            UpdateGold(1); 
    }
    public void UpdateGold(int amount)
    {
        gold += amount;
        UIManager.OnGoldChanged?.Invoke(gold);
    }

    public int GetAvailableGold() { return gold; }
}
