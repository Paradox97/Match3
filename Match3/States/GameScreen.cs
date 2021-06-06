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

namespace Match3.States
{
    public class GameScreen : Screen
    {
        Field field;


        public Texture2D[][]
            textTextures,
            buttonTextures,
            figureTextures,
            figureAnimationTextures;

        public Texture2D[]
            effectsTextures,
            fieldTextures;

        public GameScreen(MatchGame game, GraphicsDeviceManager graphics, ContentManager content) : base(game, graphics, content)
        { 
            Texture2D match3 = content.Load<Texture2D>("text/Match_3");
            Text match3Text = new Text(match3, new Vector2(0, 20));

            Texture2D classic = content.Load<Texture2D>("text/Classic");
            Text classicText = new Text(classic, new Vector2(300, 80));

            this.screenContent = new List<ScreenContent>() { match3Text, classicText };

            #region text textures

            this.textTextures = new Texture2D[5][];  //menu textures and whatnot

            this.textTextures[0] = new Texture2D[1]
            {
                content.Load<Texture2D>("text/Main_menu")
            };

            this.textTextures[1] = new Texture2D[2]
            {
                content.Load<Texture2D>("text/Match_3"),
                content.Load<Texture2D>("text/Classic")
            };

            this.textTextures[2] = new Texture2D[1]
            {
                content.Load<Texture2D>("text/Game_over")
            };

            this.textTextures[3] = new Texture2D[1]
            {
                content.Load<Texture2D>("text/High_scores")
            };


            this.textTextures[4] = new Texture2D[1]
            {
                content.Load<Texture2D>("text/Are you sure")
            };

            #endregion

            #region button textures

            this.buttonTextures = new Texture2D[5][];  //button textures

            this.buttonTextures[0] = new Texture2D[3]
            {
                content.Load<Texture2D>("buttons/Play"),
                content.Load<Texture2D>("buttons/High_scores"),
                content.Load<Texture2D>("buttons/Quit")
            };

            this.buttonTextures[1] = null;

            this.buttonTextures[2] = new Texture2D[1]
            {
                content.Load<Texture2D>("buttons/Ok")
            };

            this.buttonTextures[3] = new Texture2D[1]
            {
                content.Load<Texture2D>("text/High_scores")
            };

            this.buttonTextures[4] = new Texture2D[2]
            {
                content.Load<Texture2D>("buttons/Yes"),
                content.Load<Texture2D>("buttons/No")
            };
            #endregion

            #region figure textures

            this.figureTextures = new Texture2D[5][];

            this.figureTextures[0] = new Texture2D[4]
           {
                content.Load<Texture2D>("textures/circle/circle"),
                content.Load<Texture2D>("textures/circle/circle_linehor"),
                content.Load<Texture2D>("textures/circle/circle_linevert"),
                content.Load<Texture2D>("textures/circle/circle_bomb")
           };

            this.figureTextures[1] = new Texture2D[4]
           {
                content.Load<Texture2D>("textures/crystall/crystall"),
                content.Load<Texture2D>("textures/crystall/crystall_linehor"),
                content.Load<Texture2D>("textures/crystall/crystall_linevert"),
                content.Load<Texture2D>("textures/crystall/crystall_bomb")
           };

            this.figureTextures[2] = new Texture2D[4]
           {
                content.Load<Texture2D>("textures/heart/heart"),
                content.Load<Texture2D>("textures/heart/heart_linehor"),
                content.Load<Texture2D>("textures/heart/heart_linevert"),
                content.Load<Texture2D>("textures/heart/heart_bomb")
           };

            this.figureTextures[3] = new Texture2D[4]
           {
                content.Load<Texture2D>("textures/pyramid/pyramid"),
                content.Load<Texture2D>("textures/pyramid/pyramid_linehor"),
                content.Load<Texture2D>("textures/pyramid/pyramid_linevert"),
                content.Load<Texture2D>("textures/pyramid/pyramid_bomb")
           };

            this.figureTextures[4] = new Texture2D[4]
           {
                content.Load<Texture2D>("textures/square/square"),
                content.Load<Texture2D>("textures/square/square_linehor"),
                content.Load<Texture2D>("textures/square/square_linevert"),
                content.Load<Texture2D>("textures/square/square_bomb")
           };
            #endregion

            #region figure animation

            this.figureAnimationTextures = new Texture2D[5][];

            this.figureAnimationTextures[0] = new Texture2D[3]
           {
                content.Load<Texture2D>("textures/circle/circle_shine1"),
                content.Load<Texture2D>("textures/circle/circle_shine2"),
                content.Load<Texture2D>("textures/circle/circle_shine3")
           };

            this.figureAnimationTextures[1] = new Texture2D[3]
           {
                content.Load<Texture2D>("textures/crystall/crystall_shine1"),
                content.Load<Texture2D>("textures/crystall/crystall_shine2"),
                content.Load<Texture2D>("textures/crystall/crystall_shine3")
           };

            this.figureAnimationTextures[2] = new Texture2D[3]
           {
                content.Load<Texture2D>("textures/heart/heart_shine1"),
                content.Load<Texture2D>("textures/heart/heart_shine2"),
                content.Load<Texture2D>("textures/heart/heart_shine3")
           };

            this.figureAnimationTextures[3] = new Texture2D[3]
           {
                content.Load<Texture2D>("textures/pyramid/pyramid_shine1"),
                content.Load<Texture2D>("textures/pyramid/pyramid_shine2"),
                content.Load<Texture2D>("textures/pyramid/pyramid_shine3")
           };

            this.figureAnimationTextures[4] = new Texture2D[3]
           {
                content.Load<Texture2D>("textures/square/square_shine1"),
                content.Load<Texture2D>("textures/square/square_shine2"),
                content.Load<Texture2D>("textures/square/square_shine3")
           };
            #endregion

            #region effects textures

            this.effectsTextures = new Texture2D[5] {
                content.Load<Texture2D>("effects/blast"),
                content.Load<Texture2D>("effects/destroyer_down"),
                content.Load<Texture2D>("effects/destroyer_left"),
                content.Load<Texture2D>("effects/destroyer_right"),
                content.Load<Texture2D>("effects/destroyer_up")
                    };
            #endregion

            #region field textures

            this.fieldTextures = new Texture2D[1]
            {
                content.Load<Texture2D>("fields/field_1")
            };

            #endregion

            float fieldOffset = 5f;

            float heightOffset = (game.Window.ClientBounds.Height - fieldOffset) - fieldTextures[0].Height;          //for resize purposes, resize later

            Vector2[] fieldBounds = new Vector2[4]
            {
                new Vector2(fieldOffset, heightOffset),                                                        //topleft
                new Vector2(fieldOffset + fieldTextures[0].Width, heightOffset),                               //topright
                new Vector2(fieldOffset, heightOffset + fieldTextures[0].Height),                               //bottomleft
                new Vector2(fieldOffset + fieldTextures[0].Width, heightOffset + fieldTextures[0].Height)       //bottomright
            };

            this.field = new Field(figureTextures, figureAnimationTextures, effectsTextures, fieldTextures, fieldBounds, fieldOffset);

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (var content in screenContent)
                
                content.Draw(gameTime, spriteBatch);

            field.FieldDraw(spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime, MouseState current, MouseState previous)
        {
            foreach (ScreenContent content in screenContent)
            {
                content.Update(current, previous);
            }

            field.Update(current, previous);

            KeyboardState keyboard = Input.GetInput().keyboardState;

            if (keyboard.IsKeyDown(Keys.Escape))
                game.ChangeScreen(new MainMenuScreen(game, graphics, content));

            keyboard = Input.GetInput().keyboardState;

            if (keyboard.IsKeyDown(Keys.Escape))
                game.ChangeScreen(new MainMenuScreen(game, graphics, content));
        }
    }
}
