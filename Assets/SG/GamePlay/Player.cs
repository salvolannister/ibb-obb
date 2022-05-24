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

public static class PlayerSettings
{
    public const float Y_VELOCITY_LIMIT = 5f;
    public const float VEL_DECELERATION_FACTOR = 0.3f;
    public const float FORCE_DECELERATION_FACTOR = 0.2f;
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

    private float gravityChangeDelay = 0.5f; // 0.05f;
    public PlayerType playerType;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private Animator animator;
    private bool isJumping = false;
    private Coroutine co = null;
    private bool reversed = false;
    private Vector3 lastDir = Vector3.zero;
    private int floorLayer;
    private int playerLayer;
    private int enemyLayer;
    private int portalLayer;
    private bool maxVelocitySet;
    private float maxYSpeed;
    public void ReverseGravity()
    {
        co = StartCoroutine(_ReverseGravity());

    }

    private IEnumerator _ReverseGravity()
    {
        reversed = !reversed;
        yield return new WaitForSeconds(gravityChangeDelay);
        gravity *= -1;
        co = null;
    }

    private void RotatePlayer()
    {
        int gravityDir = reversed ? 1 : -1;
        if (lastDir == Vector3.right)
        { // Moving Right
            transform.rotation = Quaternion.LookRotation(
                (reversed) ? Vector3.back : Vector3.forward, -Vector3.up * gravityDir);
        }
        else if (lastDir == Vector3.left)
        { // Moving Left
            transform.rotation = Quaternion.LookRotation(
                (reversed) ? Vector3.forward : Vector3.back, -Vector3.up * gravityDir);
        }
    }


    void Awake()
    {

    }

    void Start()
    {
        co = null;
        floorLayer = LayerMask.NameToLayer("Floor");
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        portalLayer = LayerMask.NameToLayer("Portal");

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        int run = 0;
        if (Input.GetKey(leftShiftKey))
        {
            Translate(Vector3.left);
            run = 1;
        }
        else if (Input.GetKey(rightShiftKey))
        {
            Translate(Vector3.right);
            run = 1;
        }

        animator.SetInteger("Run", run);

        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            Jump();
        }

        //HandleMaxSpeed();

    }


    void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.layer == portalLayer)
        {
            HandleMaxYSpeed();
        }

    }

    private void HandleMaxYSpeed()
    {
        if (Math.Abs(rb.velocity.y) > PlayerSettings.Y_VELOCITY_LIMIT)
        {
            rb.velocity *= PlayerSettings.VEL_DECELERATION_FACTOR;
            rb.AddForce(Vector3.up * -gravity * PlayerSettings.FORCE_DECELERATION_FACTOR, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(gravity * Vector3.up, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision coll)
    {
        int goLayer = coll.gameObject.layer;
        if (goLayer == floorLayer || goLayer == playerLayer || goLayer == enemyLayer) //TOOD: use bit ?? how ?
        {
            if (isJumping)
            {
                isJumping = false;
            }

        }
    }

    void OnTriggerEnter(Collider other)
    {
        int goLayer = other.gameObject.layer;
        Debug.Log($"collider wiht {other.gameObject.name}, layer = {other.gameObject.layer}  and enemy layer {enemyLayer}");
        if (goLayer == enemyLayer)
        {
            InteractWithEnemy(other.gameObject);
        }
    }



    private void InteractWithEnemy(GameObject enemyGO)
    {
        Debug.Log($"Interact with enemy tag: {enemyGO.tag}");
        Enemy enemy = enemyGO.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            switch (enemyGO.tag)
            {
                case EnemyParts.Spirit:
                    {
                        enemy.Die();
                        break;
                    }
                case EnemyParts.Crawler:
                    {
                        GameManager.GameOver(this.tag);
                        break;
                    }


            }

        }
        else
        {
            Debug.LogError($" Enemy {enemyGO.transform.parent.name} is missing the enemy script");
        }
    }

    public void Die(Action callback = null)
    {
        // Make Explosion Effect
        // once finished
        if (co != null)
            StopCoroutine(co);
        callback?.Invoke();
        gameObject.SetActive(false);

    }

    private void Jump()
    {
        isJumping = true;
        animator.Play("Jump");
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * jumpForce * -gravity, ForceMode.Impulse);

    }

    public void Respawn(Transform trs)
    {
        transform.position = trs.position;
        gameObject.SetActive(true);

        if ((trs.position.y < 0))
        {
            reversed = true;
            if (gravity < 0)
                gravity *= -1;

        }
        else
        {
            reversed = false;
            if (gravity > 0)
                gravity *= -1;
        }

    }
    private void Translate(Vector3 direction)
    {
        //if (Time.realtimeSinceStartup - timeRead > (yMov < 0 ? fallTime / 10 : fallTime))
        lastDir = direction;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.transform.Translate(direction * moveSpeed * Time.deltaTime, 0);
    }

    void OnAnimatorMove()
    {
        RotatePlayer();
    }

}
