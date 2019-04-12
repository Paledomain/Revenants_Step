using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class LoadMenuBuilder : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //pull the loads for children's usage
        string[] loads = Directory.GetFiles(Application.persistentDataPath + "/saves/", "*.rev");

        foreach (string file in loads)
        {
            Debug.Log(file);
        }
    }

    private void OnGUI()
    {
        string[] loads = Directory.GetFiles(Application.persistentDataPath + "/saves/", "*.rev");

        foreach (string file in loads)
        {
            
        }
    }
}
