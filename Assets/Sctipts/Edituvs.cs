using System;
using System.Collections.Generic;
using UnityEngine;

public class Edituvs : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = mesh.uv;

        string filePath = @"C:\Users\eight\Downloads\temp.txt";
        System.IO.File.WriteAllText(filePath, System.DateTime.Now + Environment.NewLine);
        System.IO.File.AppendAllText(filePath, mesh.ToString() + Environment.NewLine);
        for (int i = 0; i < uvs.Length; i++) {
            System.IO.File.AppendAllText(filePath, i + ":x," + uvs[i].x + " ,y," + uvs[i].y + Environment.NewLine);
        }

        Vector2[] changeUvs = new Vector2[uvs.Length];
        var verticesLines = new List<List<int>>();

        verticesLines.Add(new List<int>() { 34 });
        verticesLines.Add(new List<int>() { 29, 33 });
        verticesLines.Add(new List<int>() { 22, 28, 35 });
        verticesLines.Add(new List<int>() { 16, 21, 32, 36 });
        verticesLines.Add(new List<int>() { 11, 15, 25, 37, 38 });
        verticesLines.Add(new List<int>() { 7, 10, 20, 27, 39, 40 });
        verticesLines.Add(new List<int>() { 4, 6, 14, 19, 26, 41, 42 });
        verticesLines.Add(new List<int>() { 1, 3, 8, 12, 17, 23, 30, 43 });
        verticesLines.Add(new List<int>() { 0, 2, 5, 9, 13, 18, 24, 31, 44 });
      
        foreach (var verticesLine in verticesLines) {
            float xDis = 1f / verticesLine.Count;
            float yDis = 1f / (verticesLines.Count - 1);
            float y;
            if (verticesLine.Count == 1) y = 1f;
            else if (verticesLine.Count == 9) y = 0f;
            else y = 1f - (yDis * (verticesLine.Count - 1));
            for (int i = 0; i < verticesLine.Count; i++) {                              
                float x = (i == verticesLine.Count - 1) ? 1f : 0f + (xDis * i);
                System.IO.File.AppendAllText(filePath, i + ":x," + xDis + Environment.NewLine);
                changeUvs[verticesLine[i]] = new Vector2(x, y);
            }
        }
        mesh.SetUVs(0, changeUvs);

        Vector2[] checkUvs = mesh.uv;
        
        System.IO.File.AppendAllText(filePath, "----------changed----------" + Environment.NewLine);
        for (int i = 0; i < uvs.Length; i++) {
            System.IO.File.AppendAllText(filePath, i + ":x," + checkUvs[i].x + " ,y," + checkUvs[i].y + Environment.NewLine);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
