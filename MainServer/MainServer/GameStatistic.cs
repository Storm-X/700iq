using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace MainServer
{
    public class GameStatistic
    {
        public Label gameZone;
        public Panel Container;
        public Label iCon;
        public Button stopButton;
        public Button restartIcon;
        private Timer tmr;
        public GameStatistic(int zoneName,string iqon,int x,int y)
        {
            Container = new Panel
            {
                Parent = Form1.ActiveForm,
                Size = new Size(400,40),
                Location = new Point(x, y),
            };


            gameZone = new Label
                {  
                    Parent=Container,
                    Text = zoneName.ToString(),
                    Location = new Point(34,5),
                    Size=new Size(30,30),
            };
                iCon = new Label
                {
                    Parent = Container,
                    Text = iqon,
                    Size = new Size(30, 30),
                    Location = new Point(120,5),
                };
                stopButton = new Button
                {
                    Parent = Container,
                    Text = "Пауза",
                    Size = new Size(70, 20),
                    Location = new Point(205,5),
                    Tag = zoneName
                };

                restartIcon = new Button
               {
                  Parent = Container,
                  Text = "Рестарт айкона",
                  Size = new Size(70, 40),
                  Location = new Point(280, 5),
                  Tag = zoneName
               };
        }
    }
}
