using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouselook : MonoBehaviour
{
    public float mouseSensetivity = 250f;

    float xRotation = 0f;
    public Transform playerBody;
    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensetivity * Time.deltaTime; 
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensetivity * Time.deltaTime;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // we do not want to camera to flip so we use clamp
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);// we didn't rotate it like the mouse x rotation because we need to use clamp

        playerBody.Rotate(Vector3.up * mouseX );
    }
}
