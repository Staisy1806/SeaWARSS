using System;
using System.Drawing;
using System.Windows.Forms;

namespace SeaWars
{
    public class Bot
    {
        public int[,] myMap = new int[Form1.mapSize, Form1.mapSize];
        public int[,] enemyMap = new int[Form1.mapSize, Form1.mapSize];

        public Button[,] myButtons = new Button[Form1.mapSize, Form1.mapSize];
        public Button[,] enemyButtons = new Button[Form1.mapSize, Form1.mapSize];

        private (int, int)? lastHit = null; // Координати останнього влучення
        private Random r = new Random();

        public Bot(int[,] myMap, int[,] enemyMap, Button[,] myButtons, Button[,] enemyButtons)
        {
            this.myMap = myMap;
            this.enemyMap = enemyMap;
            this.enemyButtons = enemyButtons;
            this.myButtons = myButtons;
        }

        public void ConfigureShips()
        {
            ShipPlacer shipPlacer = new ShipPlacer(myMap);
            shipPlacer.PlaceShips();
        }

        public bool Shoot()
        {
            int posX, posY;

            if (lastHit.HasValue)
            {
                // Якщо є останнє попадання, шукаємо корабель далі
                (posX, posY) = HuntShip(lastHit.Value.Item1, lastHit.Value.Item2);
            }
            else
            {
                // Якщо нема попадань, стріляємо випадково
                do
                {
                    posX = r.Next(0, Form1.mapSize);
                    posY = r.Next(0, Form1.mapSize);
                }
                while (enemyMap[posX, posY] == -1 || enemyMap[posX, posY] == -2);
            }

            bool hit = enemyMap[posX, posY] == 1;

            if (hit)
            {
                enemyMap[posX, posY] = -1;
                enemyButtons[posX, posY].BackColor = Color.Red;
                enemyButtons[posX, posY].Text = "X";
                lastHit = (posX, posY); // Запам'ятовуємо попадання
            }
            else
            {
                enemyMap[posX, posY] = -2;
                enemyButtons[posX, posY].BackColor = Color.Black;
                if (lastHit.HasValue)
                {
                    lastHit = null; // Якщо промахнулись, забуваємо останнє попадання
                }
            }

            // Перевірка, чи бот виграв
            if (CheckWin(enemyMap))
            {
                MessageBox.Show("Бот виграв!");
                ShowEndGameForm(); // Відкриваємо форму Vopros після закінчення гри
            }

            return hit;
        }

        private void ShowEndGameForm()
        {
            // Створення та показ форми Vopros після завершення гри
            Vopros voprosForm = new Vopros();
            voprosForm.Show();
        }

        private (int, int) HuntShip(int hitX, int hitY)
        {
            int[][] directions = new int[][]
            {
                new int[] { 0, 1 },  // Праворуч
                new int[] { 0, -1 }, // Ліворуч
                new int[] { 1, 0 },  // Вниз
                new int[] { -1, 0 }  // Вгору
            };

            foreach (var dir in directions)
            {
                int newX = hitX + dir[0];
                int newY = hitY + dir[1];

                if (newX >= 0 && newX < Form1.mapSize && newY >= 0 && newY < Form1.mapSize)
                {
                    if (enemyMap[newX, newY] == 1)
                    {
                        return (newX, newY); // Продовжуємо бити по кораблю
                    }
                }
            }

            return GetRandomUnshotCell();
        }

        private (int, int) GetRandomUnshotCell()
        {
            int x, y;
            do
            {
                x = r.Next(0, Form1.mapSize);
                y = r.Next(0, Form1.mapSize);
            }
            while (enemyMap[x, y] == -1 || enemyMap[x, y] == -2);

            return (x, y);
        }

        // Додаємо метод перевірки перемоги в клас Bot
        private bool CheckWin(int[,] map)
        {
            foreach (var cell in map)
            {
                if (cell == 1) return false;
            }
            return true;
        }
    }
}
