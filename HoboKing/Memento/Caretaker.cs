using System;
using System.IO;
using HoboKing.Entities;
using HoboKing.Mediator;
using Microsoft.Xna.Framework;

namespace HoboKing.Memento
{
    class Caretaker : BaseComponent
    {
        private Snapshot snapshot;
        private Player player;

        public void SetPlayer(Player player)
        {
            this.player = player;
        }

        public void Backup()
        {
            snapshot = player.Save();
            using var fileStream = File.OpenWrite("memento/snapshot.txt");
            using var writer = new StreamWriter(fileStream);
            writer.WriteLine(snapshot.GetPosition().X + "," + snapshot.GetPosition().Y + "," + snapshot.GetDateTime());
            writer.Close();
            fileStream.Close();
        }

        public void Restore()
        {
            using var fileStream = File.OpenRead("memento/snapshot.txt");
            using var reader = new StreamReader(fileStream);
            string line = reader.ReadLine();
            if (line != null)
            {
                var words = line.Split(',');
                reader.Close();
                var v = new Vector2(Convert.ToInt32(words[0]), Convert.ToInt32(words[1]));
                var s = new Snapshot(v);
                player.Restore(s);
            }
            
        }

    }
}
