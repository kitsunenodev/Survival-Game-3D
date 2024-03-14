using UnityEngine;


public class CraftingSystem : MonoBehaviour
{
    [SerializeField] 
    private RecipeData[] availableRecipes;

    [SerializeField] 
    private GameObject recipeUIPrefab;

    [SerializeField] 
    private Transform recipesParent;

    [SerializeField] 
    private KeyCode craftingPanelKey;

    [SerializeField] 
    private GameObject craftingPanel;
    // Start is called before the first frame update
    void Start()
    {
        UpdateDisplayedRecipes();
    }

    public void UpdateDisplayedRecipes()
    {
        foreach (Transform Child in recipesParent)
        {
            Destroy(Child.gameObject);
        }
        for (int i = 0; i < availableRecipes.Length; i++)
        {
           GameObject recipe = Instantiate(recipeUIPrefab, recipesParent);
           recipe.GetComponent<Recipe>().Configure(availableRecipes[i]);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(craftingPanelKey))
        {
            craftingPanel.SetActive(!craftingPanel.activeSelf);
            UpdateDisplayedRecipes();
        }
    }

    public void CloseCraftingPanel()
    {
        craftingPanel.SetActive(false);
    }
    
    
}
