using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace AI_Final
{
    public class SteeringBehaviors
    {
        Random rand = new Random();
        public Vector2 Flee(Agent a, Vector2 Target)
        {
            float PanicDistance = 200;
            if ((a.Location - Target).Length() > PanicDistance)
            {
                return a.steeringForce;
            }

            Vector2 DesiredVel = (Vector2.Normalize(a.Location - Target) * a.maxSpeed);

            return (DesiredVel - a.velocity) * 6;
        }

        public Vector2 Wander(Agent a, GameTime gameTime)
        {
            int rando = rand.Next(0, 5);

            if (rando == 1 || rando == 3 || rando == 5)
            {
                if (a.timeSinceStateLastWanderChange < gameTime.TotalGameTime.TotalSeconds - 1)
                {
                    Vector2 NewSteer = new Vector2(rand.Next(-500, 500), rand.Next(-500, 500));
                    return NewSteer;
                }
            }

            return a.steeringForce;
        }

        public Vector2 Seek(Agent a, Vector2 Target)
        {
            if(a.Location.X > Target.X - 5 && a.Location.X < Target.X + 5 &&
                a.Location.Y > Target.Y + 5 && a.Location.Y < Target.Y - 5)
            {
                return a.steeringForce;
            }

            Vector2 DesiredVelocity = (Vector2.Normalize(Target - a.Location) * a.maxSpeed);

            return (DesiredVelocity - a.velocity) * 4;
        }
    }
}
