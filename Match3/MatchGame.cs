using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Match3.GameEntities;
using Match3.ScreenEntities;
using Match3.Controls;

namespace Match3
{
    public class MatchGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Texture2D[][]
            textTextures,
            buttonTextures,
            figureTextures,
            figureAnimationTextures;

        public Texture2D[]
            effectsTextures,
            fieldTextures;

        Field field;
        Text[] text;
        Button[] buttons;



        public enum GameStates
        {
            mainMenu,
            gameState,
            gameOverState,
            highScoresState,
            quitState
        }

        private int score;

        public MatchGame()
        {
            _graphics = new GraphicsDeviceManager(this);  //to be moved to Screen
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 455;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 625;   // set this value to the desired height of your window
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _graphics.ApplyChanges();
            _spriteBatch = new SpriteBatch(GraphicsDevice);

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

            this.text = new Text[5];

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

            this.buttons = new Button[5];

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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
            //end game    
                
                Exit();
            }

            field.Select();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            field.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
