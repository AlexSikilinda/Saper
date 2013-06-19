using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace saper
{
    class ControlButton : Button
    {
        Field field;
        public int i, j;

        public ControlButton(Field f, int i, int j)
        {
            field = f;
            this.i = i;
            this.j = j;
        }

       /* protected override void OnClick(EventArgs e)  //открытие клетки
        {
            base.OnClick(e);
            field.OpenCell(i,j);
        }*/

        protected override void OnMouseDown(MouseEventArgs mevent) // правая кнопка
        {
            if (mevent.Button == MouseButtons.Right)
            {
                if(!field.cells[i,j].IsOpen)
                {
                    BackgroundImage = (System.Drawing.Image)saper.Properties.Resources.cellflag;
                }
            }
            else
            {
                field.OpenCell(i, j);
            }
        }
    }
}
