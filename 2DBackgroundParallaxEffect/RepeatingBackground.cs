using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour
{
    private Transform playerTransform;
    private float textureUnitSizeX;
    private Vector3 initialPos;

    private void OnEnable()
    {
        GameEvents.OnGameStart += ResetPosition;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= ResetPosition;
    }
    private void Start()
    {
        initialPos = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;

    }

    //Update runs once per frame
    private void LateUpdate()
    {
        if (Mathf.Abs(playerTransform.position.x - transform.position.x) >= textureUnitSizeX && GameEvents.playerFlightBegun)
        {
            float offsetPositionX = (playerTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(playerTransform.position.x + offsetPositionX, transform.position.y);
        }
    }

    private void ResetPosition()
    {
        transform.position = initialPos;
    }
}