using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BuildingRequiredElement : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;
    
    [SerializeField]
    private Text itemCost;
    
    [SerializeField]
    private Image slotImage;

    [SerializeField]
    private Color canBuildColor;
    
    [SerializeField]
    private Color canNotBuildColor;
    
    public bool hasResource;

    public void Setup(ItemInInventory resourceRequired)
    {
        itemImage.sprite = resourceRequired.itemData.itemVisual;
        itemCost.text = resourceRequired.count.ToString();

        ItemInInventory[] itemsInInventory = Inventory.instance.GetContent().Where(elem => elem.itemData == resourceRequired.itemData).ToArray();

        int totalRequiredItemInInventory = 0;
        for (int i = 0; i < itemsInInventory.Length; i++)
        {
            totalRequiredItemInInventory += itemsInInventory[i].count;
        }

        if (totalRequiredItemInInventory >= resourceRequired.count)
        {
            hasResource = true;
            slotImage.color = canBuildColor;
        }
        else
        {
            slotImage.color = canNotBuildColor;
            
            
        }

    }
}
