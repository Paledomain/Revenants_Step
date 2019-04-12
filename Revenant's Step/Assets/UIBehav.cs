using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBehav : MonoBehaviour {

    public Image stealthIndicator;
    public Image battleIndicator;
    public Image playerHealthIndicator;
    public Text APIndicator;

    public PlayerController player;

    // Update is called once per frame
    void Update ()
    {

        //check game stealth state
        if (GameManager.gm.stealth > 0)
        {
            stealthIndicator.gameObject.SetActive(true);
            battleIndicator.gameObject.SetActive(false);
            APIndicator.gameObject.SetActive(false);
            stealthIndicator.GetComponent<Image>().color = new Color32(255, (byte)(GameManager.gm.stealth * 255 / 100), (byte)(GameManager.gm.stealth * 255 / 100), 255);
        }
        if (GameManager.gm.stealth <= 0)
        {
            stealthIndicator.gameObject.SetActive(false);
            battleIndicator.gameObject.SetActive(true);
            APIndicator.gameObject.SetActive(true);
            APIndicator.text = "AP: " + player.AP;
        }

        playerHealthIndicator.fillAmount = player.health / 100;//as it is basically an image
    }
}
