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
    public class QuitScreen : Screen
    {
        public QuitScreen(MatchGame game, GraphicsDeviceManager graphics, ContentManager content) : base(game, graphics, content)
        {
            Texture2D match3 = content.Load<Texture2D>("text/Match_3");
            Text match3Text = new Text(match3, new Vector2(0, 20));

            Texture2D classic = content.Load<Texture2D>("text/Classic");
            Text classicText = new Text(classic, new Vector2(300, 80));

            Texture2D areyousure = content.Load<Texture2D>("text/Are you sure");
            Text areYouSure = new Text(areyousure, new Vector2(100, 150));

            Texture2D yes = content.Load<Texture2D>("buttons/Yes");
            Button yesButton = new Button(yes, new Vector2(50, 320));
            yesButton.click += Yes;

            Texture2D no = content.Load<Texture2D>("buttons/No");
            Button noButton = new Button(no, new Vector2(300, 320));
            noButton.click += No;

            this.screenContent = new List<ScreenContent>() { match3Text, classicText, areYouSure,
                yesButton, noButton };
        }

        public void No(object button, EventArgs args)
        {
            game.ChangeScreen(new MainMenuScreen(game, graphics, content));
        }

        public void Yes(object button, EventArgs args)
        {
            game.Exit();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (ScreenContent content in screenContent)
            {
                content.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime, MouseState current, MouseState previous)
        {
            foreach (ScreenContent content in screenContent)
            {
                content.Update(current, previous);
            }
        }
    }
}
