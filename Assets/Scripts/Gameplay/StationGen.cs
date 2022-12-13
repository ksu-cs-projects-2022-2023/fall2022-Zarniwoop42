using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Enums;
using Utils;
using System.Linq;


namespace Gameplay
{
    public class StationGen : MonoBehaviour
    {
        [SerializeField]
        private Tilemap _wallsTilemap;
        [SerializeField]
        private Tilemap _groundTilemap;
        [SerializeField]
        private Tilemap _spaceTilemap;
        [SerializeField]
        private TileBase[] _wallTiles;
        [SerializeField]
        private TileBase[] _groundTiles;
        [SerializeField]
        private TileBase[] _spaceTiles;
        [SerializeField]
        private int _maxWidth = 10;
        [SerializeField]
        private int _maxHeight = 10;

        [SerializeField]
        private int _cellsToRemove = 50;

        public GridCell<bool>[] validSpawns;

        public SpaceStationBuilding station;

        [ContextMenu("Generate Station")]
        public void Generate(int mW, int mH, int cR)
        {
            station = 
                new SpaceStationBuilding(mW, mH)
                .CarveHalls(_cellsToRemove)
                .Shrink();
            _wallsTilemap.ClearAllTiles();
            _groundTilemap.ClearAllTiles();
            //_spaceTilemap.ClearAllTiles();
            _spaceTilemap.FloodFill(new Vector3Int(0,0,0), _spaceTiles[0]);

            foreach (var cell in station.Grid.Cells)
            {
                if (cell.Value)
                {
                    var randomWall = _wallTiles[Random.Range(0, _wallTiles.Length)];
                    _wallsTilemap.SetTile(new Vector3Int(cell.X, cell.Y, 0), randomWall);
                }
                else
                {
                    var randomGround = _groundTiles[Random.Range(0, _groundTiles.Length)];
                    _groundTilemap.SetTile(new Vector3Int(cell.X, cell.Y, 0), randomGround);
                }
            }
            validSpawns = station.Grid.Cells.Where(c => !c.Value).ToArray();
        }
    }


}