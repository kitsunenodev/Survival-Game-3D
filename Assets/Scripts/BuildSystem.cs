using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class BuildSystem : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField]
    private Structure[] structures;

    [SerializeField]
    private Transform placedStructuresParent;
    
    [SerializeField]
    private Grid grid;
    
    [SerializeField]
    private Material canBuildMaterial;

    [SerializeField]
    private Material canNotBuildMaterial;

    [SerializeField]
    private Transform rotationRef;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip buildSoundEffect;

    [Header("UI References")] 
    [SerializeField]
    private Transform buildSystemUIPanel;

    [SerializeField] 
    private GameObject buildingRequiredElement;
    
    private Structure currentStructure;
    private bool inPlace;
    private bool canBuild;
    private Vector3 finalPosition;
    private bool systemEnabled = false;


    public List<PlacedStructure> placedStructures;

    private void Awake()
    {
        currentStructure = structures.First();
        DisableSystem();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            if (currentStructure.structureType == StructureType.Stairs && systemEnabled)
            {
                DisableSystem();
            }
            else
            {
               
                ChangeStructureType(GetStructureByType(StructureType.Stairs));
            }
           
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (currentStructure.structureType == StructureType.Wall  && systemEnabled)
            {
                DisableSystem();
            }
            else
            {
                ChangeStructureType(GetStructureByType(StructureType.Wall));
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (currentStructure.structureType == StructureType.Floor  && systemEnabled)
            {
                DisableSystem();
            }
            else
            {
                ChangeStructureType(GetStructureByType(StructureType.Floor));
            }
            
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateStructure();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && canBuild && inPlace && systemEnabled && HasAllResources())
        {
            BuildStructure();
        }
    }
    
    private void FixedUpdate()
    {
        if (!systemEnabled)
        {
            return;
        }
        
        canBuild = currentStructure.placementPrefab
            .GetComponentInChildren<CollisionDetectionEdge>().CheckConnection();
        finalPosition = grid.NearestPointOnGrid(transform.position);
        CheckPosition();
        RoundPlacementStructureRoatation();
        UpdatePlacementStructureMaterial();
    }

    void BuildStructure()
    {
        Instantiate(currentStructure.instantiatedPrefab,
            currentStructure.placementPrefab.transform.position,
            currentStructure.placementPrefab.transform.GetChild(0).transform.rotation, placedStructuresParent);
        
        placedStructures.Add(new PlacedStructure
        {
            prefab = currentStructure.instantiatedPrefab,
            positions = currentStructure.placementPrefab.transform.position,
            rotations = currentStructure.placementPrefab.transform.GetChild(0).transform.rotation.eulerAngles
        });
        
        audioSource.PlayOneShot(buildSoundEffect);

        for (int i = 0; i < currentStructure.resourcesCost.Length; i++)
        {
            for (int j = 0; j < currentStructure.resourcesCost[i].count; j++)
            {
                Inventory.instance.RemoveItem(currentStructure.resourcesCost[i].itemData);
            }
        }
    }
     private bool HasAllResources()
     {
         BuildingRequiredElement[] requiredElements = GameObject.FindObjectsOfType<BuildingRequiredElement>();
         return requiredElements.All(requiredElements => requiredElements.hasResource);
     }

    void DisableSystem()
    {
        systemEnabled = false;
        
        buildSystemUIPanel.gameObject.SetActive(false);
        currentStructure.placementPrefab.SetActive(false);
    }
    private void RotateStructure()
    {
        if (currentStructure.structureType != StructureType.Wall)
        {
               currentStructure.placementPrefab.transform.GetChild(0).transform.Rotate(0,90,0);
               RoundPlacementStructureRoatation();
        }
     
    }

    void RoundPlacementStructureRoatation()
    {
        float yAngle = rotationRef.localEulerAngles.y;

        int roundedRotation = 0;
        if (yAngle > -45 && yAngle <= 45)
        {
            roundedRotation = 0;
        }
        else if (yAngle > 45 && yAngle <= 135)
        {
            roundedRotation = 90;
        }
        else if (yAngle > 135 && yAngle <= 225)
        {
            roundedRotation = 180;
        }
        else if (yAngle > 225 && yAngle <= 315)
        {
            roundedRotation = 270;
        }
        
        currentStructure.placementPrefab.transform.rotation = Quaternion.Euler(0,roundedRotation,0);
    }
    
    void UpdatePlacementStructureMaterial()
    {
        MeshRenderer placementPrefabRenderer = currentStructure.placementPrefab
            .GetComponentInChildren<CollisionDetectionEdge>().meshRenderer;

        if (inPlace && canBuild && HasAllResources())
        {
            placementPrefabRenderer.material = canBuildMaterial;
        }
        else
        {
            placementPrefabRenderer.material = canNotBuildMaterial;
        }

    }
    void ChangeStructureType(Structure newStructure)
    {
        systemEnabled = true;
        buildSystemUIPanel.gameObject.SetActive(true);
        currentStructure = newStructure;

        foreach (var structure in structures)
        {
            structure.placementPrefab.SetActive(structure.structureType == currentStructure.structureType);
        }
        
        UpdateDisplayedCosts();
        
    }

    private void CheckPosition()
    {
        inPlace = currentStructure.placementPrefab.transform.position == finalPosition;

        if (!inPlace)
        {
            SetPosition(finalPosition);
        }
    }

    void SetPosition(Vector3 targetPosition)
    {
        
        Transform placementPrefabTransform = currentStructure.placementPrefab.transform;
        Vector3 positionVelocity = Vector3.zero;
        if (Vector3.Distance(placementPrefabTransform.position, targetPosition) > 10)
        {
            placementPrefabTransform.position = targetPosition;
        }
        else
        {
            Vector3 newTagetPosition = Vector3.SmoothDamp(placementPrefabTransform.position, targetPosition,
                        ref positionVelocity, 0, 15000);
            placementPrefabTransform.position = newTagetPosition;
        }
        
    }

    private Structure GetStructureByType(StructureType type)
    {
        return structures.Where(elem => elem.structureType == type).FirstOrDefault();
    }

    public void UpdateDisplayedCosts()
    {
        foreach (Transform child in buildSystemUIPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (ItemInInventory requiredResource in currentStructure.resourcesCost)
        {
            GameObject requiredElementGo = Instantiate(buildingRequiredElement, buildSystemUIPanel);
            requiredElementGo.GetComponent<BuildingRequiredElement>().Setup(requiredResource);
        }
    }

    public void LoadStrucutre(PlacedStructure[] savedDataPlacedStructures)
    {
        foreach (PlacedStructure structure in savedDataPlacedStructures)
        {
            placedStructures.Add(structure);
            GameObject newStructure = Instantiate(structure.prefab, placedStructuresParent);
            newStructure.transform.position = structure.positions;
            newStructure.transform.rotation = Quaternion.Euler(structure.rotations);
        }
    }
}

[System.Serializable]
public class Structure
{
    public GameObject placementPrefab;
    public GameObject instantiatedPrefab;
    public StructureType structureType;
    public ItemInInventory[] resourcesCost;

}

public enum StructureType {
    Stairs,
    Wall,
    Floor 
}

[System.Serializable]
public class PlacedStructure
{
    public GameObject prefab;
    public Vector3 positions;
    public Vector3 rotations;

}
