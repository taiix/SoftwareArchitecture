using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Manages tower upgrades and UI interaction during gameplay.
/// </summary>

public class UpgradeManager : MonoBehaviour
{
    [Header("Tower Properties")]
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackSpeed;

    [SerializeField] private int sellCost;

    private int towerLevel = 1;
    [SerializeField] private int upgradeCost;

    [Space]
    [Header("References")]
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject ui;

    [SerializeField] private Button upgradeBtn;
    [SerializeField] private Button destroyBtn;

    [SerializeField] private ButtonsController buttonsTextController;
    
    private bool canUseMenu;
    private bool isOccupied;

    IUpgradable selectedTower;

    private void Awake() => GameManager.instance.OnGameStateChangedNotifier += GameStateChangeState;

    void Start()
    {
        InputManager.Instance.OnMousePositionChange += PositionHandler;

        upgradeBtn.onClick.AddListener(UpgradeTower);
        destroyBtn.onClick.AddListener(DestroyTower);
    }

    private void GameStateChangeState(GameStates state)
    {
        canUseMenu = state == GameStates.BuildingState;
        if (!canUseMenu)
            ui.SetActive(false);
    }

    private void OnDisable()
    {
        InputManager.Instance.OnMousePositionChange -= PositionHandler;
        GameManager.instance.OnGameStateChangedNotifier -= GameStateChangeState;
    }

    private void PositionHandler(Vector3 selectedPosition)
    {
        float roundToGridPosition = 0.5f;
        Vector3Int gridPos = grid.WorldToCell(selectedPosition);

        Vector3 adjustedPosition = gridPos + new Vector3(roundToGridPosition, Vector3.zero.y, roundToGridPosition);

        isOccupied = OccupiedPositionsHandler.Instance.IsPositionOccupied(adjustedPosition);
    }

    private void Update()
    {
        if (!canUseMenu) return;

        SelectTowerToUpgrade();
        UpdateIU();


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                IUpgradable upgradable = hit.collider.GetComponent<IUpgradable>();
                if (upgradable == null && !EventSystem.current.IsPointerOverGameObject())
                    ui.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Updates the UI elements.
    /// </summary>
    void UpdateIU() {
        if (selectedTower is MonoBehaviour mTower)
        {
            if (mTower.TryGetComponent<Tower>(out Tower currentTower))
            {
                buttonsTextController.TextButtonUpdater(currentTower);
            }
        }
    }

    /// <summary>
    /// Destroys the selected tower and updates the UI and the gold.
    /// </summary>
    void DestroyTower()
    {
        CurrencyManager currency = CurrencyManager.instance;

        if (selectedTower != null)
        {
            if (selectedTower is MonoBehaviour mTower)
            {
                if (mTower.TryGetComponent<Tower>(out Tower currentTower))
                {
                    sellCost = currentTower.type.cost;
                    buttonsTextController.TextButtonUpdater(currentTower);
                    currency.UpdateGold(sellCost);
                }

                Destroy(mTower.gameObject);
                selectedTower = null;

                ui.SetActive(false);

                OccupiedPositionsHandler.Instance?.occupiedPositions.Remove(mTower.gameObject.transform.position);
                OccupiedPositionsHandler.Instance.OnOccupiedChanged?.Invoke(mTower.transform.position);
            }
        }
    }


    /// <summary>
    /// Upgrades the selected tower, deducts the cost, and updates the UI.
    /// </summary>
    void UpgradeTower()
    {
        CurrencyManager currency = CurrencyManager.instance;

        if (selectedTower is MonoBehaviour mTower)
        {
            if (mTower.TryGetComponent<Upgrading>(out Upgrading upgrade) && mTower.TryGetComponent<Tower>(out Tower currentTower))
            {
                upgradeCost = currentTower.upgradeCost;
                if (currency.HasEnoughCurrency(upgradeCost) && selectedTower != null)
                {
                    upgrade.Upgrade(attackRange, attackDamage, attackSpeed, towerLevel);
                    currency.DeductCurrency(upgradeCost);
                    upgrade.CostUpdate(100);

                    Vector3 currentScale = mTower.gameObject.transform.localScale;
                    float scaleFactor = 0.1f;
                    mTower.gameObject.transform.localScale += currentScale * scaleFactor;
                }
            }
        }
    }

    /// <summary>
    /// Selects a tower for upgrade and displays its UI.
    /// </summary>

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
                    selectedTower = upgradable;
                    Vector3 uiPosition = hit.collider.bounds.center;
                    ui.transform.position = uiPosition;

                    UpdateIU();

                    ui.SetActive(true);
                }
            }
        }
    }
}