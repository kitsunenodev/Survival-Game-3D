using UnityEngine;

public class ItemActionSystem : MonoBehaviour
{
    [Header("OTHER SCRIPTS REFERENCES")]
    
    [SerializeField] 
    private Equipment equipment;

    [SerializeField]
    private PlayerStat playerStat;
    
    [Header("Action Panel References")]
    
    [SerializeField]
    private GameObject actionPanel;
    
    [SerializeField] private Transform dropPoint;
    
    [SerializeField]
    private GameObject useItemButton;
    
    [SerializeField]
    private GameObject equipItemButton;
    
    [SerializeField]
    private GameObject dropItemButton;
    
    [SerializeField]
    private GameObject destroyItemButton;
 
    [HideInInspector]
    public ItemData itemSelected;
    
    
    
    public void OpenActionPanel(ItemData item, Vector3 slotPosition)
    {
        itemSelected = item;
        if (item == null)
        {
            CloseActionPanel();
            return;
        }
        switch (item.itemType)
        {
            case ItemType.Resources:
                useItemButton.SetActive(false);
                equipItemButton.SetActive(false);
                break;
            
            case ItemType.Equipment:
                useItemButton.SetActive(false);
                equipItemButton.SetActive(true);
                break;
            
            case ItemType.Consumable:
                useItemButton.SetActive(true);
                equipItemButton.SetActive(false);
                break;
        }

        actionPanel.transform.position = slotPosition;
        actionPanel.SetActive(true);
    }

    public void CloseActionPanel()
    {
        
        actionPanel.SetActive(false);
        itemSelected = null;
    }

    public void UseItem()
    {
       playerStat.ConsumeItem(itemSelected.healthRegen, itemSelected.hungerRegen, itemSelected.thirstRegen);
       Inventory.instance.RemoveItem(itemSelected);
        CloseActionPanel();
    }

   

    public void DropItem()
    {
        GameObject itemInstanciated = Instantiate(itemSelected.itemPrefab);
        itemInstanciated.transform.position = dropPoint.position;
        Inventory.instance.RemoveItem(itemSelected);
        
        Inventory.instance.RefreshContent();
        CloseActionPanel();
    }

    public void DestroyItem()
    {
        Inventory.instance.RemoveItem(itemSelected);
        Inventory.instance.RefreshContent();
        CloseActionPanel();
    }

    public void EquipItem()
    {
        equipment.EquipItem();
    }
}
