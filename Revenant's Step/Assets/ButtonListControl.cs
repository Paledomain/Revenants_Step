using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ButtonListControl : MonoBehaviour
{


    [SerializeField]
    private Button buttonTemplate;

    void Start()
    {
        string[] loads = Directory.GetFiles(Application.persistentDataPath + "/saves/", "*.rev");

        foreach (string file in loads)
        {

            string[] fileSplit = file.Split('/');
            string fileName = fileSplit[fileSplit.Length - 1];

            Button button = Instantiate(buttonTemplate) as Button;//create as many buttons as the number of saves


            button.onClick.AddListener(button.GetComponent<ButtonListButton>().onClick);//add listeners
            button.gameObject.SetActive(true);//set them active

            button.GetComponent<ButtonListButton>().SetText("" + fileName);//set their text

            button.transform.SetParent(buttonTemplate.transform.parent, false);//set proper parenting

            button.GetComponent<ButtonListButton>().SetSave(file);//appoint save file to buttons
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void onClick()
    {

    }
}
