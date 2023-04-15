using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snake_socket
{
    class Player
    {
        private Image img;
        public PictureBox[,] pbxMap;
        public int playerI, playerJ, smer;

        public Player(Image myImg) {
            this.pbxMap = Form1.pictureBoxMap;
            this.smer = 1;
            Random rnd = new Random();
            playerI = rnd.Next(pbxMap.GetLength(0));
            playerJ = rnd.Next(pbxMap.GetLength(1));
            img = myImg;
            pbxMap[playerI, playerJ].BackgroundImage = myImg;
        }

        public void Move() {
            switch (smer) {
                case 1:
                    //move up
                    if (playerI == 0)
                        break;
                    pbxMap[playerI - 1, playerJ].BackgroundImage = img;
                    pbxMap[playerI, playerJ].BackgroundImage = Form1.images["default"];
                    --playerI;
                    break;
                case -1:
                    //move down
                    if (playerI + 1 >= pbxMap.GetLength(0))
                        break;
                    pbxMap[playerI + 1, playerJ].BackgroundImage = img;
                    pbxMap[playerI, playerJ].BackgroundImage = Form1.images["default"];
                    ++playerI;
                    break;
                case 2:
                    //move right
                    if (playerJ + 1 >= pbxMap.GetLength(1))
                        break;
                    pbxMap[playerI, playerJ + 1].BackgroundImage = img;
                    pbxMap[playerI, playerJ].BackgroundImage = Form1.images["default"];
                    ++playerJ;
                    break;
                case -2:
                    //move down
                    if (playerJ == 0)
                        break;
                    pbxMap[playerI, playerJ - 1].BackgroundImage = img;
                    pbxMap[playerI, playerJ].BackgroundImage = Form1.images["default"];
                    --playerJ;
                    break;
            }
        }
    }
}
