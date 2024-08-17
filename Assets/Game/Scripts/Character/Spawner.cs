using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{

    private List<SpawnPoint> spawnPointList;
    private bool hasSpawned;

    private List<Character> characterList;

    public BoxCollider collider;

    public UnityEvent OnAllSpawnedCharacterEliminated;

    bool allEnemyDead;

    private void Awake()
    {
        var spawnPointArray = transform.parent.GetComponentsInChildren<SpawnPoint>();
        spawnPointList = new List<SpawnPoint>(spawnPointArray);
        characterList = new List<Character>();

    }



    private void Update()
    {
        if (!hasSpawned || characterList.Count == 0)
        {
            return;
        }

        bool allSpawnedAreDead = true;
        foreach (var c in characterList)
        {
            if (c.currentState != Character.CharacterState.Dead)
            {
                allSpawnedAreDead = false;
                break;
            }
        }


        if (allSpawnedAreDead && !allEnemyDead)
        {
            allEnemyDead = true;
            OnAllSpawnedCharacterEliminated?.Invoke();
        }

    }

    public void SpawnCharacters()
    {
        if (hasSpawned)
        {
            return;
        }
         
        hasSpawned = true;
        foreach (var p in spawnPointList)
        {
            if (p.enemyPrefab!=null)
            {
               var spawnedGameobject = Instantiate(p.enemyPrefab, p.transform.position, p.transform.rotation);
                characterList.Add(spawnedGameobject.GetComponent<Character>());
            }
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnCharacters();
        }
    }


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, collider.bounds.size);


    }


}
