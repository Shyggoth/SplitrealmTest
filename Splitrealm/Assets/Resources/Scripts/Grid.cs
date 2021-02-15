﻿using System;
using System.Collections.Generic;

public class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(object obj)
    {
        if(obj == null)
            return false;

        if(obj is Point)
        {
            Point p = obj as Point;
            return X == p.X && Y == p.Y;
        }

        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 6949;
            hash = hash * 7907 + X.GetHashCode();
            hash = hash * 7907 + Y.GetHashCode();
            return hash;
        }
    }

    public override string ToString()
    {
        return "P(" + X + ", " + Y + ")";
    }
}

public enum CellType
{
    Empty,
    Road,
    Structure,
    SpecialStructure,
    None
}

public class Grid
{
    CellType[,] _grid;
    int _width;
    public int Width { get { return _width; } }
    int _height;
    public int Height { get { return _height; } }
    List<Point> _roadList = new List<Point>();
    List<Point> _specialStructure = new List<Point>();

    public Grid(int width, int height)
    {
        _width = width;
        _height = height;
        _grid = new CellType[width, height];
    }

    public CellType this[int i, int j]
    {
        get
        {
            return _grid[i, j];
        }

        set
        {
            if (value == CellType.Road)
                _roadList.Add(new Point(i, j));
            else
                _roadList.Remove(new Point(i, j));

            if (value == CellType.SpecialStructure)
                _specialStructure.Add(new Point(i, j));
            else
                _specialStructure.Remove(new Point(i, j));

            _grid[i, j] = value;
        }
    }

    public static bool IsCellWalkable(CellType cellType, bool aiAgent = false)
    {
        if(aiAgent)
            return cellType == CellType.Road;

        return cellType == CellType.Empty || cellType == CellType.Road;
    }

    public Point GetRandomRoadPoint()
	{
        Random rand = new Random();
        return _roadList[rand.Next(0, _roadList.Count - 1)];
	}

    public Point GetRandomSpecialStructure()
	{
        Random rand = new Random();
        return _specialStructure[rand.Next(0, _specialStructure.Count - 1)];
    }

    public List<Point> GetAdjacentCells(Point cell, bool isAgent)
	{
        return GetWalkableAdjacentCells(cell.X, cell.Y, isAgent);
	}

    public float GetCostOfEnteringCell(Point cell)
	{
        return 1;
	}

    public List<Point> GetAllAdjacentCells(int x, int y)
	{
        List<Point> adjacentCells = new List<Point>();

        if (x > 0)
            adjacentCells.Add(new Point(x - 1, y));

        if(x < _width - 1)
            adjacentCells.Add(new Point(x + 1, y));

        if (y > 0)
            adjacentCells.Add(new Point(x, y - 1));

        if (y < _height - 1)
            adjacentCells.Add(new Point(x,  y + 1));

        return adjacentCells;
    }

    public List<Point> GetWalkableAdjacentCells(int x, int y, bool isAgent)
	{
        List<Point> adjacentCells = GetAllAdjacentCells(x, y);
        
        for(int i = adjacentCells.Count - 1; i >= 0; i--)
            if(!IsCellWalkable(_grid[adjacentCells[i].X, adjacentCells[i].Y], isAgent))
                adjacentCells.RemoveAt(i);

        return adjacentCells;
	}

    public List<Point> GetAdjacentCellOfType(int x, int y, CellType type)
	{
        List<Point> adjacentCells = GetAllAdjacentCells(x, y);

        for(int i = adjacentCells.Count - 1; i >= 0; i--)
            if(_grid[adjacentCells[i].X, adjacentCells[i].Y] != type)
                adjacentCells.RemoveAt(i);

        return adjacentCells;
	}

    public CellType[] GetAllAdjacentCellTypes(int x, int y)
	{
        CellType[] neighbours = { CellType.None, CellType.None, CellType.None, CellType.None };

        if(x > 0)
            neighbours[0] = _grid[x - 1, y];

        if(x < _width - 1)
            neighbours[2] = _grid[x + 1, y];

        if(y > 0)
            neighbours[3] = _grid[x, y - 1];

        if (y < _height - 1)
            neighbours[1] = _grid[x, y + 1];

        return neighbours;
    }
}