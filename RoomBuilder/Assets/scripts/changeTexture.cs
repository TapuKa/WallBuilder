using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeTexture : MonoBehaviour
{
    public wallConstruct wallconstruct;

    public Material mat1;
    public Material mat2;

    public GameObject wallPrefab;

    public Material changeToMaterial;
    bool isHovering = false;
    Renderer prevRe;
    Color oldCol;
    public Toggle textureToggleOne;
    public Toggle textureToggleTwo;

    RaycastHit findHit = new RaycastHit();
    Vector3 wallHit;


    void Start()
    {
        changeToMaterial = mat1;
    }


    public void toggleTexture()
    {
        if (textureToggleOne.isOn)
        {
            changeToMaterial = mat1;
            wallconstruct.wallMat = mat1;
        }

        if (textureToggleTwo.isOn)
        {
            changeToMaterial = mat2;
            wallconstruct.wallMat = mat2;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (wallconstruct.buildmode)
            {
                wallconstruct.buildmode = false;
                wallconstruct.onFloorCursor.SetActive(false);
            }
            else
            {
                wallconstruct.buildmode = true;
                wallconstruct.onFloorCursor.SetActive(true);
            }
        }

        if (wallconstruct.buildmode)
        {
            return;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction, out findHit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (findHit.transform.tag == "front" || findHit.transform.tag == "back")
                {
                    Renderer re = findHit.transform.gameObject.GetComponent<Renderer>();
                    if (re)
                    {
                        re.material = changeToMaterial;
                        Vector3 ls = findHit.transform.root.transform.localScale;
                        Vector2 texScale = new Vector2(1, ls.z / ls.x);
                        re.material.SetTextureScale("_MainTex", texScale);

                        isHovering = false;

                    }
                }
            }
            else
            {
                if (findHit.transform.tag == "front" || findHit.transform.tag == "back")
                {

                    Renderer re = findHit.transform.gameObject.GetComponent<Renderer>();

                    if (re && !isHovering)
                    {
                        prevRe = re;
                        oldCol = re.material.color;
                        re.material.color = Color.yellow;
                        wallHit = findHit.transform.root.position;
                        isHovering = true;
                    }
                    else
                    {
                        if (isHovering)
                        {
                            if (wallHit != findHit.transform.root.position)
                            {
                                prevRe.material.color = oldCol;
                                isHovering = false;
                            }

                        }
                    }
                }
                else
                {
                    if (isHovering)
                    {
                        if (wallHit != findHit.transform.root.position)
                        {
                            prevRe.material.color = oldCol;
                            isHovering = false;
                        }

                    }
                }

            }
        }
    }
}
