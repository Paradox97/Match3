using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;


using System.Timers;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.IO;
using Match3.Controls;
using Match3.GameEntities;
using Match3.ScreenEntities;

namespace Match3.GameEntities
{
    class Paths
    {
        public string[] figurePrefixes;
        
        public string[][]
               figureTexturePaths,
               figureAnimationPaths;
        
        public string[] effectsPaths;

        public Paths(string[] figurePrefixes, string[][] figureTexturePaths, string[][] figureAnimationPaths, string[] effectsPaths)
        {
            this.figurePrefixes = figurePrefixes;
            this.figureTexturePaths = figureTexturePaths;
            this.figureAnimationPaths = figureAnimationPaths;
            this.effectsPaths = effectsPaths;
        }
    }


}
