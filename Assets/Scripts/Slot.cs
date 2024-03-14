using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData item;
    public Image itemVisual;
    public Text countText;

    [SerializeField] private ItemActionSystem itemActionSystem;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
             TooltipSystem.instance.Show(item.itemDescription, item.itemName);
        }
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.instance.Hide();
    }

    public void ClickOnslot()
    {
        itemActionSystem.OpenActionPanel(item, transform.position);
    }
}
