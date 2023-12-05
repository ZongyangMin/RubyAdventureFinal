using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Zongyang Min
//Stamina Bar Script
public class UIStaminaBar : MonoBehaviour
{
    public static UIStaminaBar instance { get; private set; }
    
    public Image mask;
    float originalSize;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    public void SetValue(float value)
    {				      
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}
