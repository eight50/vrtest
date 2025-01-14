using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUVs : MonoBehaviour
{
    [SerializeField] private MeshFilter mesh;
    Vector2[] uvs;

    // Start is called before the first frame update
    void Start()
    {
        uvs = mesh.mesh.uv;

        System.IO.File.WriteAllText(@"C:\Users\eight\Downloads\temp.txt", "");

        for (int i =0; i < uvs.Length; i++) {
            System.IO.File.AppendAllText(@"C:\Users\eight\Downloads\temp.txt", i + ":x," + uvs[i].x + " ,y," + uvs[i].y + Environment.NewLine);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
