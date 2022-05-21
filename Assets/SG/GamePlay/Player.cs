using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum PlayerType
{
    obb = 0, //Color.red,
    ibb = 1 //Color.green,
}
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [Header("Set in inspector")]
    [SerializeField]
    private KeyCode jumpKey = KeyCode.None;
    [SerializeField]
    private KeyCode leftShiftKey = KeyCode.None;
    [SerializeField]
    private KeyCode rightShiftKey = KeyCode.None;

    [SerializeField]
    protected float gravity = -9.8f; //TODO: ask protected or private?
    [SerializeField]
    protected float moveSpeed = 0.5f;

    private Rigidbody rb;
    private Animator animator;
    private Collider coll;
    private bool isJumping = false;
    private Vector3 vectorVelocity;
    public PlayerType type;
    public float jumpForce = 5f;
    public int floorLayer;
    public int playerLayer;
    void Awake()
    {

    }

    void Start()
    {
        floorLayer = LayerMask.NameToLayer("Floor");
        playerLayer = LayerMask.NameToLayer("Player");
        coll = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        vectorVelocity = Vector3.zero;
        int run = 0;
        if (Input.GetKey(leftShiftKey))
        {
            Debug.Log("Getting velocity left");
            Translate(Vector3.left);
            run = 1;

        }
        else if (Input.GetKey(rightShiftKey))
        {
            Debug.Log("Getting velocity right");
            Translate(Vector3.right);
            run = 1;
        }

        animator.SetInteger("Run", run);

        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            Jump();
        }

        //set is jumping to false
    }

    private void FixedUpdate()
    {
        rb.AddForce(gravity * Vector3.up);
    }

    void OnCollisionEnter(Collision coll)
    {
        Debug.Log($"coll wiht {coll.gameObject.name}, layer = {coll.gameObject.layer}  and {floorLayer}");
        if (coll.gameObject.layer == floorLayer || coll.gameObject.layer == playerLayer) //TOOD: use bit ?? how ?
        {
            if (isJumping)
            {
                isJumping = false;
            }

        }
    }
    private void Jump()
    {
        isJumping = true;
        animator.Play("Jump");
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * jumpForce * -gravity, ForceMode.Impulse);

    }

    private void Translate(Vector3 direction)
    {
        //if (Time.realtimeSinceStartup - timeRead > (yMov < 0 ? fallTime / 10 : fallTime))
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.transform.Translate(direction * moveSpeed * Time.deltaTime, 0);
    }

    public void ChangeGravity()
    {
        gravity = -gravity;
    }
}
