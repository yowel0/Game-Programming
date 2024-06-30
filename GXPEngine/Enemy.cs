using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    public class Enemy : AnimationSprite
    {

        public int health = 5;

        float damagedTimeMax = 0.5f;
        float damagedTimer;

        Player target;
        float speed = 1;
        bool walking;
        float maxDistance = 0;
        float viewRange = 100;

        Appearingeffect aEffect;
        bool initiated = false;
        bool angry = false;

        Vec2 rndVec;
        Vec2 roamGoal;


        public Enemy(Player newTarget, float nSpeed) : base("EnemyRun.png", 12, 1)
        {
            target = newTarget;
            SetOrigin(width / 2, height / 2);
            SetXY(100, 100);
            collider.isTrigger = true;
            SetCycle(0, 12);
            aEffect = new Appearingeffect();
            AddChild(aEffect);
            alpha = 0;
            speed = nSpeed;
        }

        void Update()
        {
            if (initiated == false && aEffect.parent == null)
            {
                initiated = true;
                alpha = 1;
            }
            if (initiated == true)
            {
                walking = false;
                HandleAI();

                DamagedCheck();
                DoAnimate();
            }
        }

        void HandleAI()
        {
            if (DistanceTo(target) < viewRange || angry)
            {
                WalkToTarget(target.x,target.y);
            }
            else {
                Roam();
            }
        }

        void Roam()
        {
            if (roamGoal.x == 0 && roamGoal.y == 0)
            {
                NewRoamGoal();
            }
            if (x == roamGoal.x && y == roamGoal.y)
            {
                NewRoamGoal();
            }
            WalkToTarget(roamGoal.x, roamGoal.y);
        }

        void NewRoamGoal()
        {
            rndVec = Vec2.RandomUnitVector();
            roamGoal = new Vec2(x + rndVec.x * 50, y + rndVec.y * 50);
            //Console.WriteLine(x + " " + y);
            roamGoal.x = Mathf.Clamp(roamGoal.x, 64 + width, 64 * 16 - width);
            roamGoal.y = Mathf.Clamp(roamGoal.y, 64 + height, 64 * 10 - height);
        }

        void WalkToTarget(float tX, float tY)
        {
            Vec2 difference = new Vec2(tX - this.x, tY - this.y);
            float length = difference.Length();

            //normalize and scale
            Vec2 velocity = speed * difference.Normalized();

            x += velocity.x;
            y += velocity.y;

            if (velocity.x > 0)
            {
                _mirrorX = false;
            }
            else
            {
                _mirrorX = true;
            }
            if (length < speed)
            {
                //Console.WriteLine("stuck");
                x = tX;
                y = tY;
            }
            walking = true;
        }

        void DoAnimate()
        {
            if (walking)
            {
                SetCycle(0,12);
            }
            if (!walking)
            {
                SetCycle(0, 1);
            }
            Animate(0.2f);
        }

        public void Hit(int damage)
        {
            health -= damage;
            if(health <= 0)
            {
                this.LateDestroy();
            }
            color = 0xff0000;
            damagedTimer = damagedTimeMax * 1000;
            angry = true;
        }

        void DamagedCheck()
        {
            if (damagedTimer > 0)
            {
                damagedTimer -= Time.deltaTime;
            }
            if (damagedTimer <= 0)
            {
                color = 0xffffff;
            }
        }
    }
}
