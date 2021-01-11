using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager Instance; //we can MonoBehaviour this class from another script
	SpawnPoint[] spawnpoints;
	private void Awake()
	{
		Instance = this;
		spawnpoints = GetComponentsInChildren<SpawnPoint>();	
	}

	public Transform GetSpawnPoints()
	{
		return spawnpoints [Random.Range(0, spawnpoints.Length)].transform;
	}
}
