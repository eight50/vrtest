using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.ProBuilder;


public class EditUnderuvs : MonoBehaviour {
        void Reset() {
            UnityEngine.Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
            List<Vector3> basedVertices = new List<Vector3>();
            mesh.GetVertices(basedVertices);
            List<Vector2> basedUVs = new List<Vector2>();
            mesh.GetUVs(0, basedUVs);

            Vector2[] changeUvs = new Vector2[basedUVs.Count];
            var verticesLines = new List<List<int>>();

            //verticesLines.Add(new List<int>() { 0, 2, 5, 9, 13, 18, 24, 31, 44 });
            //verticesLines.Add(new List<int>() { 1, 3, 8, 12, 17, 23, 30, 43 });
            //verticesLines.Add(new List<int>() { 4, 6, 14, 19, 26, 41, 42 });
            //verticesLines.Add(new List<int>() { 7, 10, 20, 27, 39, 40 });
            //verticesLines.Add(new List<int>() { 11, 15, 25, 37, 38 });
            //verticesLines.Add(new List<int>() { 16, 21, 32, 36 });
            //verticesLines.Add(new List<int>() { 22, 28, 35 });
            //verticesLines.Add(new List<int>() { 29, 33 });
            //verticesLines.Add(new List<int>() { 34 });

            verticesLines.Add(new List<int>() { 34 });
            verticesLines.Add(new List<int>() { 29, 33 });
            verticesLines.Add(new List<int>() { 22, 28, 35 });
            verticesLines.Add(new List<int>() { 16, 21, 32, 36 });
            verticesLines.Add(new List<int>() { 11, 15, 25, 37, 38 });
            verticesLines.Add(new List<int>() { 7, 10, 20, 27, 39, 40 });
            verticesLines.Add(new List<int>() { 4, 6, 14, 19, 26, 41, 42 });
            verticesLines.Add(new List<int>() { 1, 3, 8, 12, 17, 23, 30, 43 });
            verticesLines.Add(new List<int>() { 0, 2, 5, 9, 13, 18, 24, 31, 44 });

            foreach(var line in verticesLines) {
                line.Reverse();
            }

            //foreach (var vertices in verticesLines) {
            //    float xDis = 1f / (vertices.Count - 1);
            //    float yDis = 1f / (verticesLines.Count - 1);
            //    float x, y;
            //    if (vertices.Count == 1) {
            //        y = 1f;
            //    } else if (vertices.Count == verticesLines.Count) {
            //        y = 0f;
            //    } else {
            //        y = 1f - (yDis * (vertices.Count - 1));
            //    }
            //    for (int i = 0; i < vertices.Count; i++) {
            //        if (vertices.Count == 1) {
            //            x = 0.5f;
            //        } else if (i == vertices.Count - 1) {
            //            x = 1f;
            //        } else {
            //            x = xDis * i;
            //        }
            //        changeUvs[vertices[i]] = new Vector2(x, y);
            //    }
            //}

            // 極の頂点から半径を取得
            Vector3 topVertexWorldPosition = transform.TransformPoint(basedVertices[verticesLines[0][0]]);
            float sphereRadius = Mathf.Abs(topVertexWorldPosition.y);
            Debug.Log("Radius: " + sphereRadius);
            foreach (var verticesIndex in verticesLines) {
                float x, y;
                for (int i = 0; i < verticesIndex.Count; i++) {
                    Vector3 vertex = basedVertices[verticesIndex[i]];
                    Vector3 vertexWorldPosition = transform.TransformPoint(vertex);
                    Debug.Log("vertexWorldPosition: " + vertexWorldPosition + "index: " + verticesIndex[i]);
                    if (verticesIndex.Count == 1) {
                        y = 1f;
                    } else if (verticesIndex.Count == verticesLines.Count) {
                        y = 0f;
                    } else {
                        // basedvertices[vertices[i]].y / topvertices.y 半径で割って0～1の値にする
                        float normY = Mathf.Abs(vertexWorldPosition.y / sphereRadius);
                        y = Mathf.Asin(normY) / Mathf.PI;
                        y *= 2;
                        Debug.Log("norm(y): " + normY);
                    }

                //// どの象限かによって変える
                    if (verticesIndex.Count == 1) { // 極付近の頂点
                    x = 0.5f;
                    ////} else if (i == verticesIndex.Count - 1) { // 右端の頂点
                    ////    if (vertexWorldPosition.x >= 0f && vertexWorldPosition.z >= 0f) {
                    ////        x = 1f;
                    ////    } else if (vertexWorldPosition.x >= 0f && vertexWorldPosition.z >= 0f) {
                    ////        x = 1f;
                    ////    } else if (vertexWorldPosition.x >= 0f && vertexWorldPosition.z >= 0f) {
                    ////        x = 1f;
                    ////    } else {
                    ////        x = 1f;
                    ////    }
                    ////} else if(i == 0) { // 左端の頂点
                    ////    if (vertexWorldPosition.x >= 0f && vertexWorldPosition.z >= 0f) {
                    ////        x = 0f;
                    ////    } else if (vertexWorldPosition.x >= 0f && vertexWorldPosition.z >= 0f) {
                    ////        x = 0f;
                    ////    } else if (vertexWorldPosition.x >= 0f && vertexWorldPosition.z >= 0f) {
                    ////        x = 0f;
                    ////    } else {
                    ////        x = 0f;
                    ////    }
                    } else if (i == 0) { 
                        x = 0f; 
                    } else {
                        float normX = (vertexWorldPosition.x / sphereRadius);
                        float normZ = (vertexWorldPosition.z / sphereRadius);
                        UnityEngine.Debug.Log("norm(x, z): " + normX + ", " + normZ);

                        //if(normX < 0f + Mathf.Epsilon && normX > 0f - Mathf.Epsilon) {
                        //    normX *= -1f;
                        //}
                        //if (normX < 0f + Mathf.Epsilon && normX > 0f - Mathf.Epsilon) {
                        //    normX *= -1f;
                        //}

                        float radian = normZ / normX;
                        x = (Mathf.Atan(radian) / (Mathf.PI));
                        UnityEngine.Debug.Log("OK");
                        // 定義域は0～π/2なので、2倍してTan^-1の値域を0～1の間にする
                        x *= 2;
                        // Tan^-1の値が負の場合、正の値へ対称移動させる
                        if (x < 0f) {
                            x += 1f;
                        }
                        UnityEngine.Debug.Log("x: " + x);
                    }
                    Debug.Log("res:(x,y) " + x + ", " + y);
                    changeUvs[verticesIndex[i]] = new Vector2(x, y);
                }
            }

            mesh.SetUVs(0, changeUvs);
        }

        // Update is called once per frame
        void Update() {

        }
    }
