using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerMy : MonoBehaviour
{

    public static GameManagerMy Instance;
    public bool ismaster;
    public bool isMultiplayer;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
