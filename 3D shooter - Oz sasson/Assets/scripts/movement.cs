using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 9f;

    public Transform groundCheck;

    public float groundDistance = 0.4f;

    public LayerMask groundMask;    

    public float gravity = -5f;

    bool isGrounded;

    Vector3 velocity;

    public float jumpHight = 1f;

    public float mouseSensetivity = 250f;

    float xRotation = 0f;
    public Transform playerBody;

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        Cursor.lockState = CursorLockMode.Locked;

    }
    public void Update()
    {

        if (!PV.IsMine)
            return;
        Move();
        look();
    }
    void Move()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
    void look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensetivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensetivity * Time.deltaTime;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // we do not want to camera to flip so we use clamp
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);// we didn't rotate it like the mouse x rotation because we need to use clamp

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
