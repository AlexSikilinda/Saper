using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace saper
{
    public partial class Form1 : Form
    {
        Field field;
        GroupBox f;
        
        public Form1()
        {
            InitializeComponent();
            this.Resize += ResizeF;
            NewGame(10);
            listBox1.DataSource = field.levels;
            listBox1.DisplayMember = "getName";
        }

        void NewGame(int n)   
        {
            if (n < 5) n = 5;
            if (n > 20) n = 20;
            if (listBox1.SelectedItem != null)
            {
                field = new Field(n, (listBox1.SelectedItem as LevelEl).percent);   // дописать код!!!! выбор процента и длинны поля
            }
            else
            {
                field = new Field(n, 15);
            }
            AddButtons();
            field.Change += ChangeButton;
            field.Loose += Loose;
            field.Win += Win;
            ChangeForm();
        }

        void AddButtons()
        {
            f = new GroupBox();
            f.Location = new Point(100, 100);
            f.Size = new Size(40 * field.N, 40 * field.N);
            f.Parent = this;
            
            for (int i = 0; i < field.N; ++i)
            {
                for (int j = 0; j < field.N; ++j)
                {
                    ControlButton b = new ControlButton(field, i, j);
                    b.Width = 37; b.Height = 37;
                    b.Location = new Point(j * 40, i * 40);
                    b.FlatStyle = FlatStyle.Flat;
                    b.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.cell));
                    b.ForeColor = SystemColors.Control; ;
                    b.Parent = f;
                }
            }
            ResizeF(this, new EventArgs());
        }

        void ChangeButton(object s, ChangeArgs e)
        {
            foreach (object b in f.Controls)
            {
                if ((b as ControlButton)!=null && (b as ControlButton).i == e.I) //студия лагает не дает записать &&
                {
                    if ((b as ControlButton).j == e.J)
                    {
                        if (e.MinArr == "0")
                        {
                            (b as ControlButton).Text = "";
                            (b as ControlButton).BackgroundImage = (System.Drawing.Image)saper.Properties.Resources.cellclear;
                        }
                        else
                        {
                            (b as ControlButton).Text = e.MinArr;
                            (b as ControlButton).BackgroundImage = (System.Drawing.Image)saper.Properties.Resources.cellopen;
                        }
                        
                    }
                }
            }
        }

        void Loose(object s, EventArgs e)
        {
            FindCell((System.Drawing.Image)saper.Properties.Resources.cellmine);
            var loose = MessageBox.Show("проиграл");
            field.Change -= ChangeButton;
            field.Loose -= Loose;
        }

        void Win(object s, EventArgs e)
        {
            FindCell((System.Drawing.Image)saper.Properties.Resources.cellflag);
            MessageBox.Show("Победа!!!");
            field.Win -= Win;
        }

        void FindCell(System.Drawing.Image z)
        {
            for (int i = 0; i < field.N; i++)
            {
                for (int j = 0; j < field.N; j++)
                {
                    if (field.cells[i, j].HasMine)
                    {
                        foreach (ControlButton t in f.Controls)
                        {
                            if (t.i == i && t.j == j)
                            {
                                t.BackgroundImage = z;
                            }
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)  // писать и писать еще!
        {
            f.Parent = null;
            if (textBox1.Text != "")
            {
                NewGame(Convert.ToInt32(textBox1.Text));
            }
            else NewGame(10);
        }

        void ResizeF(object sender, EventArgs e)
        {
            groupBox1.Location = new Point((Width-groupBox1.Size.Width)/2,10);
            f.Location = new Point((Width - f.Size.Width)/2, 100);
        }

        void ChangeForm()
        {
            Width = f.Width + 200;
            Height = f.Height + 200;
            if (Width <= groupBox1.Width) Width = groupBox1.Width + 40;
        }
    }
}
