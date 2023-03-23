using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeformableMesh : MonoBehaviour
{

    public float maximumDepression;
    public float depressionStep;
    public float reversionStep;
    private bool isColliding = false;
    public GameObject interiorObject;
    public List<Vector3> originalVertices;
    public List<Vector3> modifiedVertices;
    public List<DeformableVertex> deformableVertices;
    public List<KeyValuePair<int, Vector3>> changingVertexIndices = new List<KeyValuePair<int, Vector3>>();
    public Dictionary<int, float> vertDistances = new Dictionary<int, float>();
    public Dictionary<int, Vector3> originalPos = new Dictionary<int, Vector3>();
    public Vector2[] meshUVs;
    public Transform[] touchPositions;
    public float distanceToTouch = 0.5f;
    private KeyValuePair<int, Vector3> defaultKey;
    private Mesh mesh;
    private Matrix4x4 scaledMatrix;
    private Texture2D texture;

    public void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        MeshRegenerated();
        defaultKey = default(KeyValuePair<int, Vector3>);
        depressionStep /= 100.0f;
        reversionStep /= 100.0f;
        foreach (Vector3 vertex in originalVertices)
        {
            deformableVertices.Add(new DeformableVertex(vertex, depressionStep, reversionStep, interiorObject, this.transform));
        }
    }

    public void Update()
    {
        if (!isColliding)
        {
            for (int i = 0; i < deformableVertices.Count; i++)
            {
                deformableVertices[i].RevertVertex();
            }
        }
        for (int i = 0; i < deformableVertices.Count; i++)
        {
            if (deformableVertices[i].currentPos != modifiedVertices[i]) {
                modifiedVertices.RemoveAt(i);
                modifiedVertices.Insert(i, deformableVertices[i].currentPos);
            }
        }
        mesh.SetVertices(modifiedVertices);
    }

    public void revertAllVertices()
    {
        isColliding = false;
    }

    public void MeshRegenerated()
    {
        //plane = GetComponent<GeneratePlaneMesh>();
        mesh.MarkDynamic();
        originalVertices = mesh.vertices.ToList();
        modifiedVertices = mesh.vertices.ToList();
        Debug.Log("Mesh Regenerated");
    }

    public void AddDepression(List<Vector3> depressionPoints, List<Vector3> depressionNormals, float radius)
    {
        isColliding = true;
        List<Vector3> depressionLocalPos = new List<Vector3>();
        //List<Vector3> depressionNormalPos = new List<Vector3>();
        for (int i = 0; i < depressionPoints.Count; i++)
        {
            Vector3 localPos = this.transform.InverseTransformPoint(depressionPoints[i]);
            if (touchPositions.Length != 0) {
                for (int j = 0; j < touchPositions.Length; j++) {
                    if ((localPos - touchPositions[j].localPosition).magnitude < distanceToTouch) {
                        depressionLocalPos.Add(localPos);
                        //depressionNormalPos.Add(depressionNormals[i]);
                    }
                }
            } else {
                depressionLocalPos.Add(localPos);
                //depressionNormalPos.Add(depressionNormals[i]);
            }
            
        }
        //Debug.Log(depressionLocalPos[0]);
        //Debug.Log(depressionPoints[0]);
        for (int i = 0; i < modifiedVertices.Count; i++)
        {
            bool isBeingTouched = false;
            bool shouldRevert = false;
            float minDistance = radius;
            Vector3 normal = Vector3.zero;
            float maxDepression = 0.0f;
            for (int j = 0; j < depressionLocalPos.Count; j++)
            {
                var distance = (depressionLocalPos[j] - modifiedVertices[i]).magnitude;
                if (distance < radius)
                {
                    isBeingTouched = true;
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        normal = depressionNormals[j];
                        maxDepression = maximumDepression * ((radius - distance) / radius);
                    }
                    //Debug.Log("Changing vert");
                    //newChangingVertices.Add(new KeyValuePair<int, Vector3>(i, normalPos));
                    //object[] parms = new object[3] { i, normalPos, 3.0f };
                    //StartCoroutine("compressVertex", parms);
                    //var newVert = originalVertices[i] + normalPos * maximumDepression;
                    //modifiedVertices.RemoveAt(i);
                    //modifiedVertices.Insert(i, newVert);
                } else if (distance > minDistance * 1.5f && !isBeingTouched)
                {
                    shouldRevert = true;
                }
            }
            if (isBeingTouched)
            {
                deformableVertices[i].maxDepression = maxDepression;
                deformableVertices[i].touchNormal = normal;
                deformableVertices[i].stillTouched = isBeingTouched;
                deformableVertices[i].DepressVertex();
                //texture.SetPixel((int)Mathf.Round(meshUVs[i].x * 1024), (int)Mathf.Round(meshUVs[i].y * 2048), Color.black);
            } else if (shouldRevert)
            {
                deformableVertices[i].RevertVertex();
            }
            
        }
        
        //changingVertexIndices = newChangingVertices;
        //compressVertices(changingVertexIndices, normalPos, .2f);
    }

    // public IEnumerator compressVertices()
    // {
        // while (true)
        // {
            // for (int i = 0; i < changingVertexIndices.Count; i++)
            // {
                // int index = changingVertexIndices[i].Key;
                // if ((modifiedVertices[index] - originalVertices[index]).magnitude < maximumDepression)
                // {
                    // var newVert = modifiedVertices[index] + changingVertexIndices[i].Value * (maximumDepression * depressionStep);
                    // modifiedVertices.RemoveAt(index);
                    // modifiedVertices.Insert(index, newVert);
                // }
                // if (vertDistances.ContainsKey(index))
                // {
                    // vertDistances.Remove(index);
                    // originalPos.Remove(index);
                // }
            // }
            // for (int i = 0; i < originalVertices.Count; i++)
            // {
                // KeyValuePair<int, Vector3> pair = changingVertexIndices.FirstOrDefault(x => x.Key == i);
                // if ((modifiedVertices[i] - originalVertices[i]).magnitude != 0 && pair.Equals(defaultKey))
                // {
                    // if (!vertDistances.ContainsKey(i))
                    // {
                        // vertDistances.Add(i, reversionStep);
                        // originalPos.Add(i, modifiedVertices[i]);
                    // }
                    // var newVert = Vector3.Lerp(originalPos[i], originalVertices[i], vertDistances[i]);
                    // modifiedVertices.RemoveAt(i);
                    // modifiedVertices.Insert(i, newVert);
                    // vertDistances[i] += reversionStep;
                // }
            // }
            // //changingVertexIndices.Clear();
            // mesh.SetVertices(modifiedVertices);
            // yield return null;
        // }
        
    // }
    // public IEnumerator revertVertices()
    // {
        // while (true)
        // {
            
            // mesh.SetVertices(modifiedVertices);
            // yield return null;
        // }
    // }
}