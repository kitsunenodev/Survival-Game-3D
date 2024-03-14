using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem instance;

    [SerializeField]
    private Tooltip tooltip;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than ne instance of Tooltip system");
            return;
        }
        instance = this;
    }

    public void Show(string content, string header ="")
    { 
        tooltip.SetTexts(content, header);
        tooltip.gameObject.SetActive(true);  
    }

    public void Hide()
    {
        tooltip.gameObject.SetActive(false);
    }
}
