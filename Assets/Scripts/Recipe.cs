using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class Recipe : MonoBehaviour
{
    private RecipeData currentRecipe;
    
    [SerializeField]
    private Image craftableItemImage;
    
    [SerializeField]
    private GameObject elementRequiredPrefab;
    
    [SerializeField]
    private Transform elementsRequiredParent;
    
    [SerializeField]
    private Button craftButton;
    
    [SerializeField]
    private Sprite canBuildIcon;
    
    [SerializeField]
    private Sprite canNotBuildIcon;

    [SerializeField]
    private Color missingColor;
    
    [SerializeField]
    private Color availableColor;

    public void Configure(RecipeData recipe)
    {
        currentRecipe = recipe;
        craftableItemImage.sprite = recipe.itemCrafted.itemVisual;
        craftableItemImage.transform.parent.GetComponent<Slot>().item = currentRecipe.itemCrafted;

        bool canCraft = true;
        
        for (int i = 0; i < recipe.requiredItems.Length; i++)
        {
            //Get all the required elements for the recipe
            GameObject requiredItemGO = Instantiate(elementRequiredPrefab, elementsRequiredParent);
            Image requiredItemGOImage = requiredItemGO.GetComponent<Image>();
            ItemData requiredItem = recipe.requiredItems[i].itemData;
            ElementRequired elementRequired = requiredItemGO.GetComponent<ElementRequired>();

            
            //Slot to show the tooltip of the items
            requiredItemGO.GetComponent<Slot>().item = requiredItem;

            ItemInInventory[] itemInInventory =
                Inventory.instance.GetContent().Where(elem => elem.itemData == requiredItem).ToArray();

            int totalQuantity = 0;
            for (int j = 0; j < itemInInventory.Length; j++)
            {
                totalQuantity += itemInInventory[j].count;
            }
            
            if ( totalQuantity >= recipe.requiredItems[i].count)
            {
                requiredItemGOImage.color = availableColor;
            }
            else
            {
                requiredItemGOImage.color = missingColor;
                canCraft = false;
            }
            
            elementRequired.elementImage.sprite = recipe.requiredItems[i].itemData.itemVisual;
            elementRequired.elementCountText.text = recipe.requiredItems[i].count.ToString();
        }

        craftButton.image.sprite = canCraft ? canBuildIcon : canNotBuildIcon;
        craftButton.enabled = canCraft;
        ResizeElementParent();
    }

    public void CraftItem()
    {
        for (int i = 0; i < currentRecipe.requiredItems.Length; i++)
        {
            for (int j = 0; j <  currentRecipe.requiredItems[i].count; j++)
            {
                Inventory.instance.RemoveItem(currentRecipe.requiredItems[i].itemData);
            }
        }
        Inventory.instance.AddItem(currentRecipe.itemCrafted);
        
        
    }

    private void ResizeElementParent()
    {
        Canvas.ForceUpdateCanvases();
        elementsRequiredParent.GetComponent<ContentSizeFitter>().enabled = false;
        elementsRequiredParent.GetComponent<ContentSizeFitter>().enabled = true;
    }
}
