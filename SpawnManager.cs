using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;
    private bool _stopSpawning = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }
    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {
            Vector3 enemyStartPos = new Vector3(Random.Range(-7,8), 7, 0);
            GameObject newEnemy = Instantiate (_enemyPrefab, enemyStartPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform; //this and the line above help us organize the enemies in the hierarchy
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {
            Vector3 powerupStartPos = new Vector3(Random.Range(-7,8), 7, 0);
            int randomPowerup = Random.Range(0, 3);
            GameObject newPowerup = Instantiate (powerups[randomPowerup], powerupStartPos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));

        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
