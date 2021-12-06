using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
            using (TextWriter writer = new StreamWriter("memento/snapshot.txt", true))
            {
                writer.WriteLine(snapshot.GetPosition().X + "," + snapshot.GetPosition().Y + "," + snapshot.GetDateTime());
            }
        }

        public void Restore()
        {
            string line;
            using (TextReader reader = new StreamReader("memento/snapshot.txt"))
            {
                line = reader.ReadLine();
            }
            var words = line.Split(',');
            var v = new Vector2(Convert.ToInt32(words[0]), Convert.ToInt32(words[1]));
            var s = new Snapshot(v);
            player.Restore(s);
        }

        public void Restore(int i)
        {
            var lines = new List<string>();
            using (TextReader reader = new StreamReader("memento/snapshot.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    lines.Add(line);
            }

            var words = lines[i].Split(',');
            var v = new Vector2(Convert.ToInt32(words[0]), Convert.ToInt32(words[1]));
            var s = new Snapshot(v);

            player.Restore(s);
        }

        public List<(int, int)> GetSaveCount()
        {
            var lines = new List<string>();
            using (TextReader reader = new StreamReader("memento/snapshot.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    lines.Add(line);
            }

            var indexes = new List<(int, int)>();

            for (var i = 0; i < lines.Count; i++)
            {
                var words = lines[i].Split(',');
                var y = double.Parse(words[1]);

                var indexA = Math.Floor(y / 1000);

                var index = (Convert.ToInt32(indexA), i);
                indexes.Add(index);
            }

            return indexes;
        }
    }
}
