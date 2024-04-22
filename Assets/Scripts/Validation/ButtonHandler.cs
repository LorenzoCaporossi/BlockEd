using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Name;
    public bool isSelected;

    public int gasFee;

    public GameObject image;
    public Color activeImage;
    public Color inactiveImage;

    public UIHandler handler;

    //private Shadow Shadow;
    private Outline outline;

    private void Start()
    {
        handler = GameObject.Find("Main Panel").GetComponent<UIHandler>();
        //Shadow = GetComponent<Shadow>();
        outline = GetComponent<Outline>();
        image.SetActive(false);

        GameManager.instance.buttonsSelected.Clear();
        GameManager.instance.gasFee = 0;
    }

    public void SetIsSelected()
    {
        if (!isSelected)
        {
            isSelected = true;
            GetComponent<Image>().color = activeImage;
            outline.enabled = true;
            //Shadow.enabled = true;
            GameManager.instance.buttonsSelected.Add(Name);
            GameManager.instance.gasFee += gasFee;

            if (GameManager.instance.buttonsSelected.Count > 0)
                handler.EnableButton1(true);
        }
        else if (isSelected)
        {
            isSelected = false;
            GetComponent<Image>().color = inactiveImage;
            outline.enabled = false;
            //Shadow.enabled = false;
            GameManager.instance.buttonsSelected.Remove(Name);
            GameManager.instance.gasFee -= gasFee;

            if (GameManager.instance.buttonsSelected.Count == 0)
                handler.EnableButton1(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.SetActive(false);
    }
}
