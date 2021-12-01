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
        public abstract  void Destroy();
        public abstract void SetVisible(bool visible);
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
            mainMenuItems = new MenuItemsComponent("Main Menu", menuPosition, 2f);
            mainMenuItems.Add(new MenuItem("Start", new Vector2(menuPosition.X, menuPosition.Y + 70), 0.6f));
            mainMenuItems.Add(new MenuItemsComponent("Options", new Vector2(menuPosition.X, menuPosition.Y + 140), 0.6f));
            mainMenuItems.Add(new MenuItem("Exit Game", new Vector2(menuPosition.X, menuPosition.Y + 210), 0.6f));

            // Options menu


            mainMenuItems.Print();
            mainMenu = new MenuComponent(game, mainMenuItems);
            new GameScene(Game, mainMenu);
        }

        public override void Destroy()
        {
            Game.Components.Remove(mainMenu);
        }
        public override void SetVisible(bool visible)
        {
            mainMenu.Visible = visible;
        }
    }

    //internal class Options : GameState
    //{
    //    private MenuComponent mainMenu;
    //    private MenuItemsComponent mainMenuItems;

    //    public Options(HoboKingGame game, GraphicsDevice graphics)
    //    {
    //        Game = game;
    //        var menuPosition = new Vector2(graphics.Viewport.Width / 4f, 600);
    //        mainMenuItems = new MenuItemsComponent("Main Menu", menuPosition, 2f);
    //        mainMenuItems.Add(new MenuItem("Start", new Vector2(menuPosition.X, menuPosition.Y + 70), 0.6f));
    //        mainMenuItems.Add(new MenuItem("Options", new Vector2(menuPosition.X, menuPosition.Y + 140), 0.6f));
    //        mainMenuItems.Add(new MenuItem("Exit Game", new Vector2(menuPosition.X, menuPosition.Y + 210), 0.6f));
    //        mainMenuItems.Print();
    //        mainMenu = new MenuComponent(game, mainMenuItems);
    //        new GameScene(Game, mainMenu);
    //    }

    //    public override void Destroy()
    //    {
    //        Game.Components.Remove(mainMenu);
    //    }
    //    public override void SetVisible(bool visible)
    //    {
    //        mainMenu.Visible = visible;
    //    }
    //}

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

        public override void SetVisible(bool visible)
        {
            mapComponent.Visible = visible;
        }
    }


    //internal class PauseMenu : GameState
    //{
    //    private MenuComponent OptionsMenu;
    //    private MenuItemsComponent optionsMenuItems;

    //    public PauseMenu(HoboKingGame game, GraphicsDevice graphics)
    //    {
    //        Game = game;
    //        var menuPosition = new Vector2(graphics.Viewport.Width / 4f, 600);
    //        optionsMenuItems = new MenuItemsComponent(game, menuPosition, Color.White, Color.Green, 1);
    //        optionsMenuItems.AddItem("Return");
    //        optionsMenuItems.AddItem("Exit Game");
    //        OptionsMenu = new MenuComponent(game, optionsMenuItems);
    //        //Game.Components.Add(OptionsMenu);
    //        new GameScene(game, OptionsMenu, optionsMenuItems);
    //    }

    //    public override void Destroy()
    //    {
    //        Game.Components.Remove(OptionsMenu);
    //    }

    //    public override void SetVisible(bool visible)
    //    {
    //        OptionsMenu.Visible = visible;
    //    }
    //}
}
