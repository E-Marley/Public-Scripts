using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStop : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float gradualSpeedLossRate;
    [SerializeField] float groundBounceForce;
    [SerializeField] float groundSpeedLoss;
    private float bigNumber = 100000;
    private float necessaryTime = 1f;
    float elapsed;


    private void OnEnable()
    {
        GameEvents.OnPlayerObstacleCollide += ForceAppliedByObstacle;
        GameEvents.OnGameStart += ResetPhysics;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerObstacleCollide += ForceAppliedByObstacle;
        GameEvents.OnGameStart -= ResetPhysics;
    }

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        if (GameEvents.playerFlightBegun)
        {
            rb.velocity += Vector2.left * gradualSpeedLossRate * Time.fixedDeltaTime;
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, 0, bigNumber), rb.velocity.y);
        }
    }

    private void ForceAppliedByObstacle(float forceAppliedInX, float forceAppliedInY)
    {
        rb.AddForce(new Vector2(forceAppliedInX, forceAppliedInY), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GameEvents.PlaySound(2);


            if (rb.velocity.x > 0)
            {
                rb.AddForce(new Vector2(-groundSpeedLoss, groundBounceForce), ForceMode2D.Impulse);
            }

            if(rb.velocity.magnitude < 0.5f)
            {
                GameEvents.PlaySound(4);
                GameEvents.PlayerStopped();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            elapsed += Time.fixedDeltaTime;
            if (elapsed > necessaryTime)
            {
                GameEvents.PlaySound(4);
                GameEvents.PlayerStopped();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            elapsed = 0;
        }

    }

    private void ResetPhysics()
    {
        elapsed = 0;
    }
}
