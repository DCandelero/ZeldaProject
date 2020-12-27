using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;

    [Header("Config Player")]
    public float movementSpeed = 3f;
    private Vector3 direction;
    private bool isWalk;

    // INPUT
    private float horizontal;
    private float vertical;

    [Header("Attack config")]
    public ParticleSystem fxAttack;
    public Transform hitBox;
    [Range(0.2f, 1f)]
    public float hitRange = 0.5f;   
    private bool isAttack;
    public LayerMask hitMask;
    public Collider[] hitInfo;
    public int amountDmg;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();

        MoveCharacter();

        UpdateAnimator();
    }

    #region methods

        void Inputs() {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            if(Input.GetButtonDown("Fire1") && !isAttack) { // Get button options from Fire1 (already configured in unity)
                Attack();
            } 

            if(Input.GetButtonDown("Fire2")) {
                Defense();
            }
        }

        void Attack() {
            isAttack = true;
            anim.SetTrigger("Attack");
            fxAttack.Emit(1);

            hitInfo = Physics.OverlapSphere(hitBox.position, hitRange, hitMask);

            foreach(Collider objCollided in hitInfo) {
                objCollided.gameObject.SendMessage("GetHit", amountDmg, SendMessageOptions.DontRequireReceiver);
            }
        }

        void Defense() {
            anim.SetTrigger("Defend");
        }

        void MoveCharacter() {
            direction = new Vector3(horizontal, 0f, vertical).normalized;

            // Player rotation
            if(direction.magnitude > 0.1f) {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                transform.rotation  = Quaternion.Euler(0, targetAngle, 0);
                isWalk = true;
            } else {
                isWalk = false;
            }

            controller.Move(direction * movementSpeed * Time.deltaTime); // Time.deltaTime is used for fill gaps of frame rate on differents machines   
        }

        void UpdateAnimator() {
            anim.SetBool("isWalk", isWalk);
        }

        void AttackIsDone() {
            isAttack = false;
        }

    #endregion

    private void OnDrawGizmosSelected() {
        if(hitBox != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitBox.position, hitRange);
        }
    }

}

