using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotgun : Gun
{
	[SerializeField] Camera cam;
	public override void Use()
	{
		shoot();
	}


	void shoot()
	{
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f)); //shoot at the middle of the screen
		ray.origin = cam.transform.position;
		if(Physics.Raycast(ray, out RaycastHit hit))
		{
			hit.collider.gameObject.GetComponent<Idamageable>()?.TakeDamagee(((GunInfo)ItemInfo).damage); // we are looking for the function Take damage, if we found it we are taking the damage from the singleton that built in every weapon
		}
	}
}
