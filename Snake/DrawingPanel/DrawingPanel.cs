using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SnakeGame;



namespace DrawingPanel
{
    public class DrawingPanel: Panel
    {
        private DrawWorld world;


        public DrawingPanel()
        {
            // Setting this property to true prevents flickering
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Pass in a reference to the world, so we can draw the objects in it
        /// </summary>
        /// <param name="_world"></param>
        public void SetWorld(DrawWorld _world)
        {
            world = _world;
        }

        /// <summary>
        /// Override the behavior when the panel is redrawn
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // If we don't have a reference to the world yet, nothing to draw.
            if (world == null)
                return;

            using (SolidBrush drawBrush = new SolidBrush(Color.Black))
            {

                // Turn on anti-aliasing for smooth round edges
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Draw the top wall
                Rectangle topWall = new Rectangle(0, 0, world.width * DrawWorld.pixelsPerCell, DrawWorld.pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, topWall);

                // Draw the right wall
                Rectangle rightWall = new Rectangle((world.width - 1) * DrawWorld.pixelsPerCell, 0, DrawWorld.pixelsPerCell, world.height * DrawWorld.pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, rightWall);

                // Draw the bottom wall
                Rectangle bottomWall = new Rectangle(0, (world.height - 1) * DrawWorld.pixelsPerCell, world.width * DrawWorld.pixelsPerCell, DrawWorld.pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, bottomWall);

                // Draw the left wall
                Rectangle leftWall = new Rectangle(0, 0, DrawWorld.pixelsPerCell, world.height * DrawWorld.pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, leftWall);

            }

            // Draw the "world" (just one dot) within this panel by using the PaintEventArgs

            world.Draw(e);
        }
    }
}
