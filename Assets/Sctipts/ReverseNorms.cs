using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseNorms : MonoBehaviour {

    // Use this for initialization
    void Start() {
        MeshFilter filter = GetComponent(typeof(MeshFilter)) as MeshFilter;
        if (filter != null) {
            Mesh mesh = CopyMesh(filter.mesh);

            Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
                normals[i] = -normals[i];
            mesh.normals = normals;

            for (int m = 0; m < mesh.subMeshCount; m++) {
                int[] triangles = mesh.GetTriangles(m);
                for (int i = 0; i < triangles.Length; i += 3) {
                    int temp = triangles[i + 0];
                    triangles[i + 0] = triangles[i + 1];
                    triangles[i + 1] = temp;
                }
                mesh.SetTriangles(triangles, m);
            }
        }

        this.GetComponent<MeshCollider>().sharedMesh = filter.mesh;
    }

    // Update is called once per frame
    void Update() {

    }

    public Mesh CopyMesh(Mesh mesh) {
        var copy = new Mesh();
        foreach (var property in typeof(Mesh).GetProperties()) {
            if (property.GetSetMethod() != null && property.GetGetMethod() != null) {
                property.SetValue(copy, property.GetValue(mesh, null), null);
            }
        }
        return copy;
    }
}

