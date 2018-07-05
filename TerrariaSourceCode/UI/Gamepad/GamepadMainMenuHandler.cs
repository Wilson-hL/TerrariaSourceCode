﻿// Decompiled with JetBrains decompiler
// Type: Terraria.UI.Gamepad.GamepadMainMenuHandler
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.GameInput;

namespace Terraria.UI.Gamepad
{
    public class GamepadMainMenuHandler
    {
        public static int LastMainMenu = -1;
        public static List<Vector2> MenuItemPositions = new List<Vector2>(20);
        public static int LastDrew = -1;
        public static bool CanRun = false;

        public static void Update()
        {
            if (!GamepadMainMenuHandler.CanRun)
            {
                var page = UILinkPointNavigator.Pages[1000];
                page.CurrentPoint = page.DefaultPoint;
                var vector2 =
                    new Vector2((float) Math.Cos((double) Main.GlobalTime * 6.28318548202515),
                        (float) Math.Sin((double) Main.GlobalTime * 6.28318548202515 * 2.0)) * new Vector2(30f, 15f) +
                    Vector2.UnitY * 20f;
                UILinkPointNavigator.SetPosition(2000,
                    new Vector2((float) Main.screenWidth, (float) Main.screenHeight) / 2f + vector2);
            }
            else
            {
                if (!Main.gameMenu || Main.MenuUI.IsVisible || GamepadMainMenuHandler.LastDrew != Main.menuMode)
                    return;
                var lastMainMenu = GamepadMainMenuHandler.LastMainMenu;
                GamepadMainMenuHandler.LastMainMenu = Main.menuMode;
                switch (Main.menuMode)
                {
                    case 17:
                    case 18:
                    case 19:
                    case 21:
                    case 22:
                    case 23:
                    case 24:
                    case 26:
                        if (GamepadMainMenuHandler.MenuItemPositions.Count >= 4)
                        {
                            var menuItemPosition = GamepadMainMenuHandler.MenuItemPositions[3];
                            GamepadMainMenuHandler.MenuItemPositions.RemoveAt(3);
                            if (Main.menuMode == 17)
                            {
                                GamepadMainMenuHandler.MenuItemPositions.Insert(0, menuItemPosition);
                                break;
                            }

                            break;
                        }

                        break;
                    case 28:
                        if (GamepadMainMenuHandler.MenuItemPositions.Count >= 3)
                        {
                            GamepadMainMenuHandler.MenuItemPositions.RemoveAt(1);
                            break;
                        }

                        break;
                }

                var page = UILinkPointNavigator.Pages[1000];
                if (lastMainMenu != Main.menuMode)
                    page.CurrentPoint = page.DefaultPoint;
                for (var index = 0; index < GamepadMainMenuHandler.MenuItemPositions.Count; ++index)
                {
                    if (index == 0 && lastMainMenu != GamepadMainMenuHandler.LastMainMenu &&
                        (PlayerInput.UsingGamepad && Main.InvisibleCursorForGamepad))
                    {
                        Main.mouseX = PlayerInput.MouseX = (int) GamepadMainMenuHandler.MenuItemPositions[index].X;
                        Main.mouseY = PlayerInput.MouseY = (int) GamepadMainMenuHandler.MenuItemPositions[index].Y;
                        Main.menuFocus = -1;
                    }

                    var link = page.LinkMap[2000 + index];
                    link.Position = GamepadMainMenuHandler.MenuItemPositions[index];
                    link.Up = index != 0 ? 2000 + index - 1 : -1;
                    link.Left = -3;
                    link.Right = -4;
                    link.Down = index != GamepadMainMenuHandler.MenuItemPositions.Count - 1 ? 2000 + index + 1 : -2;
                }

                GamepadMainMenuHandler.MenuItemPositions.Clear();
            }
        }
    }
}