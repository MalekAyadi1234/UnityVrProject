
using UnityEngine;
using UnityEngine.AI;



namespace scgFullBodyController
{
    public class PassengerFollow : MonoBehaviour
    {

        NavMeshAgent agent;

        public AiGunController AiGun;

        Transform player;

        public LayerMask groundLayer, playerLayer;

        public float health;

        //Patroling
        public Vector3 walkPoint;
        bool walkPointSet;
        public float walkPointRange;
        [HideInInspector] public bool moving;

        //States
        public float sightRange, attackRange;
        public bool playerInSightRange, playerInAttackRange;
        public bool overrideAttack;

        //Weapon orienting
        public Transform spine;
        public float m_ForwardAmount;
        float m_TurnAmount;
        Animator m_Animator;

        //Increase this in correspondence with the navmesh agent speed and acceleration to match the animation with it
        public float moveDamping;


        Vector3 lastPos;
        public Transform shootPoint;

        private void Awake()
        {
            //Find our player, the navmesh agent and the animator
            player = GameObject.FindWithTag("hitCollider").transform;
            agent = GetComponent<NavMeshAgent>();
            m_Animator = GetComponent<Animator>();
        }


        ///////////////////////////////////////////////
        private void Update()
        {
            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

            //If we have not been attacked
            if (!overrideAttack)
            {
                if (!playerInSightRange && !playerInAttackRange) Patroling();
                if (playerInSightRange && !playerInAttackRange) Patroling();
                if (playerInAttackRange && playerInSightRange)
                    //AttackPlayerFixed();
                    ChasePlayer();
               // else if (AiGun.firing)
                  //  AiGun.FireCancel();
            }
            else
            {
                //Immeditaley attack player if attacked
                if (!playerInAttackRange && player != null)
                    ChasePlayer();

                if (playerInAttackRange)
                    overrideAttack = false;
            }

            //Update the turn animation to our y axis rotation
            m_TurnAmount = transform.rotation.y;

            //Check for movement and play the walking animation
            if (transform.position != lastPos)
            {
                m_ForwardAmount = 1 * moveDamping;
                moving = true;
            }
            else
            {
                m_ForwardAmount = 0;
                moving = false;
            }

            lastPos = transform.position;

            //Update our animator constantly
            UpdateAnimator();
        }

        private void Patroling()
        {
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
                agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 1f)
                walkPointSet = false;
        }

        private void SearchWalkPoint()
        {
            //Calculate random point in range
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
                walkPointSet = true;
        }
        public void ChasePlayer()
        {
            agent.SetDestination(player.position);
        }

     
   
        

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightRange);
        }

        public void UpdateAnimator()
        {
            //Update the blend trees
            m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
            m_Animator.SetFloat("Turn", m_TurnAmount * 0.3f, 0.1f, Time.deltaTime);
        }
    }
}