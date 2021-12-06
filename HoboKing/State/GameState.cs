using System;
using System.Linq;
using HoboKing.Components;
using HoboKing.Composite;
using HoboKing.Memento;
using HoboKing.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing.State
{
    internal abstract class GameState
    {
        protected HoboKingGame Game{ get; set; }
        public abstract void Destroy();
    }

    internal class Menu : GameState
    {
        private MenuComponent mainMenu;
        private MenuItemsComponent mainMenuItems;

        public Menu(HoboKingGame game, GraphicsDevice graphics)
        {
            Game = game;
            var menuPosition = new Vector2(graphics.Viewport.Width / 4f, 100);

            // Main menu - fixed size
            mainMenuItems = new MenuItemsComponent("Main Menu", menuPosition, 0.6f);
            mainMenuItems.Add(new MenuItem("Start", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));
            mainMenuItems.Add(new MenuItemsComponent("Load", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));
            mainMenuItems.Add(new MenuItemsComponent("Options", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));
            mainMenuItems.Add(new MenuItem("Exit Game", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));

            // Options menu - fixed size
            menuPosition = new Vector2(graphics.Viewport.Width / 4f, 600);
            mainMenuItems.GetChild(2).Add(new MenuItem("Option A", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));
            mainMenuItems.GetChild(2).Add(new MenuItem("Option B", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));
            mainMenuItems.GetChild(2).Add(new MenuItem("Option C", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));
            mainMenuItems.GetChild(2).Add(new MenuItem("Return", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));

            // Load menu - dynamic size
            // need to load all saves
            // need to categorize saves by sections (menus)
            menuPosition = new Vector2(graphics.Viewport.Width / 4f, 100);
            var caretaker = new Caretaker();
            var indexes = caretaker.GetSaveCount();

            var uniqueSections = indexes.Select(t => t.Item1).Distinct().ToList();
            uniqueSections.Sort();

            foreach (var section in uniqueSections)
            {
                var menuPositionTemp = new Vector2(graphics.Viewport.Width / 4f, 100);

                var menuSection = new MenuItemsComponent($"Section {section}",
                    new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f);

                var sectionsSaves = indexes.Where(t => t.Item1 == section).Select(s => s.Item2).ToList();

                var i = 0;
                foreach (var menuSave in sectionsSaves.TakeWhile(menuSave => i < 10))
                {
                    menuSection.Add(new MenuItem($"Load save ID:{menuSave}", new Vector2(menuPositionTemp.X, menuPositionTemp.Y += 70), 0.6f));
                    i++;
                }
                menuSection.Add(new MenuItem("Return", new Vector2(menuPositionTemp.X, menuPositionTemp.Y += 70), 0.6f));

                mainMenuItems.GetChild(1).Add(menuSection);

            }

            mainMenuItems.GetChild(1).Add(new MenuItem("Return", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));

            mainMenuItems.Print();
            mainMenu = new MenuComponent(game, mainMenuItems);
            new GameScene(Game, mainMenu);
        }

        public override void Destroy()
        {
            Game.Components.Remove(mainMenu);
        }
    }

    internal class Playing : GameState
    {
        private Connector connector;
        private MapComponent mapComponent;

        public Playing(HoboKingGame game, int loadState = -1)
        {
            Game = game;
            connector = new Connector();
            mapComponent = new MapComponent(Game, connector, loadState);
            new GameScene(Game, mapComponent);
        }

        public override void Destroy()
        {
            Game.Components.Remove(mapComponent);
            _ = connector.Disconnect();
        }
    }


    internal class PauseMenu : GameState
    {
        private MenuComponent optionsMenu;
        private MenuItemsComponent optionsMenuItems;

        public PauseMenu(HoboKingGame game, GraphicsDevice graphics)
        {
            Game = game;
            var menuPosition = new Vector2(graphics.Viewport.Width / 4f, 600);

            // Main menu
            optionsMenuItems = new MenuItemsComponent("Pause Menu", menuPosition, 0.6f);
            optionsMenuItems.Add(new MenuItem("Return", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));
            optionsMenuItems.Add(new MenuItem("Exit To Menu", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));
            optionsMenuItems.Add(new MenuItem("Exit Game", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));

            optionsMenuItems.Print();

            optionsMenu = new MenuComponent(game, optionsMenuItems);
            new GameScene(game, optionsMenu);
        }

        public override void Destroy()
        {
            Game.Components.Remove(optionsMenu);
        }
    }
}
