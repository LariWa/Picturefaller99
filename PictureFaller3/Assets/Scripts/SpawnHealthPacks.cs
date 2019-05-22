using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHealthPacks : MonoBehaviour
{
    public GameObject HealthPack;
    Rigidbody player;
    public float spawnTime = 5f;
    Vector3 playerCurrentPos;
    // Start is called before the first frame update
    void Start()
    {

        player = GetComponent<Rigidbody>();
        InvokeRepeating("SpawnPacks", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        playerCurrentPos = player.position;
    }

    void SpawnPacks() {
        Instantiate(HealthPack, new Vector3(Random.Range(-8f, 8f), (playerCurrentPos.y - 300), Random.Range(-8f, 8f)), Quaternion.identity);
    }
}
