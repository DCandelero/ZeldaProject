using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamicCam : MonoBehaviour
{

    public GameObject vCam2;
    public GameObject vCam3;

    private void OnTriggerEnter(Collider other) {
        switch (other.gameObject.tag) {
            case "CamTrigger":
                vCam2.SetActive(true);
                break;
            case "CamTrigger2":
                vCam3.SetActive(true);
                break;
        }
    }

    private void OnTriggerExit(Collider other) {
        switch (other.gameObject.tag) {
            case "CamTrigger":
                vCam2.SetActive(false);
                break;
            case "CamTrigger2":
                vCam3.SetActive(false);
                break;
        }
    }
}
