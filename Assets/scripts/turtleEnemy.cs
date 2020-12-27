using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turtleEnemy : MonoBehaviour
{

    void GetHit(int amount) {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
}
