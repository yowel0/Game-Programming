using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Collections.Generic;                      // System.Drawing contains drawing tools such as Color definitions

public class MyGame : Game {
    Player player = null;
    Level level1;
    Enemy enemy;
    Hud hud;

    List<Enemy> enemies = new List<Enemy>();

    public int levelIndex;

    int gunAmmo;

    Random rnd = new Random();

    float spawnTimer;
    float spawnTimerMax = 1500f;

    int wave = 0;
    float waveTimer = 0;
    float waveTimeMax = 0;
    float waveTimeBase = 10000;
    float waveTimePerWave = 5000;

    public MyGame() : base(320, 256, false, false, 960, 768, true)     // Create a window that's 960 by 768 and NOT fullscreen but does enable pixelart
	{
        // Draw some thinss on a canvas:
        LoadLevel(0);

        // Add the canvas to the engine to display it:
        Console.WriteLine("MyGame initialized");
    }

	// For every game object, Update is called every frame, by the engine:
	void Update() {


        if (Input.GetKeyDown(Key.SPACE) && levelIndex != 1)
		{
            LoadLevel(1);
        }
        if (levelIndex == 1)
        {
            if(player == null)
            {
                player = game.FindObjectOfType<Player>();
            }
            //if the player dies go to the game-over screen
            if (player.health == 0)
            {
                LoadLevel(2);
            }
            if (wave == 4)
            {
                LoadLevel(3);
            }
            CheckEnemyCount();
            SpawnTimer();
            CheckNewWave();
            if (FindObjectOfType<Player>() != null && FindObjectOfType<Player>().FindObjectOfType<Gun>() != null)
                gunAmmo = FindObjectOfType<Player>().FindObjectOfType<Gun>().ammo;
            else
                gunAmmo = 0;
            hud.UpdateGameUI(player.health, gunAmmo, wave, waveTimer, enemies.Count);
        }
    }
	  
	static void Main()                          // Main() is the first method that's called when the program is run
	{
		new MyGame().Start();                   // Create a "MyGame" and start it
	}

    void DestroyAll()
    {
        List<GameObject> children = GetChildren();
        foreach (GameObject child in children)
        {
            Console.WriteLine(child);
            child.Destroy();
        }
    }

	void LoadLevel(int index)
	{
        //destroy everything and load a level based on the index
        DestroyAll();
        levelIndex = index;
        if(index == 0)
        {
            hud = new Hud();
            LateAddChild(hud);
            hud.UpdateTextUI("Press space to play");
            hud.AddTextUI("R to Reload", 1);
            hud.AddTextUI("WASD to Move", 2);
            hud.AddTextUI("Beat Wave 3 To WIN", 3);
        }
        if(index == 1)
        {
            //reset the wave  when replaying the game
            wave = 0;
            waveTimer = 0;
            waveTimeMax = 0;
            //reset the enemy count;
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies.Remove(enemies[i]);
            }
            player = game.FindObjectOfType<Player>();

            level1 = new Level(player,"Map1.tmx");
            hud = new Hud();

            //game

            AddChild(level1);
            LateAddChild(hud);

        }
        if(index == 2)
        {
            hud = new Hud();
            LateAddChild(hud);
            hud.UpdateTextUI("Game-Over, Press space to play again");
        }
        if(index == 3)
        {
            hud = new Hud();
            AddChild(hud);
            hud.UpdateTextUI("You Won, Press space to play again");
        }
    }

    void CheckEnemyCount()
    {
        //removes the enemy from the enemies list if the parent is null
        for(int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i].parent == null)
            {
                enemies.Remove(enemies[i]);
            }
        }
    }

    void SpawnTimer()
    {
        //spawns an enemy everytime the timer runs out
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnTimerMax && waveTimer > 0)
        {
            spawnTimer = 0;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        //spawns an enemy on a random position
        enemy = new Enemy(player, (float)rnd.NextDouble() + 1);
        enemies.Add(enemy);
        enemy.SetXY(rnd.Next(64 + enemy.width, 64*16 - enemy.width), rnd.Next(64 + enemy.height, 64 * 10 - enemy.height));
        level1.AddChild(enemy);
    }

    void CheckNewWave()
    {
        //new wave if every enemy is dead and the timer has ran out
        //every wave has longer round and faster enemy spawn timer
        waveTimer -= Time.deltaTime;
        if(waveTimer <= 0)
        {
            waveTimer = 0;
        }
        if(waveTimer <= 0 && enemies.Count == 0)
        {
            waveTimeMax = waveTimeBase + waveTimePerWave * wave;
            waveTimer = waveTimeMax;
            wave++;
            spawnTimerMax -= spawnTimerMax * 0.2f;
        }
    }
}