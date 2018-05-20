
using UnityEngine;
using System.Collections;

public class cameraMovement : MonoBehaviour
{

    public float rotationSpeed = 2.0f;
    public float panningSpeed = -1.0f;
    public float zoomSpeed = 1.0f;

    private Vector3 mouseOrigin;
    private bool isPanning;
    private bool isRotating;
    private bool isZooming;

    private Vector3 mousePosPan;


    private void panCamera()
    {
        if (mousePosPan != Input.mousePosition)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
            
            Vector3 move = new Vector3(pos.x * panningSpeed, pos.y * panningSpeed, 0);
            transform.Translate(move, Space.Self);
        }

        mousePosPan = Input.mousePosition;
    }

    private void Start()
    {
        mousePosPan = Input.mousePosition;
    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftAlt))
        {

            mouseOrigin = Input.mousePosition;
            isRotating = true;
        }


        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftAlt))
        {

            mouseOrigin = Input.mousePosition;
            isPanning = true;

        }


        if (Input.GetMouseButtonDown(2) && Input.GetKey(KeyCode.LeftAlt))
        {

            mouseOrigin = Input.mousePosition;
            isZooming = true;
        }



        if (isRotating)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            transform.RotateAround(transform.position, transform.right, -pos.y * rotationSpeed);
            transform.RotateAround(transform.position, Vector3.up, pos.x * rotationSpeed);
        }


        if (isPanning)
        {
            panCamera();
        }

        if (isZooming)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
            Vector3 move = pos.y * zoomSpeed * transform.forward;
            transform.Translate(move, Space.World);
        }

        if (!Input.GetMouseButton(0)) isRotating = false;
        if (!Input.GetMouseButton(1)) isPanning = false;
        if (!Input.GetMouseButton(2)) isZooming = false;

    }
}