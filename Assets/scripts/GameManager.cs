using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyState {
    IDLE, ALERT, PATROL, FOLLOW, FURY
}
public class GameManager : MonoBehaviour
{
    public Transform player;

    [Header("Slime IA")]
    public float slimeIdleWaitTime;
    public float slimeDistanceToAttack = 2.5f;
    public float slimeAlertTime = 3f;
    public float slimeAttackDelay = 1f;
    public Transform[] slimesWayPoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
