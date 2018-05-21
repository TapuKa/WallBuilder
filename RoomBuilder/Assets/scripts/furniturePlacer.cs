using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class furniturePlacer : MonoBehaviour
{

    public GameObject onFloorCursor;
    public GameObject furniture;
    public wallConstruct wallconstruct;
    float rot;
    float speed = 200;
    public Toggle toggleFurniture;
    public bool isPlacingToogle;
    bool isColliding;
    public bool furnitureMode;
    Color colTemp;
    RaycastHit findHit = new RaycastHit();
    Vector3 pointOnMesh;

    void PlaceFurniture(Vector3 pos, GameObject furniture)
    {
        furniture.transform.position = new Vector3(Mathf.Lerp(furniture.transform.position.x, pos.x, 1), 0, Mathf.Lerp(furniture.transform.position.z, pos.z, 1));
    }

    public void ToggleFurnitureMode()
    {
        isPlacingToogle = toggleFurniture.isOn;
        if (isPlacingToogle)
        {
            wallconstruct.buildmode = false;
            furniture.SetActive(true);
            furnitureMode = true;
        }
        else
        {
            wallconstruct.buildmode = true;
            furniture.SetActive(false);
            furnitureMode = false;
        }

    }
    void Update()
    {

        if (!furnitureMode || wallconstruct.buildmode || EventSystem.current.IsPointerOverGameObject()) //Cursor ueber UI
        {

            furniture.SetActive(false);
            return;
        }

        if (Input.GetKey(KeyCode.LeftAlt)) // nicht im buildmodus
        {
            return;
        }
        furniture.SetActive(true);

        Cursor.visible = false;
        

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction, out findHit))
        {
            pointOnMesh = findHit.point;
            pointOnMesh.y = 0.01f;
            PlaceFurniture(pointOnMesh, furniture);

            float rot = Input.GetAxis("Mouse ScrollWheel") * speed;
            furniture.transform.Rotate(Vector3.down * rot);

            if (Input.GetMouseButtonUp(0))
            {

                Instantiate(furniture, furniture.transform.position, furniture.transform.rotation);
            }
        }
    }

}
