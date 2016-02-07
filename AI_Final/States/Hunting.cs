using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace AI_Final
{
    public class Hunting : State
    {
        public override void Enter(Agent a)
        {
            a.col = Color.Red;
        }

        public override Vector2 Execute(Agent a, GameTime gameTime, Vector2 Target)
        {
            return this.steering.Seek(a, this.input.mouseDirection);
        }

        public override void Exit(Agent a)
        {

        }
    }
}
