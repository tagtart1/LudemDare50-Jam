using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipDynamic : MonoBehaviour
{
    public static TooltipDynamic Instance { get; private set; }

    [SerializeField] private RectTransform canvas;
    [SerializeField] Player player; //really shouldnt need this reference, only to get mouseposition
    
    [SerializeField] private RectTransform background;
    [SerializeField] private TextMeshProUGUI text;
    private RectTransform rectTransform;

    
    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
       
        HideTooltip();
    }

    private void SetText(string tooltipText)
    {
        text.SetText(tooltipText);
        text.ForceMeshUpdate();


        Vector2 textSize = text.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(6, 6);
        background.sizeDelta = textSize + paddingSize;
    }


    private void Update()
    {
        rectTransform.anchoredPosition = player.GetMousePosition() / canvas.localScale.x;
    }

    private void ShowTooltip(string tooltipText)
    {
        background.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        SetText(tooltipText);
    }

    private void HideTooltip()
    {
        background.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    public  static void ShowToolTip_Static(string tooltipText)
    {
        Instance.ShowTooltip(tooltipText);
    }

   

    public static void HideTooltip_Static()
    {
        Instance.HideTooltip();
    }
}
