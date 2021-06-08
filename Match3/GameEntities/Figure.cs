using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using Match3.ScreenEntities;

namespace Match3.GameEntities
{
    class Figure:ICloneable
    {
        public ContentManager content;

        public string[] texturePaths;

        public string[] effectsPaths;

        public string[] animationPaths;

        public Texture2D[]
            textureset,
            animationset,
            effectsset;

        public Texture2D
            texture;

        public Sprite sprite;
        public string[] pathPrefixes;

        public Vector2[] bounds;
        public Vector2 position;

        public int figureType;

        public int
            state, stateByTime = 15;
        
        int stateOfAnimation;

        const int ANIMATIONTIME = 1;

        public bool
            isSelected,
            isDragged,
            isBlownUp,
            isFalling,
            isDestroyed;

        private struct State{
            
            public string[] texturePaths;

            public string[] effectsPaths;

            public string[] animationPaths;

            public Vector2[] bounds;

            public Vector2 position;

            public int type;
        }

        private struct animationState
        {
            public Vector2 position;
        }

        private List<animationState> threadUnsafeAnimation;

        private List<State> states;

        enum FigureClass
        {
            bomb,
            destroyerHorizontal,
            destroyerVertical
        }

        public int subType;

        const int MAX_STATES = 3;
        public void Bomb()
        {

        }

        public void HorizontalDestroyer()
        {

        }

        public void VerticalDestroyer()
        {

        }

        public Figure(Figure figure, int newType, Field field, ContentManager content) //creating figure from another one
        {
            position = figure.position;
            bounds = figure.bounds;
            figureType = newType;

            this.content = content;

            this.pathPrefixes = field.figurePrefixes;
            this.texturePaths = field.figureTexturePaths[figureType];
            this.animationPaths = field.figureAnimationPaths[figureType];
            this.effectsPaths = field.effectsPaths;

            this.sprite = new Sprite(pathPrefixes[0] + texturePaths[0], position, content);
            this.texture = sprite.texture;

            states = new List<State>();
            threadUnsafeAnimation = new List<animationState>();
            Falldown();
        }

        public void Falldown()
        {
            for (int i = 200; i > -10;)
            {
                threadUnsafeAnimation.Add(new animationState() { position = this.position - new Vector2(0, i) });
                i = i - 10;
            }
        }

        public Figure(Vector2 position, Vector2[] bounds, int type, string[] prefixes, string[] texturePaths, string[] animationPaths, string[] effectsPaths, ContentManager content)
        {
            this.position = position;
            this.bounds = bounds;
            this.figureType = type;

            this.content = content;

            this.pathPrefixes = prefixes;
            this.texturePaths = texturePaths;
            this.animationPaths = animationPaths;
            this.effectsPaths = effectsPaths;

            this.sprite = new Sprite(pathPrefixes[0] + texturePaths[0], position, content);
            this.texture = sprite.texture;

            states = new List<State>();
            threadUnsafeAnimation = new List<animationState>();
            Falldown();
        }

        public bool IsBusy()
        {
            if (threadUnsafeAnimation.Count != 0)
                return true;

            return false;
        }

        public void Change(Figure next)
        {
            states.Add(new State {position = next.position, bounds = next.bounds});

            Console.WriteLine(states[0].type + "DSDSDSDAD");
        }

        public void ChangePosition(Figure next)
        {



        }

        public void Update()
        {

            if (states.Count != 0)
            {
                Console.WriteLine(states[0].type + "DSDSDSDAD");


                /*
                figureType = states[0].type;

                texturePaths = states[0].texturePaths;

                animationPaths = states[0].animationPaths;

                effectsPaths = states[0].effectsPaths;
                */
                position = states[0].position;
                bounds = states[0].bounds;
            }

            Animate();
            //texture = content.Load<Texture2D>(pathPrefixes[0] + texturePaths[0]);
        }

        public void PostUpdate()
        {
            ReAnimate();

            if (states.Count != 0)
            {
                sprite = new Sprite(pathPrefixes[0] + texturePaths[0], position, content);
                states.RemoveAt(0);
            }

         /*   if (nextAnimationStates.Count != 0)
            {
                sprite = new Sprite(pathPrefixes[0] + texturePaths[0], position, content);
                nextAnimationStates.RemoveAt(0);
            }
         */
        }

        public void Animate()
        {
            if (threadUnsafeAnimation.Count == 0)
                return;
            
            stateOfAnimation += 1;

            if (stateOfAnimation % ANIMATIONTIME == 0)
            {
                position = threadUnsafeAnimation[0].position;
            }
        }

        public void ReAnimate()
        {
            if ((stateOfAnimation % ANIMATIONTIME == 0) && (threadUnsafeAnimation.Count != 0))
            {
                sprite = new Sprite(pathPrefixes[0] + texturePaths[0], position, content);
                threadUnsafeAnimation.RemoveAt(0);
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            sprite.Draw(spritebatch);
        }

        public void Next()
        {
            state += 1;
            this.texture = animationset[(state/stateByTime) % MAX_STATES];
        }

        public void FigureSelect()
        {
            this.isSelected = true;
        }

        public void FigureDeselect()
        {
            this.isSelected = false;
        }

        public void FigureBlast()
        {
            this.isBlownUp = true;
        }

        public void FigureDestroyDown()
        {
            this.isDestroyed = true;
            this.texture = effectsset[1];
        }

        public void FigureDestroyLeft()
        {
            this.isDestroyed = true;
            this.texture = effectsset[2];
        }

        public void FigureDestroyRight()
        {
            this.isDestroyed = true;
            this.texture = effectsset[3];
        }

        public void FigureDestroyUp()
        {
            this.isDestroyed = true;
            this.texture = effectsset[4];
        }

        public int FigureAnimate()            //some animations may require completion
        {
            if (this.isSelected == true)
            {
                state = state + 1;
                this.texture = animationset[(state / stateByTime) % MAX_STATES];
                return 1;
            }

            if (this.isBlownUp == true)
            {
                this.isBlownUp = false;
                this.texture = effectsset[0];
                return 2;       
            }

            if (this.isDestroyed == true)
            {
                this.isDestroyed = false;
                this.isBlownUp = true;
                return 3;
            }

            state = 0;

            return 0;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void FigureCreateOfType(int type)
        {
            this.figureType = type;
        }
        
        
        public void FigureCreateNotOfType(int type)             
        {
            /*
            FigureType[] figures = ((FigureType[])Enum.GetValues(typeof(FigureType)));

            List<FigureType> figuresSubSet = new List<FigureType>();

            for (int i = 0; i< figures.Length; i++)
            {
                if ((int)figures[i] != type)
                    figuresSubSet.Add(figures[i]);
            }

            this.figureType = (int)figuresSubSet[new Random().Next(1, figuresSubSet.Count - 1)];
            */
        }
       

    }
}
