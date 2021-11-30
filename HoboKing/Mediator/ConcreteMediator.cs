using System;
using HoboKing.Components;
using HoboKing.Entities;
using HoboKing.Graphics;
using HoboKing.Memento;
using Microsoft.Xna.Framework;

namespace HoboKing.Mediator
{
    class ConcreteMediator : IMediator
    {
        public ConcreteMediator(MapComponent mapComponent, Player playerComponent, Caretaker caretakerComponent)
        {
            this.cameraComponent = new Camera();
            this.playerComponent = playerComponent;
            this.caretakerComponent = caretakerComponent;
            this.mapComponent = mapComponent;
            //this.caretakerComponent = caretakerComponent;
        }

        private readonly MapComponent mapComponent;
        private readonly Camera cameraComponent;
        private readonly Player playerComponent;
        private readonly Caretaker caretakerComponent;

        public void Notify(object sender, string ev)
        {
            switch (ev)
            {
                case "cameraFollow":
                    cameraComponent.Follow(playerComponent);
                    break;
                case "save":
                    caretakerComponent.Backup();
                    break;
                case "load":
                    caretakerComponent.Restore();
                    break;
                case "assignPlayer":
                    caretakerComponent.SetPlayer(playerComponent);
                    break;
            }
        }

        public Matrix GetMatrix()
        {
            return cameraComponent.Transform;
        }

        public string GetId()
        {
            return playerComponent.ConnectionId;
        }

        public void SetId(string id)
        {
            playerComponent.ConnectionId = id;
        }

        public Vector2 GetPosition()
        {
            return playerComponent.Position;
        }
    }
}