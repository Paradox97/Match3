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
    class FigureState
    {
        public string[] texturePaths;

        public string[] effectsPaths;

        public string[] animationPaths;

        public Vector2[] bounds;

        public Vector2 position;

        public int type;

        public int subType;

        public FigureState(Vector2 position)
        {
            this.position = position;
        }

        public FigureState(Figure figure) //
        {
            int type = figure.figureType;
            this.type = type;

            int subType = figure.subType;
            this.subType = subType;

            string[] texturepaths = figure.texturePaths;
            this.texturePaths = texturepaths;

            string[] effectspaths = figure.effectsPaths;
            this.effectsPaths = effectspaths;

            string[] animationpaths = figure.animationPaths;
            this.animationPaths = animationpaths;
        }
    }
}
