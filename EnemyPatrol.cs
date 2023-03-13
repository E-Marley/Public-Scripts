using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    //Patrolling
    [SerializeField] Transform[] patrolPoints;
    private NavMeshAgent agent;
    private int n;

    //Landing on enemies
    [SerializeField] Renderer[] originalMaterials;
    private Color originalColour;
    private bool flashing;
    [SerializeField] int hitsToDie;
    private int hits;
    private CapsuleCollider col;
    private bool canBeHurt = true;
    private SoundPlayer cameraSoundPlayer;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        col = GetComponentInChildren<CapsuleCollider>();   
    }


    private void Start()
    {
        agent.autoBraking = false;
        foreach (Renderer mat in originalMaterials)
        {
           originalColour = mat.material.color; 
        }
        cameraSoundPlayer = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SoundPlayer>();
    }

    private void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();

        if (flashing) //Enemy colour dims periodically if already injured by player, determined by LandedOnEnemy method
        {
            foreach (Renderer mat in originalMaterials)
            {
                mat.material.color = Color.Lerp(originalColour, Color.black, Mathf.PingPong(Time.time, 0.8f));
            }
        }

    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (patrolPoints.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = patrolPoints[n].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        n = (n + 1) % patrolPoints.Length;
    }

    public void LandedOnEnemy()
    {
        if (canBeHurt)
        {
            hits++;

            if (hits == hitsToDie)
            {
                DeathAnimation();
            }
            else
            { //Enemy colour dims periodically if already injured by player (method in Update)
                cameraSoundPlayer.PlaySound(2);
                flashing = true;
                col.gameObject.GetComponent<EnemyDamage>().WaitToDamageAgain();
                StartCoroutine(ProtectedFromDamage());
            }
        }
    }

    private void DeathAnimation()
    {
        cameraSoundPlayer.PlaySound(3);
        agent.isStopped = true;
        col.enabled = false;
        GetComponentInChildren<Animator>().SetBool("isDead", true);
        Destroy(gameObject, 2);
    }

    IEnumerator ProtectedFromDamage()
    {
        canBeHurt = false;
        yield return new WaitForSeconds(1.5f);
        canBeHurt = true;
    }
}
