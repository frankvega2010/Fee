﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public List<GameObject> enteredZones;
    public List<GameObject> enteredTurrets;

    public int bulletSpeed;
    public GameObject proyectileTemplate;
    public float fireRate;
    public float lifespan;
    public float exitDistance;
    public float distance = 0;
    public bool canShoot;
    public bool isPreview;
    public bool canBePlaced;
    public bool isInTurretZone;

    private Outline outline;
    private GameObject currentTarget;
    private float lifespanTimer;
    private float fireRateTimer;
    private Proyectile proyectile;
    public TurretRadius turretRadius;
    private Rigidbody rig;
    
    // Start is called before the first frame update
    void Start()
    {
        //turretRadius = transform.GetChild(0).gameObject.GetComponent<TurretRadius>();
        outline = GetComponent<Outline>();
        proyectile = proyectileTemplate.GetComponent<Proyectile>();
        proyectile.speed = bulletSpeed;
        turretRadius.onTurretDetectEnemy = SetTarget;
        turretRadius.onTurretLostEnemy = ChangeTarget;
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPreview)
        {
            if(currentTarget)
            {
                distance = Vector3.Distance(transform.position, currentTarget.transform.position);
            }
            else
            {
                distance = exitDistance;
            }

            if(distance >= exitDistance)
            {
                outline.OutlineColor = Color.white;
            }

            lifespanTimer += Time.deltaTime;

            if (lifespanTimer >= lifespan)
            {
                Destroy(gameObject);
            }

            if (!canShoot)
            {
                fireRateTimer += Time.deltaTime;

                if (fireRateTimer >= fireRate)
                {
                    fireRateTimer = 0;
                    canShoot = true;
                }
            }
            else
            {
                ShootTarget();
            }
        }
    }

    private void SetTarget(GameObject newTarget)
    {
        if (!isPreview)
        {
            outline.OutlineColor = Color.red;
            currentTarget = newTarget;
        }
            //Debug.Log("Seteando Target");
            
    }

    private void ShootTarget()
    {
        if (!isPreview)
        {
            if (currentTarget)
            {
                GameObject newProyectile = Instantiate(proyectileTemplate);
                Proyectile proy = newProyectile.GetComponent<Proyectile>();
                proy.startPosition = transform.position + transform.up * 7;
                proy.target = currentTarget;
                newProyectile.SetActive(true);
                canShoot = false;
            }
        }
        
    }

    private void ChangeTarget(GameObject newTarget)
    {
        if (!isPreview)
        {
            if (newTarget == currentTarget)
            {
                turretRadius.gameObject.SetActive(false);
                proyectile.target = null;
                turretRadius.gameObject.SetActive(true);
            }
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "planet")
        {
            rig.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "turret")
        {
            enteredTurrets.Add(other.gameObject);
            if (enteredTurrets.Count > 0)
            {
                canBePlaced = false;
            }

            Debug.Log("Torreta choca con " + other.gameObject.name);
            
        }

        if (other.gameObject.tag == "road")
        {
            Debug.Log("Torreta choca con " + other.gameObject.name);
            canBePlaced = false;
        }

        if (other.gameObject.tag == "turretZone")
        {
            Debug.Log("Torreta choca con " + other.gameObject.name);
            enteredZones.Add(other.gameObject);
            if(enteredZones.Count > 0)
            {
                isInTurretZone = true;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "turret")
        {
            enteredTurrets.Remove(other.gameObject);
            if (enteredTurrets.Count <= 0)
            {
                canBePlaced = true;
            }

            Debug.Log("Torreta DEJO de chocar con " + other.gameObject.name);
        }

        if (other.gameObject.tag == "road")
        {
            Debug.Log("Torreta DEJO de chocar con " + other.gameObject.name);
            canBePlaced = true;
        }

        if (other.gameObject.tag == "turretZone")
        {
            Debug.Log("Torreta choca con " + other.gameObject.name);
            enteredZones.Remove(other.gameObject);
            if (enteredZones.Count <= 0)
            {
                isInTurretZone = false;
            }
        }
    }

    public void TurnOffOutline()
    {
        outline.enabled = false;
    }
}
