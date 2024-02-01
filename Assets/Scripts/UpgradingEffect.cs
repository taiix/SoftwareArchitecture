using System.Linq;
using UnityEngine;

public class UpgradingEffect : MonoBehaviour
{
    [SerializeField] private Material pulseShader;
    [SerializeField] private Material[] normalMat;


    private void Update()
    {
        if (GameManager.instance.State == GameStates.BuildingState)
        {
            IUpgradable[] towers = FindObjectsOfType<MonoBehaviour>().OfType<IUpgradable>().ToArray();

            //save curr mat
            foreach (IUpgradable towerMaterial in towers)
            {
                if (towerMaterial is MonoBehaviour mTower)
                {
                    if (mTower.TryGetComponent<Renderer>(out Renderer renderer))
                    {
                        normalMat = renderer.materials;
                    }
                }
            }

            //apply shader
            foreach (IUpgradable tower in towers)
            {
                ChangeShader(tower, pulseShader);
            }
        }
        else
        {
            IUpgradable[] towers = FindObjectsOfType<MonoBehaviour>().OfType<IUpgradable>().ToArray();

            foreach (IUpgradable tower in towers)
            {
                for (int i = 0; i < normalMat.Length; i++)
                {
                    ChangeShader(tower, normalMat[i]);
                }
            }
        }
    }

    void ChangeShader(IUpgradable tower, Material mat)
    {
        if (tower is MonoBehaviour mTower)
        {
            Transform transform = mTower.transform;
            mTower.GetComponent<Renderer>().material = pulseShader;

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);

                // Do something with the child (e.g., change its material)
                if (childTransform.TryGetComponent<Renderer>(out Renderer childRenderer))
                {
                    childRenderer.material = mat;
                }
            }
        }
    }
}
