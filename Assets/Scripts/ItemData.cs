using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/New item")]

public class ItemData : ScriptableObject
{
    [Header("Data")]
    [NotNull] public string itemName;
    public string itemDescription;
    public Sprite itemVisual;
    public GameObject itemPrefab;
    public bool stackable;
    public int maxStack;

    [Header("Effects")]
    public float healthRegen;

    public float hungerRegen;

    public float thirstRegen;
    
    [Header("Types")]
    public ItemType itemType;
    public EquipmentType equipmentType;

    [Header("Armor Points")]
    public float armorPoints;

    [Header("Damage")]
    public int damage;


}

public enum ItemType{
    Resources,
    Equipment,
    Consumable

}

public enum EquipmentType
{
    Head,
    Chest,
    Hand,
    Leg,
    Foot,
    Weapon
}
