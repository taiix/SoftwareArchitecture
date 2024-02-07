using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    public GameObject prefab;

    public bool playerInputEnabled;

    public Vector3 potentialPosition;
    public Grid grid;

    private const float roundToGridPosition = 0.5f;

    [SerializeField] private OccupiedPositionsHandler occupiedPos;

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

    private void Build()
    {
        Camera cam = Camera.main;

        Vector3 mousePos = Input.mousePosition;

        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {

            Vector3 checkPosition = hit.collider.transform.position;

            if (!occupiedPos.occupiedPositions.Contains(checkPosition) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (prefab == null) return;

                if (prefab.TryGetComponent<Tower>(out Tower tower))
                {
                    CurrencyManager currency = CurrencyManager.instance;
                    int cost = tower.type.cost;

                    if (currency.HasEnoughCurrency(cost)) { 
                        currency.DeductCurrency(cost);
                        SelectedTower(prefab);
                    }
                }
            }
            else
            {
                prefab = null;
            }

        }
    }

    public void SelectedTower(GameObject prefab)
    {
        Vector3Int gridPos = grid.WorldToCell(potentialPosition);

        Vector3 adjustedPosition = gridPos + new Vector3(roundToGridPosition, Vector3.zero.y, roundToGridPosition);

        GameObject go = Instantiate(prefab, adjustedPosition, Quaternion.identity);
        occupiedPos.occupiedPositions.Add(go.transform.position);
    }
}
