﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEntity : MonoBehaviour {

	[SerializeField] protected float movementSpeed = 1f;
	[SerializeField] protected  float rotationSpeed = 1f;
	
	protected void MoveTowards (Vector3 direction)
	{
		Quaternion towardsTargetRotation = Quaternion.LookRotation(direction, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, towardsTargetRotation, rotationSpeed * Time.deltaTime);
		transform.position += transform.forward * movementSpeed * Time.deltaTime;

	}
}