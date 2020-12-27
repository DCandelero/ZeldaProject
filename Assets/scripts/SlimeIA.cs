using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeIA : MonoBehaviour
{
    private GameManager _GameManager;

    private Animator anim;
    public ParticleSystem hitEffect;
    public int HP;
    private bool isDead;
    private bool isWalk;
    private bool isAlert;
    private bool isAttack;
    private bool isPlayerVisible;
    public enemyState state;

    public const float idleWaitTime = 3f;

    private NavMeshAgent agent;
    private int idWayPoint;
    private Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        StateManager();

        slimeState();
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {

            isPlayerVisible = true;

            if(state == enemyState.IDLE || state == enemyState.PATROL) {
                ChangeState(enemyState.ALERT);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player") {
            
            isPlayerVisible = false;

        }
    }



    #region personal methods

    void StateManager() {
        switch(state) {

            case enemyState.ALERT:

                destination = transform.position;
                agent.destination = destination;
                agent.stoppingDistance = 0;

                isAlert = true;

                break;
            case enemyState.FOLLOW:

                if(isPlayerVisible) {
                    destination = _GameManager.player.position;
                    agent.destination = destination;
                    if(agent.remainingDistance <= agent.stoppingDistance) {
                        Attack();
                    }
                } else {
                    ChangeState(enemyState.ALERT);
                }
                
                break;
            case enemyState.FURY:

                destination = _GameManager.player.position;
                agent.destination = destination;
                isAlert = true;
                if(agent.remainingDistance <= agent.stoppingDistance) {
                        Attack();
                }

                break;
        }
    }

    void slimeState() {
        if(agent.desiredVelocity.magnitude >= 0.1f) {
            isWalk = true;
        } else {
            isWalk = false;
        }
        anim.SetBool("isWalk", isWalk);
        anim.SetBool("isAlert", isAlert);
    }

    void ChangeState(enemyState newState) {
        StopAllCoroutines(); // Encerra todas corrotinas desse slime
        state = newState;
        isAlert = false;
        print(state);
        switch(state) {
            case enemyState.IDLE:

                destination = transform.position;
                agent.destination = destination;
                agent.stoppingDistance = 0;

                StartCoroutine("IDLE");

                break;
            case enemyState.ALERT:

                destination = transform.position;
                agent.destination = destination;
                agent.stoppingDistance = 0;

                StartCoroutine("ALERT");

                break;
            case enemyState.FOLLOW:

                destination = _GameManager.player.position;
                agent.stoppingDistance = _GameManager.slimeDistanceToAttack;
                agent.destination = destination;

                break;
            case enemyState.FURY:

                destination = _GameManager.player.position;
                agent.stoppingDistance = _GameManager.slimeDistanceToAttack;
                agent.destination = destination;
                agent.speed *= 2;

                isAlert = true;

                break;
            case enemyState.PATROL:

                idWayPoint = Random.Range(0, _GameManager.slimesWayPoints.Length);
                destination = _GameManager.slimesWayPoints[idWayPoint].position;
                agent.destination = destination;
                agent.stoppingDistance = 0;

                StartCoroutine("PATROL");

                break;
        }
    }

    void Attack() {
        if(!isAttack) {
            isAttack = true;
            anim.SetTrigger("Attack");
        }
    
    }

    void AttackIsDone() {
        StartCoroutine("AttackCoroutine");
    }

    void GetHit(int amount) {

        if(isDead) { return; }

        HP -= amount;

        if(HP > 0) {
            ChangeState(enemyState.FURY);
            anim.SetTrigger("GetHit");
            hitEffect.Emit(3);
        } else {
            StopAllCoroutines();

            anim.SetTrigger("Die");
            hitEffect.Emit(10);
            StartCoroutine("Died");
        }
    }

    IEnumerator Died() {
        isDead = true;
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }

    IEnumerator AttackCoroutine() {
        yield return new WaitForSeconds(_GameManager.slimeAttackDelay);
        isAttack = false;
    }

    IEnumerator IDLE() {
        yield return new WaitForSeconds(_GameManager.slimeIdleWaitTime);

        IdleOrPatrol(50);
    }

    IEnumerator PATROL() {
        yield return new WaitUntil( () => agent.remainingDistance <= 0 );

        IdleOrPatrol(25);
    }

    IEnumerator ALERT() {
        yield return new WaitForSeconds(_GameManager.slimeAlertTime);

        if(isPlayerVisible) {
            ChangeState(enemyState.FOLLOW);
        } else {
            IdleOrPatrol(0);
        }
    }

    void IdleOrPatrol(int idlePercentageChance) {
        if(Rand() < idlePercentageChance) {
            ChangeState(enemyState.IDLE);
        } else {
            ChangeState(enemyState.PATROL);
        }
    }

    int Rand() {
        int rand = Random.Range(0, 100);
        return rand;
    }

    #endregion
}
