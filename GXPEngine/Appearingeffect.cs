using GXPEngine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class Appearingeffect : AnimationSprite
    {
        public Appearingeffect() : base("appearing.png",7,1,-1,false,false)
        {
            SetOrigin(width/2,height/2);
            SetCycle(0, 7);
        }

        void Update()
        {
            Animate(0.2f);
            if(currentFrame == 6)
            {
                Destroy();
            }
        }
    }
}
