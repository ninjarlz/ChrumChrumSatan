﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Piglet : MonoBehaviour
{
    [FormerlySerializedAs("vel")] [SerializeField] private float _vel = 3;
    [SerializeField] private Animator animator;

    
    //[SerializeField] private float angular = 30;

    void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity =
            new Vector3(Random.Range(-1f, 1f), 0,Random.Range(-1f, 1f)).normalized * _vel;
        if (Mathf.Abs(gameObject.GetComponent<Rigidbody>().velocity.x) >
            Mathf.Abs(gameObject.GetComponent<Rigidbody>().velocity.z))
        {
            if (gameObject.GetComponent<Rigidbody>().velocity.x > 0)
                animator.SetBool("Right", true);
            else
                animator.SetBool("Left", true);
        }
        else
        {
            if(gameObject.GetComponent<Rigidbody>().velocity.z > 0)
                animator.SetBool("Up", true);
            else
                animator.SetBool("Down", true);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall") || collision.gameObject.CompareTag("piglet"))
        {
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
            
            gameObject.GetComponent<Rigidbody>().velocity =
                new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * _vel;
            //gameObject.GetComponent<Rigidbody2D>().angularVelocity = angular;
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);
            if (Mathf.Abs(gameObject.GetComponent<Rigidbody>().velocity.x) >
                Mathf.Abs(gameObject.GetComponent<Rigidbody>().velocity.z))
            {
                if (gameObject.GetComponent<Rigidbody>().velocity.x > 0)
                    animator.SetBool("Right", true);
                else
                    animator.SetBool("Left", true);
            }
            else
            {
                if(gameObject.GetComponent<Rigidbody>().velocity.z > 0)
                    animator.SetBool("Up", true);
                else
                    animator.SetBool("Down", true);
            }
        }
        
    }

//    private void OnCollisionExit2D(Collision2D collision)
//    {
//        gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0f;
//    }

    void Update()
    {
        if (Satan.active)
            _vel = 10;
        else
            _vel = 3;
    }

    public void die()
    {
        Destroy(gameObject);
    }
}