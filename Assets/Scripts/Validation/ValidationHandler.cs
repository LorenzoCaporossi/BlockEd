using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ValidationHandler : MonoBehaviour 
{ 
    [Header("Buttons")]
    public Button button_1;
    public Button button_2;

    [Header("Hover Image")]
    public GameObject button_2_Image;

    public UIHandler handler;

    private void Start()
    {
        button_2_Image.SetActive(false);
    }

    public void ProofOfWorkLogic()
    {
        handler.panelID = 5;
        handler.dataID = 4;
        handler.Button1Logic();
        gameObject.SetActive(false);
    }

    public void ProofOfStakeLogic()
    {
        if (GameManager.instance.GetCoinCount() < 32)
        {
            button_2_Image.SetActive(true);
        }
        else
        {
            button_2_Image.SetActive(false);
            handler.panelID = 13;
            handler.dataID = 10;
            handler.Button1Logic();
            gameObject.SetActive(false);
        }
    }
}
