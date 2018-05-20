using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls
{

    public Vector3 startMarker;
    public Vector3 endMarker;
    public GameObject wall;
    public Material mat;
    public float angle;
    public GameObject extraWallA;
    public GameObject extraWallB;
    public bool isSideWall;

    public static GameObject wallPre;
    public static Material mat1;
    public int ID { get; private set; }

    public Walls(int id, Vector3 start)
    {
        startMarker = start;
        endMarker = start;
        ++id;
        ID = id;
        isSideWall = false;
    }

    public void changeMaterial(Material mat, Material sideMat)
    {
        GameObject child = wall.transform.GetChild(0).gameObject;
        int numOfChildren = child.transform.childCount;
        for (int i = 0; i < numOfChildren; i++)
        {
            GameObject childofChild = child.transform.GetChild(i).gameObject;
            if (childofChild.GetComponent<Renderer>().material != null)
            {
                if (childofChild.tag == "front" || childofChild.tag == "back")
                {
                    childofChild.GetComponent<Renderer>().material = mat;
                    Vector3 ls = wall.transform.localScale;
                    Vector2 texScale = new Vector2(1, ls.z / ls.x);
                    childofChild.GetComponent<Renderer>().material.SetTextureScale("_MainTex", texScale);
                }
                else
                    childofChild.GetComponent<Renderer>().material = sideMat;
            }
        }
    }

    void Start()
    {

        wallPre = wall;
        mat1 = mat;
    }


}
