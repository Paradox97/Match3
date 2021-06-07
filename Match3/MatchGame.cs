using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Match3.GameEntities;
using Match3.ScreenEntities;
using Match3.Controls;
using Match3.States;

namespace Match3
{
    public class MatchGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Texture2D[][]
            textTextures,
            buttonTextures,
            figureTextures,
            figureAnimationTextures;

        public Texture2D[]
            effectsTextures,
            fieldTextures;

        Field field;

        private Screen currentScreen,
            nextScreen;

        private MouseState currentMouseState,
            previousMouseState;

        private KeyboardState currentKeyboardState,
            previousKeyboardState;


        public int score;

        public void ChangeScreen(Screen screen)
        {
            nextScreen = screen;
        }

        public MatchGame()
        {
            graphics = new GraphicsDeviceManager(this);  //to be moved to Screen
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 455;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 625;   // set this value to the desired height of your window
            base.Initialize();
        }

        protected override void LoadContent()
        {
            graphics.ApplyChanges();
            spriteBatch = new SpriteBatch(GraphicsDevice);

            currentScreen = new MainMenuScreen(this, graphics, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if(nextScreen != null)
            {
                currentScreen = nextScreen;
                nextScreen = null;
            }

            previousMouseState = currentMouseState;
            previousKeyboardState = currentKeyboardState;

            Input input = Input.GetInput();

            currentMouseState = input.mouseState;
            currentKeyboardState = input.keyboardState;

            currentScreen.Update(gameTime, currentMouseState, previousMouseState, currentKeyboardState, previousKeyboardState);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            currentScreen.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }
    }
}
