using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunscript : MonoBehaviour
{
    public float damage = 50f;

    public Camera fpscam;

    public int maxammo = 10;

    private int currentAmmo;

    public float reloadTime = 30f;

    private bool isReloding = false;    
    public ParticleSystem mazlleflush;

    public Animator animator;
    void Start()
    {
        currentAmmo = maxammo;
    }

    private void OnEnable()
    {
        isReloding = false;
        animator.SetBool("realoding", false);
    }

    void Update()
    {
        if (isReloding)
            return;

        if(currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            shoot();
        }

    }

    IEnumerator Reload()
    {
        isReloding = true;
        Debug.Log("reloading");

        animator.SetBool("realoding", true);

        yield return new WaitForSeconds(reloadTime -2.5f);

        animator.SetBool("realoding", false);
        yield return new WaitForSeconds(1f);

        currentAmmo = maxammo;
        isReloding = false;



    }

    void shoot()
    {
        RaycastHit hit;
        mazlleflush.Play();

        currentAmmo--;
        if (Physics.Raycast(fpscam.transform.position, fpscam.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.TakeDamege(20);
            }            
        }
    }
}
