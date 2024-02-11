using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages the building of towers by the player.
/// </summary>
public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    [SerializeField] private OccupiedPositionsHandler occupiedPos; // Reference to the handler for occupied positions
    [SerializeField] private Grid grid; // Reference to the grid for tower placement

    private GameObject prefab; // Selected tower prefab
    private bool playerInputEnabled; // Indicating if player input is enabled
    private Vector3 potentialPosition; // Potential position for tower placement
    private const float roundToGridPosition = 0.5f; // Offset for aligning tower to grid

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void TowerBuild(GameObject selectedTower) => prefab = selectedTower;

    private void Start()
    {
        InputManager.Instance.OnMousePositionChange += OnBuildingPlacePositionChanged;
        TowerSelector.OnTowerSelected += TowerBuild;
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnMousePositionChange -= OnBuildingPlacePositionChanged;
        TowerSelector.OnTowerSelected -= TowerBuild;
    }

    /// <summary>
    /// Enables or disables player input for tower building.
    /// </summary>
    public void PlayerInputEnabled(bool isEnabled) => playerInputEnabled = isEnabled;

    private void OnBuildingPlacePositionChanged(Vector3 pos)
    {
        if (!playerInputEnabled) return;
        potentialPosition = pos;
    }

    private void Update()
    {
        if (!playerInputEnabled)
        {
            prefab = null;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Build();
        }
    }

    /// <summary>
    /// Attempts to build a tower at the current position.
    /// </summary>
    private void Build()
    {
        Camera cam = Camera.main;

        Vector3 mousePos = Input.mousePosition;

        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            Vector3 checkPosition = hit.collider.transform.position;

            // Check if the position is not occupied and not over a UI element
            if (!occupiedPos.occupiedPositions.Contains(checkPosition) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (prefab == null) return;

                if (prefab.TryGetComponent<Tower>(out Tower tower))
                {
                    CurrencyManager currency = CurrencyManager.instance;
                    int cost = tower.type.cost;

                    if (currency.HasEnoughCurrency(cost))
                    {
                        currency.DeductCurrency(cost);
                        SelectedTower(prefab);
                        prefab = null;
                    }
                }
            }
            else
            {
                prefab = null;
            }
        }
    }

    /// <summary>
    /// Instantiates the selected tower at the adjusted position on the grid.
    /// </summary>
    public void SelectedTower(GameObject prefab)
    {
        Vector3Int gridPos = grid.WorldToCell(potentialPosition);

        Vector3 adjustedPosition = gridPos + new Vector3(roundToGridPosition, Vector3.zero.y, roundToGridPosition);

        GameObject go = Instantiate(prefab, adjustedPosition, Quaternion.identity);
        occupiedPos.occupiedPositions.Add(go.transform.position);
    }
}
