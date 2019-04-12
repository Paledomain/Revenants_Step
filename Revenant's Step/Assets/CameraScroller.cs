using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroller : MonoBehaviour {

    public Camera theCamera;
    public float speed = 5;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update ()
    {
        //move camera when mouse close to screen edge or when w-a-s-d kes are pressed
		if(0 <= Input.mousePosition.x && 0.05f * Screen.width > Input.mousePosition.x || Input.GetKey(KeyCode.A))
        {
            theCamera.transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * speed, Space.Self);
        }

        if (0.95f*Screen.width < Input.mousePosition.x && Screen.width >= Input.mousePosition.x || Input.GetKey(KeyCode.D))
        {
            theCamera.transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * speed, Space.Self);
        }

        if (0 <= Input.mousePosition.y && 0.05f * Screen.height > Input.mousePosition.y || Input.GetKey(KeyCode.S))
        {
            theCamera.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * speed, Space.Self);
        }

        if (0.95f * Screen.height < Input.mousePosition.y && Screen.height >= Input.mousePosition.y || Input.GetKey(KeyCode.W))
        {
            theCamera.transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * speed, Space.Self);
        }


        if (!Input.GetMouseButton(1))//zoom in or out with wheel if right click not pressed
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && theCamera.orthographicSize > 1) // forward
            {
                theCamera.orthographicSize--;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                theCamera.orthographicSize++;
            }
        }


        if (Input.GetMouseButton(1))//rotate left or right if right click is pressed
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) 
            {
                theCamera.transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * 40 * speed, Space.World);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                theCamera.transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * 40 * speed, Space.World);
            }
        }

    }
}
