using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    
    public Button loadGameButton;
    
    public Button clearSavedDataButton;

    public static bool loadSavedData;

    [SerializeField]    
    private Dropdown resolutionDropdown;
    
    [SerializeField]    
    private Dropdown qualityDropdown;

    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Slider soundSlider;

    [SerializeField]
    private GameObject settingsPanel;
    
    [SerializeField]
    private GameObject controlsPanel;
    

    [SerializeField]
    private Toggle fullScreenToggle;
    private void Start()
    {
        audioMixer.GetFloat("Volume", out float soundValue);
        soundSlider.value = soundValue;
        clearSavedDataButton.interactable = loadGameButton.interactable = System.IO.File.Exists(Application.persistentDataPath + "/SavedData.json");


        String[] qualities = QualitySettings.names;
        qualityDropdown.ClearOptions();

        List<String> qualitiesOption = new List<string>();
        int currentQualityIndex = 0;

        for (int i = 0; i < qualities.Length; i++)
        {
            qualitiesOption.Add(qualities[i]);

            if (i == QualitySettings.GetQualityLevel())
            {
                currentQualityIndex = i;
            }
        }
        
        qualityDropdown.AddOptions(qualitiesOption);
        qualityDropdown.value = currentQualityIndex;
        qualityDropdown.RefreshShownValue();
        
        Resolution[] resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<String> resolutionsOption = new List<string>();
        int currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " (" + resolutions[i].refreshRate + " Hz)";
            resolutionsOption.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
            
            resolutionDropdown.AddOptions(resolutionsOption);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
            fullScreenToggle.isOn = Screen.fullScreen;

        }
        
    }

    public void NewGameButton()
    {  
        loadSavedData = false;
        SceneManager.LoadScene("Scene");
    }

    public void LoadSaveButton()
    {
        loadSavedData = true;
        SceneManager.LoadScene("Scene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);

    }

    public void ClearSavedData()
    {
        System.IO.File.Delete(Application.persistentDataPath + "/SavedData.json");
        clearSavedDataButton.interactable =  loadGameButton.interactable = false;
    }

    public void EnableDisableSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        controlsPanel.SetActive(false);
    }
    
    public void EnableDisableControlsPanel()
    {
        controlsPanel.SetActive(!controlsPanel.activeSelf);
        settingsPanel.SetActive(false);
    }

    public void BackToTitle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene");
    }
    
    

    
}
