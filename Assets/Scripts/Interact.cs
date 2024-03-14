using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField]
    private float interactRange;

    public Transform interactPoint;

    public InteractBehavior playerInteractBehavior;

    [SerializeField] private GameObject interactText;
    [SerializeField] private LayerMask layerMask;
    // Update is called once per frame
    void Update()
    {
        Collider[] Colliders = Physics.OverlapCapsule(transform.position,
            interactPoint.position, interactRange, layerMask);
        if (Colliders.Length > 0)
        {
            interactText.SetActive(true);
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Colliders[0].transform.CompareTag("Item"))
                {
                    
                    playerInteractBehavior.DoPickUpItem(Colliders[0].transform.gameObject.GetComponent<Item>());
                }
                
                if (Colliders[0].transform.CompareTag("Harvestable"))
                {
                    playerInteractBehavior.DoHarvest(Colliders[0].transform.gameObject.GetComponent<Harvestable>());
                }
                
            }
            
           
        }
        else
        {
            interactText.SetActive(false); 
        }
    }
}
