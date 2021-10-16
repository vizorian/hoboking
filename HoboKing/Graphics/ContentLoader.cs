using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HoboKing.Graphics
{
    static class ContentLoader
    {
        // Grass tiles
        public static Texture2D GrassNW { get; set; }
        public static Texture2D GrassN { get; set; }
        public static Texture2D GrassNE { get; set; }
        public static Texture2D GrassE { get; set; }
        public static Texture2D GrassSE { get; set; }
        public static Texture2D GrassS { get; set; }
        public static Texture2D GrassSW { get; set; }
        public static Texture2D GrassW { get; set; }
        public static Texture2D GrassCenter { get; set; }
        public static Texture2D GrassLeft { get; set; }
        public static Texture2D GrassRight { get; set; }

        // Ice tiles
        public static Texture2D IceNW { get; set; }
        public static Texture2D IceN { get; set; }
        public static Texture2D IceNE { get; set; }
        public static Texture2D IceE { get; set; }
        public static Texture2D IceSE { get; set; }
        public static Texture2D IceS { get; set; }
        public static Texture2D IceSW { get; set; }
        public static Texture2D IceW { get; set; }
        public static Texture2D IceCenter { get; set; }
        public static Texture2D IceLeft { get; set; }
        public static Texture2D IceRight { get; set; }

        // Sand tiles
        public static Texture2D SandNW { get; set; }
        public static Texture2D SandN { get; set; }
        public static Texture2D SandNE { get; set; }
        public static Texture2D SandE { get; set; }
        public static Texture2D SandSE { get; set; }
        public static Texture2D SandS { get; set; }
        public static Texture2D SandSW { get; set; }
        public static Texture2D SandW { get; set; }
        public static Texture2D SandCenter { get; set; }
        public static Texture2D SandLeft { get; set; }
        public static Texture2D SandRight { get; set; }

        // Other
        public static Texture2D BatChest { get; set; }
        public static Texture2D TileTexture { get; set; }
        public static Texture2D TileLeftTexture { get; set; }
        public static Texture2D TileRightTexture { get; set; }
        public static SpriteFont Font { get; set; }
        public static Texture2D Woodcutter { get; set; }

        public static void LoadContent(ContentManager contentManager)
        {
            // Grass tiles
            GrassNW = contentManager.Load<Texture2D>("Grass blocks\\GrassNW");
            GrassN = contentManager.Load<Texture2D>("Grass blocks\\GrassN");
            GrassNE = contentManager.Load<Texture2D>("Grass blocks\\GrassNE");
            GrassE = contentManager.Load<Texture2D>("Grass blocks\\GrassE");
            GrassSE = contentManager.Load<Texture2D>("Grass blocks\\GrassSE");
            GrassS = contentManager.Load<Texture2D>("Grass blocks\\GrassS");
            GrassSW = contentManager.Load<Texture2D>("Grass blocks\\GrassSW");
            GrassW = contentManager.Load<Texture2D>("Grass blocks\\GrassW");
            GrassCenter = contentManager.Load<Texture2D>("Grass blocks\\GrassCenter");
            GrassLeft = contentManager.Load<Texture2D>("Grass blocks\\GrassLeft");
            GrassRight = contentManager.Load<Texture2D>("Grass blocks\\GrassRight");

            // Ice tiles
            IceNW = contentManager.Load<Texture2D>("Ice blocks\\IceNW");
            IceN = contentManager.Load<Texture2D>("Ice blocks\\IceN");
            IceNE = contentManager.Load<Texture2D>("Ice blocks\\IceNE");
            IceE = contentManager.Load<Texture2D>("Ice blocks\\IceE");
            IceSE = contentManager.Load<Texture2D>("Ice blocks\\IceSE");
            IceS = contentManager.Load<Texture2D>("Ice blocks\\IceS");
            IceSW = contentManager.Load<Texture2D>("Ice blocks\\IceSW");
            IceW = contentManager.Load<Texture2D>("Ice blocks\\IceW");
            IceCenter = contentManager.Load<Texture2D>("Ice blocks\\IceCenter");
            IceLeft = contentManager.Load<Texture2D>("Ice blocks\\IceLeft");
            IceRight = contentManager.Load<Texture2D>("Ice blocks\\IceRight");

            // Sand tiles
            SandNW = contentManager.Load<Texture2D>("Sand blocks\\SandNW");
            SandN = contentManager.Load<Texture2D>("Sand blocks\\SandN");
            SandNE = contentManager.Load<Texture2D>("Sand blocks\\SandNE");
            SandE = contentManager.Load<Texture2D>("Sand blocks\\SandE");
            SandSE = contentManager.Load<Texture2D>("Sand blocks\\SandSE");
            SandS = contentManager.Load<Texture2D>("Sand blocks\\SandS");
            SandSW = contentManager.Load<Texture2D>("Sand blocks\\SandSW");
            SandW = contentManager.Load<Texture2D>("Sand blocks\\SandW");
            SandCenter = contentManager.Load<Texture2D>("Sand blocks\\SandCenter");
            SandLeft = contentManager.Load<Texture2D>("Sand blocks\\SandLeft");
            SandRight = contentManager.Load<Texture2D>("Sand blocks\\SandRight");

            // Other
            BatChest = contentManager.Load<Texture2D>("batchest");
            TileTexture = contentManager.Load<Texture2D>("ground");
            TileLeftTexture = contentManager.Load<Texture2D>("ground_left");
            TileRightTexture = contentManager.Load<Texture2D>("ground_right");
            Woodcutter = contentManager.Load<Texture2D>("Critters\\Hobo Woodcutter\\Woodcutter");
            Font = contentManager.Load<SpriteFont>("Debug");
        }
    }
}
