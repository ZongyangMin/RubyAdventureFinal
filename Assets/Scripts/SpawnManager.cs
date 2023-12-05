using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //Christopher Mccort
    //Power Up Spawn Positions
    public GameObject powerUpPosition1;
    public GameObject powerUpPosition2;
    public GameObject powerUpPosition3;
    public GameObject powerUpPosition4;
    public GameObject powerUpPosition5;
    public GameObject powerUpPosition6;
    public GameObject powerUpPosition7;

    //Christopher Mccort
    //Power Up Prefabs
    public GameObject spreadShotPowerUp;
    public GameObject speedBoostPowerUp;
    public GameObject extraProtectPowerUp;
    public GameObject healthRestore;
    public GameObject extraPointPowerUp;

    //Zongyang Min
    //Monster Spawn Positions
    public GameObject monsterPosition1;
    public GameObject monsterPosition2;
    public GameObject monsterPosition3;
    public GameObject monsterPosition4;

    //Zongyang Min
    //Monster Prefabs
    public GameObject robotNew;
    public GameObject flyingSlime;
    public GameObject fastDino;
    public GameObject rockGolem;

    public bool gameStart;
    public bool gameInitiate;

    private int x;
    private float elapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        gameStart = false;
        gameInitiate = false;
        elapsed = 0f;
    }

    //Zongyang Min
    //Run the Monster Spawn Method Every 5 seconds
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= 5f && gameInitiate)
        {
            elapsed = elapsed % 1f;
            MonsterSpawn();
        }
    }

    //Zongyang Min
    //Random Spawn Monsters
    void MonsterSpawn()
    {
        x = Random.Range(1,5);
        if(x == 1)
        {
            Instantiate(robotNew, monsterPosition1.transform.position, Quaternion.identity);
        }
        else if(x == 2)
        {
            Instantiate(flyingSlime, monsterPosition2.transform.position, Quaternion.identity);
        }
        else if(x == 3)
        {
            Instantiate(fastDino, monsterPosition3.transform.position, Quaternion.identity);
        }
        else if(x == 4)
        {
            Instantiate(rockGolem, monsterPosition4.transform.position, Quaternion.identity);
        }
    }
}
