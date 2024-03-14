using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [Header("OTHER SCRIPTS REFERENCES")]
    
    [SerializeField]
    private Equipment equipment;
    
    [SerializeField]
    private ItemActionSystem itemActionSystem;

    [SerializeField]
    private CraftingSystem craftSystem;

    [SerializeField]
    private BuildSystem buildSystem;
    
    
    [Header("INVENTORY SYSTEM VARIABLES")]
    public static Inventory instance;
    [SerializeField]
    private List<ItemInInventory> content = new List<ItemInInventory>();

    [SerializeField]
    private GameObject inventoryPanel;
    
    [SerializeField] 
    private Transform inventorySlotParent;
    
    public Sprite emptySlotImage;
    
    private const int InventorySize = 24;
    private bool isOpen;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There are more than one instance of Inventory");
            return;
        }

        instance = this;
    }

    public void Start()
    {
        RefreshContent();
        CloseInventory();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isOpen) CloseInventory();
            else OpenInventory();

        }
    }
    
    public void AddItem(ItemData item)
    {
        ItemInInventory[] itemsInInventory = content.Where(elem => elem.itemData == item).ToArray();

        bool itemAdded = false;
        
        if (itemsInInventory.Length > 0 && item.stackable)
        {
            for (int i = 0; i < itemsInInventory.Length; i++)
            {
                if (itemsInInventory[i].count < item.maxStack)
                {
                    itemsInInventory[i].count++;
                    itemAdded = true;
                    break;
                }
            }

            if (!itemAdded)
            {
                content.Add(new ItemInInventory
                {
                    itemData = item,
                    count = 1
                });
            }
        }
        else
        {
            content.Add(new ItemInInventory
            {
                itemData = item,
                count = 1
            });
        }
        
        RefreshContent();
    }
    
    public void RemoveItem(ItemData item)
    {
        ItemInInventory itemInInventory = content.Where(elem => elem.itemData == item).FirstOrDefault();

        if (itemInInventory == null)
        {
            return;
        }
        if (itemInInventory.count > 1)
        {
            itemInInventory.count--;
        }
        else
        {
            content.Remove(itemInInventory);
        }
        RefreshContent();
    }

    public List<ItemInInventory> GetContent()
    {
        return content;
    }

    private void OpenInventory()
    {
        isOpen = true;
        inventoryPanel.SetActive(true);
    }

    public void CloseInventory()
    {
        isOpen = false;
        inventoryPanel.SetActive(false);
        TooltipSystem.instance.Hide();
        itemActionSystem.CloseActionPanel();
    }

    public void RefreshContent()
    {
        for (int i = 0; i < inventorySlotParent.childCount; i++)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = null;
            currentSlot.itemVisual.sprite = emptySlotImage;
            currentSlot.countText.enabled = false;
        }
        for (int i = 0; i < content.Count; i++)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = content[i].itemData;
            currentSlot.itemVisual.sprite = content[i].itemData.itemVisual;
            if (currentSlot.item.stackable)
            {
                currentSlot.countText.enabled = true;
                currentSlot.countText.text = content[i].count.ToString();
            }
        }
        
        equipment.UpdateUnequipButtons();
        craftSystem.UpdateDisplayedRecipes();
        buildSystem.UpdateDisplayedCosts();
    }

    public bool IsFull()
    {
        return InventorySize == content.Count;
    }

    public void LoadData(List<ItemInInventory> savedDataInventoryContent)
    {
        content = savedDataInventoryContent;
        RefreshContent();
    }

    public void ClearContent()
    {
        content.Clear();
    }
}

[System.Serializable]
public class ItemInInventory
{
    public ItemData itemData;
    public int count;
}
