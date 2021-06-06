﻿using Microsoft.Xna.Framework;
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


        private int score;

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

            currentScreen = new MainMenuScreen(this, graphics.GraphicsDevice, Content);

            #region text textures
           
            this.textTextures = new Texture2D[5][];  //menu textures and whatnot

            this.textTextures[0] = new Texture2D[1]
            {
                Content.Load<Texture2D>("text/Main_menu")
            };

            this.textTextures[1] = new Texture2D[2]
            {
                Content.Load<Texture2D>("text/Match_3"),
                Content.Load<Texture2D>("text/Classic")
            };

            this.textTextures[2] = new Texture2D[1]
            {
                Content.Load<Texture2D>("text/Game_over")
            };

            this.textTextures[3] = new Texture2D[1]
            {
                Content.Load<Texture2D>("text/High_scores")
            };


            this.textTextures[4] = new Texture2D[1]
            {
                Content.Load<Texture2D>("text/Are you sure")
            };

            #endregion

            #region button textures

            this.buttonTextures = new Texture2D[5][];  //button textures

            this.buttonTextures[0] = new Texture2D[3]
            {
                Content.Load<Texture2D>("buttons/Play"),
                Content.Load<Texture2D>("buttons/High_scores"),
                Content.Load<Texture2D>("buttons/Quit")
            };

            this.buttonTextures[1] = null;

            this.buttonTextures[2] = new Texture2D[1]
            {
                Content.Load<Texture2D>("buttons/Ok")
            };

            this.buttonTextures[3] = new Texture2D[1]
            {
                Content.Load<Texture2D>("text/High_scores")
            };

            this.buttonTextures[4] = new Texture2D[2]
            {
                Content.Load<Texture2D>("buttons/Yes"),
                Content.Load<Texture2D>("buttons/No")
            };
            #endregion

            #region figure textures

            this.figureTextures = new Texture2D[5][];

            this.figureTextures[0] = new Texture2D[4]
           {
                Content.Load<Texture2D>("textures/circle/circle"),
                Content.Load<Texture2D>("textures/circle/circle_linehor"),
                Content.Load<Texture2D>("textures/circle/circle_linevert"),
                Content.Load<Texture2D>("textures/circle/circle_bomb")
           };

            this.figureTextures[1] = new Texture2D[4]
           {
                Content.Load<Texture2D>("textures/crystall/crystall"),
                Content.Load<Texture2D>("textures/crystall/crystall_linehor"),
                Content.Load<Texture2D>("textures/crystall/crystall_linevert"),
                Content.Load<Texture2D>("textures/crystall/crystall_bomb")
           };

            this.figureTextures[2] = new Texture2D[4]
           {
                Content.Load<Texture2D>("textures/heart/heart"),
                Content.Load<Texture2D>("textures/heart/heart_linehor"),
                Content.Load<Texture2D>("textures/heart/heart_linevert"),
                Content.Load<Texture2D>("textures/heart/heart_bomb")
           };

            this.figureTextures[3] = new Texture2D[4]
           {
                Content.Load<Texture2D>("textures/pyramid/pyramid"),
                Content.Load<Texture2D>("textures/pyramid/pyramid_linehor"),
                Content.Load<Texture2D>("textures/pyramid/pyramid_linevert"),
                Content.Load<Texture2D>("textures/pyramid/pyramid_bomb")
           };

            this.figureTextures[4] = new Texture2D[4]
           {
                Content.Load<Texture2D>("textures/square/square"),
                Content.Load<Texture2D>("textures/square/square_linehor"),
                Content.Load<Texture2D>("textures/square/square_linevert"),
                Content.Load<Texture2D>("textures/square/square_bomb")
           };
            #endregion

            #region figure animation

            this.figureAnimationTextures = new Texture2D[5][];

            this.figureAnimationTextures[0] = new Texture2D[3]
           {
                Content.Load<Texture2D>("textures/circle/circle_shine1"),
                Content.Load<Texture2D>("textures/circle/circle_shine2"),
                Content.Load<Texture2D>("textures/circle/circle_shine3")
           };

            this.figureAnimationTextures[1] = new Texture2D[3]
           {
                Content.Load<Texture2D>("textures/crystall/crystall_shine1"),
                Content.Load<Texture2D>("textures/crystall/crystall_shine2"),
                Content.Load<Texture2D>("textures/crystall/crystall_shine3")
           };

            this.figureAnimationTextures[2] = new Texture2D[3]
           {
                Content.Load<Texture2D>("textures/heart/heart_shine1"),
                Content.Load<Texture2D>("textures/heart/heart_shine2"),
                Content.Load<Texture2D>("textures/heart/heart_shine3")
           };

            this.figureAnimationTextures[3] = new Texture2D[3]
           {
                Content.Load<Texture2D>("textures/pyramid/pyramid_shine1"),
                Content.Load<Texture2D>("textures/pyramid/pyramid_shine2"),
                Content.Load<Texture2D>("textures/pyramid/pyramid_shine3")
           };

            this.figureAnimationTextures[4] = new Texture2D[3]
           {
                Content.Load<Texture2D>("textures/square/square_shine1"),
                Content.Load<Texture2D>("textures/square/square_shine2"),
                Content.Load<Texture2D>("textures/square/square_shine3")
           };
            #endregion

            #region effects textures

            this.effectsTextures = new Texture2D[5] {
                Content.Load<Texture2D>("effects/blast"),
                Content.Load<Texture2D>("effects/destroyer_down"),
                Content.Load<Texture2D>("effects/destroyer_left"),
                Content.Load<Texture2D>("effects/destroyer_right"),
                Content.Load<Texture2D>("effects/destroyer_up")
                    };
            #endregion

            #region field textures
            
            this.fieldTextures = new Texture2D[1]
            {
                Content.Load<Texture2D>("fields/field_1")
            };

            #endregion

            float fieldOffset = 5f;

            float heightOffset = (Window.ClientBounds.Height - fieldOffset) - fieldTextures[0].Height;          //for resize purposes, resize later

            Vector2[] fieldBounds = new Vector2[4]
            {
                new Vector2(fieldOffset, heightOffset),                                                        //topleft
                new Vector2(fieldOffset + fieldTextures[0].Width, heightOffset),                               //topright
                new Vector2(fieldOffset, heightOffset + fieldTextures[0].Height),                               //bottomleft
                new Vector2(fieldOffset + fieldTextures[0].Width, heightOffset + fieldTextures[0].Height)       //bottomright
            };

            this.field = new Field(figureTextures, figureAnimationTextures, effectsTextures, fieldTextures, fieldBounds, fieldOffset);
            //field.Draw();
            //field.Draw();
        }

        protected override void Update(GameTime gameTime)
        {
            if(nextScreen != null)
            {
                currentScreen = nextScreen;
                nextScreen = null;
            }

            // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //  {
            //end game    

            //   Exit();
            // }
            previousMouseState = currentMouseState;

            Input input = Input.GetInput();

            currentMouseState = input.mouseState;

            currentScreen.Update(gameTime, currentMouseState, previousMouseState);

            //field.FieldInput();
            //field.Draw();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here

            //field.Draw();
            //field.Draw();

            currentScreen.Draw(gameTime, spriteBatch);
            //spriteBatch.Begin();

            //field.FieldDraw(spriteBatch);

            //spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
