using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace saper
{
    public class Field                              // класс поля
    {
        public Cell[,] cells;                       //массив "клеток"
        public int N { get; set; }   //N - длинна стороны поля
        public event EventHandler<ChangeArgs> Change;  //событие изменения кнопки
        public event EventHandler<EventArgs> Win; // событие победы
        public event EventHandler<EventArgs> Loose; // событие проигрыша 
        public List<LevelEl> levels; // сложность игры
 
        public Field(int n, int percent)     //конструктор
        {
            N = n;
            cells = new Cell[N, N];
            int amt = N * N * percent / 100; //количество мин
            Random r = new Random();
            for (int i = 0; i < amt; i++)
            {
                int x = r.Next(N);
                int y = r.Next(N);
                if (cells[x, y].HasMine)
                {
                    i--;
                    continue;
                }
                cells[x, y].HasMine = true;
            }

            levels = new List<LevelEl>(); //заполняем сложность
            levels.Add(new LevelEl { name = "Простая", percent = 15 });
            levels.Add(new LevelEl { name = "Средняя", percent = 20 });
            levels.Add(new LevelEl { name = "Сложная", percent = 30 });
        }
   
        public int MinesAround(int i, int j)        //количество мин вокруг
        {
            int res = 0;
            for (int k = 0; k < N; k++)
            {
                for (int t = 0; t < N; t++)
                {
                    if (Math.Abs(i - k) <= 1 && Math.Abs(t - j) <= 1 && cells[k, t].HasMine)
                    {
                        res++;
                    }
                }
            }
            return res;
        }

        void OpenLast()                       // возвращяет "true", если открыты все безопасные поля
        {
            foreach (Cell t in cells)
            {
                if (!t.IsOpen && !t.HasMine) return;
            }
            if (Win != null)
            {
                Win(this, new EventArgs());
            }
        }

        public void OpenCell(int i, int j)
        {
            if (cells[i, j].HasMine) 
            {
                if (Loose != null)
                {
                    Loose(this, new EventArgs());
                }
            }
            cells[i, j].IsOpen = true;
            if (Change != null)
            {
                Change(this, new ChangeArgs { I = i, J = j, MinArr = Convert.ToString(MinesAround(i, j)) });
            }
            if (MinesAround(i, j) == 0)
            {
                for (int k = 0; k < N; k++)
                {
                    for (int p = 0; p < N; p++)
                    {
                        if (Math.Abs(i - k) <= 1 && Math.Abs(p - j) <= 1 && cells[k, p].IsOpen==false)
                        {
                            OpenCell(k, p);
                        }
                    }
                }
            }
            OpenLast();
        }
    }
}
