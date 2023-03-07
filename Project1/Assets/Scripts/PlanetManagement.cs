using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManagement : MonoBehaviour
{
    public float rotationSpeed = 0.0f;
    private Camera mainCam;
    private const float camSizeMult = 0.09f;
    public float timeRemaining = 120.0f;

    private const int MAX_LASERS = 3;

    public List<GameObject> lasers = new List<GameObject>();
    public List<EnemyStuff> enemyCollection = new List<EnemyStuff>();

    [SerializeField]
    GameObject laserPref;

    [SerializeField]
    GameObject enemy1Pref;

    [SerializeField]
    GameObject enemy2Pref;

    [SerializeField]
    PlayerStuff player;

    [SerializeField]
    GameObject explosion;

    [SerializeField]
    MapManager map;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;

        for (int i = 0; i < Random.Range(5, 10); i++)
        {
            CreateEnemy(true);
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        // Pan camera ahead and behind
        float panAmount = rotationSpeed * camSizeMult;
        mainCam.transform.position = new Vector3(panAmount, 0, -10);
        transform.position = new Vector3(panAmount * 0.7f, -4.99f, 3.85f);

        // Decrease timer
        timeRemaining -= Time.deltaTime;
        

        // Check lasers
        HandleLasers();

        // Let map check enemies first
        map.CheckIconsForDelete();

        // Let me check enemies
        EnemyCollisions();

        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime), Space.Self);

        if (timeRemaining < 0)
        {
            timeRemaining = 0;
        }
        if (timeRemaining > 999)
        {
            timeRemaining = 999;
        }
    }

    public void CreateLaser(float height, bool left)
    {
        if (lasers.Count < MAX_LASERS)
        {
            GameObject newLaser = Instantiate(laserPref, new Vector3(0, height, -6), Quaternion.identity);

            if (left)
            {
                newLaser.GetComponent<LaserPath>().direction = -1;
            }

            lasers.Add(newLaser);
        }
        
    }

    void HandleLasers()
    {
        for (int i = lasers.Count-1; i >= 0; i--)
        {
            // Collisions
            foreach (EnemyStuff enemy in enemyCollection)
            {
                enemy.LaserCollisionCheck(lasers[i].transform.position);
            }

            if (lasers[i].GetComponent<LaserPath>().deleteMe) {
                Destroy(lasers[i]);
                lasers.RemoveAt(i);
            }
        }
    }

    void CreateEnemy(bool type)
    {
        GameObject newEnemy;
        if (type)
        {
            newEnemy = Instantiate(enemy1Pref, new Vector3(0, 0, -5), Quaternion.identity);
        }
        else
        {
            newEnemy = Instantiate(enemy2Pref, new Vector3(0, -5, -5), Quaternion.identity);
        }
        
        EnemyStuff enemyCode = newEnemy.GetComponent<EnemyStuff>();

        // Planet ref
        enemyCode.planet = GetComponent<PlanetManagement>();

        // Player ref
        if (!type)
        {
            enemyCode.player = player;
        }

        // Random starting pos offscreen
        enemyCode.myHoriPos = (Random.Range(60, 300) + transform.eulerAngles.y) % 360;

        map.CreateEnemyIcon(enemyCode);

        enemyCollection.Add(enemyCode);

    }

    void EnemyCollisions()
    {
        for (int i = enemyCollection.Count - 1; i >= 0; i--)
        {
            if (enemyCollection[i].deleteMe)
            {
                CreateExplosion(enemyCollection[i].transform.position);

                if (!enemyCollection[i].type)
                {
                    Destroy(enemyCollection[i].myCannon);
                }
                Destroy(enemyCollection[i].gameObject);

                timeRemaining += 1;

                enemyCollection.RemoveAt(i);
            } 
            else if (player.invincible <= 0)
            {
                // Player touch enemy
                if (EnemyPlayerCollision(enemyCollection[i]))
                {
                    player.invincible = 2.0f;
                    timeRemaining -= 30.0f;

                    enemyCollection[i].deleteMe = true;
                }
            }
        }

        if (enemyCollection.Count == 0)
        {
            for (int i = 0; i < Random.Range(5, 10); i++)
            {
                CreateEnemy(true);
            }
        }
        
    }

    void CreateExplosion(Vector3 location)
    {
        Instantiate(explosion, location, Quaternion.identity);
    }

    bool EnemyPlayerCollision(EnemyStuff enemy)
    {
        return Mathf.Pow(player.radius + enemy.radius, 2) > Mathf.Pow(player.transform.position.x - enemy.transform.position.x, 2) + Mathf.Pow(player.transform.position.y - enemy.transform.position.y, 2);
    }

}
