using UnityEngine;
using UnityEngine.Events;

public class TowerSelector : MonoBehaviour
{
    public static UnityAction<GameObject> OnTowerSelected;

    public void SelectTower(GameObject prefab)
    {
        if (GameManager.instance.State == GameStates.BuildingState)
        {
            OnTowerSelected?.Invoke(prefab);
        }
    }
}
