﻿// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// /** 
// *	neural network
// *	input = Mathf.Sin(Main.time * octaves[i])
// */
// public class NN
// {
// 	public static readonly float octavesMin = 1f;
// 	public static readonly float octavesMax = 5f;
//
// 	public readonly int width;
// 	public readonly int layers = 4;
// 	public float[,] neurons;
// 	public float[,] bias;
// 	public float[,,] weights;
// 	public float[] octaves;
//
// 	// init with random weights and biasis
// 	public NN(int width)
// 	{
// 		this.width = width;
// 		neurons = new float[layers, width];
// 		bias = new float[layers - 1, width];
// 		weights = new float[layers, width, width];
// 		octaves = new float[width];
// 		for (int i = 0; i < layers - 1; i++)
// 		{
// 			for (int j = 0; j < width; j++)
// 			{
// 				bias[i, j] = Random.Range(-1f, 1f);
// 			}
// 		}
//
// 		for (int i = 0; i < layers; i++)
// 		{
// 			for (int j = 0; j < width; j++)
// 			{
// 				for (int k = 0; k < width; k++)
// 				{
// 					weights[i, j, k] = Random.Range(-1f, 1f);
// 				}
// 			}
// 		}
//
// 		for (int i = 0; i < width; i++)
// 		{
// 			octaves[i] = Random.Range(octavesMin, octavesMax);
// 		}
// 	}
//
// 	// init by copying
// 	public NN(NN a)
// 	{
// 		width = a.width;
// 		neurons = new float[layers, width];
// 		bias = new float[layers - 1, width];
// 		weights = new float[layers, width, width];
// 		octaves = new float[width];
// 		for (int i = 0; i < layers - 1; i++)
// 		{
// 			for (int j = 0; j < width; j++)
// 			{
// 				bias[i, j] = a.bias[i, j];
// 			}
// 		}
//
// 		for (int i = 0; i < layers; i++)
// 		{
// 			for (int j = 0; j < width; j++)
// 			{
// 				for (int k = 0; k < width; k++)
// 				{
// 					weights[i, j, k] = a.weights[i, j, k];
// 				}
// 			}
// 		}
//
// 		for (int i = 0; i < width; i++)
// 		{
// 			octaves[i] = a.octaves[i];
// 		}
// 	}
//
// 	public void Mutate(float chance, float rate)
// 	{
// 		for (int i = 0; i < layers - 1; i++)
// 		{
// 			for (int j = 0; j < width; j++)
// 			{
// 				if (Random.value < chance * width) bias[i, j] += Random.Range(-rate, rate);
// 				if (bias[i, j] < -1f) bias[i, j] = -1f;
// 				else if (bias[i, j] > 1f) bias[i, j] = 1f;
// 			}
// 		}
//
// 		for (int i = 0; i < layers; i++)
// 		{
// 			for (int j = 0; j < width; j++)
// 			{
// 				for (int k = 0; k < width; k++)
// 				{
// 					if (Random.value < chance) weights[i, j, k] += Random.Range(-rate, rate);
// 					if (weights[i, j, k] < -1f) weights[i, j, k] = -1f;
// 					else if (weights[i, j, k] > 1f) weights[i, j, k] = 1f;
// 				}
// 			}
// 		}
//
// 		for (int i = 0; i < width; i++)
// 		{
// 			if (Random.value < chance * width)
// 			{
// 				octaves[i] += Random.Range(-0.5f, 0.5f);
// 				if (octaves[i] < octavesMin) octaves[i] = octavesMin;
// 				else if (octaves[i] > octavesMax) octaves[i] = octavesMax;
// 			}
// 		}
// 	}
//
// 	// propagation
// 	public void Update()
// 	{
// 		for (int i = 0; i < width; i++)
// 		{
// 			neurons[0, i] = Mathf.Sin(Main.time * octaves[i]);
// 		}
//
// 		for (int i = 1; i < layers; i++)
// 		{
// 			for (int j = 0; j < width; j++)
// 			{
// 				neurons[i, j] = 0;
// 				for (int k = 0; k < width; k++)
// 				{
// 					neurons[i, j] += neurons[i - 1, k] * weights[i, j, k];
// 				}
//
// 				neurons[i, j] += bias[i - 1, j];
// 				neurons[i, j] /= width * 0.25f;
// 				if (neurons[i, j] < -1f) neurons[i, j] = -1f;
// 				else if (neurons[i, j] > 1f) neurons[i, j] = 1f;
// 			}
// 		}
// 	}
// }