using UnityEngine;
using System.Collections;

public class WaveSpawn : MonoBehaviour 
{

	public int WaveSize;
	public GameObject EnemyPrefab;
	public float EnemyInterval;
	public Transform spawnPoint;
	public float startTime;
	public Transform[] WayPoints;
	int enemyCount=0;
	private int waveIndex = 0;
    private bool canSpawn = true;
	private float countdown = 2f;

	// void Start ()
    // {
	// 	InvokeRepeating("SpawnEnemy", startTime, EnemyInterval);
	// }

	void Update()
    {
        if (countdown <= 0)
        {
            StartCoroutine(SpawnWave());
            countdown = waveIndex * 0.7f;
        }

        countdown -= Time.deltaTime;
    }

	IEnumerator SpawnWave()
    {   
        waveIndex++;

        if(canSpawn && waveIndex <= 100 ){
            for (int i = 0; i < waveIndex; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.5f);
            }
        } else
        {
            canSpawn = false;

        }
    }

	void SpawnEnemy()
	{
		GameObject enemy = GameObject.Instantiate(EnemyPrefab,spawnPoint.position,Quaternion.identity) as GameObject;
	}
}
