using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformableVertex : MonoBehaviour
{
    private Vector3 originalPos;
    public Vector3 currentPos;
    public bool stillTouched;
    public Vector3 touchNormal;
    private Vector3 posAtRevertStart;
    public float maxDepression;
    private float depressionRate;
    private float reversionRate;
    private float distanceToOriginal;
    private float distanceToInteriorObject;
    // Start is called before the first frame update
    public DeformableVertex(Vector3 originalPos, float depressionRate, float reversionRate, GameObject interiorObject, Transform mainObjectTransform)
    {
        this.originalPos = originalPos;
        this.depressionRate = depressionRate;
        this.reversionRate = reversionRate;
        currentPos = originalPos;
        Vector4 worldPos4 = mainObjectTransform.localToWorldMatrix * originalPos;
        //Vector3 worldPos = new Vector3(worldPos4.x, worldPos4.y, worldPos4.z) + mainObjectTransform.position;
        Vector3 worldPos = originalPos + mainObjectTransform.position;
        if (interiorObject.GetComponent<SkinnedMeshRenderer>()) {
            distanceToInteriorObject = (worldPos - interiorObject.transform.position).magnitude - interiorObject.GetComponent<SkinnedMeshRenderer>().bounds.extents.magnitude + 0.048f;
        }
        distanceToInteriorObject = 3.0f;
        //Debug.Log(distanceToInteriorObject);
    }

    void Start()
    {
        stillTouched = false;
    }

    public void SetTouched(bool touched)
    {
        distanceToOriginal = 0.0f;
        stillTouched = touched;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DepressVertex()
    {
        distanceToOriginal = 0.0f;
        float distance = (originalPos - currentPos).magnitude;
        
        if (distance <= maxDepression && distance <= distanceToInteriorObject)
        {
            float depressionDistance = (maxDepression < distanceToInteriorObject) ? maxDepression : distanceToInteriorObject;
            currentPos += touchNormal * (depressionDistance * depressionRate);
        }
    }

    public void RevertVertex()
    {
        if (originalPos == currentPos)
        {
            return;
        }
        if (distanceToOriginal == 0.0f)
        {
            posAtRevertStart = currentPos;
        }
        currentPos = Vector3.Lerp(posAtRevertStart, originalPos, distanceToOriginal);
        distanceToOriginal += reversionRate;
    }
}
