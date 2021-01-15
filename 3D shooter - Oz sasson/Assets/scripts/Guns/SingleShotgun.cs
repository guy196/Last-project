using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotgun : Gun
{
	[SerializeField] Camera cam;
	[SerializeField] GameObject particle;
	public override void Use()
	{
		shoot();
	}

	private void Start()
	{
		particle = RefManager.Instance.particlesystem;
	}
	void shoot()
	{
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f)); //shoot at the middle of the screen
		ray.origin = cam.transform.position;
		if(Physics.Raycast(ray, out RaycastHit hit))
		{
			hit.collider.gameObject.GetComponent<Idamageable>()?.TakeDamagee(((GunInfo)ItemInfo).damage);

			Vector3 position = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, hit.collider.transform.position.z);
			// we are looking for the callback idmahable and for the function function Take damage, if we found it we are taking the damage from the singleton that built in every weapon
			GameObject particleInstantiate = Instantiate(particle, position, Quaternion.identity);
			new WaitForSeconds(2);
			Destroy(particleInstantiate);
		}
	}
}
