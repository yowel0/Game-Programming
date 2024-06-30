using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    public class Gun : Sprite
    {
        Player player;

        int damage;

        bool autoFire;
        public int ammo;
        int maxAmmo = 25;

        float shootTimer;
        float shootTimeMax;

        float reloadTimer;
        float reloadTimeMax = 0.5f;

        float newRotation;

        int bulletAmount;
        int bulletRotationOffset;

        Vector2 local;

        public Gun(string gunSprite,int pDamage, bool pAutoFire, float pShootCooldown, int pMaxAmmo, int pBulletAmount = 1, int pBulletRotationOffset = 30) : base(gunSprite, false, false) 
        {
            SetOrigin(width / 3, height / 2.5f);
            maxAmmo = pMaxAmmo;
            ammo = maxAmmo;
            autoFire = pAutoFire;
            shootTimeMax = pShootCooldown * 1000;
            damage = pDamage;
            bulletAmount = pBulletAmount;
            bulletRotationOffset = pBulletRotationOffset;
        }

        void Update()
        {
            if (parent != null)
            {
                if (player == null)
                    player = (Player)parent;
                //local = parent.InverseTransformPoint(Input.mouseX, Input.mouseY);
                local = new Vector2(Input.mouseX - game.width/2, Input.mouseY - game.height/2);
                newRotation = Mathf.Atan2(y - local.y, x - local.x) * 180 / Mathf.PI + 180;
                rotation = newRotation;
                if (rotation > 90 && rotation < 270)
                {
                    Mirror(false, true);
                    SetOrigin(width / 3, height / 2f);
                }
                else
                {
                    Mirror(false, false);
                    SetOrigin(width / 3, height / 2f);
                }
                reloadTimer -= Time.deltaTime;
                shootTimer -= Time.deltaTime;
                if (reloadTimer <= 0)
                    reloadTimer = 0;
                if (shootTimer <= 0)
                    shootTimer = 0;
                Inputs();
            }
        }

        void Inputs()
        {
            if (Input.GetMouseButtonDown(0) && !autoFire)
                Shoot();
            if (Input.GetMouseButton(0) && autoFire)
                Shoot();
            if (Input.GetKeyDown(Key.R))
                Reload();
        }

        void Shoot()
        {
            if (ammo > 0 && reloadTimer <= 0 && shootTimer <= 0)
            {
                player.shootSound.Play();
                shootTimer = shootTimeMax;

                for (int i = 0; i < bulletAmount; i++)
                {
                    Bullet bullet = new Bullet(damage);
                    bullet.SetXY(x, y);
                    bullet.rotation = (rotation + bulletRotationOffset * i) - bulletAmount/2 * bulletRotationOffset;
                    parent.AddChild(bullet);
                    bullet.Move(25, 0);
                }
                ammo--;
            }
        }

        void Reload()
        {
            if (reloadTimer <= 0)
            {
                player.reloadSound.Play();
                reloadTimer = reloadTimeMax * 1000;
                ammo = maxAmmo;
            }
        }
    }
}
