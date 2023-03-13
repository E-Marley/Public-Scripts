using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesMovingForward : MonoBehaviour
{
    [SerializeField] float averageSpeed;
    float speedRange = 1;
    float randomSpeedVariation;
    
    void Start()
    {
        randomSpeedVariation = Random.Range(-speedRange, speedRange);
    }

    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * (averageSpeed + randomSpeedVariation));
    }
}
