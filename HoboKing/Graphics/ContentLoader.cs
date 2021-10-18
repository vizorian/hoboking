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
        // Main menu buttons
        public static Texture2D StartSingleplayerButton { get; private set; }
        public static Texture2D StartMultiplayerButton { get; private set; }
        public static Texture2D OptionsButton { get; private set; }
        public static Texture2D ExitButton { get; private set; }
        public static Texture2D PauseButton { get; private set; }
        public static Texture2D ResumeButton { get; private set; }
        public static Texture2D ReturnButton { get; private set; }
        public static Texture2D ExitToMenuButton { get; private set; }
        public static Texture2D LoadingScreen { get; private set; }

        // Grass tiles
        public static Texture2D GrassNW { get; private set; }
        public static Texture2D GrassN { get; private set; }
        public static Texture2D GrassNE { get; private set; }
        public static Texture2D GrassE { get; private set; }
        public static Texture2D GrassSE { get; private set; }
        public static Texture2D GrassS { get; private set; }
        public static Texture2D GrassSW { get; private set; }
        public static Texture2D GrassW { get; private set; }
        public static Texture2D GrassCenter { get; private set; }
        public static Texture2D GrassCornerNW { get; private set; }
        public static Texture2D GrassCornerNE { get; private set; }
        public static Texture2D GrassCornerSW { get; private set; }
        public static Texture2D GrassCornerSE { get; private set; }
        public static Texture2D GrassLeft { get; private set; }
        public static Texture2D GrassRight { get; private set; }

        // Ice tiles
        public static Texture2D IceNW { get; private set; }
        public static Texture2D IceN { get; private set; }
        public static Texture2D IceNE { get; private set; }
        public static Texture2D IceE { get; private set; }
        public static Texture2D IceSE { get; private set; }
        public static Texture2D IceS { get; private set; }
        public static Texture2D IceSW { get; private set; }
        public static Texture2D IceW { get; private set; }
        public static Texture2D IceCenter { get; private set; }
        public static Texture2D IceCornerNW { get; private set; }
        public static Texture2D IceCornerNE { get; private set; }
        public static Texture2D IceCornerSW { get; private set; }
        public static Texture2D IceCornerSE { get; private set; }
        public static Texture2D IceLeft { get; private set; }
        public static Texture2D IceRight { get; private set; }

        // Sand tiles
        public static Texture2D SandNW { get; private set; }
        public static Texture2D SandN { get; private set; }
        public static Texture2D SandNE { get; private set; }
        public static Texture2D SandE { get; private set; }
        public static Texture2D SandSE { get; private set; }
        public static Texture2D SandS { get; private set; }
        public static Texture2D SandSW { get; private set; }
        public static Texture2D SandW { get; private set; }
        public static Texture2D SandCenter { get; private set; }
        public static Texture2D SandCornerNW { get; private set; }
        public static Texture2D SandCornerNE { get; private set; }
        public static Texture2D SandCornerSW { get; private set; }
        public static Texture2D SandCornerSE { get; private set; }
        public static Texture2D SandLeft { get; private set; }
        public static Texture2D SandRight { get; private set; }

        // Other
        public static Texture2D BatChest { get; private set; }
        public static Texture2D TileTexture { get; private set; }
        public static Texture2D TileLeftTexture { get; private set; }
        public static Texture2D TileRightTexture { get; private set; }
        public static SpriteFont Font { get; private set; }
        public static Texture2D Woodcutter { get; private set; }

        public static void LoadContent(ContentManager contentManager)
        {
            // Buttons
            StartSingleplayerButton = contentManager.Load<Texture2D>("Buttons\\StartSingleplayerButton");
            StartMultiplayerButton = contentManager.Load<Texture2D>("Buttons\\StartMultiplayerButton");
            OptionsButton = contentManager.Load<Texture2D>("Buttons\\OptionsButton");
            ExitButton = contentManager.Load<Texture2D>("Buttons\\ExitButton");
            //PauseButton = contentManager.Load<Texture2D>("PauseButton");
            //ResumeButton = contentManager.Load<Texture2D>("Buttons\\ResumeButton");
            ReturnButton = contentManager.Load<Texture2D>("Buttons\\ReturnButton");
            ExitToMenuButton = contentManager.Load<Texture2D>("Buttons\\ExittoMenuButton");
            //LoadingScreen = contentManager.Load<Texture2D>("LoadingScreen");

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
            GrassCornerNW = contentManager.Load<Texture2D>("Grass blocks\\GrassCornerNW");
            GrassCornerNE = contentManager.Load<Texture2D>("Grass blocks\\GrassCornerNE");
            GrassCornerSW = contentManager.Load<Texture2D>("Grass blocks\\GrassCornerSW");
            GrassCornerSE = contentManager.Load<Texture2D>("Grass blocks\\GrassCornerSE");
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
            IceCornerNW = contentManager.Load<Texture2D>("Ice blocks\\IceCornerNW");
            IceCornerNE = contentManager.Load<Texture2D>("Ice blocks\\IceCornerNE");
            IceCornerSW = contentManager.Load<Texture2D>("Ice blocks\\IceCornerSW");
            IceCornerSE = contentManager.Load<Texture2D>("Ice blocks\\IceCornerSE");
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
            SandCornerNW = contentManager.Load<Texture2D>("Sand blocks\\SandCornerNW");
            SandCornerNE = contentManager.Load<Texture2D>("Sand blocks\\SandCornerNE");
            SandCornerSW = contentManager.Load<Texture2D>("Sand blocks\\SandCornerSW");
            SandCornerSE = contentManager.Load<Texture2D>("Sand blocks\\SandCornerSE");
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
