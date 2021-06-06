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
    public class MainMenuScreen : Screen
    {
        public MainMenuScreen(MatchGame game, GraphicsDeviceManager graphics, ContentManager content) : base(game, graphics, content)
        {
            Texture2D match3 = content.Load<Texture2D>("text/Match_3");
            Text match3Text = new Text(match3, new Vector2(0, 20));

            Texture2D classic = content.Load<Texture2D>("text/Classic");
            Text classicText = new Text(classic, new Vector2(300, 80));

            Texture2D play = content.Load<Texture2D>("buttons/Play");
            Button playButton = new Button(play, new Vector2(50, 180));
            playButton.click += Play;

            Texture2D highScores = content.Load<Texture2D>("buttons/High_scores");
            Button highScoresButton = new Button(highScores, new Vector2(50, 320));
            highScoresButton.click += HighScores;

            Texture2D quit = content.Load<Texture2D>("buttons/Quit");
            Button quitButton = new Button(quit, new Vector2(50, 460));
            quitButton.click += Quit;



            this.screenContent = new List<ScreenContent>() { match3Text, classicText, playButton, 
                highScoresButton, quitButton };
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

        public void Play(object button, EventArgs args)
        {
            game.ChangeScreen(new GameScreen(game, graphics, content));
        }

        public void HighScores(object button, EventArgs args)
        {
            Console.WriteLine("HighScores not implemented yet");
        }

        public void Quit(object button, EventArgs args)
        {
            game.ChangeScreen(new QuitScreen(game, graphics, content));
        }

        public override void Update(GameTime gameTime, MouseState current, MouseState previous, KeyboardState currentKeyboard, KeyboardState previousKeyboard)
        {
            foreach (ScreenContent content in screenContent)
            {
                content.Update(current, previous);
            }
        }
    }
}
