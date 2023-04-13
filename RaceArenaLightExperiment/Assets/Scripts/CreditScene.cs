using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditScene : MonoBehaviour
{
    public GameObject creditsImage;
    void Start()
    {
        creditsImage.SetActive(false);
    }

    public void switchVisibility()
    {
        creditsImage.SetActive(!creditsImage.activeSelf);
    }

    void Update()
    {
        
    }
}
