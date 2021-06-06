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
        public GameScreen(MatchGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            Texture2D match3 = content.Load<Texture2D>("text/Match_3");
            Text match3Text = new Text(match3, new Vector2(0, 20));

            Texture2D classic = content.Load<Texture2D>("text/Classic");
            Text classicText = new Text(classic, new Vector2(300, 80));

            this.screenContent = new List<ScreenContent>() { match3Text, classicText };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            graphicsDevice.Clear(Color.CornflowerBlue);

            foreach (var content in screenContent)
                
                content.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            //throw new NotImplementedException();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime, MouseState current, MouseState previous)
        {
            foreach (ScreenContent content in screenContent)
            {
                content.Update(current, previous);
            }

            KeyboardState keyboard = Input.GetInput().keyboardState;

            if (keyboard.IsKeyDown(Keys.Escape))
                game.ChangeScreen(new MainMenuScreen(game, graphicsDevice, content));

            keyboard = Input.GetInput().keyboardState;

            if (keyboard.IsKeyDown(Keys.Escape))
                game.ChangeScreen(new MainMenuScreen(game, graphicsDevice, content));

            //throw new NotImplementedException();
        }
    }
}
