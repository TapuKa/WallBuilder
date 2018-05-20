using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class wallConstruct : MonoBehaviour
{

    RaycastHit findHit = new RaycastHit();
    Vector3 pointOnMesh;
    Vector3 pointStartClick;
    Vector3 pointm;

    public GameObject wallPrefab;
    public Material wallMat;
    public Material wallSidesMat;
    public Material highlightwallMat;
    public GameObject wallTempPrefab;
    public GameObject onWallCursor;
    public GameObject onFloorCursor;
    public GameObject cornerMarker;
    public GameObject triangle;

    public GameObject extrapreViewsidewallA;
    public GameObject extrapreViewsidewallB;
    public GameObject extrasidewallA;
    public GameObject extrasidewallB;

    public Toggle buildToggle;
    public Toggle textureOneToggle;
    public Toggle textureTwoToggle;
    public Toggle appendToLast;
    static GameObject wallPre;
    Vector3 offset;
    float newAlpha;
    float oldAlpha;
    float moveForward;
    GameObject cornerA;
    GameObject cornerB;
    GameObject cornerC;
    GameObject cornerD;
    GameObject previeWallCornerA;
    GameObject previeWallCornerB;
    GameObject previeWallCornerC;
    GameObject previeWallCornerD;

    public furniturePlacer furnitureplacer;
    public bool buildmode = true;

    bool isFirstClick = true;
    bool isFirstNewWall = true;
    bool isClickOnWall = false;
    bool isSideWall = false;
    public bool isAppending = true;

    static int maxWalls = 1000;
    int wallcount = 0;

    Walls[] wall = new Walls[maxWalls];


    public void toggleBuildMode()
    {
        buildmode = buildToggle.isOn;

    }
    public void toggletexturedMode()
    {
        buildmode = buildToggle.isOn;

        if (textureOneToggle.isOn)
        {
            textureOneToggle.isOn = false;
            textureTwoToggle.isOn = true;
        }
        else
        {
            textureOneToggle.isOn = true;
            textureTwoToggle.isOn = false;
        }
    }

    public void toggleappendToLast()
    {
        isAppending = appendToLast.isOn;
    }
    void placeWall(Walls wl)
    {
        wl.endMarker = pointOnMesh;
        wl.wall.transform.LookAt(wl.endMarker);
        Vector3 ls = wl.wall.transform.localScale;
        wl.wall.transform.localScale = new Vector3(ls.x, ls.y, Vector3.Distance(wl.startMarker, wl.endMarker));
        wallcount++;
    }

    void buildExtraSegments(int wc)
    {
        cornerA = wallTempPrefab.transform.Find("cornerPreA").gameObject;
        cornerB = wallTempPrefab.transform.Find("cornerPreB").gameObject;
        cornerC = wall[wallcount - 1].wall.transform.Find("cornerC").gameObject;
        cornerD = wall[wallcount - 1].wall.transform.Find("cornerD").gameObject;

        wall[wc].extraWallA = Instantiate(extrasidewallA, cornerA.transform.position, Quaternion.identity);
        wall[wc].extraWallB = Instantiate(extrasidewallB, cornerB.transform.position, Quaternion.identity);

        Renderer re = wall[wc].extraWallA.GetComponent<Renderer>();
        re.material = wall[wc - 1].mat;
        float extrascaleZ = Vector3.Distance(cornerA.transform.position, cornerC.transform.position);
        Vector3 extrals = wall[wc].extraWallA.transform.localScale;
        extrals.z = extrascaleZ;

        wall[wc].extraWallA.transform.localScale = extrals;
        wall[wc].extraWallA.transform.LookAt(cornerC.transform);

        Vector2 texScale = new Vector2(1, extrals.z / extrals.x);
        if (re.material.name == "wallMatTwo")
        {
            texScale.x = 2f;
        }
        re.material.SetTextureScale("_MainTex", texScale);
        extrascaleZ = Vector3.Distance(cornerB.transform.position, cornerD.transform.position);
        extrals = extrasidewallB.transform.localScale;
        extrals.z = extrascaleZ;
        re = wall[wc].extraWallB.GetComponent<Renderer>();
        re.material = wall[wc - 1].mat;
        wall[wc].extraWallB.transform.localScale = extrals;
        wall[wc].extraWallB.transform.LookAt(cornerD.transform);
        texScale = new Vector2(1, extrals.z / extrals.x);
        if (re.material.name == "wallMatTwo")
        {
            texScale.x = 2f;
        }
        re.material.SetTextureScale("_MainTex", texScale);

        wall[wc].TriangleOne = Instantiate(triangle, Vector3.zero, Quaternion.identity);
        Mesh mesh = wall[wc].TriangleOne.GetComponent<MeshFilter>().mesh;

        Vector3[] vertices = mesh.vertices;
        vertices[0] = wall[wc].endMarker;
        vertices[0].y = 2f;
        vertices[1] = cornerA.transform.position;
        vertices[2] = cornerC.transform.position;
        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        wall[wc].TriangleTwo = Instantiate(triangle, Vector3.zero, Quaternion.identity);
        mesh = wall[wc].TriangleTwo.GetComponent<MeshFilter>().mesh;
        vertices[0] = wall[wc].endMarker;
        vertices[0].y = 2f;
        vertices[1] = cornerD.transform.position;
        vertices[2] = cornerB.transform.position;
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }

    void buildWall(int wc, Vector3 pos)
    {
        if (isFirstNewWall) //erste click etabliert startpunkt der ersten Wand
        {
            if (isFirstClick)
            {
                if (!isSideWall) // zweiter click zur vervollstaendigung der ersten Wand
                {
                    //  pos = pointOnMesh;
                    wall[wc] = new Walls(wc, pos);
                    wall[wc].wall = Instantiate(wallPre, wall[wc].startMarker, Quaternion.identity);
                    wall[wc].mat = wallMat;
                    wall[wc].wall.SetActive(false);
                    wall[wc].angle = wall[wc].wall.transform.eulerAngles.y;
                    isFirstClick = false;
                }
            }
            else
            if (!isSideWall) // zweiter click zur vervollstaendigung der ersten Wand
            {
                placeWall(wall[wc]);
                wall[wc].changeMaterial(wallMat, wallSidesMat);
                isFirstNewWall = false;
            }
        }
        else
        {
            if (!isFirstNewWall && !isClickOnWall && !isSideWall)  // es gibt bereits eine Wand, die mit neuem click verlaengert wird
            {
                wall[wc] = new Walls(wc, pos);
                wall[wc].wall = Instantiate(wallPre, wall[wc].startMarker, Quaternion.identity);

                if (wc > 0 && !wall[wallcount - 1].isSideWall)
                {
                    buildExtraSegments(wc);
                }

                wall[wc].mat = wallMat;
                wall[wc].wall.SetActive(false);
                placeWall(wall[wc]);
                wall[wc].angle = wall[wc].wall.transform.eulerAngles.y;
                wall[wc].changeMaterial(wallMat, wallSidesMat);
            }
        }

        if (isClickOnWall) // erste Click für Wand die an beliebiger Position einer bestehenden Wand ansetzt
        {
            pointStartClick.y = 0;
            pos.y = 0;
            wall[wc] = new Walls(wc, pos);
            wall[wc].wall = Instantiate(wallPre, wall[wc].startMarker, Quaternion.identity);
            wall[wc].mat = wallMat;
            wall[wc].wall.SetActive(false);
            wall[wc].angle = wall[wc].wall.transform.eulerAngles.y;
            isClickOnWall = false;
            isSideWall = true;
        }
        else
        if (isSideWall && !isClickOnWall)// zweiter Click für Wand die an beliebiger Position einer bestehenden Wand ansetzt
        {
            placeWall(wall[wc]);
            wall[wc].isSideWall = true;
            wall[wc].changeMaterial(wallMat, wallSidesMat);
            isSideWall = false;
        }
    }

    void previewWall(Vector3 pos, Vector3 offset)
    {
        if (wallcount > 0)
        {
            if (!isSideWall)
            {
                wallTempPrefab.transform.position = pos + offset;

                Vector3 ls = wallTempPrefab.transform.localScale;
                wallTempPrefab.transform.localScale = new Vector3(ls.x, ls.y, Vector3.Distance(wallTempPrefab.transform.position - offset, pointOnMesh));
                wallTempPrefab.transform.LookAt(pointOnMesh);
                if (wallTempPrefab.transform.localScale.z > 0)
                {
                    wallTempPrefab.SetActive(true);
                }
                else
                {
                    wallTempPrefab.SetActive(false);
                    extrapreViewsidewallB.SetActive(false);
                    extrapreViewsidewallA.SetActive(false);
                }
                cornerD = wall[wallcount - 1].wall.transform.Find("cornerD").gameObject;
                cornerC = wall[wallcount - 1].wall.transform.Find("cornerC").gameObject;
                float extrascaleZ = Vector3.Distance(extrapreViewsidewallA.transform.position, cornerC.transform.position);
                Vector3 extrals = extrapreViewsidewallA.transform.localScale;
                extrals.z = extrascaleZ;

                extrapreViewsidewallA.transform.localScale = extrals;
                extrapreViewsidewallA.transform.LookAt(cornerC.transform);
                extrascaleZ = Vector3.Distance(extrapreViewsidewallB.transform.position, cornerD.transform.position);
                extrals = extrapreViewsidewallB.transform.localScale;
                extrals.z = extrascaleZ;
                extrapreViewsidewallB.transform.localScale = extrals;
                extrapreViewsidewallB.transform.LookAt(cornerD.transform);
                if (extrascaleZ < 1)
                    extrapreViewsidewallB.SetActive(true);
                if (extrascaleZ < 1)
                    extrapreViewsidewallA.SetActive(true);

            }
            else
            {
                float scaleZ = Vector3.Distance(pointStartClick, pointOnMesh);
                pointStartClick.y = 0;
                wallTempPrefab.transform.position = pointStartClick;
                Vector3 ls = wallTempPrefab.transform.localScale;
                wallTempPrefab.transform.localScale = new Vector3(ls.x, ls.y, scaleZ);
                wallTempPrefab.transform.LookAt(pointOnMesh);
                extrapreViewsidewallB.SetActive(false);
                extrapreViewsidewallA.SetActive(false);

                if (wallTempPrefab.transform.localScale.z > 0)
                {
                    wallTempPrefab.SetActive(true);
                }
            }
        }
        else
        {
            float scaleZ = Vector3.Distance(pointStartClick, pointOnMesh);
            if (scaleZ > 0)
            {
                wallTempPrefab.transform.position = pointStartClick;
                Vector3 ls = wallTempPrefab.transform.localScale;
                wallTempPrefab.transform.localScale = new Vector3(ls.x, ls.y, scaleZ);
                wallTempPrefab.transform.LookAt(pointOnMesh);
                wallTempPrefab.SetActive(true);
            }
        }
    }

    void showCursor(Vector3 pos, GameObject cursor)
    {
        cursor.transform.position = pos;
    }

    void Start()
    {
        wallTempPrefab.SetActive(false);
        onWallCursor.SetActive(false);
        onWallCursor.SetActive(false);
        //   _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        wallPre = wallPrefab;
        // mat1 = wallMat;
        appendToLast.isOn = true;
        offset = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) //Cursor ueber UI
        {
            onWallCursor.SetActive(false);
            wallTempPrefab.SetActive(false);
            onFloorCursor.SetActive(false);
            return;
        }

        if (!buildmode || Input.GetKey(KeyCode.LeftAlt)) // nicht im buildmodus
        {
            return;
        }
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction, out findHit))
        {
            pointOnMesh = findHit.point;

            if (Input.GetMouseButtonDown(0))
            {
                if (findHit.transform.tag == "floor")
                {
                    if (isFirstClick)
                    {
                        pointStartClick = pointOnMesh;
                        buildWall(wallcount, pointOnMesh);
                    }                   
                }
                else
                if (findHit.transform.tag == "front" || findHit.transform.tag == "back" || findHit.transform.tag == "side")
                {
                    isClickOnWall = true;
                    pointStartClick = pointOnMesh;
                    buildWall(wallcount, pointStartClick);
                    wallTempPrefab.SetActive(false);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (findHit.transform.tag == "floor" && !isSideWall)  // neue Wand an das Ende der letzten
                {
                    if (wallcount > 0)
                    {
                        if (isAppending)
                            buildWall(wallcount, wall[wallcount - 1].endMarker);
                    }
                    else
                        buildWall(wallcount, pointStartClick);
                }
                if (isSideWall)                                     //neue Wand an dieSeite einer beliebig  bestehenden
                {
                    pointOnMesh.y = 0;
                    buildWall(wallcount, pointOnMesh);
                }
                wallTempPrefab.SetActive(false);
                extrapreViewsidewallB.SetActive(false);
                extrapreViewsidewallA.SetActive(false);
                wall[wallcount - 1].wall.SetActive(true);
                isSideWall = false;
            }


            if (findHit.transform.tag == "floor" && !isFirstClick)
            {
                if (wallcount > 0)
                {
                    //                   prevPoint = pointOnMesh;
                    if (isAppending || isSideWall)
                    {
                        extrapreViewsidewallA.transform.position = wallTempPrefab.transform.Find("cornerPreA").gameObject.transform.position;
                        extrapreViewsidewallB.transform.position = wallTempPrefab.transform.Find("cornerPreB").gameObject.transform.position;
                        previewWall(wall[wallcount - 1].endMarker, offset);
                    }
                    else
                    {
                        wallTempPrefab.SetActive(false);
                        extrapreViewsidewallB.SetActive(false);
                        extrapreViewsidewallA.SetActive(false);
                    }
                }
                else
                {
                    previewWall(pointStartClick, Vector3.zero);
                }
            }
            if (findHit.transform.tag == "floor") //Cursor am Boden
            {
                onFloorCursor.SetActive(true);
                onWallCursor.SetActive(false);
                showCursor(pointOnMesh, onFloorCursor);
            }

            if (findHit.transform.tag == "front" || findHit.transform.tag == "back" || findHit.transform.tag == "side") //Cursor auf Wand
            {
                pointOnMesh.y = 0f;
                previewWall(pointStartClick, Vector3.zero);
                if (onFloorCursor && wallTempPrefab)
                {
                    wallTempPrefab.SetActive(false);
                    extrapreViewsidewallB.SetActive(false);
                    extrapreViewsidewallA.SetActive(false);
                }

                onFloorCursor.SetActive(false);
                extrapreViewsidewallB.SetActive(false);
                extrapreViewsidewallA.SetActive(false);
                onWallCursor.SetActive(true);
                showCursor(pointOnMesh, onWallCursor);
            }
        }
    }
}
