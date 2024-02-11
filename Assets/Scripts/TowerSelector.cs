using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Allows selecting a tower for placement during the building state of the game.
/// </summary>
public class TowerSelector : MonoBehaviour
{
    public static UnityAction<GameObject> OnTowerSelected;

    /// <summary>
    /// Selects a tower prefab for placement if it can be bought and the game state is building.
    /// </summary>
    public void SelectTower(GameObject prefab)
    {
        if (GameManager.instance.State == GameStates.BuildingState)
        {
            if (prefab.TryGetComponent<Tower>(out Tower selectedTower)) {
                if (selectedTower.canBeBought)
                {
                    OnTowerSelected?.Invoke(prefab);
                }
            }
        }
    }
}
