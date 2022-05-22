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
    private bool isJumping = false;
    private Coroutine co = null;
    private bool reversed = false;
    private Vector3 lastDir = Vector3.zero;
    public float gravityChangeDelay = 0.05f;
    public PlayerType playerType;
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
                (reversed) ? new Vector3(0, 0, -1.0f) : new Vector3(0, 0, 1.0f), -Vector3.up * gravityDir);
        }
        else if (lastDir == Vector3.left)
        { // Moving Left
            transform.rotation = Quaternion.LookRotation(
                (reversed) ? new Vector3(0, 0, 1.0f) : new Vector3(0, 0, -1.0f), -Vector3.up * gravityDir);
        }
    }

    public float jumpForce = 5f;
    public int floorLayer;
    public int playerLayer;
    public int enemyLayer;
    void Awake()
    {

    }

    void Start()
    {
        co = null;
        floorLayer = LayerMask.NameToLayer("Floor");
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");

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

    }

    private void FixedUpdate()
    {
        rb.AddForce(gravity * Vector3.up);
    }

    void OnCollisionEnter(Collision coll)
    {
        Debug.Log($"collision wiht {coll.gameObject.name}, layer = {coll.gameObject.layer}  and {floorLayer}");
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
                        Debug.Log(" enemy was spirit");
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
        float gravityDelay = gravityChangeDelay;
        gravityChangeDelay = 0;

        if ((trs.position.y < 0 && !reversed) || (trs.position.y > 0 && reversed))
        {
            ReverseGravity();
        }

        gravityChangeDelay = gravityDelay;
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

    public void ChangeGravity()
    {
        gravity = -gravity;
    }
}
