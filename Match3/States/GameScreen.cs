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

        public GameScreen(MatchGame game, GraphicsDeviceManager graphics, ContentManager content) : base(game, graphics, content)
        { 
            Texture2D match3 = content.Load<Texture2D>("text/Match_3");
            Text match3Text = new Text(match3, new Vector2(0, 20));

            Texture2D classic = content.Load<Texture2D>("text/Classic");
            Text classicText = new Text(classic, new Vector2(300, 80));

            this.screenContent = new List<ScreenContent>() { match3Text, classicText };

            //for resize purposes, resize later
            float[] fieldDimensions = new float[2] { 450, 450 };
            float heightOffset = (game.Window.ClientBounds.Height) - fieldDimensions[1];         
            string[] pathPrefixes = new string[3] { "fields/", "textures/", "effects/"};

            field = new Field(heightOffset, fieldDimensions, content, pathPrefixes);
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
