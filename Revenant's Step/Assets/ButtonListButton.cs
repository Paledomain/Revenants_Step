using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListButton : MonoBehaviour {//content on this class and other classes 
    //pertaining to the creation of the load menu have been mostly taken from
    //https://www.youtube.com/watch?v=ZI6DwJtjlBA


    [SerializeField]
    private Text buttonText;
    private string saveDirectory;

	public void SetText (string textString)
    {
        buttonText.text = textString;
	}

    public void SetSave(string saveString)
    {
        saveDirectory = saveString;
    }

    public string GetSave()
    {
        return saveDirectory;
    }


    public void onClick () {
        Debug.Log("button clicked");
        GameManager.gm.Load(saveDirectory);//to load the wanted save
	}
}
