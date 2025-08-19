using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SeaWars
{
    public partial class Form1 : Form
    {
        public const int mapSize = 10;
        public int cellSize = 35;

        public string alphabet = "АБВГДЕЖЗИК";

        public int[,] myMap = new int[mapSize, mapSize];
        public int[,] enemyMap = new int[mapSize, mapSize];

        public Button[,] myButtons = new Button[mapSize, mapSize];
        public Button[,] enemyButtons = new Button[mapSize, mapSize];

        private Queue<int> shipsQueue = new Queue<int>(new int[] { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 });
        private List<(int, int)> currentShipCells = new List<(int, int)>();
        private int currentShipSize = 0;
        private bool isPlacingShips = true;
        public static Form1 currentForm;
        private Button startButton;
        private Button backButton;
        public Bot bot;

        private Dictionary<int, int> expectedShips = new Dictionary<int, int>()
        {
            { 4, 1 },
            { 3, 2 },
            { 2, 3 },
            { 1, 4 }
        };

        public Form1()
        {
            currentForm = this;
            this.Text = "Морський бій";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Paint += new PaintEventHandler(DrawBackground);
            Init();
        }

        private void DrawBackground(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                new Point(0, 0), new Point(this.Width, 0),
                Color.DarkKhaki, Color.Firebrick))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        public void Init()
        {
            foreach (var btn in myButtons)
                if (btn != null) this.Controls.Remove(btn);
            foreach (var btn in enemyButtons)
                if (btn != null) this.Controls.Remove(btn);

            isPlacingShips = true;
            shipsQueue = new Queue<int>(new int[] { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 });
            currentShipCells.Clear();
            currentShipSize = shipsQueue.Dequeue();

            myMap = new int[mapSize, mapSize];
            enemyMap = new int[mapSize, mapSize];
            myButtons = new Button[mapSize, mapSize];
            enemyButtons = new Button[mapSize, mapSize];

            CreateMaps();
            CreateControlButtons();
        }

        private void CreateMaps()
        {
            this.Width = mapSize * 2 * cellSize + 180;
            this.Height = (mapSize + 4) * cellSize + 50;

            for (int i = 0; i <= mapSize; i++)
            {
                for (int j = 0; j <= mapSize; j++)
                {
                    if (i == 0 && j == 0) continue;
                    if (i == 0 || j == 0)
                    {
                        Label label = new Label()
                        {
                            Text = i == 0 ? alphabet[j - 1].ToString() : i.ToString(),
                            Location = new Point(j * cellSize, i * cellSize + 25),
                            Size = new Size(cellSize, cellSize),
                            TextAlign = ContentAlignment.MiddleCenter,
                            Font = new Font("Arial", 10, FontStyle.Bold),
                            ForeColor = Color.White,
                            BackColor = Color.Transparent
                        };
                        this.Controls.Add(label);
                    }
                    else
                    {
                        int rowIndex = i - 1;
                        int colIndex = j - 1;

                        Button myButton = new Button()
                        {
                            Location = new Point(j * cellSize, i * cellSize + 25),
                            Size = new Size(cellSize, cellSize),
                            BackColor = Color.White,
                            FlatStyle = FlatStyle.Flat
                        };
                        myButton.Click += (sender, e) => PlaceShip(rowIndex, colIndex, myButton);
                        myButtons[rowIndex, colIndex] = myButton;
                        this.Controls.Add(myButton);
                    }
                }
            }

            for (int i = 0; i <= mapSize; i++)
            {
                for (int j = 0; j <= mapSize; j++)
                {
                    if (i == 0 && j == 0) continue;
                    if (i == 0 || j == 0)
                    {
                        Label label = new Label()
                        {
                            Text = i == 0 ? alphabet[j - 1].ToString() : i.ToString(),
                            Location = new Point(mapSize * cellSize + 40 + j * cellSize, i * cellSize + 25),
                            Size = new Size(cellSize, cellSize),
                            TextAlign = ContentAlignment.MiddleCenter,
                            Font = new Font("Arial", 10, FontStyle.Bold),
                            ForeColor = Color.White,
                            BackColor = Color.Transparent
                        };
                        this.Controls.Add(label);
                    }
                    else
                    {
                        int rowIndex = i - 1;
                        int colIndex = j - 1;

                        Button enemyButton = new Button()
                        {
                            Location = new Point(mapSize * cellSize + 40 + j * cellSize, i * cellSize + 25),
                            Size = new Size(cellSize, cellSize),
                            BackColor = Color.White,
                            FlatStyle = FlatStyle.Flat
                        };
                        enemyButton.Click += (sender, e) => ShootAtEnemy(rowIndex, colIndex);
                        enemyButtons[rowIndex, colIndex] = enemyButton;
                        this.Controls.Add(enemyButton);
                    }
                }
            }
        }

        private void CreateControlButtons()
        {
            int bottomOffset = this.Height - 80;

            startButton = new Button()
            {
                Text = "Почати гру",
                Location = new Point(30, bottomOffset),
                Size = new Size(120, 35),
                Font = new Font("Arial", 9, FontStyle.Bold),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            startButton.Click += (sender, e) => StartGame();
            this.Controls.Add(startButton);

            backButton = new Button()
            {
                Text = "Назад",
                Location = new Point(this.Width - 150, bottomOffset),
                Size = new Size(100, 35),
                Font = new Font("Arial", 9, FontStyle.Bold),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            backButton.Click += (sender, e) => BackToMainMenu();
            this.Controls.Add(backButton);
        }

        private void PlaceShip(int row, int col, Button button)
        {
            if (!isPlacingShips || myMap[row, col] != 0) return;

            myMap[row, col] = 1;
            button.BackColor = Color.FromArgb(0xBF, 0x21, 0x21);
            currentShipCells.Add((row, col));

            if (currentShipCells.Count == currentShipSize)
            {
                currentShipCells.Clear();
                if (shipsQueue.Count > 0)
                {
                    currentShipSize = shipsQueue.Dequeue();
                }
                else
                {
                    isPlacingShips = false;
                    startButton.Enabled = true;
                }
            }
        }

        private void ShootAtEnemy(int row, int col)
        {
            if (isPlacingShips) return;

            if (enemyMap[row, col] == -1 || enemyMap[row, col] == -2)
                return;

            bool hit = enemyMap[row, col] == 1;

            if (hit)
            {
                enemyMap[row, col] = -1;
                enemyButtons[row, col].BackColor = Color.FromArgb(0xBF, 0x21, 0x21);
                enemyButtons[row, col].Text = "X";

                if (CheckWin(enemyMap))
                {
                    MessageBox.Show("Ви виграли!");
                    EndGame();
                    return;
                }
            }
            else
            {
                enemyMap[row, col] = -2;
                enemyButtons[row, col].BackColor = Color.FromArgb(0x11, 0x13, 0x14);
            }

            if (CheckWin(myMap))
            {
                MessageBox.Show("Бот виграв!");
                EndGame();
                return;
            }

            bot.Shoot();
        }

        private void StartGame()
        {
            if (isPlacingShips)
            {
                MessageBox.Show("Розмістіть всі кораблі перед початком гри!");
                return;
            }

            if (!ValidateShipPlacement())
            {
                MessageBox.Show("Невірне розміщення кораблів!");
                Init();
                return;
            }

            MessageBox.Show("Гра почалась!");
            startButton.Enabled = false;
            bot = new Bot(enemyMap, myMap, enemyButtons, myButtons);
            bot.ConfigureShips();
        }

        private void BackToMainMenu()
        {
            var homeForm = Application.OpenForms["Home"];
            if (homeForm == null)
            {
                Home home = new Home();
                home.Show();
            }
            else
            {
                homeForm.Show();
            }
        }

        private bool CheckWin(int[,] map)
        {
            for (int i = 0; i < mapSize; i++)
                for (int j = 0; j < mapSize; j++)
                    if (map[i, j] == 1) return false;
            return true;
        }

        private bool ValidateShipPlacement()
        {
            bool[,] visited = new bool[mapSize, mapSize];
            Dictionary<int, int> foundShips = new Dictionary<int, int>();

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (myMap[i, j] == 1 && !visited[i, j])
                    {
                        List<(int, int)> currentShip = new List<(int, int)>();
                        TraverseShip(i, j, visited, currentShip);

                        int shipSize = currentShip.Count;
                        if (!expectedShips.ContainsKey(shipSize)) return false;

                        foundShips[shipSize] = foundShips.ContainsKey(shipSize) ? foundShips[shipSize] + 1 : 1;
                    }
                }
            }

            foreach (var ship in expectedShips)
            {
                if (!foundShips.ContainsKey(ship.Key) || foundShips[ship.Key] != ship.Value)
                    return false;
            }
            return true;
        }

        private void TraverseShip(int i, int j, bool[,] visited, List<(int, int)> currentShip)
        {
            if (i < 0 || j < 0 || i >= mapSize || j >= mapSize || visited[i, j] || myMap[i, j] == 0)
                return;

            visited[i, j] = true;
            currentShip.Add((i, j));

            TraverseShip(i + 1, j, visited, currentShip);
            TraverseShip(i - 1, j, visited, currentShip);
            TraverseShip(i, j + 1, visited, currentShip);
            TraverseShip(i, j - 1, visited, currentShip);
        }

        private void EndGame()
        {
            Form current = this;
            Vopros voprosForm = new Vopros();

            this.FormClosed += (s, e) =>
            {
                voprosForm.Show();
            };

            this.Close();
        }
    }
}
