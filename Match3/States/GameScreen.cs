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

            //for resize purposes, resize later
            float fieldHeight = 450, fieldWidth = 450, fieldOffset = 5f;

            float heightOffset = (game.Window.ClientBounds.Height - fieldOffset) - fieldHeight;         
            float widthOffset =  (game.Window.ClientBounds.Height - fieldOffset) - fieldHeight;

            float figureSize = 50;

            string[] texturePathPrefixes = new string[3] { "fields/", "textures/", "effects/"};

            Vector2[] fieldBounds = new Vector2[4]
            {
                new Vector2(fieldOffset, heightOffset),                                                        //topleft
                new Vector2(fieldOffset + fieldWidth, heightOffset),                               //topright
                new Vector2(fieldOffset, heightOffset + fieldHeight),                               //bottomleft
                new Vector2(fieldOffset + fieldWidth, heightOffset + fieldHeight)       //bottomright
            };

            this.field = new Field(figureTextures, figureAnimationTextures, effectsTextures, figureSize, fieldOffset, fieldBounds, content, texturePathPrefixes);

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (var content in screenContent)
                
                content.Draw(gameTime, spriteBatch);

            field.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime, MouseState current, MouseState previous, KeyboardState currentKeyboard, KeyboardState previousKeyboard)
        {
            foreach (ScreenContent content in screenContent)
            {
                content.Update(current, previous);
            }

            field.Update(current, previous);

            if ((currentKeyboard.IsKeyDown(Keys.Escape) == true)
                &&
                (previousKeyboard.IsKeyDown(Keys.Escape) == false)
                )
                game.ChangeScreen(new MainMenuScreen(game, graphics, content));

        }
    }
}
