using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(MeshFilter))]

public class ingest_ply_file : MonoBehaviour
{
    //Gizmo  control
    public bool GizmosOn = true;


    Vector3[] positions;
    Color[] colors;
    GameObject[] vertecies;

    //mesh variables
    Mesh mesh;
    Vector3[] meshVerts;
    int[] meshTriangles;

    public int numGizmoIncrement;

    HashSet<string> hs = new HashSet<string>();
    int SIZE;
    public int global_scale = 10;
    public float gizmo_scale = 10;
    string[] lines;
    string[] curPosition;
    string[] curColor;
    public string file_name;
    public int colorInt = 0;
    public Color RGB_color;

    public float r, g, b;


    // Start is called before the first frame update
    void Start()
    {

        float maxY=0, minY=0, maxX=0, minX = 0;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        /*
        format ascii 1.0
        comment pointcloud saved from Realsense Viewer
        element vertex 49512
        property float32 x
        property float32 y
        property float32 z
        property uchar red
        property uchar green
        property uchar blue
        end_header
        -1.45268 0.902427 -1.93575 //x,y,z
        22 32 5  //RGB
        */
        //readFile.getAsset
        //lines = File.ReadAllLines(Application.dataPath + "/" + model + ".xyz");
        lines = File.ReadAllLines("/Users/chrisnelson/Documents/PROJECTS/build/" + file_name);
        SIZE = lines.Length;
        
        positions = new Vector3[SIZE / 2 ]; 
        colors = new Color[SIZE / 2];
        int count = 0;
        //Debug.Log(lines[11]);
        for (int i = 11; i < lines.Length; i += numGizmoIncrement, count++)
        {
            //Debug.Log(lines[i]);
            //Debug.Log(lines[i+1]);
            curPosition = lines[i].Split(' ');
            curColor = lines[i + 1].Split(' ');
            positions[count] = new Vector3(float.Parse(curPosition[0]), float.Parse(curPosition[1]), float.Parse(curPosition[2]));

            //scale up the cloud
            if(i == lines.Length-2)
                Debug.Log("true last position before scale is: " + positions[count].ToString());

            positions[count] *= global_scale;

            //get max XY
            maxX = Mathf.Max(maxX, positions[count].x);
            maxY = Mathf.Max(maxY, positions[count].y);

            //get min XY
            minX = Mathf.Min(minX, positions[count].x);
            minY = Mathf.Min(minY, positions[count].y);

            if (i == lines.Length - 2)
            {
                Debug.Log("true last position after scale is: " + positions[count].ToString());
                Debug.Log("COUNT: " + count.ToString());
                Debug.Log("SIZE/2: " + (SIZE/2).ToString());
            }

            colors[count] = new Color(float.Parse(curColor[0])/255, float.Parse(curColor[1])/255, float.Parse(curColor[2])/255);
            string HScolor = curColor[0] + " " + curColor[1] + " " + curColor[2];
            hs.Add(HScolor);
        }



        //vertecies = new GameObject[positions.Length];


        //for (int i = 0; i < vertecies.Length; i++)
        //{
        //    vertecies[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    vertecies[i].transform.position = positions[i];
        //    vertecies[i].transform.localScale = new Vector3(.05f, .05f, .05f);
        //    vertecies[i].GetComponent<Renderer>().material.SetColor("_Color", colors[i]);
        //}

        Debug.Log("size of pos array: " + (SIZE/2).ToString());
        Debug.Log("number of positions: " + positions.Length.ToString());
        Debug.Log("number of colors: " + colors.Length.ToString());
        Debug.Log("number of unique colors: " + hs.Count);

        Debug.Log("first position is: " + positions[0].ToString());
        Debug.Log("last position is: " + positions[SIZE/2-6].ToString());

        Debug.Log("Max X: " + maxX.ToString() + " ~~~ Min X: " + minX.ToString() + " ~~~ Max Y: " + maxY.ToString() + " ~~~ Min Y: " + minY.ToString());


        createShape();
        updateMesh();

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDrawGizmos()
    {
        if(GizmosOn)
        {
            RGB_color = new Color(r, g, b);
            Debug.Log("drawing gizmos");
            if (!Application.isPlaying) return;

            int i = 0;
            foreach (Vector3 p in positions)
            {
                Gizmos.DrawSphere(p, gizmo_scale);
                // Color j = colors[colorInt];
                Gizmos.color = colors[i];
                //Debug.Log(colors[i]);
                i++;
            }
        }
       
    }

    void createShape()
    {
        meshVerts = positions;
        meshTriangles = new int[positions.Length+2];

        for (int i = 0; i+2 < positions.Length; i+=2)
        {
            meshTriangles[i] = i;
            meshTriangles[i + 1] = i + 1;
            meshTriangles[i + 2] = i + 2;
        };
        Debug.Log("triangle size: " + meshTriangles.Length.ToString());

    }

    void updateMesh()
    {
        mesh.Clear();

        mesh.vertices = meshVerts;
        mesh.triangles = meshTriangles;
    }
}

