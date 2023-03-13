using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MovingEntity {

	enum state {
		Wander,
		Chase,
		Dead}

	state currentState;
	Vector3 targetPosition;
	Vector3 towardsTarget;
	float wanderRadius = 5f;
	Transform currentTarget;
	float maxChaseDistance = 7f;
	float playAreaLimit = 13f;
	[SerializeField] ParticleSystem puff;
	private bool particleOn = false;
	float chaseSpeedMultiplier = 2.5f;
	Animator wolfAnimator;


	void Awake()
	{
		wolfAnimator = GetComponent<Animator> ();
	}

	void Start ()
	{
		RecalculateTargetPosition ();
		StartCoroutine (FiniteStateMachine ());
	}


	void RecalculateTargetPosition ()
	{
		targetPosition = transform.position + Random.insideUnitSphere * wanderRadius;
		targetPosition.y = 0;
	}

	IEnumerator FiniteStateMachine()
	{
		while (true) 
		{
			yield return StartCoroutine (currentState.ToString ());
		}
	}

	void ChangeState(state nextState)
	{
		//Debug.Log(currentState + "->" + nextState);
		currentState = nextState;
	}

	IEnumerator Wander ()
	{
		wolfAnimator.SetInteger ("wolfState", 0);
		while (currentState == state.Wander)
		{
			Vector3 towardsTarget = targetPosition - transform.position;
			MoveTowards(towardsTarget.normalized);

			if (towardsTarget.z >= playAreaLimit || 
			    towardsTarget.z <= -playAreaLimit ||
			    towardsTarget.x >= playAreaLimit || 
			    towardsTarget.x <= -playAreaLimit)
			{
				RecalculateTargetPosition ();
			}

			if (towardsTarget.magnitude < 0.25f)
			{
				RecalculateTargetPosition ();
			}
			
			//Debug.DrawLine(transform.position,targetPosition,Color.green);
		yield return 0;
		}

	}

	IEnumerator Chase() 
	{
		wolfAnimator.SetInteger ("wolfState", 1);
		movementSpeed *= chaseSpeedMultiplier;
		while(currentState==state.Chase)
			{
			Vector3 towardsTarget = currentTarget.position - transform.position;
			MoveTowards(towardsTarget);
			//Debug.DrawLine(transform.position,targetPosition,Color.red);
			
			if(towardsTarget.magnitude>maxChaseDistance)
			{
				ChangeState(state.Wander);
			}
			yield return 0;
			}
		movementSpeed /= 2;
	}
	
	IEnumerator Dead()
	{
		wolfAnimator.SetInteger ("wolfState", 2);
		yield return 0;
		yield return 2;
        	
		if (particleOn)
        	{
			puff.Emit(50);
			particleOn = false;
		}
		transform.localScale = new Vector3(0, 0, 0);

	}

	public void OnDeadHandler()
	{
		GetComponent<Collider>().enabled = false;
		ChangeState(state.Dead);
		particleOn = true;
	}

	void OnTriggerEnter (Collider other)
		{
		if (other.CompareTag("OuterCircle"))
		{
			RecalculateTargetPosition();
		}
		if (other.CompareTag ("Circle"))
		{
			currentTarget = other.transform;
			OnDeadHandler();
		}
		if (other.CompareTag ("Player"))
		{
			currentTarget = other.transform;
			ChangeState (state.Chase);
		}
	}

}