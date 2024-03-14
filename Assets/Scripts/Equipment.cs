using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Security.Cryptography;

public class Equipment : MonoBehaviour
{
    [Header("REFERENCES")]
    
    [SerializeField] 
    private ItemActionSystem itemActionSystem;

    [SerializeField]
    private PlayerStat playerStat;
    
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip gearUpSoundEffect;
    
    [Header("Equipment library References")]

    [SerializeField] 
    private EquipmentLibrary equipmentLibrary;

    //Equipment slots 
    [SerializeField] 
    private Image headSlotImage;
    [SerializeField] 
    private Image chestSlotImage;
    [SerializeField] 
    private Image handSlotImage;
    [SerializeField] 
    private Image legSlotImage;
    [SerializeField] 
    private Image footSlotImage;
    [SerializeField] 
    private Image weaponSlotImage;

    //Item equipped
    [HideInInspector]
    public ItemData headItem;
    [HideInInspector]
    public ItemData chestItem;
    [HideInInspector]
    public ItemData handItem;
    [HideInInspector]
    public ItemData legItem;
    [HideInInspector]
    public ItemData footItem;
    [HideInInspector]
    public ItemData weaponItem;

    //buttons to remove your equipment
    [SerializeField] 
    private Button headSlotUnequipButton;
    [SerializeField] 
    private Button chestSlotUnequipButton;
    [SerializeField] 
    private Button handSlotUnequipButton;
    [SerializeField] 
    private Button legSlotUnequipButton;
    [SerializeField] 
    private Button footSlotUnequipButton;
    [SerializeField] 
    private Button weaponSlotUnequipButton;
    
    void DisablePreviousEquippedItem(ItemData itemToDisable)
    {
        if (itemToDisable == null)
        {
            return;
        }
        
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.itemToPrefab.Where(
            elem => elem.itemData == itemToDisable).First();
        
        for (int i = 0; i < equipmentLibraryItem.prefabsToDisable.Length; i++)
        {
            equipmentLibraryItem.prefabsToDisable[i].SetActive(true);
        }
        
        equipmentLibraryItem.itemPrefab.SetActive(false);

        playerStat.armorPoints -= itemToDisable.armorPoints;
        Inventory.instance.AddItem(itemToDisable);
    }
    
    public void UnequipItem(EquipmentType equipmentType)
    {
        if (Inventory.instance.IsFull())
        {
            return;
        }

        ItemData currentItem = null;

        switch (equipmentType)
        {
            case EquipmentType.Head:
                currentItem = headItem;
                headItem = null;
                headSlotImage.sprite = Inventory.instance.emptySlotImage;
                break;
            
            case EquipmentType.Chest:
                currentItem = chestItem;
                chestItem = null;
                headSlotImage.sprite = Inventory.instance.emptySlotImage;
                break;
            
            case EquipmentType.Hand:
                currentItem = handItem;
                handItem = null;
                handSlotImage.sprite = Inventory.instance.emptySlotImage;
                break;
            
            case EquipmentType.Leg:
                currentItem = legItem;
                legItem = null;
                legSlotImage.sprite = Inventory.instance.emptySlotImage;
                break;
            
            case EquipmentType.Foot:
                currentItem = footItem;
                footItem = null;
                footSlotImage.sprite = Inventory.instance.emptySlotImage;
                break;
            case EquipmentType.Weapon:
                currentItem = weaponItem;
                weaponItem = null;
                weaponSlotImage.sprite = Inventory.instance.emptySlotImage;
                break;
        }
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.itemToPrefab.Where(
            elem => elem.itemData == currentItem).FirstOrDefault();

        if (equipmentLibraryItem != null)
        {
            for (int i = 0; i < equipmentLibraryItem.prefabsToDisable.Length; i++)
            {
                equipmentLibraryItem.prefabsToDisable[i].SetActive(true);
            }
                    
            equipmentLibraryItem.itemPrefab.SetActive(false);
        }

        if (currentItem)
        {
            playerStat.armorPoints -= currentItem.armorPoints;
            Inventory.instance.AddItem(currentItem);
        }
        
        Inventory.instance.RefreshContent();
    }
    
    public void UpdateUnequipButtons()
    {
        headSlotUnequipButton.onClick.RemoveAllListeners();
        headSlotUnequipButton.gameObject.SetActive(headItem);
        headSlotUnequipButton.onClick.AddListener(delegate { UnequipItem(EquipmentType.Head); });
        
        chestSlotUnequipButton.onClick.RemoveAllListeners();
        chestSlotUnequipButton.gameObject.SetActive(chestItem);
        chestSlotUnequipButton.onClick.AddListener(delegate { UnequipItem(EquipmentType.Chest); });
        
        handSlotUnequipButton.onClick.RemoveAllListeners();
        handSlotUnequipButton.gameObject.SetActive(handItem);
        handSlotUnequipButton.onClick.AddListener(delegate { UnequipItem(EquipmentType.Hand); });
        
        legSlotUnequipButton.onClick.RemoveAllListeners();
        legSlotUnequipButton.gameObject.SetActive(legItem);
        legSlotUnequipButton.onClick.AddListener(delegate { UnequipItem(EquipmentType.Leg); });
        
        footSlotUnequipButton.onClick.RemoveAllListeners();
        footSlotUnequipButton.gameObject.SetActive(footItem);
        footSlotUnequipButton.onClick.AddListener(delegate { UnequipItem(EquipmentType.Foot); });
        
        weaponSlotUnequipButton.onClick.RemoveAllListeners();
        weaponSlotUnequipButton.gameObject.SetActive(weaponItem);
        weaponSlotUnequipButton.onClick.AddListener(delegate { UnequipItem(EquipmentType.Weapon); });
    }
    
    public void EquipItem(ItemData equipment = null)
    {

        ItemData itemtoequip = equipment ? equipment : itemActionSystem.itemSelected;
        
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.itemToPrefab.Where(
            elem => elem.itemData == itemtoequip).First();

        if (equipmentLibraryItem != null)
        {
            equipmentLibraryItem.itemPrefab.SetActive(true);
            switch(itemtoequip.equipmentType)
            {
                case EquipmentType.Head:
                    DisablePreviousEquippedItem(headItem);
                    headSlotImage.sprite = itemtoequip.itemVisual;
                    headItem = itemtoequip;
                    break;
                
                case EquipmentType.Chest:
                    DisablePreviousEquippedItem(chestItem);
                    chestSlotImage.sprite = itemtoequip.itemVisual;
                    chestItem = itemtoequip;
                    break;
                
                case EquipmentType.Hand:
                    DisablePreviousEquippedItem(handItem);
                    handSlotImage.sprite = itemtoequip.itemVisual;
                    handItem = itemtoequip;
                    break;
                
                case EquipmentType.Leg:
                    DisablePreviousEquippedItem(legItem);
                    legSlotImage.sprite = itemtoequip.itemVisual;
                    legItem = itemtoequip;
                    break;
                
                case EquipmentType.Foot:
                    DisablePreviousEquippedItem(footItem);
                    footSlotImage.sprite = itemtoequip.itemVisual;
                    footItem = itemtoequip;
                    break;
                
                case EquipmentType.Weapon:
                    DisablePreviousEquippedItem(weaponItem);
                    weaponSlotImage.sprite = itemtoequip.itemVisual;
                    weaponItem = itemtoequip;
                    break;
            }
            
            for (int i = 0; i < equipmentLibraryItem.prefabsToDisable.Length; i++)
            {
                equipmentLibraryItem.prefabsToDisable[i].SetActive(false);
            }

            audioSource.PlayOneShot(gearUpSoundEffect);
            playerStat.armorPoints += itemtoequip.armorPoints;
            Inventory.instance.RemoveItem(itemtoequip);
        }
        itemActionSystem.CloseActionPanel();
    }

    public void LoadEquipments(ItemData[] savedEquipments)
    {
        Inventory.instance.ClearContent();
        foreach (EquipmentType type in System.Enum.GetValues(typeof( EquipmentType)))
        {
            UnequipItem(type);
        }

        foreach (ItemData item in savedEquipments)
        {
            if (item)
            { 
                EquipItem(item); 
            }
        }
    }
    
    
}
