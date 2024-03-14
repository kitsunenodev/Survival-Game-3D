using UnityEngine;
using System.Linq;

public class UIManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject[] UIPanels;

    [SerializeField]
    private ThirdPersonOrbitCamBasic playerCam;

    private float defaultHorizontalAimingSpeed;
    private float defaultVerticalAimingSpeed;

    [HideInInspector]
    public bool panelOpen;
    private void Start()
    {
        defaultHorizontalAimingSpeed = playerCam.horizontalAimingSpeed;
        defaultVerticalAimingSpeed = playerCam.verticalAimingSpeed;
    }

    private void Update()
    {
        panelOpen = UIPanels.Any((panel) => panel == panel.activeSelf);
        if (panelOpen)
        {
            playerCam.horizontalAimingSpeed = 0;
            playerCam.verticalAimingSpeed = 0;
        }
        else
        {
            playerCam.horizontalAimingSpeed = defaultHorizontalAimingSpeed;
            playerCam.verticalAimingSpeed = defaultVerticalAimingSpeed;
        }
    }
}
