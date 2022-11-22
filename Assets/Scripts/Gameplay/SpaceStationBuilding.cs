using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Enums;
using System;
using System.Linq;

namespace Gameplay
{
    public class SpaceStationBuilding
    {
        private readonly int _maxWidth;
        private readonly int _maxHeight;

        public Grid<GridCell<bool>> Grid { get; private set; }

        public SpaceStationBuilding(int maxWidth, int maxHeight)
        {
            _maxWidth = maxWidth;
            _maxHeight = maxHeight;

            Grid = new Grid<GridCell<bool>>(maxWidth, maxHeight, InitializeGridCell);
        }

        private GridCell<bool> InitializeGridCell(int x, int y)
        {
            return new GridCell<bool>(x, y, true);
        }

        public SpaceStationBuilding CarveHalls(int cellsToRemove)
        {
            var walkerPos = new Vector2Int(_maxWidth / 2, _maxHeight / 2);
            while (cellsToRemove > 0)
            {
                var randomDir = RandomWalker.GetRandomEnumValue<Direction>();
                var newWalkerPos = walkerPos + randomDir.ToCoords();
                if (!Grid.AreCoordsValid(newWalkerPos, true)) continue;
                var cell = Grid.Get(newWalkerPos);
                if (cell.Value)
                {
                    cell.Value = false;
                    cellsToRemove--;
                }
                walkerPos = newWalkerPos;
            }
            return this;
        }

        public SpaceStationBuilding Shrink()
        {
            var emptyCells = Grid.Cells.Where(c => !c.Value).ToArray();

            var minX = emptyCells.Min(c => c.X);
            var maxX = emptyCells.Max(c => c.X);
            var shrankWidth = maxX - minX + 3;
            var minY = emptyCells.Min(c => c.Y);
            var maxY = emptyCells.Max(c => c.Y);
            var shrankHeight = maxY - minY + 3;
            var newGrid = new Grid<GridCell<bool>>(shrankWidth, shrankHeight, InitializeGridCell);
            for (var x = minX - 1; x <= maxX; x++)
            {
                for (var y = minY - 1; y <= maxY; y++)
                {
                    var value = Grid.Get(x, y).Value;
                    newGrid.Get(x - minX + 1, y - minY + 1).Value = value;
                }
            }
            Grid = newGrid;

            return this;
        }
       
    }
}