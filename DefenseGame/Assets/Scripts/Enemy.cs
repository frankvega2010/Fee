﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void OnEnemyAction(GameObject enemy);
    public static OnEnemyAction OnEnemyClicked;

    public float speed;
    public GameObject target;

    private Vector3 destination;
    private float distanceToStop;
    private Rigidbody rig;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        destination = new Vector3(target.transform.position.x,transform.position.y, target.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > distanceToStop)
        {
            transform.LookAt(destination);
            rig.AddForce(transform.forward * speed, ForceMode.Force);
        }
    }

    private void OnMouseDown()
    {
        if(OnEnemyClicked != null)
        {
            OnEnemyClicked(gameObject);
        }
       //Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            Debug.Log("Collision Test");
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
