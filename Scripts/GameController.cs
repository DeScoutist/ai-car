using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public int population = 20;
	public int best = 5;
	public float mutateChance = .1f;
	public int generation = 0;

	public GameObject trackPrefab;
	public Car carPrefab;

	public List<Car> cars = new List<Car>();

	public List<NeuronNetwork> history = new List<NeuronNetwork>();

	public float time = 0;

	void Start()
	{
		Generate();
	}

	void FixedUpdate()
	{
		if (time >= 10)
		{
			Generate();

			time = 0;
		}

		time += Time.fixedDeltaTime;
	}

	void Generate()
	{
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}

		List<NeuronNetwork> bestNetworks = new List<NeuronNetwork>();

		if (cars.Count > 0)
		{
			cars.Sort((a, b) => b.fitness.CompareTo(a.fitness));

			Debug.Log(cars[0].fitness);
			using (StreamWriter w = File.AppendText("fitness" + ".log"))
			{
				w.WriteLine(cars[0].fitness);
			}

			history.Add(new NeuronNetwork(cars[0].neuronNetwork, 0));

			for (int i = 0; i < best; i++)
			{
				bestNetworks.Add(new NeuronNetwork(cars[i].neuronNetwork, 0));
			}
		}

		int width = 100;

		cars.Clear();

		for (int i = 0; i < population; i++)
		{
			GameObject track = Instantiate(trackPrefab, transform);

			track.transform.position = new Vector3(width * i, 0, 0);

			Car car = Instantiate(carPrefab, transform);

			car.transform.position = new Vector3(width * i, 2, 0);

			if (i < bestNetworks.Count)
			{
				car.neuronNetwork = new NeuronNetwork(bestNetworks[i], 0);
			}

			if (i >= bestNetworks.Count && i < bestNetworks.Count * 2)
			{
				car.neuronNetwork = new NeuronNetwork(bestNetworks[i - bestNetworks.Count], mutateChance);
			}

			if (i >= bestNetworks.Count * 2 && i < bestNetworks.Count * 3)
			{
				car.neuronNetwork = new NeuronNetwork(bestNetworks[i - bestNetworks.Count * 2], mutateChance * 5);
			}

			cars.Add(car);
		}

		generation++;
	}


	private void OnDrawGizmosSelected()
	{
		if (Application.isPlaying)
		{
			for (int i = 0; i < history.Count; i++)
			{
				NNHelper.DrawNN(transform.position + (Vector3.up * 100) + (Vector3.right * (30 * i)), history[i]);
			}
		}
	}
}