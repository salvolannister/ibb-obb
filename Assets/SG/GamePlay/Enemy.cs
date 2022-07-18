using UnityEngine;

namespace Assets.SG.GamePlay
{
    public static class EnemyParts
    {
        public const string Spirit = "Spirit";
        public const string Crawler = "Crawler";
    }

    public class Enemy : MonoBehaviour
    {

        [Header(" Set in inspector")]
        public float velocity;
        [SerializeField]
        private bool isStatic = false;
        public Transform[] checkPoints;
        public bool IsAlive
        {
            get { return isAlive; }
        }
        private bool isAlive;
        private Vector3 originalPos;
        private Vector3 destination;
        private Transform trs;
        private int enemyLayer;
        private const float DIST_OFFSET = 0.002f;
        void OnEnable()
        {
            isAlive = true;
            if (!isStatic)
            {
                destination = checkPoints[0].position;

            }
        }
        // Start is called before the first frame update
        void Start()
        {
            trs = transform;
            originalPos = trs.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (isAlive && !isStatic)
                Patrol();
        }

        private void Patrol()
        {
            float distance = Vector3.Distance(trs.position, destination);
            if (distance <= DIST_OFFSET)
            {
                destination = UpdateDestination();
            }
            trs.position = Vector3.Lerp(trs.position, destination, 0.1f * Time.deltaTime * velocity);
        }

        private Vector3 UpdateDestination()
        {
            return destination == checkPoints[0].position ? checkPoints[1].position : checkPoints[0].position;
        }

        void OnTriggerEnter(Collider coll)
        {
            if (!isStatic)
            {
                destination = UpdateDestination();
            }
        }

        internal void Die()
        {
            isAlive = false;
            GameManager.AddDeadEnemy(this);
            gameObject.SetActive(false);

        }

        internal void Respawn()
        {
            transform.position = originalPos;
            isAlive = true;
        }
    }
}