using System;

namespace SeaWars
{
    public class ShipPlacer
    {
        private int[,] map;
        private Random random = new Random();
        private readonly int[] shipSizes = { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };

        public ShipPlacer(int[,] map)
        {
            this.map = map;
        }

        public void PlaceShips()
        {
            foreach (int size in shipSizes)
            {
                bool placed = false;
                while (!placed)
                {
                    int row = random.Next(1, Form1.mapSize);
                    int col = random.Next(1, Form1.mapSize);
                    bool horizontal = random.Next(2) == 0;

                    if (CanPlaceShip(row, col, size, horizontal))
                    {
                        PlaceShip(row, col, size, horizontal);
                        placed = true;
                    }
                }
            }
        }

        private bool CanPlaceShip(int row, int col, int size, bool horizontal)
        {
            for (int i = 0; i < size; i++)
            {
                int r = horizontal ? row : row + i;
                int c = horizontal ? col + i : col;
                if (r >= Form1.mapSize || c >= Form1.mapSize || map[r, c] != 0)
                    return false;

                if (!IsSurroundingAreaClear(r, c))
                    return false;
            }
            return true;
        }

        private bool IsSurroundingAreaClear(int row, int col)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int r = row + i;
                    int c = col + j;
                    if (r >= 0 && r < Form1.mapSize && c >= 0 && c < Form1.mapSize && map[r, c] != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void PlaceShip(int row, int col, int size, bool horizontal)
        {
            for (int i = 0; i < size; i++)
            {
                int r = horizontal ? row : row + i;
                int c = horizontal ? col + i : col;
                map[r, c] = 1;
            }
        }
    }
}
