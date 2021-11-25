using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing.Graphics
{
    internal static class ContentLoader
    {
        // Grass tiles
        public static Texture2D GrassNw { get; private set; }
        public static Texture2D GrassN { get; private set; }
        public static Texture2D GrassNe { get; private set; }
        public static Texture2D GrassE { get; private set; }
        public static Texture2D GrassSe { get; private set; }
        public static Texture2D GrassS { get; private set; }
        public static Texture2D GrassSw { get; private set; }
        public static Texture2D GrassW { get; private set; }
        public static Texture2D GrassCenter { get; private set; }
        public static Texture2D GrassCornerNw { get; private set; }
        public static Texture2D GrassCornerNe { get; private set; }
        public static Texture2D GrassCornerSw { get; private set; }
        public static Texture2D GrassCornerSe { get; private set; }
        public static Texture2D GrassLeft { get; private set; }
        public static Texture2D GrassRight { get; private set; }

        // Ice tiles
        public static Texture2D IceNw { get; private set; }
        public static Texture2D IceN { get; private set; }
        public static Texture2D IceNe { get; private set; }
        public static Texture2D IceE { get; private set; }
        public static Texture2D IceSe { get; private set; }
        public static Texture2D IceS { get; private set; }
        public static Texture2D IceSw { get; private set; }
        public static Texture2D IceW { get; private set; }
        public static Texture2D IceCenter { get; private set; }
        public static Texture2D IceCornerNw { get; private set; }
        public static Texture2D IceCornerNe { get; private set; }
        public static Texture2D IceCornerSw { get; private set; }
        public static Texture2D IceCornerSe { get; private set; }
        public static Texture2D IceLeft { get; private set; }
        public static Texture2D IceRight { get; private set; }

        // Sand tiles
        public static Texture2D SandNw { get; private set; }
        public static Texture2D SandN { get; private set; }
        public static Texture2D SandNe { get; private set; }
        public static Texture2D SandE { get; private set; }
        public static Texture2D SandSe { get; private set; }
        public static Texture2D SandS { get; private set; }
        public static Texture2D SandSw { get; private set; }
        public static Texture2D SandW { get; private set; }
        public static Texture2D SandCenter { get; private set; }
        public static Texture2D SandCornerNw { get; private set; }
        public static Texture2D SandCornerNe { get; private set; }
        public static Texture2D SandCornerSw { get; private set; }
        public static Texture2D SandCornerSe { get; private set; }
        public static Texture2D SandLeft { get; private set; }
        public static Texture2D SandRight { get; private set; }

        // Fonts
        public static SpriteFont MenuFont { get; private set; }
        public static SpriteFont DebugFont { get; private set; }

        // Other
        public static Texture2D BatChest { get; private set; }
        public static Texture2D TileTexture { get; private set; }
        public static Texture2D TileLeftTexture { get; private set; }
        public static Texture2D TileRightTexture { get; private set; }
        public static Texture2D Woodcutter { get; private set; }
        public static Texture2D Background { get; private set; }

        public static void LoadContent(ContentManager contentManager)
        {
            // Grass tiles
            GrassNw = contentManager.Load<Texture2D>("Grass blocks\\GrassNW");
            GrassN = contentManager.Load<Texture2D>("Grass blocks\\GrassN");
            GrassNe = contentManager.Load<Texture2D>("Grass blocks\\GrassNE");
            GrassE = contentManager.Load<Texture2D>("Grass blocks\\GrassE");
            GrassSe = contentManager.Load<Texture2D>("Grass blocks\\GrassSE");
            GrassS = contentManager.Load<Texture2D>("Grass blocks\\GrassS");
            GrassSw = contentManager.Load<Texture2D>("Grass blocks\\GrassSW");
            GrassW = contentManager.Load<Texture2D>("Grass blocks\\GrassW");
            GrassCenter = contentManager.Load<Texture2D>("Grass blocks\\GrassCenter");
            GrassCornerNw = contentManager.Load<Texture2D>("Grass blocks\\GrassCornerNW");
            GrassCornerNe = contentManager.Load<Texture2D>("Grass blocks\\GrassCornerNE");
            GrassCornerSw = contentManager.Load<Texture2D>("Grass blocks\\GrassCornerSW");
            GrassCornerSe = contentManager.Load<Texture2D>("Grass blocks\\GrassCornerSE");
            GrassLeft = contentManager.Load<Texture2D>("Grass blocks\\GrassLeft");
            GrassRight = contentManager.Load<Texture2D>("Grass blocks\\GrassRight");

            // Ice tiles
            IceNw = contentManager.Load<Texture2D>("Ice blocks\\IceNW");
            IceN = contentManager.Load<Texture2D>("Ice blocks\\IceN");
            IceNe = contentManager.Load<Texture2D>("Ice blocks\\IceNE");
            IceE = contentManager.Load<Texture2D>("Ice blocks\\IceE");
            IceSe = contentManager.Load<Texture2D>("Ice blocks\\IceSE");
            IceS = contentManager.Load<Texture2D>("Ice blocks\\IceS");
            IceSw = contentManager.Load<Texture2D>("Ice blocks\\IceSW");
            IceW = contentManager.Load<Texture2D>("Ice blocks\\IceW");
            IceCenter = contentManager.Load<Texture2D>("Ice blocks\\IceCenter");
            IceCornerNw = contentManager.Load<Texture2D>("Ice blocks\\IceCornerNW");
            IceCornerNe = contentManager.Load<Texture2D>("Ice blocks\\IceCornerNE");
            IceCornerSw = contentManager.Load<Texture2D>("Ice blocks\\IceCornerSW");
            IceCornerSe = contentManager.Load<Texture2D>("Ice blocks\\IceCornerSE");
            IceLeft = contentManager.Load<Texture2D>("Ice blocks\\IceLeft");
            IceRight = contentManager.Load<Texture2D>("Ice blocks\\IceRight");

            // Sand tiles
            SandNw = contentManager.Load<Texture2D>("Sand blocks\\SandNW");
            SandN = contentManager.Load<Texture2D>("Sand blocks\\SandN");
            SandNe = contentManager.Load<Texture2D>("Sand blocks\\SandNE");
            SandE = contentManager.Load<Texture2D>("Sand blocks\\SandE");
            SandSe = contentManager.Load<Texture2D>("Sand blocks\\SandSE");
            SandS = contentManager.Load<Texture2D>("Sand blocks\\SandS");
            SandSw = contentManager.Load<Texture2D>("Sand blocks\\SandSW");
            SandW = contentManager.Load<Texture2D>("Sand blocks\\SandW");
            SandCenter = contentManager.Load<Texture2D>("Sand blocks\\SandCenter");
            SandCornerNw = contentManager.Load<Texture2D>("Sand blocks\\SandCornerNW");
            SandCornerNe = contentManager.Load<Texture2D>("Sand blocks\\SandCornerNE");
            SandCornerSw = contentManager.Load<Texture2D>("Sand blocks\\SandCornerSW");
            SandCornerSe = contentManager.Load<Texture2D>("Sand blocks\\SandCornerSE");
            SandLeft = contentManager.Load<Texture2D>("Sand blocks\\SandLeft");
            SandRight = contentManager.Load<Texture2D>("Sand blocks\\SandRight");

            // Fonts
            MenuFont = contentManager.Load<SpriteFont>("Fonts\\MenuFont");
            DebugFont = contentManager.Load<SpriteFont>("Fonts\\Debug");

            // Other
            BatChest = contentManager.Load<Texture2D>("batchest");
            TileTexture = contentManager.Load<Texture2D>("ground");
            TileLeftTexture = contentManager.Load<Texture2D>("ground_left");
            TileRightTexture = contentManager.Load<Texture2D>("ground_right");
            Woodcutter = contentManager.Load<Texture2D>("Critters\\Hobo Woodcutter\\Woodcutter");
            Background = contentManager.Load<Texture2D>("Background");
        }
    }
}