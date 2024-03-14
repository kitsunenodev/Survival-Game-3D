using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Equipment equipment;

    [SerializeField]
    private PlayerStat playerStat;

    [SerializeField]
    private BuildSystem buildSystem;

    [SerializeField]
    private MainMenu mainMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        if (MainMenu.loadSavedData)
        {
            LoadData();
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadData();
        }
    }

    public void SaveData()
    {
        SavedData savedData = new SavedData
        {
            playerPosition = playerTransform.position,
            inventoryContent = Inventory.instance.GetContent(),
            headItem = equipment.headItem,
            chestItem = equipment.chestItem,
            handItem = equipment.handItem,
            legItem = equipment.legItem,
            footItem = equipment.legItem,
            weaponItem = equipment.weaponItem,
            currentHealth = playerStat.currentHealth,
            currentHunger = playerStat.currentHunger,
            currentThirst = playerStat.currentHunger,
            placedStructures = buildSystem.placedStructures.ToArray()
        };

        string jsonData = JsonUtility.ToJson(savedData);
        string path = Application.persistentDataPath + "/SavedData.json";
        System.IO.File.WriteAllText(path, jsonData);
        mainMenu.loadGameButton.interactable = mainMenu.clearSavedDataButton.interactable = true;
    }
    
    public void LoadData()
    {
        string path = Application.persistentDataPath + "/SavedData.json";
        string jsonData = System.IO.File.ReadAllText(path);
        SavedData savedData = JsonUtility.FromJson<SavedData>(jsonData);

        playerTransform.position = savedData.playerPosition;
        equipment.LoadEquipments( new ItemData[]
            {
                savedData.headItem,
                savedData.chestItem,
                savedData.handItem,
                savedData.legItem,
                savedData.footItem,
                savedData.weaponItem
            }
            );
        
        Inventory.instance.LoadData(savedData.inventoryContent);
        playerStat.currentHealth = savedData.currentHealth;
        playerStat.currentHunger = savedData.currentHunger;
        playerStat.currentThirst = savedData.currentThirst;
        playerStat.UpdateHealthBarFill();
        buildSystem.LoadStrucutre(savedData.placedStructures);


    }

}

public class SavedData
{
    public Vector3 playerPosition;
    public List<ItemInInventory> inventoryContent;
    public ItemData headItem;
    public ItemData chestItem;
    public ItemData handItem;
    public ItemData legItem;
    public ItemData footItem;
    public ItemData weaponItem;
    public float currentHealth;
    public float currentHunger;
    public float currentThirst;
    public PlacedStructure[] placedStructures;
}
