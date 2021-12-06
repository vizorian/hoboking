using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HoboKing.Composite;
using HoboKing.Control;
using HoboKing.Graphics;
using HoboKing.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HoboKing.Components
{
    internal class MenuComponent : DrawableGameComponent
    {
        private readonly HoboKingGame hoboKingGame;
        private readonly List<MenuItemsComponent> oldMenus;
        private MenuItemsComponent targetMenu;

        public MenuComponent(HoboKingGame hoboKingGame, MenuItemsComponent targetMenu) : base(hoboKingGame)
        {
            this.hoboKingGame = hoboKingGame;
            this.targetMenu = targetMenu;
            oldMenus = new List<MenuItemsComponent>();
        }

        [ExcludeFromCodeCoverage]
        public override void Update(GameTime gameTime)
        {
            if (InputController.KeyPressed(Keys.Enter))
            {
                var selection = targetMenu.SelectedMenuItem.Text;
                switch (selection)
                    {
                        case "Start":
                            hoboKingGame.ChangeStateAndDestroy(new Playing(hoboKingGame));
                            break;
                        case "Load":
                            oldMenus.Add(targetMenu);
                            targetMenu = targetMenu.SelectedMenuItem as MenuItemsComponent;
                            break;
                        case "Return":
                            switch (oldMenus.LastOrDefault())
                            {
                                case null:
                                    hoboKingGame.ChangeStateAndDestroy(new Playing(hoboKingGame));
                                    break;
                                default:
                                    targetMenu = oldMenus.Last();
                                    oldMenus.RemoveAt(oldMenus.Count-1);
                                    break;
                            }

                            break;
                        case "Options":
                            oldMenus.Add(targetMenu);
                            targetMenu = targetMenu.SelectedMenuItem as MenuItemsComponent;
                            break;
                        case "Exit Game":
                            hoboKingGame.Exit();
                            break;
                        case "Exit To Menu":
                            hoboKingGame.ChangeStateAndDestroy(new Menu(hoboKingGame, hoboKingGame.GraphicsDevice));
                            break;
                        default:
                            var broken = selection.Split(' ');
                            if (broken[0] == "Section")
                            {
                                oldMenus.Add(targetMenu);
                                targetMenu = targetMenu.SelectedMenuItem as MenuItemsComponent;
                            }else if (broken[1] == "save")
                            {
                                var id = broken[2].Substring(3);
                                hoboKingGame.ChangeStateAndDestroy(new Playing(hoboKingGame, Convert.ToInt32(id)));
                            }
                            break;
                    }


            }
            if (InputController.KeyPressed(Keys.Down))
                targetMenu.Next();
            if (InputController.KeyPressed(Keys.Up))
                targetMenu.Previous();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            hoboKingGame.SpriteBatch.Begin();

            var color = Color.White;
            hoboKingGame.SpriteBatch.DrawString(ContentLoader.MenuFont, targetMenu.Text, new Vector2(targetMenu.GetChild(0).Position.X, targetMenu.GetChild(0).Position.Y - 70), color);

            for (int i = 0; i < targetMenu.GetCount(); i++)
            {
                color = Color.White;
                var item = targetMenu.GetChild(i);
                if (item == targetMenu.SelectedMenuItem)
                    color = Color.Green;

                hoboKingGame.SpriteBatch.DrawString(ContentLoader.MenuFont, item.Text, item.Position, color);
            }
            hoboKingGame.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}