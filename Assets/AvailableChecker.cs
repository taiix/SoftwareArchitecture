using UnityEngine;
using UnityEngine.UI;

public class AvailableChecker : MonoBehaviour
{
    private CurrencyManager currencyManager;
    private ButtonInfo buttonInfo;

    private Button button;

    private int currentCost;

    private ColorBlock colorBlock;

    private void Start()
    {
        currencyManager = CurrencyManager.instance;

        buttonInfo = GetComponent<ButtonInfo>();

        button = GetComponent<Button>();
        colorBlock = GetComponent<Button>().colors;

        currentCost = buttonInfo.towerInfo.cost;
    }

    private void Update()
    {
        if (currencyManager.HasEnoughCurrency(currentCost))
        {
            
            //colorBlock.normalColor = Color.white;
            //colorBlock.selectedColor = Color.white;
            //colorBlock.disabledColor = Color.white;

            button.colors = ColorBlock.defaultColorBlock;
        }
        else {
            colorBlock.normalColor = Color.red;
            colorBlock.selectedColor = Color.red;
            colorBlock.disabledColor = Color.red;
            colorBlock.highlightedColor = Color.red;
            colorBlock.pressedColor = Color.red;
            button.colors = colorBlock;
        }
    }
}
