using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public TextMeshProUGUI coinText;

    public int coinCount;

    public int GetCoinCount()
    {
        return coinCount;
    }

    public List<string> Cubes;
    public List<string> buttonsSelected;
    public int gasFee;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        coinCount = 30;
        Cubes = new List<string>();
        buttonsSelected = new List<string>();
        gasFee = 0;

        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        coinText.text = coinCount.ToString();
    }
}
