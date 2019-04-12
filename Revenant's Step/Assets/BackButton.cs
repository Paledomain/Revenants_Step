using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{

    public Button backButton;


    void Start()//button listener added at runtime to avoid gamemanager complications
    {
        backButton.onClick.AddListener(GameManager.gm.ChangeToLastScene);
    }

}
