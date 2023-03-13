using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerPhysics : MonoBehaviour
{
    [SerializeField] protected float forceAppliedInX;
    [SerializeField] protected float forceAppliedInY;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameEvents.PlayerCollidedWithObstacle(forceAppliedInX, forceAppliedInY);
        }
    }
}
