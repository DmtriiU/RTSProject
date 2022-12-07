using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTSProject
{
    public partial class GameWindow : Form
    {
        List<Unit> selectedObjects;
        List<Unit> selectedTargets;
        Point cursorLocation;
        Point? targetPoint;
        Point x, y;
        Rectangle? rect;

        public GameWindow()
        {
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Map.Create(@"data\maps\map1.txt");
            selectedObjects = new List<Unit>();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Map.Update();
            Refresh();

            //textBox7.Text = $"{Map.units[0].position}";
            //foreach (var unit in selectedObjects)
            //{
            //    if ((int)unit.position.X != targetPoint.X && (int)unit.position.Y != targetPoint.Y && !unit.parameters.enemy)
            //    {
            //        unit.Move(unit, targetPoint);
            //    }
            //}

            //foreach (var unit in selectedObjects)
            //{
            //    if (unit.Path.Count > 0)
            //    {
            //        if (unit.position != unit.Path[unit.Path.Count - 1] && !unit.Parameters.enemy)
            //        {
            //            for (int i = 0; i < unit.Vectors.Count; i++)
            //            {
            //                while (unit.position != unit.Path[i + 1])
            //                {
            //                    unit.Move(unit.Vectors[i]);
            //                    textBox7.Text = $"{unit.position}";
            //                    Refresh();
            //                }
            //            }
            //        }
            //    }
            //}
        }


        public void Find()
        {
            //textBox9.Text = rect.ToString();
            //foreach (var unit in Map.Units)
            //{
            //    if (rect.Contains(new Point((int)unit.position.X - Map.origin.X, (int)unit.position.Y - Map.origin.Y)) || new Rectangle(new Point((int)unit.position.X - Map.origin.X, (int)unit.position.Y - Map.origin.Y), new Size(50, 50)).Contains(x))
            //    {
            //        selectedObjects.Add(unit);
            //    }
            //}
            //rect = new Rectangle();

            //foreach (var unit in selectedObjects)
            //{
            //    unit.SetTarget(targetPoint);
            //}
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Map.Draw(e.Graphics);
            if(rect != null)
            {
                Map.DrawRect(e.Graphics, (Rectangle)rect);
            }
            if(targetPoint != null)
            {
                Map.DrawTargetPoint(e.Graphics, (Point)targetPoint);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            cursorLocation = e.Location;
            Map.Refresh(cursorLocation);
            textBox10.Text = Map.origin.ToString();
            textBox6.Text = e.Location.ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                selectedObjects.Clear();
                x = e.Location;
                foreach (var unit in Map.Units)
                {
                    if (new Rectangle(new Point((int)unit.position.X - Map.origin.X, (int)unit.position.Y - Map.origin.Y), new Size(50, 50)).Contains(x))
                    {
                        if (!unit.Parameters.enemy)
                        {
                            selectedObjects.Add(unit);
                        }
                        else
                            selectedObjects.Add(unit);
                        
                    }
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                rect = new Rectangle();
                targetPoint = new Point(cursorLocation.X + Map.origin.X, cursorLocation.Y + Map.origin.Y);
                textBox8.Text = targetPoint.ToString();
                if (selectedObjects.Count > 0)
                {
                    moveButton.Enabled = true;
                }
                else
                {
                    moveButton.Enabled = false;
                }
                //Find();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                y = e.Location;
                if (x.X < y.X && x.Y < y.Y)
                {
                    rect = new Rectangle(new Point(x.X + Map.origin.X, x.Y + Map.origin.Y), new Size(y.X - x.X, y.Y - x.Y));
                }
                else
                {
                    rect = new Rectangle(y, new Size(x.X - y.X, x.Y - y.Y));
                }
            }
            foreach (var unit in Map.Units)
            {
                 var rectangle = (Rectangle)rect;
                 if (rectangle.Contains(new Point((int)unit.position.X/* - Map.origin.X*/, (int)unit.position.Y/* - Map.origin.Y*/)))
                 {
                    if (!unit.Parameters.enemy)
                    {
                        selectedObjects.Add(unit);
                    }
                 }
            }
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            foreach (var unit in selectedObjects)
            {
                unit.SetTarget((Point)targetPoint);
            }
            targetPoint = null;
        }

        private void attackButton_Click(object sender, EventArgs e)
        {
            //if(selectedObjects.Count != 0)
            //{
            //    foreach (var unit in selectedObjects)
            //    {
            //        if (unit.Parameters.enemy)
            //        {
            //            unit.Attack(unit);
            //        }
            //    }
            //}
        }
    }
}
