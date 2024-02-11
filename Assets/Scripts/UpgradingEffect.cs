using UnityEngine;

/// <summary>
/// Apply pulse shader during Building State
/// </summary>
public class UpgradingEffect : MonoBehaviour
{
    [SerializeField] private Material pulseShader;
    [SerializeField] private Material[] mats;

    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();

        mats = GetComponent<Renderer>().materials;
    }

    private void Update()
    {
        if (GameManager.instance?.State == GameStates.BuildingState)
        {
            ApplyPulseShader();
        }
        else
        {
            RevertToNormalMaterial();
        }
    }

    private void ApplyPulseShader()
    {
        rend.material = pulseShader;
    }

    private void RevertToNormalMaterial()
    {
        rend.materials = mats;
    }
}
