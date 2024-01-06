using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private TowerInfo towerInfo;

    [SerializeField] private GameObject infoPage;


    public TextMeshProUGUI towerTypeText;

    public TextMeshProUGUI costText;

    public TextMeshProUGUI attackRangeText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI damageText;

    public Vector2 offset;

    public void OnPointerEnter(PointerEventData eventData)
    {
        towerTypeText.text = "Tower Type: " + towerInfo.towerType.ToString();
        costText.text = "Cost: " + towerInfo.cost.ToString();
        attackRangeText.text = "Attack Range: " + towerInfo.attackRange.ToString();
        attackSpeedText.text = "Attack Speed: " + towerInfo.attackSpeed.ToString();
        damageText.text = "Damage: " + towerInfo.damage.ToString();

        infoPage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoPage.SetActive(false);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        infoPage.transform.position = new Vector2(eventData.position.x, eventData.position.y) + offset;
    }
}
