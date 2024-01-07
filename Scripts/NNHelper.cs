using UnityEngine;

public class NNHelper
{
	public static void DrawNN(Vector3 position, NeuronNetwork neuronNetwork)
	{
		Gizmos.color = Color.green;

		for (int layer = 0; layer < neuronNetwork.neurons.Count; layer++)
		{
			for (int neuron = 0; neuron < neuronNetwork.neurons[layer].Count; neuron++)
			{
				Gizmos.DrawSphere(position + new Vector3(layer * 5, neuron * 5, 0),
					Mathf.Clamp(neuronNetwork.neurons[layer][neuron] / 10, 0.1f, 3));
			}
		}

		for (int layer = 0; layer < neuronNetwork.weights.Count; layer++)
		{
			for (int neuron = 0; neuron < neuronNetwork.weights[layer].Count; neuron++)
			{
				for (int link = 0; link < neuronNetwork.weights[layer][neuron].Count; link++)
				{
					Gizmos.color = new Color(neuronNetwork.weights[layer][neuron][link], 0, 0);

					Gizmos.DrawLine(position + new Vector3((layer + 1) * 5, neuron * 5, 0),
						position + new Vector3((layer) * 5, link * 5, 0));
				}
			}
		}

		for (int layer = 0; layer < neuronNetwork.biasWeights.Count; layer++)
		{
			for (int neuron = 0; neuron < neuronNetwork.biasWeights[layer].Count; neuron++)
			{
				Gizmos.color = new Color(neuronNetwork.biasWeights[layer][neuron], 0, 0);

				Gizmos.DrawLine(position + new Vector3(layer * 5, neuronNetwork.neurons[layer].Count * 5, 0),
					position + new Vector3((layer + 1) * 5, neuron * 5, 0));
			}
		}

		Gizmos.color = Color.blue;

		for (int layer = 0; layer < neuronNetwork.biases.Count; layer++)
		{
			Gizmos.DrawSphere(position + new Vector3(layer * 5, neuronNetwork.neurons[layer].Count * 5, 0),
				Mathf.Clamp(neuronNetwork.biases[layer], 0.1f, 3));
		}
	}
}