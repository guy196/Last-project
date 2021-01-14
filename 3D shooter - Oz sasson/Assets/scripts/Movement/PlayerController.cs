using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class PlayerController : MonoBehaviourPunCallbacks, Idamageable
{
	[SerializeField] GameObject cameraholder;


	[SerializeField] float mouseSensetivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

	[SerializeField] Item[] items;

	[SerializeField] GameObject playercontroller;

	[SerializeField] GameObject cam;

	[SerializeField] TMP_Text textHealthBar;

	//public TMP_Text Walltimer;

	public bool wallTime = true;

	public float SecondsLeft = 10f;

	public int distance = 4;

	int itemIndex;
	int previousItemIndex = -1;

	float verticalLookRtotation;
	bool grounded;
	Vector3 smoothMoveVelocity;
	Vector3 moveAmount;

	PhotonView PV;

	Rigidbody rb;

	const float maxhealth = 100f;

	float currentHealth = maxhealth;

	PlayerManager playerManager;

	public GameObject wall;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		PV = GetComponent<PhotonView>();
		Cursor.lockState = CursorLockMode.Locked;

		playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>(); //finds the player manager script	
	}

	private void Start()
	{
		textHealthBar = RefManager.Instance.healthTextRef;

		if (PV.IsMine)
		{
			EquipItem(0);
		}
		else
		{
			Destroy(GetComponentInChildren<Camera>().gameObject);
			Destroy(rb);
		}
	}
	private void Update()
	{
		if (wallTime == true)
		{

		}

		if (!PV.IsMine)
			return;
		Look();
		Move();
		Jump();
		Wall();
		for (int i = 0; i < items.Length; i++)
		{
			if(Input.GetKeyDown((i + 1).ToString())) //check the nums at the key(0,1,2,3,4,5,6) for example if i = 0 so it checks if we press one
			{
				EquipItem(i);
				break;//if we pressed one it is set active the 0 in the arrays 
			}

			if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
			{
				if (itemIndex >= items.Length - 1)
				{
					EquipItem(0);
				}
				else
				{
					EquipItem(itemIndex + 1);
				}
			}
			else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
			{
				if (itemIndex <= 0)
				{
					EquipItem(items.Length - 1);
				}
				else
				{
					EquipItem(itemIndex - 1);
				}
			}
		}
		if (Input.GetMouseButtonDown(0))
		{
			items[itemIndex].Use();
		}
		textHealthBar.text = currentHealth.ToString();
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

	void EquipItem(int _index)
	{
		if (_index == previousItemIndex)
			return;

		itemIndex = _index;

		items[itemIndex].itemGameobject.SetActive(true);

		if (previousItemIndex != -1)
		{
			items[previousItemIndex].itemGameobject.SetActive(false);
		}

		previousItemIndex = itemIndex;

		if (PV.IsMine)
		{
			Hashtable hash = new Hashtable();
			hash.Add("itemIndex", itemIndex);
			PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
		}
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) //When there is a change and the information about this change has recived
	{
		if(!PV.IsMine && targetPlayer == PV.Owner) // we do not want to sycnic the informatiom for ourself so we checking if pv is not mine
		{
			EquipItem((int)changedProps["itemIndex"]); //we passing the index to the other players
		}
	}

	public void TakeDamagee(float damage)
	{
		PV.RPC("RPC_TakeDamage", RpcTarget.All, damage); //runs on the shooter's computer
	}

	[PunRPC]
	void RPC_TakeDamage(float damage) //runs only on the victon computer
	{
		if (!PV.IsMine)
			return;
		Debug.Log("took damage:" + damage);

		currentHealth -= damage;

		if(currentHealth <= 0)
		{
			Die();
		}
	}
	void Die()
	{
		playerManager.Die();
	}
	void Wall()
	{
		if (Input.GetKeyDown(KeyCode.E) && wallTime == true)
		{
			Debug.Log("test");
			Instantiate(wall, cam.transform.position + transform.forward * distance, cam.transform.rotation);
			wallTime = false;
			StartCoroutine(TimerTake());
		}
	}

	public IEnumerator TimerTake()
	{
		yield return new WaitForSeconds(3);
		wallTime = true;
	}

}
