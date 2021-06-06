using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Match3.Controls
{
    public class Input
    {
        private static Input input;

        public MouseState mouseState;
        public KeyboardState keyboardState;

        private Input()
        {
           keyboardState = Keyboard.GetState();
           mouseState = Mouse.GetState();
        }

        public static Input GetInput()
        {
            input = new Input();
            return input;
        }

        public static Input GetOldInput()
        {
            if (input == null)
                input = new Input();

            return input;
        }
    }
}
