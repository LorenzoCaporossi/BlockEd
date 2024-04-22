using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValidationChallenge : MonoBehaviour
{
    public TMP_InputField headerInput;
    public TextMeshProUGUI hashValue;
    public GameObject bubble;
    public GameObject wrongBubble;
    public GameObject rightBubble;

    public Button button;

    public UIHandler handler;

    private string updatedHashValue = "0xc3f67f";

    public void Start()
    {
        headerInput.text = string.Empty;
        bubble.SetActive(false);
        wrongBubble.SetActive(false);
        rightBubble.SetActive(false);

        if (handler.group1Connected)
            hashValue.text = updatedHashValue;

        button.gameObject.SetActive(false);
    }

    public void CloseValidation()
    {
        if(!handler.group1Connected)
            handler.group1Connected = true;
        else if(handler.group1Connected && !handler.group2Connected)
            handler.group2Connected = true;

        handler.panelID = 4;
        handler.Button1Logic();
        gameObject.SetActive(false);
    }

    public void CheckHeaderInput()
    {
        if (headerInput.text == hashValue.text)
        {
            rightBubble.SetActive(true);
            button.gameObject.SetActive(true);
        }
        else
        {
            wrongBubble.SetActive(true);
            button.gameObject.SetActive(false);
        }
    }
}
