using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    bool menuOpened;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject settingsMenu;

    [SerializeField]
    private GameObject controlsMenu;
    
    [SerializeField]
    private ThirdPersonOrbitCamBasic cameraScript;

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuOpened = !menuOpened;
            pauseMenu.SetActive(menuOpened);
            settingsMenu.SetActive(false);
            controlsMenu.SetActive(false);

            Time.timeScale = menuOpened ? 0 : 1;

            cameraScript.enabled = !menuOpened;
        }
    }
}
