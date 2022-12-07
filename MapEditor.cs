using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTSProject
{
    public partial class MapEditor : Form
    {
        public MapEditor()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.Create(@"data\maps\NewMap.txt");
            var data = File.ReadAllText(@"data\maps\map1.txt").Split('\n', ':');
            var mapData = new Dictionary<string, object>();
            for (int i = 0; i < data.Length - 1; i += 2)
            {
                mapData.Add(data[i].Trim(), data[i + 1].Trim('\r', ' '));
            }
            mapData["name"] = textBox1.Text;
            mapData["width"] = textBox1.Text;
            mapData["height"] = textBox2.Text;
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(e.Location.ToString());
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Map.Draw(e.Graphics);
        }

        private void Form3_Load(object sender, EventArgs e)
        {
           //Map.Create(@"data\maps\map1.txt");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new StartMenu().Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Map.Draw(e.Graphics);
        }
    }
}
