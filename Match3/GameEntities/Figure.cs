using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Match3.ScreenEntities;

namespace Match3.GameEntities
{
    class Figure:ICloneable
    {
        public enum FigureType
        {
            Circle = 0,
            Crystall = 1,
            Heart = 2,
            Pyramid = 3,
            Square = 4,
        }

        public Texture2D[]
            textureset,
            animationset,
            effectsset;

        public Texture2D
            texture;

        public Sprite sprite;


        public Vector2[] bounds;
        public Vector2 position;

        public int figureType;

        public int
            state, stateByTime = 15;

        public bool
            isSelected,
            isDragged,
            isBlownUp,
            isDestroyed;

        const int MAX_STATES = 3;
        public Figure(Vector2 position, Vector2[] bounds)
        {
            this.position = position;
            this.bounds = bounds;
            FigureCreate();
        }

        public Figure(Vector2 position, Vector2[] bounds, int type)
        {
            this.position = position;
            this.bounds = bounds;
            this.figureType = type;
        }

        public Figure(Vector2 position, Vector2[] bounds, int type, Texture2D[] textureSet, Texture2D[] animationSet, Texture2D[] effectsSet)
        {
            this.position = position;
            this.bounds = bounds;
            this.figureType = type;
            this.textureset = textureSet;
            this.animationset = animationSet;
            this.effectsset = effectsSet;
            this.texture = textureset[0];
        }

        public void FigureGetTexture(Texture2D[] textureSet, Texture2D[] animationSet, Texture2D[] effectsSet)
        {
            this.textureset = textureSet;
            this.animationset = animationSet;
            this.effectsset = effectsSet;
            this.texture = textureset[0];

            //this.sprite = new Sprite(textureSet, animationSet, effectsSet, position);
        }

        public void Next()
        {
            state += 1;
            this.texture = animationset[(state/stateByTime) % MAX_STATES];
        }

       /* public object Clone()
        {
            return this.MemberwiseClone();
        }
       */

        public void FigureCreate()
        {
            FigureType[] figures = ((FigureType[])Enum.GetValues(typeof(FigureType)));
            this.figureType = (int)figures[new Random().Next(0, figures.Length)];
            // return (int)figureType;
        }

        //events
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
            FigureType[] figures = ((FigureType[])Enum.GetValues(typeof(FigureType)));

            List<FigureType> figuresSubSet = new List<FigureType>();

            for (int i = 0; i< figures.Length; i++)
            {
                if ((int)figures[i] != type)
                    figuresSubSet.Add(figures[i]);
            }

            this.figureType = (int)figuresSubSet[new Random().Next(1, figuresSubSet.Count - 1)];
        }
       

    }
}
