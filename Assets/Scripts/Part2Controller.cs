using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;

public class Part2Controller : MonoBehaviour
{
    public GameObject boxAContents;
    public GameObject boxBContents;
    public GameObject boxCContents;
    public GameObject boxA;
    public GameObject boxB;
    public GameObject boxC;
    public GameObject[] covers;
    public Mesh cube;
    public Mesh cylinder;
    public Mesh sphere;
    private int currentStep = 0;
    private Vector3 normalSize = new Vector3(0.008574f, 0.009164f, 0.009228f);
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting on size test. A = Large, B = Medium, C = Small");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("d")) {
            moveToNextStep();
        }
    }

    void moveToNextStep() {
        if (currentStep == 0) {
            Debug.Log("Switching to Shape Test. A = Cube, B = Sphere, C = Cylinder");
            boxAContents.transform.localScale = normalSize;
            boxBContents.transform.localScale = normalSize;
            boxCContents.transform.localScale = new Vector3(normalSize.x, 0.003942f, normalSize.z);
            boxAContents.GetComponent<MeshFilter>().mesh = cube;
            boxBContents.GetComponent<MeshFilter>().mesh = sphere;
            boxCContents.GetComponent<MeshFilter>().mesh = cylinder;
            boxAContents.GetComponentInChildren<MeshCollider>().sharedMesh = cube;
            boxCContents.GetComponentInChildren<MeshCollider>().sharedMesh = cylinder;
            boxA.GetComponent<Outline>().OutlineColor = Color.blue;
            boxB.GetComponent<Outline>().OutlineColor = Color.blue;
            boxC.GetComponent<Outline>().OutlineColor = Color.blue;
            currentStep++;
        } else if(currentStep == 1) {
            Debug.Log("Switching to Hardness Test. A = Hard, B = Soft, C = Medium");
            boxCContents.transform.localScale = normalSize;
            boxAContents.GetComponent<MeshFilter>().mesh = sphere;
            boxCContents.GetComponent<MeshFilter>().mesh = sphere;
            boxAContents.GetComponentInChildren<SG_Material>().maxForce = 100;
            boxBContents.GetComponentInChildren<SG_Material>().maxForce = 15;
            boxCContents.GetComponentInChildren<SG_Material>().maxForce = 60;
            boxAContents.GetComponentInChildren<MeshCollider>().sharedMesh = sphere;
            boxBContents.GetComponentInChildren<MeshCollider>().sharedMesh = sphere;
            boxCContents.GetComponentInChildren<MeshCollider>().sharedMesh = sphere;
            boxA.GetComponent<Outline>().OutlineColor = Color.green;
            boxB.GetComponent<Outline>().OutlineColor = Color.green;
            boxC.GetComponent<Outline>().OutlineColor = Color.green;
            boxA.GetComponent<MeshRenderer>().enabled = false;
            boxB.GetComponent<MeshRenderer>().enabled = false;
            boxC.GetComponent<MeshRenderer>().enabled = false;
            foreach(GameObject cover in covers) {
                cover.GetComponent<MeshRenderer>().enabled = false;
            }
            currentStep++;
        } else if (currentStep == 2) {
            Debug.Log("Switching to Size Test. A = Large, B = Medium, C = Small");
            boxAContents.transform.localScale = normalSize;
            boxBContents.transform.localScale = new Vector3(0.004743f, 0.005234f, 0.005544f);
            boxCContents.transform.localScale = new Vector3(0.002542f, 0.002676f, 0.002690f);
            boxAContents.GetComponentInChildren<SG_Material>().maxForce = 100;
            boxBContents.GetComponentInChildren<SG_Material>().maxForce = 100;
            boxCContents.GetComponentInChildren<SG_Material>().maxForce = 100;
            boxA.GetComponent<Outline>().OutlineColor = Color.red;
            boxB.GetComponent<Outline>().OutlineColor = Color.red;
            boxC.GetComponent<Outline>().OutlineColor = Color.red;
            boxA.GetComponent<MeshRenderer>().enabled = true;
            boxB.GetComponent<MeshRenderer>().enabled = true;
            boxC.GetComponent<MeshRenderer>().enabled = true;
            foreach(GameObject cover in covers) {
                cover.GetComponent<MeshRenderer>().enabled = true;
            }
            currentStep = 0;
        }
    }
}
