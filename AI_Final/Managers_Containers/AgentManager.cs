using System;
using System.Linq;
using System.Text;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace AI_Final
{
    public class AgentManager : Microsoft.Xna.Framework.GameComponent
    {
        SteeringBehaviors steeringBehaviors;
        StateContainer stateContainer;

        public InputController input;

        int numberOfAgentsToSpawnOnStart;

        float SeekAndFleeProximity;

        public bool HasCreatedAgents, LeftMouseIsDown, RightMouseIsDown;

        List<Agent> agents;

        Random rand;

        public AgentManager(Game game) : base(game)
        {
            steeringBehaviors = new SteeringBehaviors();
            stateContainer = new StateContainer();

            agents = new List<Agent>();

            numberOfAgentsToSpawnOnStart = 100;

            SeekAndFleeProximity = 200f;

            rand = new Random();

            SetStatesWithSteeringBehavior();

            HasCreatedAgents = false;

            this.input = new InputController(game);

            this.stateContainer.hunt.input = this.input;
            this.stateContainer.flee.input = this.input;
        }

        public override void Update(GameTime gameTime)
        {
            this.input.Update();

            CheckMouseInput();

            if(!HasCreatedAgents)
                SpawnInitialAgents();

            foreach(Agent a in this.agents)
            {
                a.Update(gameTime);

                if(LeftMouseIsDown)
                {
                    if ((a.Location - this.input.mouseDirection).Length() < SeekAndFleeProximity)
                    {
                        ChangeAgentState(a, stateContainer.flee);
                    }

                    else
                    {
                        ChangeAgentState(a, stateContainer.wander);
                    }
                }

                else if(RightMouseIsDown)
                {
                    if ((a.Location - this.input.mouseDirection).Length() < SeekAndFleeProximity)
                    {
                        ChangeAgentState(a, stateContainer.hunt);
                    }

                    else
                    {
                        ChangeAgentState(a, stateContainer.wander);
                    }
                }

                else
                {
                    if(a.state != stateContainer.wander)
                    {
                        ChangeAgentState(a, stateContainer.wander);
                    }
                }
            }


            base.Update(gameTime);
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Agent a in agents)
            {
                a.Draw(sb);
            }
        }

        private void SpawnInitialAgents()
        {
            for (int i = 0; i < this.numberOfAgentsToSpawnOnStart; i++)
            {
                SpawnAgent();
            }

            HasCreatedAgents = true;
        }

        private void CheckMouseInput()
        {
            if (this.input.mouse.LeftButton == ButtonState.Pressed && this.input.mouse.RightButton == ButtonState.Released)
            {
                this.LeftMouseIsDown = true;
            }

            if (this.input.mouse.RightButton == ButtonState.Pressed && this.input.mouse.LeftButton == ButtonState.Released)
            {
                this.RightMouseIsDown = true;
            }

            if (this.input.mouse.RightButton == ButtonState.Released && this.input.mouse.LeftButton == ButtonState.Released)
            {
                this.RightMouseIsDown = false;
                this.LeftMouseIsDown = false;
            }

            if (this.input.mouse.RightButton == ButtonState.Pressed && this.input.mouse.LeftButton == ButtonState.Pressed)
            {
                this.RightMouseIsDown = false;
                this.LeftMouseIsDown = false;
            }
        }

        private void SetStatesWithSteeringBehavior()
        {
            this.stateContainer.flee.steering = this.steeringBehaviors;
            this.stateContainer.hunt.steering = this.steeringBehaviors;
            this.stateContainer.wander.steering = this.steeringBehaviors;
        }

        public void ChangeAgentState(Agent a, State newState)
        {
            a.state.Exit(a);
            a.state = newState;
            a.state.Enter(a);
        }

        void SpawnAgent()
        {
            Agent a = new Agent(this.Game);
            a.Initialize();

            this.PositionAgent(a);

            while(CheckForOverlap(a))
            {
                this.PositionAgent(a);
            }
            
            a.state = this.stateContainer.wander;
            a.state.Enter(a);
            this.AddAgent(a);
        }

        void PositionAgent(Agent a)
        {
            a.Location = new Vector2(rand.Next(this.Game.GraphicsDevice.Viewport.Bounds.Left + 10, this.Game.GraphicsDevice.Viewport.Bounds.Right - 10),
                rand.Next(this.Game.GraphicsDevice.Viewport.Bounds.Top + 10, this.Game.GraphicsDevice.Viewport.Bounds.Bottom - 10));

            a.SetTranformAndRect();
        }

        private bool CheckForOverlap(Agent a)
        {
            foreach(Agent agent in this.agents)
            {
                if (a.Intersects(agent))
                    return true;
            }

            return false;
        }

        void AddAgent(Agent a)
        {
            this.agents.Add(a);
        }

        void RemoveAgent(Agent a)
        {
            this.agents.Remove(a);
        }
    }
}
