using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Header("Tower Properties")]
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackSpeed;

    [SerializeField] private int upgradeCost;

    private int towerLevel = 1;

    [Space]
    [Header("References")]
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject ui;

    [SerializeField] private Button upgradeBtn;
    [SerializeField] private Button destroyBtn;

    IUpgradable selectedTower;
    private bool isOccupied;

    void Start()
    {
        InputManager.Instance.OnMousePositionChange += PositionHandler;

        upgradeBtn.onClick.AddListener(UpgradeTower);
        destroyBtn.onClick.AddListener(DestroyTower);
    }

    private void OnDisable() => InputManager.Instance.OnMousePositionChange -= PositionHandler;

    private void PositionHandler(Vector3 selectedPosition)
    {
        float roundToGridPosition = 0.5f;
        Vector3Int gridPos = grid.WorldToCell(selectedPosition);

        Vector3 adjustedPosition = gridPos + new Vector3(roundToGridPosition, Vector3.zero.y, roundToGridPosition);

        isOccupied = OccupiedPositionsHandler.Instance.IsPositionOccupied(adjustedPosition);
    }

    private void Update()
    {
        SelectTowerToUpgrade();
    }

    void DestroyTower()
    {
        if (selectedTower != null)
        {
            if (selectedTower is MonoBehaviour mTower)
            {
                Debug.Log("Destroy tower " + selectedTower.ToString());

                Destroy(mTower.gameObject);
                selectedTower = null;

                ui.SetActive(false);

                OccupiedPositionsHandler.Instance.occupiedPositions.Remove(mTower.gameObject.transform.position);
                OccupiedPositionsHandler.Instance.OnOccupiedChanged?.Invoke(mTower.transform.position);
            }
        }
    }

    void UpgradeTower()
    {
        CurrencyManager currency = CurrencyManager.instance;

        if (currency.HasEnoughCurrency(upgradeCost) && selectedTower != null)
        {
            if (selectedTower is MonoBehaviour mTower)
            {
                if (mTower.TryGetComponent<Upgrading>(out Upgrading upgrade) && mTower.TryGetComponent<Tower>(out Tower currentTower))
                {
                    upgradeCost = currentTower.upgradeCost;
                    upgrade.Upgrade(attackRange, attackDamage, attackSpeed, towerLevel);
                    currency.DeductCurrency(upgradeCost);
                    upgrade.CostUpdate(100);
                }
            }
        }
    }

    void SelectTowerToUpgrade()
    {
        if (!isOccupied) return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                IUpgradable upgradable = hit.collider.GetComponent<IUpgradable>();
                if (upgradable != null)
                {
                    ui.SetActive(true);
                    selectedTower = upgradable;
                    Vector3 uiPosition = hit.collider.bounds.center;
                    ui.transform.position = uiPosition;
                }
            }
        }
    }
}