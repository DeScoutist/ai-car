using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NeuronNetwork
{
	// layer.neuron
	// [neuron0.0, neuron0.1], [neuron1.0, neuron1.1]
	public List<List<float>> neurons = new List<List<float>>();

	// layer.neuronTo.neuronFrom (layer starts from 1, inputs does not have weights)
	// [[weight1.0.0, weight1.0.1], [weight1.1.0, weight1.1.1]], [[weight2.0.0, weight2.0.1], [weight2.1.0, weight2.1.1]]
	public List<List<List<float>>> weights = new List<List<List<float>>>();

	// layer (layer starts from 1, inputs does not have biases)
	// bias1, bias2
	public List<float> biases = new List<float>();

	// layer.neuron (layer starts from 1, inputs does not have weights)
	// bias1.0, bias1.1
	public List<List<float>> biasWeights = new List<List<float>>();

	public NeuronNetwork(int inputLayers, int[] hiddenLayers, int outputLayers)
	{
		// Input layers
		neurons.Add(new List<float>());

		for (int neuron = 0; neuron < inputLayers; neuron++)
		{
			neurons[0].Add(0);
		}

		// Hidden layers
		for (int layer = 0; layer < hiddenLayers.Length; layer++)
		{
			neurons.Add(new List<float>());
			weights.Add(new List<List<float>>());
			biases.Add(0);
			biasWeights.Add(new List<float>());

			for (int neuron = 0; neuron < hiddenLayers[layer]; neuron++)
			{
				neurons[layer + 1].Add(0);
				weights[layer].Add(new List<float>());
				biasWeights[layer].Add(0);

				for (int link = 0; link < neurons[layer].Count; link++)
				{
					weights[layer][neuron].Add(0);
				}
			}
		}

		// Output layers
		neurons.Add(new List<float>());
		weights.Add(new List<List<float>>());
		biases.Add(0);
		biasWeights.Add(new List<float>());

		for (int neuron = 0; neuron < outputLayers; neuron++)
		{
			int layer = neurons.Count - 2;

			neurons.Last().Add(0);
			weights[layer].Add(new List<float>());
			biasWeights[layer].Add(0);

			for (int link = 0; link < neurons[layer].Count; link++)
			{
				weights[layer][neuron].Add(0);
			}
		}
	}

	public NeuronNetwork(NeuronNetwork parent, float mutateChance = .05f)
	{
		for (int layer = 0; layer < parent.neurons.Count; layer++)
		{
			neurons.Add(new List<float>());

			for (int neuron = 0; neuron < parent.neurons[layer].Count; neuron++)
			{
				neurons[layer].Add(parent.neurons[layer][neuron]);
			}
		}

		for (int layer = 0; layer < parent.weights.Count; layer++)
		{
			weights.Add(new List<List<float>>());

			for (int neuron = 0; neuron < parent.weights[layer].Count; neuron++)
			{
				weights[layer].Add(new List<float>());

				for (int link = 0; link < parent.weights[layer][neuron].Count; link++)
				{
					weights[layer][neuron].Add(parent.weights[layer][neuron][link]);
				}
			}
		}

		for (int layer = 0; layer < parent.biases.Count; layer++)
		{
			biases.Add(parent.biases[layer]);
		}

		for (int layer = 0; layer < parent.biasWeights.Count; layer++)
		{
			biasWeights.Add(new List<float>());

			for (int neuron = 0; neuron < parent.biasWeights[layer].Count; neuron++)
			{
				biasWeights[layer].Add(parent.biasWeights[layer][neuron]);
			}
		}

		if (mutateChance > 0)
		{
			Mutate();
		}
	}

	public void Mutate(float chance = .05f)
	{
		for (int layer = 0; layer < neurons.Count; layer++)
		{
			for (int neuron = 0; neuron < neurons[layer].Count; neuron++)
			{
				if (Random.value <= chance)
				{
					neurons[layer][neuron] = RandomValue();
				}
			}
		}

		for (int layer = 0; layer < weights.Count; layer++)
		{
			for (int neuron = 0; neuron < weights[layer].Count; neuron++)
			{
				for (int link = 0; link < weights[layer][neuron].Count; link++)
				{
					if (Random.value <= chance)
					{
						weights[layer][neuron][link] = RandomValue();
					}
				}
			}
		}

		for (int layer = 0; layer < biases.Count; layer++)
		{
			if (Random.value <= chance)
			{
				biases[layer] = RandomValue();
			}
		}

		for (int layer = 0; layer < biasWeights.Count; layer++)
		{
			for (int neuron = 0; neuron < biasWeights[layer].Count; neuron++)
			{
				if (Random.value <= chance)
				{
					biasWeights[layer][neuron] = RandomValue();
				}
			}
		}
	}

	public float RandomValue()
	{
		return Random.Range(-1f, 1f);
	}

	public void Randomise()
	{
		for (int layer = 0; layer < neurons.Count; layer++)
		{
			for (int neuron = 0; neuron < neurons[layer].Count; neuron++)
			{
				neurons[layer][neuron] = RandomValue();
			}
		}

		for (int layer = 0; layer < weights.Count; layer++)
		{
			biases[layer] = RandomValue();

			for (int neuron = 0; neuron < weights[layer].Count; neuron++)
			{
				biasWeights[layer][neuron] = RandomValue();

				for (int link = 0; link < weights[layer][neuron].Count; link++)
				{
					weights[layer][neuron][link] = RandomValue();
				}
			}
		}
	}

	public void Calculate(float[] input)
	{
		// Set input data
		for (int neuron = 0; neuron < neurons[0].Count; neuron++)
		{
			neurons[0][neuron] = input[neuron];
		}

		// Calculate
		for (int layer = 1; layer < neurons.Count; layer++)
		{
			for (int neuron = 0; neuron < neurons[layer].Count; neuron++)
			{
				neurons[layer][neuron] = 0;

				for (int link = 0; link < weights[layer - 1][neuron].Count; link++)
				{
					neurons[layer][neuron] += neurons[layer - 1][link] * weights[layer - 1][neuron][link];
				}

				neurons[layer][neuron] += biases[layer - 1] * biasWeights[layer - 1][neuron];

				neurons[layer][neuron] = Mathf.Clamp(neurons[layer][neuron], -1f, 1f);
			}
		}
	}
}