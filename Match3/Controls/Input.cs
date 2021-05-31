using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Match3.Controls
{
    class Input
    {
        private static Input input;

        public MouseState mouseInput;
        public Input()
        {
            this.mouseInput = Mouse.GetState();
        }

        public static Input getMouseInput()
        {
            input = new Input();
            return input;
        }
    }
}
