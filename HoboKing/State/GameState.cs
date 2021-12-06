using System;
using HoboKing.Components;
using HoboKing.Composite;
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
            var menuPosition = new Vector2(graphics.Viewport.Width / 4f, 600);

            // Main menu
            mainMenuItems = new MenuItemsComponent("Main Menu", menuPosition, 0.6f);
            mainMenuItems.Add(new MenuItem("Start", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));
            mainMenuItems.Add(new MenuItemsComponent("Options", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));
            mainMenuItems.Add(new MenuItem("Exit Game", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));

            // Options menu
            menuPosition = new Vector2(graphics.Viewport.Width / 4f, 600);
            mainMenuItems.GetChild(1).Add(new MenuItem("Option A", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));
            mainMenuItems.GetChild(1).Add(new MenuItem("Option B", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));
            mainMenuItems.GetChild(1).Add(new MenuItem("Option C", new Vector2(menuPosition.X, menuPosition.Y += 70), 0.6f));
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

        public Playing(HoboKingGame game)
        {
            Game = game;
            connector = new Connector();
            mapComponent = new MapComponent(Game, connector);
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
