using GXPEngine.Core;
using System;
using GXPEngine;
using TiledMapParser;
using System.Threading;

public class Player : AnimationSprite
{

    int speed = 3;
    int dx;
    int dy;


    public int health = 5;

    Inventory inventory = new Inventory();

    float invincibilityTimer;
    float invincibilityTimeMax = 0.5f;

    public Sound shootSound = new Sound("shoot.wav");
    public Sound reloadSound = new Sound("reload.wav");
    Sound hitSound = new Sound("hit.wav");

    public Player(String fileName, int cols, int rows, TiledObject obj = null) : base("VirtualGuySpriteSheet.png", 12, 2)
    {
        SetOrigin(width/2,height/2);
        AddChild(inventory);
    }

    void Update()
    {
        // getcollisions
        GameObject[] collisions = GetCollisions();
        // movement
        Movement();
        DoAnimate();

        //reloading and shooting
        if (Input.GetKeyDown(Key.R))
        {
                //currentGun.Reload();
        }
        if (Input.GetMouseButtonDown(0))
        {
                //currentGun.Shoot();
        }

        //checks if the player can be hit again
        InvincibilityCheck();
    }


    //hits if collided with enemy
    void OnCollision(GameObject other)
    {
        if (other.GetType().Name == "Enemy")
        {
            if(invincibilityTimer <= 0)
            {
                Hit(1);
            }
        }
    }

    void Movement()
    {
        dx = 0;
        dy = 0;
        if (Input.GetKey(Key.W))
        {
            dy = dy - speed;
        }
        if (Input.GetKey(Key.S))
        {
            dy = dy + speed;
        }
        if (Input.GetKey(Key.A))
        {
            dx = dx - speed;
            _mirrorX = true;
        }
        if (Input.GetKey(Key.D))
        {
            dx = dx + speed;
            _mirrorX = false;
        }
        MoveUntilCollision(dx, dy);
    }

    void DoAnimate()
    {
        if (dx == 0 && dy == 0)
        {
            //idle
            SetCycle(0, 11);
        }
        if (dx != 0 || dy != 0)
        {
            //walk
            SetCycle(12, 12);
        }
        Animate(0.5f);
    }

    public void Hit(int damage)
    {
        hitSound.Play();
        health -= damage;
        color = 0xff0000;
        invincibilityTimer = invincibilityTimeMax * 1000;
            
    }

    void InvincibilityCheck()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        if(invincibilityTimer <= 0)
        {
            color = 0xffffff;
        }
    }
}