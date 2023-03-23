using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PhysicsDeformer : MonoBehaviour
{

    public float collisionRadius = 0.1f;
    public DeformableMesh deformableMesh;
    private Haptics hapticFeedback;
    public float duration, frequency, amplitude;

    // Use this for initialization
    void Start()
    {
        hapticFeedback = FindObjectOfType<Haptics>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionStay(Collision collision)
    {
        List<Vector3> collisionPoints = new List<Vector3>();
        List<Vector3> collisionNormals = new List<Vector3>();
        foreach (var contact in collision.contacts)
        {
            collisionPoints.Add(contact.point);
            collisionNormals.Add(contact.normal);
        }
        deformableMesh.AddDepression(collisionPoints, collisionNormals, collisionRadius);
        if (hapticFeedback)
        {
            SteamVR_Input_Sources source = collision.gameObject.GetComponentInParent<SteamVR_Behaviour_Pose>().inputSource;
            hapticFeedback.Pulse(duration, frequency, amplitude, source);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        deformableMesh.revertAllVertices();
    }
}