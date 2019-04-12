//from https://www.youtube.com/watch?v=ZuhX3ef2gas
//https://pastebin.com/UsgZZ7Fk

//NOT USED AT THE MOMENT

using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{

    // The target we are following
    [SerializeField]
    private Transform target;
    // The distance in the x-z plane to the target
    [SerializeField]
    private float distance = 10.0f;
    // the height we want the camera to be above the target
    [SerializeField]
    private float height = 5.0f;

    [SerializeField]
    private float rotationDamping;
    [SerializeField]
    private float heightDamping;

    public Transform player;

    public Material trans;

    public Material ogMat;

    public Transform camObject;


    // Use this for initialization
    void Start()
    {
        //GetPlayer();
        SetUp(false);
    }


    void UseTransparentMaterial(Transform thing)
    {

        Renderer thisRenderer;
        thisRenderer = thing.GetComponent<Renderer>();
        thisRenderer.sharedMaterial = trans;
    }

    void UseNormalMaterial(Transform thing)
    {

        Renderer thisRenderer;
        thisRenderer = thing.GetComponent<Renderer>();
        thisRenderer.sharedMaterial = ogMat;
    }


    void CameraClippingDetection()
    {
        RaycastHit hit;
        Debug.DrawLine(target.position, transform.position, Color.red, 0.1f);
        if (Physics.Linecast(target.position, transform.position, out hit))
        {
            camObject = hit.transform;
            if (ogMat == null)
            {
                ogMat = camObject.GetComponent<Renderer>().material;
                UseTransparentMaterial(hit.transform);
            }
        }
        else
        {
            if (camObject)
            {
                UseNormalMaterial(camObject);
                ogMat = null;
                camObject = null;
            }

        }


    }



    void GetPlayer()
    {
        if (GetComponentInParent<PlayerController>())
        {
            player = GetComponentInParent<PlayerController>().transform;
            target = player;
            player.GetComponent<PlayerController>().cam = GetComponent<Camera>();
        }
        else
        {
            Debug.LogError("cannot find player");
        }
    }


    void SetUp(bool parent)
    {
        if (parent)
        {
            transform.SetParent(player, true);
        }
        else
        {
            transform.SetParent(null, true);
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target)
            return;
        CameraClippingDetection();
        // Calculate the current rotation angles
        var wantedRotationAngle = target.eulerAngles.y;
        var wantedHeight = target.position.y + height;

        var currentRotationAngle = transform.eulerAngles.y;
        var currentHeight = transform.position.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // Set the height of the camera
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Always look at the target
        transform.LookAt(target);
    }

}