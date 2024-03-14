using System;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private Text headerField;

    [SerializeField]    
    private Text contentField;

    [SerializeField]
    private LayoutElement layoutElement;

    [SerializeField]
    private int maxCharacter;

    [SerializeField] private RectTransform rectTransform;

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;

        float pivotX = mousePosition.x / Screen.width;
        float pivotY = mousePosition.y / Screen.height;
        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = mousePosition;
        


    }

    public void SetTexts(string content, string header = "")
    {
        if (header =="")
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;
        layoutElement.enabled = (contentLength > maxCharacter || headerLength > maxCharacter);
    }
}
