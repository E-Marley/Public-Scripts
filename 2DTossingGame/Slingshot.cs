using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [SerializeField] LineRenderer[] lineRenderers; 
    [SerializeField] Transform[] stripPositions;
    [SerializeField] Transform center;
    [SerializeField] Transform idlePosition;
    [SerializeField] Vector3 currentPosition;
    [SerializeField] float maxLength;
    [SerializeField] float bottomBoundary;
    bool isMouseDown = false;

    [SerializeField] GameObject player;
    Rigidbody2D playerRB;
    Collider2D playerCollider;
    [SerializeField] float playerPositionOffset;
    [SerializeField] float force;

    private void OnEnable()
    {
        GameEvents.OnGameStart += ResetPosition;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= ResetPosition;
    }

   private void Awake()
    {
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;
        lineRenderers[0].SetPosition(0, stripPositions[0].position);
        lineRenderers[1].SetPosition(0, stripPositions[1].position);

        playerRB = player.GetComponent<Rigidbody2D>();
        playerCollider = player.GetComponent<Collider2D>();


        ResetPosition();
    }


    private void FixedUpdate()
    {
        if (isMouseDown && !GameEvents.playerFlightBegun)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;

            currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            currentPosition = center.position + Vector3.ClampMagnitude(currentPosition
                - center.position, maxLength);

            currentPosition = ClampBoundary(currentPosition);

            SetStrips(currentPosition);
        }
        else
        {
            ResetStrips();
        }
    }

    private void OnMouseDown()
    {
        isMouseDown = true;
    }

    private void OnMouseUp()
    {
        isMouseDown = false;
        Shoot();
        currentPosition = idlePosition.position;
    }
    private void ResetPosition()
    {
        ResetStrips();

        playerCollider.enabled = false;

        playerRB.isKinematic = true;

    }

    private void Shoot()
    {
        playerRB.isKinematic = false;
        Vector3 shootForce = (currentPosition - center.position) * force * -1;
        shootForce.z = 0;
        playerRB.velocity = shootForce;
        playerCollider.enabled = true;
        GameEvents.PlayerStarted();
        GameEvents.playerFlightBegun = true;
    }

    private void ResetStrips()
    {
        currentPosition = idlePosition.position;
        SetStrips(currentPosition);
    }

    private void SetStrips(Vector3 position)
    {
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);

        if (!GameEvents.playerFlightBegun)
        {
            Vector3 dir = position - center.position;
            player.transform.position = position + dir.normalized * playerPositionOffset;
            player.transform.right = -dir.normalized;
        }
    }

    private Vector3 ClampBoundary(Vector3 vector)
    {
        vector.y = Mathf.Clamp(vector.y, bottomBoundary, 1000);
        return vector;
    }
}
