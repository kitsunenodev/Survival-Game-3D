using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class InteractBehavior : MonoBehaviour
{
   [Header("REFERENCES")] 
   [SerializeField]
   private Equipment equipment;

   [SerializeField]
   private AudioSource audioSource;

   [SerializeField]
   private EquipmentLibrary equipmentLibrary;
   
   [SerializeField]
   private MoveBehaviour moveBehavior;
   
   [SerializeField] 
   private Inventory inventory;
   
   [SerializeField]
   private Animator playerAnimator;
   
   [Header("Tools configuration")]
   [SerializeField] 
   private GameObject pickaxeVisual;
   
   [SerializeField] 
   private GameObject axeVisual;


   [SerializeField] 
   private AudioClip pickaxeSound;
   [SerializeField]
   private AudioClip axeSound;
   
   [SerializeField]
   private Vector3 spawnItemOffset = new Vector3(0, 0.5f, 0);
   
   [HideInInspector]
   public bool isBusy;
   
   private Harvestable currentHarvestable;
   private Item currentItem;
   private Tool currentTool;
   
   
   [Header("Other")] 
   [SerializeField]
   private AudioClip gatheringSound;
   public void DoPickUpItem(Item item)
   {
      if (isBusy)
      {
         return;
      }
      if (inventory.IsFull())
      {
         Debug.Log("can't pick up " + item.name +" because the inventory is already full");
         return;
      }

      StartCoroutine(RotateTowardObject(item.transform));
      isBusy = true;
      currentItem = item;
      playerAnimator.SetTrigger("PickUp");
      moveBehavior.canMove = false;
   }
   public void DoHarvest(Harvestable harvestable)
   {
      if (isBusy)
      {
         return;
      }

      StartCoroutine(RotateTowardObject(harvestable.transform));
      isBusy = true;
      currentHarvestable = harvestable;
      currentTool = currentHarvestable.tool;
      EnableToolGameobject();
      playerAnimator.SetTrigger("Harvest");
      moveBehavior.canMove = false;
      
   }

   private void EnableToolGameobject(bool enabled = true)
   { 
      EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.itemToPrefab.Where(
         elem => elem.itemData == equipment.weaponItem).FirstOrDefault();

      if (equipmentLibraryItem != null)
      {
         for (int i = 0; i < equipmentLibraryItem.prefabsToDisable.Length; i++)
         {
            equipmentLibraryItem.prefabsToDisable[i].SetActive(enabled);
         }
         equipmentLibraryItem.itemPrefab.SetActive(!enabled);
      }
      
      switch (currentTool)
      {
         case Tool.Pickaxe:
            pickaxeVisual.SetActive(enabled);
            audioSource.clip = pickaxeSound;
            break;
         
         case Tool.Axe:
            axeVisual.SetActive(enabled);
            audioSource.clip = axeSound;
            break;
         
      }
   }

   public IEnumerator BreakHarvestable()
   {
      Harvestable currentlyHarvested = currentHarvestable;
      //prevent the harvestable to be harvest a second time
      currentlyHarvested.gameObject.layer = LayerMask.NameToLayer("Default");
      
      if (currentlyHarvested.disableKinematicOnHarvest)
      {
         Rigidbody currentHarvestedrigidbody = currentlyHarvested.gameObject.GetComponent<Rigidbody>();
         currentHarvestedrigidbody.isKinematic = false;
         currentHarvestedrigidbody.AddForce(transform.forward, ForceMode.Impulse);
      }

      yield return new WaitForSeconds(currentlyHarvested.destroyDelay);
      foreach (Resource resource in currentlyHarvested.harvestableLoot)
      {
         Resource currentResource = resource;

         if (Random.Range(1,101) <= currentResource.dropProbability )
         {
            GameObject instanciateResource = Instantiate(currentResource.itemData.itemPrefab);
            instanciateResource.transform.position = currentlyHarvested.transform.position + spawnItemOffset;
         }
         
      }
      Destroy(currentlyHarvested.gameObject);
   }
   public void AddItemToInventory()
   {
      inventory.AddItem(currentItem.itemData);
      audioSource.PlayOneShot(gatheringSound);
      Destroy(currentItem.gameObject);
   }

   public void ReEnablePlayerMovement()
   {
      isBusy = false;
      EnableToolGameobject(false);
      moveBehavior.canMove = true;
   }

   void PlayHarvestingSoundEffect()
   {
      audioSource.Play();
   }

   public IEnumerator RotateTowardObject(Transform target)
   {
      Transform currentTransform = transform;
      Quaternion targetRotation = Quaternion.LookRotation(
         target.position- currentTransform.position, currentTransform.up);
      for (float i = 0; i <= 1; i+= 0.01f)
      {
         transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, i);
         yield return new WaitForSeconds(0.005f);
      }
   }

  
}
