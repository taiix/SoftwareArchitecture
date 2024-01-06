using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public UnityAction<Vector3> OnMousePositionChange;
    private Camera cam;

    [SerializeField]
    private LayerMask layer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() => cam = Camera.main;

    private void Update() => GetMousePosition();

    public void GetMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;

        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, layer))
        {
            Vector3 pos = hit.point;
            OnMousePositionChange?.Invoke(pos);
        }
    }
}
