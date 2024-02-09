using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public TowerInfo towerInfo;

    [SerializeField] private GameObject infoPage;

    [SerializeField] private TextMeshProUGUI towerCostTextUI;
    [SerializeField] private TextMeshProUGUI towerTypeText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI attackRangeText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI damageText;

    [SerializeField] private Vector2 offset;

    void Start() => towerCostTextUI.text = "$" + towerInfo.cost.ToString();

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
