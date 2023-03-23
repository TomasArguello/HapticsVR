using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustPatientHeight : MonoBehaviour
{
    private bool downPressed;
    private bool upPressed;
    public Transform PatientAndChair;
    public float moveSpeed;
    public float minY;
    public float maxY; 
    private float curY;
    private bool hologramToggled = false;
    public GameObject regularPatient;
    public GameObject hologramPatient;
    // Start is called before the first frame update
    void Start()
    {
        downPressed = false;
        upPressed = false;
        curY = PatientAndChair.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown("s")) {
            downPressed = true;
        } else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp("s")) {
            downPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown("w")) {
            upPressed = true;
        } else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp("w")) {
            upPressed = false;
        }

        if (Input.GetKeyDown("h")) {
            ToggleHologram();
        }

        if (downPressed && curY >= minY) {
            curY -= moveSpeed;
        }
        if (upPressed && curY <= maxY) {
            curY += moveSpeed;
        }

        PatientAndChair.localPosition = new Vector3(PatientAndChair.localPosition.x, curY, PatientAndChair.localPosition.z);
    }

    void ToggleHologram() {
        if (hologramToggled) {
            regularPatient.SetActive(true);
            hologramPatient.SetActive(false);
            hologramToggled = false;
        } else {
            regularPatient.SetActive(false);
            hologramPatient.SetActive(true);
            hologramToggled = true;
        }
    }

}
