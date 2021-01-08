using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] GameObject cameraholder;

	[SerializeField] float mouseSensetivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

	[SerializeField] Item[] items;

	int itemIndex;
	int previousIndex = -1;

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
		if (PV.IsMine)
		{
			Equiped(0);
		}
		else
			Destroy(GetComponentInChildren<Camera>().gameObject);
			Destroy(rb);
	}
	private void Update()
	{
		if (!PV.IsMine)
			return;
		Look();
		Move();
		Jump();

		for (int i = 0; i < items.Length; i++)
		{
			if(Input.GetKeyDown((i + 1).ToString())) //check the nums at the key(0,1,2,3,4,5,6) for example if i = 0 so it checks if we press one
			{
				Equiped(i);
				break;//if we pressed one it is set active the 0 in the arrays 
			}
		}
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
		if (!PV.IsMine)
			return;
		rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
	}

	void Equiped(int _index)
	{
		if (_index == previousIndex)
			return;
			_index = itemIndex;
		items[itemIndex].itemGameobject.SetActive(true); //sets the item we equiped to the number at the array we want for example Equiped(0)
		if (previousIndex != -1) //caled every time we swutch weapons and turn off the last weapon we used exept the first time because previousIndex == 0
		{
			items[previousIndex].itemGameobject.SetActive(false);
		}

		previousIndex = itemIndex;
	}

}
