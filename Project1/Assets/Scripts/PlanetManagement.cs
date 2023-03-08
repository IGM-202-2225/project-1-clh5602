using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlanetManagement : MonoBehaviour
{
    public float rotationSpeed = 0.0f;
    private Camera mainCam;
    private const float camSizeMult = 0.09f;
    public float timeRemaining = 60.0f;
    private int wave = 0;

    private float gameOverTimer = 0;

    public float score = 0;

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
    GameObject textPref;

    [SerializeField]
    PlayerStuff player;

    [SerializeField]
    GameObject explosion;

    [SerializeField]
    TitleScreen titleScreen;

    public bool inGame = false;

    [SerializeField]
    Text comboMeter;
    int combo = 0;
    float comboTimer = 0;

    [SerializeField]
    MapManager map;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Pan camera ahead and behind
        float panAmount = rotationSpeed * camSizeMult;
        mainCam.transform.position = new Vector3(panAmount, 0, -10);
        transform.position = new Vector3(panAmount * 0.7f, -4.99f, 3.85f);

        if (inGame)
        {
            // Decrease timer
            timeRemaining -= Time.deltaTime;

            // Check lasers
            HandleLasers();

            // Let map check enemies first
            map.CheckIconsForDelete();

            // Let me check enemies
            EnemyCollisions();

            // Cap time remaining, also game over
            if (timeRemaining < 0)
            {
                timeRemaining = 0;
                GameOver();
            }
            if (timeRemaining > 999)
            {
                timeRemaining = 999;
            }
        }
        else // Game over + title screen
        {
            if (!titleScreen.visible)
            {
                if (gameOverTimer > 0)
                {
                    gameOverTimer -= Time.deltaTime;
                }
                else
                {
                    titleScreen.visible = true;
                }
            }
        }


        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime), Space.Self);

        // Update Combo stuff
        ComboHandler();
    }


    // Manages player lasers
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


    // Check for laser collisions
    void HandleLasers()
    {
        for (int i = lasers.Count-1; i >= 0; i--)
        {
            // Collisions
            foreach (EnemyStuff enemy in enemyCollection)
            {
                enemy.LaserCollisionCheck(lasers[i].transform.position);
            }

            // Delete offscreen lasers
            if (lasers[i].GetComponent<LaserPath>().deleteMe) {
                Destroy(lasers[i]);
                lasers.RemoveAt(i);
            }
        }
    }



    // Both enemies have same script basically
    void CreateEnemy(bool type)
    {
        GameObject newEnemy;
        if (type) // Enemy 1
        {
            newEnemy = Instantiate(enemy1Pref, new Vector3(0, 20, -5), Quaternion.identity);
        }
        else // Enemy 2
        {
            newEnemy = Instantiate(enemy2Pref, new Vector3(0, -25, -5), Quaternion.identity);
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


    // loop thru every enemy. Delete enemies that need to be defeated, also player enemy collisions
    void EnemyCollisions()
    {
        // Loop from back of list to front
        for (int i = enemyCollection.Count - 1; i >= 0; i--)
        {
            if (enemyCollection[i].deleteMe) // if enemy needs to be delete
            {
                // Splode
                CreateExplosion(enemyCollection[i].transform.position);
                
                // turret enemy, delte cannon and bullet
                if (!enemyCollection[i].type)
                {
                    Destroy(enemyCollection[i].myCannon);
                    if (enemyCollection[i].myBullet != null)
                    {
                        Destroy(enemyCollection[i].myBullet.gameObject);
                    }
                }

                // If enemy defeated by player, give points and time
                if (enemyCollection[i].giveReward)
                {
                    if (enemyCollection[i].type)
                    {
                        score += ComboAdjusted(1000);
                    }
                    else
                    {
                        score += ComboAdjusted(1500);
                    }
                    timeRemaining += ComboAdjusted(0.5f);
                    IncreaseCombo();
                    if (score > 9999999999)
                    {
                        score = 9999999999;
                    }
                }

                Destroy(enemyCollection[i].gameObject);

                enemyCollection.RemoveAt(i);
            } 
            else if (player.invincible <= 0) // if enemy not defeated, collision check
            {
                // Player touch enemy
                if (EnemyPlayerCollision(enemyCollection[i]))
                {
                    player.invincible = 2.0f;
                    timeRemaining -= 15.0f;
                    comboTimer = 0;

                    if (timeRemaining > 1.0f)
                    {
                        CreateText("-15 SEC", 0, 2.5f, true);
                    }

                    enemyCollection[i].deleteMe = true;
                    enemyCollection[i].giveReward = false; // If ran into enemy instead of shoot
                }

                // Enemy bullet
                if (enemyCollection[i].myBullet != null && EnemyLaserCollisionCheck(enemyCollection[i].myBullet.transform.position))
                {
                    player.invincible = 2.0f;
                    timeRemaining -= 15.0f;
                    comboTimer = 0;

                    if (timeRemaining > 1.0f)
                    {
                        CreateText("-15 SEC", 0, 2.5f, true);
                    }

                    enemyCollection[i].myBullet.deleteMe = true;
                }
            }
        }

        // Start new wave once all enemies defeated
        if (enemyCollection.Count == 0 && inGame)
        {
            NewWave();
        }
        
    }


    // Create explosion animation at spot
    void CreateExplosion(Vector3 location)
    {
        Instantiate(explosion, location, Quaternion.identity);
    }


    // Check, Circle
    bool EnemyPlayerCollision(EnemyStuff enemy)
    {
        return Mathf.Pow(player.radius + enemy.radius, 2) > Mathf.Pow(player.transform.position.x - enemy.transform.position.x, 2) + Mathf.Pow(player.transform.position.y - enemy.transform.position.y, 2);
    }


    // enemy laser check
    public bool EnemyLaserCollisionCheck(Vector3 laserPos)
    {

        return (Mathf.Pow(player.radius * 1.8f, 2) >= Mathf.Pow(laserPos.x - player.transform.position.x, 2) + Mathf.Pow(laserPos.y - player.transform.position.y, 2)) ;
    }


    // Create a message to the screen
    void CreateText(string message, float x, float y, bool red)
    {
        GameObject newMess = Instantiate(textPref, new Vector3(x, y, -9), Quaternion.identity);
        newMess.GetComponent<TextMesh>().text = message;

        if (red)
        {
            newMess.GetComponent<TextPopUp>().rgb = new Vector3(1, 0, 0);
        }
    }


    // Creates wave of enemies and pushes message
    void NewWave()
    {
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            CreateEnemy(false);
        }

        for (int i = 0; i < Random.Range(8, 14); i++)
        {
            CreateEnemy(true);
        }

        wave++;

        CreateText("- WAVE " + wave.ToString() + " -", 0, 2.5f, false);
    }


    // Decreases combo timer, sets the display properties
    void ComboHandler()
    {
        if (comboTimer <= 0)
        {
            combo = 0;
            comboMeter.text = "";
            return;
        }

        comboMeter.text = "x" + combo.ToString();
        comboMeter.color = new Color( 1, 1 - (combo/40.0f), 1 - (combo/20.0f), comboTimer/4.0f);
        
        comboTimer -= Time.deltaTime;
    }


    // Increases combo and refreshes timer
    void IncreaseCombo()
    {
        combo++;
        comboTimer = 4.0f;
    }


    // Formula to increase value based on current combo
    float ComboAdjusted(float origVal)
    {
        return origVal * (Mathf.Log(combo + 10, 10) * 1.6f - 0.6f);
    }


    // Out of time. Destroys all enemies, starts timer to bring back title screen
    void GameOver()
    {
        CreateText("- GAME OVER -", 0, 2.5f, true);
        inGame = false;

        for (int i = enemyCollection.Count - 1; i >= 0; i--)
        {
            enemyCollection[i].deleteMe = true;
            enemyCollection[i].giveReward = false;
        }
        comboTimer = 0;

        map.CheckIconsForDelete();
        EnemyCollisions();

        gameOverTimer = 4;
    }

    // When start key pressed
    public void StartGame(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && titleScreen.visible)
        {
            timeRemaining = 60;
            inGame = true;
            titleScreen.visible = false;
            score = 0;
            gameOverTimer = 0;
            wave = 0;
            NewWave();
        }
        
    }
}
