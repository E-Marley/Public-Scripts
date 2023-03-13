using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    [SerializeField] private bool infiniteHorizontal;
    [SerializeField] private bool infiniteVertical;

    private Transform playerTransform;
    private Vector3 lastPlayerPosition;
    private float textureUnitSizeX;
    private float textureUnitSizeY;
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
        lastPlayerPosition = playerTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {

        if (GameEvents.playerFlightBegun)
        {
            Vector3 deltaMovement = playerTransform.position - lastPlayerPosition;
            transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
            lastPlayerPosition = playerTransform.position;

            if (infiniteHorizontal)
            {
                if (Mathf.Abs(playerTransform.position.x - transform.position.x) >= textureUnitSizeX)
                {
                    float offsetPositionX = (playerTransform.position.x - transform.position.x) % textureUnitSizeX;
                    transform.position = new Vector3(playerTransform.position.x + offsetPositionX, transform.position.y);
                }
            }

            if (infiniteVertical)
            {
                if (Mathf.Abs(playerTransform.position.y - transform.position.y) >= textureUnitSizeY)
                {
                    float offsetPositionY = (playerTransform.position.y - transform.position.y) % textureUnitSizeY;
                    transform.position = new Vector3(transform.position.x, playerTransform.position.y + offsetPositionY);
                }
            }
        }
    }

    private void ResetPosition()
    {
        transform.position = initialPos;
        lastPlayerPosition = playerTransform.position;
    }

}
