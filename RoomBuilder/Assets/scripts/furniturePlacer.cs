using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class furniturePlacer : MonoBehaviour
{

    public GameObject onFloorCursor;
    public GameObject furniture;
    public wallConstruct walconstruct;
    float rot;
    float speed = 200;
    public Toggle toggleFurniture;
    public bool isPlacingToogle;
    bool isColliding;
    public bool furnitureMode;
    Color colTemp;



    RaycastHit findHit = new RaycastHit();
    Vector3 pointOnMesh;

    void placeFurniture(Vector3 pos, GameObject furniture)
    {
        furniture.transform.position = new Vector3(Mathf.Lerp(furniture.transform.position.x, pos.x, 1), 0, Mathf.Lerp(furniture.transform.position.z, pos.z, 1));
        //furniture.transform.position = pos;
    }

    public void toggleFurnitureMode()
    {
        isPlacingToogle = toggleFurniture.isOn;
        if (isPlacingToogle)
        {
            walconstruct.buildmode = false;
            furniture.SetActive(true);
            furnitureMode = true;
        }
        else
        {
            walconstruct.buildmode = true;
            furniture.SetActive(false);
            furnitureMode = false;

        }

    }

    // Use this for initialization
    void Start()
    {
      //  onFloorCursor.SetActive(true);
    }

    // Update is called once per frame


  

    void Update()
    {

        if (!furnitureMode ||  walconstruct.buildmode || EventSystem.current.IsPointerOverGameObject()) //Cursor ueber UI
        {

            furniture.SetActive(false);
            return;
        }

        if (Input.GetKey(KeyCode.LeftAlt)) // nicht im buildmodus
        {
            return;
        }
        furniture.SetActive(true);

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction, out findHit))
        {
            pointOnMesh = findHit.point;
            pointOnMesh.y = 0.01f;
            placeFurniture(pointOnMesh, furniture);

            float rot = Input.GetAxis("Mouse ScrollWheel") * speed;
            furniture.transform.Rotate(Vector3.down * rot);

            if (Input.GetMouseButtonUp(0))
            {

                GameObject go = Instantiate(furniture, furniture.transform.position, furniture.transform.rotation);


            }
        }
    }

}
