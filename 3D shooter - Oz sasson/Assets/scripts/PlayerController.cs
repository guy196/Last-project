using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] GameObject cameraholder;
	[SerializeField] float mouseSensetivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

	float verticalLookRtotation;
	bool grounded;
	Vector3 smoothMoveVelocity;
	Vector3 moveAmount;

	PhotonView PV;

	Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		PV = GetComponent<PhotonView>();
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Start()
	{
		if (!PV.IsMine)
		{
			Destroy(GetComponentInChildren<Camera>().gameObject);
		}
	}
	private void Update()
	{
		if (!PV.IsMine)
			return;
		Look();
		Move();
		Jump();

	}

	void Move()
	{
		Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized; //normalized prevent from moving faster if we holding 2 keys at once
		moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
	}
	void Jump()
	{
		if (Input.GetKeyDown(KeyCode.Space) && grounded)
		{
			rb.AddForce(transform.up * jumpForce);
		}
	}
	void Look()
	{
		transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensetivity);

		//The Horizontal and Vertical ranges change from 0 to +1 or -1 with increase/decrease in 0.05f steps. GetAxisRaw has changes from 0 to 1 or -1 immediately, so with no steps.
		// Mouse x it is the y rotation and not the x rotation

		verticalLookRtotation += Input.GetAxis("Mouse Y") * mouseSensetivity;

		verticalLookRtotation = Mathf.Clamp(verticalLookRtotation, -90f, 90f);

		cameraholder.transform.localEulerAngles = Vector3.left * verticalLookRtotation;
	}

	public void SetGroundedState(bool _grounded)
	{
		grounded = _grounded;
	}

	private void FixedUpdate()
	{
		rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
	}

}
