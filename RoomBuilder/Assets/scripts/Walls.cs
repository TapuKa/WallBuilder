using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls
{
    public static int ID = 0;
    public Vector3 startMarker;
    public Vector3 endMarker;
    public GameObject wall;
    public Material mat;
    public float angle;
    public GameObject extraWallA;
    public GameObject extraWallB;
    public bool isSideWall;
    public GameObject TriangleOne;
    public GameObject TriangleTwo;

    public static GameObject wallPre;
    public static Material mat1;

    public Walls(int id, Vector3 start)
    {
        startMarker = start;
        endMarker = start;
        ++id;
        ID++;
        isSideWall = false;
        wallPre = wall;
        mat1 = mat;
    }

    public int GetId()
    {
        return ID;
    }

    public void SetTexture(Material mat, Material sideMat)
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

    public void PlaceWall(Walls wl, Vector3 point, int wallId, Material mat1, Material mat2)
    {
        wl.endMarker = point;
        wl.wall.transform.LookAt(wl.endMarker);
        Vector3 ls = wl.wall.transform.localScale;
        wl.wall.transform.localScale = new Vector3(ls.x, ls.y, Vector3.Distance(wl.startMarker, wl.endMarker));
        SetTexture(mat1, mat2);
    }
}
