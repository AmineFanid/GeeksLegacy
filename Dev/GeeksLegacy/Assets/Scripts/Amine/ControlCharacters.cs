using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class ControlCharacters : MonoBehaviour
{

    private Animator AnimateurRobin;
    private Rigidbody2D Rigidbody;

    private float ControleY;

    public void Start() {

        AnimateurRobin = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        ControleY = Input.GetAxis("Vertical");

    }
}
