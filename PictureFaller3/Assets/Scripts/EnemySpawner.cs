using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public GameObject explosionParticle;
    [SerializeField] private GameObject enemy;
    [SerializeField] private TextMeshProUGUI countdownDisplay;
    [SerializeField] private Vector2[] spawns;

    [Space]

    [SerializeField] private float pauseBetweenWaves;
    [SerializeField] private float[] enemysInWaves;
    [SerializeField] private float[] delaysInWaves;
    [SerializeField] private float delayRandomness;
    [Space]
    [SerializeField] private int[] enemHPInWaves; //AVG
    [SerializeField] private int[] enemDMGInWaves;
    [SerializeField] private float[] enemShotSpdInWaves;
    [SerializeField] private float[] enemSpdInWaves;

    private int wave = 0;
    private float countdown = -99;

    //private HealthSpawner hpSpawner;

    private bool isOn;
    private int currentEnemys;
    [SerializeField] private GameObject player;

    void Start()
    {
        //hpSpawner = GetComponent<HealthSpawner>();
        StartCoroutine(startWave(wave));

    }

    private void Update()
    {

        //print(wave);
        countdown -= Time.deltaTime;

        countdownDisplay.text = "Wave: " + (wave + 1);

  
        if(currentEnemys <= 0 && !isOn)
        {
            //hpSpawner.start();

            StartCoroutine(startWave(wave));
        }
    }


    private IEnumerator startWave(int _wave)
    {
        isOn = true;

        if (_wave >= enemysInWaves.Length) _wave = enemysInWaves.Length-1;

        for (int i = 0; i < enemysInWaves[_wave]; i++)
        {
            spawn(_wave);
            yield return new WaitForSeconds(delaysInWaves[_wave] + Random.Range(-delayRandomness, delayRandomness)); //maybe only positive random delay added, not subtracted.... so delaysInWaves sind minima
        }

        yield return new WaitForSeconds(delaysInWaves[_wave]*2);

        countdown = pauseBetweenWaves;
        //hpSpawner.stop();
        isOn = false;

        _wave++;
        if (_wave >= delaysInWaves.Length) _wave = delaysInWaves.Length;
        wave = _wave;


    }

    private void spawn(int wav)
    {
        var pos = spawns[Random.Range(0, spawns.Length)];

        while (Vector2.Distance(player.transform.position, pos) < 2f)
            pos = spawns[Random.Range(0, spawns.Length)];

        float scale = Random.Range(0.7f, 1f);
        var exp = Instantiate(explosionParticle, pos, Quaternion.identity);
        exp.transform.localScale = new Vector3(scale, scale, scale);

        var e = Instantiate(enemy, pos, Quaternion.identity);
        //e.GetComponent<AIDestinationSetter>().target = player.transform;
        e.GetComponentInChildren<EnemyStats>().setHPMAX(enemHPInWaves[wav]);
        e.GetComponentInChildren<Skillset>().getPrimary().averageDamage = enemDMGInWaves[wav];
        currentEnemys++;
    }


    public int getWave()
    {
        return wave;
    }
    public float getCountdown()
    {
        return countdown;
    }

    public void enemyDied()
    {
        currentEnemys--;
    }
}
