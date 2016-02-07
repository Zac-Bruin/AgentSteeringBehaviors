using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AI_Final
{ 

    public abstract class State
    {
        public SteeringBehaviors steering;
        public InputController input;

        public virtual void Enter(Agent a)
        {

        }

        public virtual Vector2 Execute(Agent a, GameTime gameTime, Vector2 Target)
        {
            return new Vector2(0, 0);
        }

        public virtual void Exit(Agent a)
        {

        }
    }
}
