using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AI_Final
{
    public class Agent : DrawableSprite
    {
        public State state;
        public Vector2 head, velocity, UpRef, target, accel, steeringForce, center, lastSteerForce;

        public float timeSinceStateLastWanderChange, mass, maxSpeed, maxForce;

        bool hasChangedWander;

        //Random rand;

        public Agent(Game game) : base(game)
        {
            Utility.rand = new Random();

            this.mass = Utility.rand.Next(2, 5);

            this.velocity = new Vector2(Utility.rand.Next(-100, 100), Utility.rand.Next(-100, 100));

            UpRef = new Vector2(0, -1);

            this.maxSpeed = 300;

            this.Direction = new Vector2(Utility.rand.Next(0, 100), Utility.rand.Next(0, 100));
            this.Direction.Normalize();
        }

        protected override void LoadContent()
        {
            this.spriteTexture = content.Load<Texture2D>("Agent");

            this.center = new Vector2
              (this.Location.X + (this.spriteTexture.Width * this.scale / 2),
               this.Location.Y + (this.spriteTexture.Height * this.scale / 2));

            SetTranformAndRect();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            lastSteerForce = this.steeringForce;

            this.steeringForce = this.state.Execute(this, gameTime, new Vector2(this.GraphicsDevice.Viewport.Width / 2, this.GraphicsDevice.Viewport.Height / 2));
            
            if(this.steeringForce != lastSteerForce)
            {
                if(this.state is Wandering)
                {
                    this.hasChangedWander = true;
                    this.timeSinceStateLastWanderChange = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000;
                }
            }

            else
            {
                this.hasChangedWander = false;
            }

            this.UpdateAgent(gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
            base.Update(gameTime);
        }

        private void UpdateAgent(double timeElapsed)
        {
            Vector2 accel = this.steeringForce / this.mass;
            this.velocity += accel * (float)timeElapsed;

            if (this.velocity.Length() > this.maxSpeed)
                this.velocity = Vector2.Normalize(this.velocity) * this.maxSpeed;

            if(this.velocity.LengthSquared() > .00001)
            {
                this.head = Vector2.Normalize(this.velocity);
                this.CalculateRotation();
            }

            this.Location += this.velocity * (float)(timeElapsed);

            WrapAroundScreen();

            this.center = new Vector2
                (this.Location.X + (this.spriteTexture.Width * this.scale / 2),
                this.Location.Y + (this.spriteTexture.Height * this.scale / 2));
        }

        Vector2 Flee(Vector2 FleeTarget)
        {
            float PanicDistance = 300;
            if ((this.Location - FleeTarget).Length() > PanicDistance)
            {
                return new Vector2(0, 0);
            }

            Vector2 DesiredVel = (Vector2.Normalize(this.Location - FleeTarget) * this.maxSpeed);

            return (DesiredVel - this.velocity);
        }

        private void CalculateRotation()
        {
            double a = this.head.Length();
            double b = this.UpRef.Length();
            double c = (this.head.X * this.UpRef.X) + (this.head.Y * this.UpRef.Y);

            double d = a * b;
            double e = c / d;

            if (this.head.X > 0)
            {
                this.Rotate = (float)Math.Acos(e);
                return;
            }

            if (this.head.X < 0)
            {
                this.Rotate = -(float)Math.Acos(e);
                return;
            }
        }

        private void WrapAroundScreen()
        {
            if (this.center.X < (this.Game.GraphicsDevice.Viewport.Bounds.Left - 20))
                this.Location.X = this.Game.GraphicsDevice.Viewport.Bounds.Right;

            else if (this.center.X > (this.Game.GraphicsDevice.Viewport.Bounds.Right + 20))
                this.Location.X = this.Game.GraphicsDevice.Viewport.Bounds.Left;

            if (this.center.Y > (this.Game.GraphicsDevice.Viewport.Bounds.Bottom + 20))
                this.Location.Y = this.Game.GraphicsDevice.Viewport.Bounds.Top;

            else if (this.center.Y < (this.Game.GraphicsDevice.Viewport.Bounds.Top - 20))
                this.Location.Y = this.Game.GraphicsDevice.Viewport.Bounds.Bottom;
        }
    }
}
