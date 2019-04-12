using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofManager : MonoBehaviour
{

    public GameObject player, roof;
    public bool isVisible = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MeshRenderer m = roof.GetComponent<MeshRenderer>();

        if (isVisible)
        {
            m.enabled = true;
        }
        else
        {
            m.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)//make roof invisible upon entering trigger zone
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Entered");
            isVisible = false;
        }
    }

    private void OnTriggerExit(Collider other)//make it visible again when left
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Left");
            isVisible = true;
        }
    }
}
