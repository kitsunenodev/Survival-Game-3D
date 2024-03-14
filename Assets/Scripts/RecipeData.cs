using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe/New Recipe")]
public class RecipeData : ScriptableObject
{
    public ItemData itemCrafted;
    
    public ItemInInventory[] requiredItems;
}
