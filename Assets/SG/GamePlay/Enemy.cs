using UnityEngine;


public static class EnemyParts
{
    public const string Spirit = "Spirit";
    public const string Crawler = "Crawler";
}

public class Enemy : MonoBehaviour
{
    /*
    Characters can kill an enemy
    Characters can be killed by enemy
    The enemies move around
    */

    [Header(" Set in inspector")]
    public float velocity;
    [SerializeField]
    private bool isStatic = false;
    public Transform[] checkPoints;
    public bool IsAlive
    {
        get { return m_IsAlive; }
    }
    private bool boxTaken = false;
    private Vector3 originalPos;
    private bool m_IsAlive;
    private Transform m_trs;
    private Vector3 destination;
    private int enemyLayer;
    private const float DIST_OFFSET = 0.002f;
    void OnEnable()
    {
        m_IsAlive = true;
        m_trs = transform;
        if (!isStatic)
        {
            destination = checkPoints[0].position;

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        originalPos = m_trs.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsAlive && !isStatic)
            Patrol();
    }

    private void Patrol()
    {
        float distance = Vector3.Distance(m_trs.position, destination);
        if (distance<= DIST_OFFSET)
        {
            destination = UpdateDestination();
        }
        Debug.Log($" Distance is {distance}");
        m_trs.position = Vector3.Lerp(m_trs.position, destination, 0.1f *Time.deltaTime * velocity);
    }


    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(destination, 0.3f);
    }
    private Vector3 UpdateDestination()
    {
        return destination == checkPoints[0].position ? checkPoints[1].position : checkPoints[0].position;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == enemyLayer && !isStatic)
        {
            UpdateDestination();
        }
    }

    internal void Die()
    {
        Debug.Log(" Enemy died");
        m_IsAlive = false;
        GameManager.AddDeadEnemy(this);
        gameObject.SetActive(false);
        // Die effect
        // spawn box 

    }

    internal void Respawn()
    {
        transform.position = originalPos;
        m_IsAlive = true;
    }
}
