using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static bool playerFlightBegun;

    public delegate void startNewGame();
    public static event startNewGame OnGameStart;

    public delegate void playerUsesBoost();
    public static event playerUsesBoost OnPlayerUseBoost;

    public delegate void playerBoostAction(float boostAmount);
    public static event playerBoostAction OnPlayerBoost;

    public delegate void playerStartAction();
    public static event playerStartAction OnPlayerStart;

    public delegate void playerStopAction();
    public static event playerStopAction OnPlayerStop;

    public delegate void soundAction(int soundIndex);
    public static event soundAction OnPlaySound;

    public delegate void playerObstacleCollisionAction(float forceAppliedInX, float forceAppliedInY);
    public static event playerObstacleCollisionAction OnPlayerObstacleCollide;


    public static void PlayerStarted()
    {
        if (OnPlayerStart != null)
        {
            OnPlayerStart();
        }
    }

    public static void PlayerUsesBoost()
    {
        if (OnPlayerUseBoost != null)
        {
            OnPlayerUseBoost();
        }
    }

    public static void PlayerBoosted(float boostAmount)
    {
        if (OnPlayerBoost != null)
        {
            OnPlayerBoost(boostAmount);
        }
    }

    public static void PlayerStopped()
    {
        if (OnPlayerStop != null)
        {
            OnPlayerStop();
        }
    }

    public static void GameStarted()
    {
        if (OnGameStart != null)
        {
            OnGameStart();
        }
    }

    public static void PlaySound(int soundIndex)
    {
        if (OnPlaySound != null)
        {
            OnPlaySound(soundIndex);
        }
    }

    public static void PlayerCollidedWithObstacle(float forceAppliedInX, float forceAppliedInY)
    {
        if (OnPlayerObstacleCollide != null)
        {
            OnPlayerObstacleCollide(forceAppliedInX, forceAppliedInY);
        }
    }


}
