using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class Bullet : Sprite
    {
        int damage;
        public Bullet(int pDamage) : base("Bullet.png") 
        {
            collider.isTrigger = true;
            SetOrigin(width / 2, height / 2);
            damage = pDamage;
        }

        void Update()
        {
            Move(10, 0);

            GameObject[] collisions = GetCollisions();
            for (int i = 0; i < GetCollisions().Count(); i++)
            {
                Console.WriteLine(collisions[i].GetType().Name);
                if (collisions[i].GetType().Name == "AnimationSprite")
                {
                    LateDestroy();
                    break;
                }
                if (collisions[i].GetType().Name == "Enemy")
                {
                    Enemy enemy = (Enemy)collisions[i];
                    enemy.Hit(damage);
                    LateDestroy();
                    break;
                }
            }
        }

        //void OnCollision(GameObject other)
        //{
        //    //if (other.GetType().Name == "Enemy")
        //    //{   
        //    //    Enemy enemy = (Enemy)other;
        //    //    enemy.Hit(damage);
        //    //    LateDestroy();
        //    //}
        //    for (int i = 0; i < GetCollisions().Count(); i++)
        //    {
        //        Console.WriteLine();
        //    }
        //}
    }
}
