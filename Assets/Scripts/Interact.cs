using UnityEngine;
using UnityEngine.Serialization;

public class Interact : MonoBehaviour
{
    [SerializeField]
    private float interactRange;

    public InteractBehavior playerInteractBehavior;

    [SerializeField] private GameObject interactText;
    [SerializeField] private LayerMask layerMask;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange, layerMask))
        {
            interactText.SetActive(true);
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.transform.CompareTag("Item"))
                {
                    
                    playerInteractBehavior.DoPickUpItem(hit.transform.gameObject.GetComponent<Item>());
                }
                
                if (hit.transform.CompareTag("Harvestable"))
                {
                    playerInteractBehavior.DoHarvest(hit.transform.gameObject.GetComponent<Harvestable>());
                }
                
            }
            
           
        }
        else
        {
            interactText.SetActive(false); 
        }
    }
}
