using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AI_Final
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        AgentManager agentManager;

        Texture2D ghost, candy, spriteMarker;

        InputHandler input;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 700;

            input = new InputHandler(this);
            this.Components.Add(input);

            agentManager = new AgentManager(this);
            this.Components.Add(agentManager);
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            IsMouseVisible = false;

            ghost = Content.Load<Texture2D>("Ghost");
            candy = Content.Load<Texture2D>("Candy");
            spriteMarker = Content.Load<Texture2D>("SpriteMarker");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

            if(agentManager.LeftMouseIsDown)
            {
                spriteBatch.Draw(ghost, new Vector2(agentManager.input.mouseDirection.X - (float)ghost.Width/2,
                    agentManager.input.mouseDirection.Y - (float)ghost.Height/2), Color.Pink);
            }

            else if(agentManager.RightMouseIsDown)
            {
                spriteBatch.Draw(candy, agentManager.input.mouseDirection, Color.White);
            }

            else
            {
                spriteBatch.Draw(spriteMarker, agentManager.input.mouseDirection, Color.Black);
            }

            agentManager.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
