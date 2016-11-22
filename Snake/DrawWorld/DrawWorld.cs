using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SnakeGame
{
    public class DrawWorld
    {
 
        // Determines the size in pixels of each grid cell in the world
        public const int pixelsPerCell = 5;

        // Width of the world in cells (not pixels)
        public int width
        {
            get;
            private set;
        }

        // Height of the world in cells (not pixels)
        public int height
        {
            get;
            private set;
        }

        // This represents the "objects" in the world (just one dot)
        private int dotX;
        private int dotY;


        public DrawWorld(int w, int h)
        {
            width = w;
            height = h;
        }

        // Update the world
        public void SetDot(int x, int y)
        {
            dotX = x;
            dotY = y;
        }


        /// <summary>
        /// Helper method for DrawingPanel
        /// Given the PaintEventArgs that comes from DrawingPanel, draw the contents of the world on to the panel.
        /// </summary>
        /// <param name="e"></param>
        public void Draw(PaintEventArgs e)
        {
            using (SolidBrush drawBrush = new SolidBrush(Color.Black))
            {
                // Draw the single dot that represents the world
                Rectangle dotBounds = new Rectangle(dotX * pixelsPerCell, dotY * pixelsPerCell, pixelsPerCell, pixelsPerCell);
                e.Graphics.FillEllipse(drawBrush, dotBounds);



                //Rectangle lineBounds = new Rectangle((point1.X - point2.X) * pixelsPerCell, point1 )
            }

            
        }

    }

}