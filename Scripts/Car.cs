using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Car : MonoBehaviour
{
	public const float MaxDistance = 20;

	public float speed = 0;
	public float rotate = 0;

	private Rigidbody _rigidbody;

	public NeuronNetwork neuronNetwork;

	public float fitness;

	public bool isDied = false;

	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();

		if (neuronNetwork == null)
		{
			neuronNetwork = new NeuronNetwork(4, new[] {4, 3}, 2);

			neuronNetwork.Randomise();
		}
	}

	void FixedUpdate()
	{
		if (!isDied)
		{
			neuronNetwork.Calculate(new[]
			{
				GetDistanceToObstacle(Vector3.left),
				GetDistanceToObstacle(Vector3.right),
				GetDistanceToObstacle(Vector3.forward),
				speed
			});

			speed = neuronNetwork.neurons.Last()[0] * 100;
			rotate = neuronNetwork.neurons.Last()[1] * 3;

			_rigidbody.AddForce(transform.forward * speed);
			transform.Rotate(transform.up, rotate);

			Vector3 velocity = _rigidbody.velocity;

			velocity.x = Mathf.Clamp(velocity.x, -10, 20);
			velocity.y = Mathf.Clamp(velocity.y, -10, 20);
			velocity.z = Mathf.Clamp(velocity.z, -10, 20);

			_rigidbody.velocity = velocity;

			fitness = transform.position.z;
		}
	}

	public float GetDistanceToObstacle(Vector3 vector, bool drawRay = true)
	{
		RaycastHit hit;

		if (Physics.Raycast(transform.position, transform.TransformDirection(vector), out hit))
		{
			if (drawRay)
			{
				Debug.DrawRay(transform.position, transform.TransformDirection(vector) * hit.distance, Color.blue);
			}

			return hit.distance;
		}

		return MaxDistance;
	}

	private void OnDrawGizmosSelected()
	{
		if (Application.isPlaying)
		{
			NNHelper.DrawNN(transform.position + (Vector3.up * 10), neuronNetwork);
		}
	}

	public void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Obstacle")
		{
			isDied = true;
		}
	}
}