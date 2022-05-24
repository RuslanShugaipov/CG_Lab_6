using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_6
{
    public partial class Form1 : Form
    {
        private double angleX = 0;
        private double angleY = 0;
        private double angleZ = 45;
        private float I = 0;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            draw_Cube();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            angleX = trackBar1.Value;
            draw_Cube();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            angleY = trackBar2.Value;
            draw_Cube();
        }

        void draw_Cube()
        {
            using (var g = Graphics.FromImage(pictureBox1.Image))
            {
                g.Clear(pictureBox1.BackColor);
                pictureBox1.Invalidate();
                float centerX = (float)pictureBox1.Width / 2;
                float centerY = (float)pictureBox1.Height / 2;
                g.TranslateTransform(centerX, centerY);
                Cube cube = new Cube(100.0f, new int[] { 0, 0, 0 });
                Pen pen = new Pen(Brushes.Red);
                cube.rotationY(angleY);
                cube.rotationX(angleX);
                cube.rotationZ(angleZ);
                cube.Draw(g, pen, pictureBox1, I);
            }
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            I = trackBar4.Value;
            draw_Cube();
        }
    }
}
