using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;







public static class MazeGenerator
{

    public static DirectionOfWall[,] Generate(int width, int height)
    {
        DirectionOfWall[,] grid = new DirectionOfWall[width, height];
        DirectionOfWall basement = DirectionOfWall.rightDirection | DirectionOfWall.leftDirection | DirectionOfWall.topDirection | DirectionOfWall.bottomDirection;
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                grid[i, j] = basement;
            }
        }
        return DFS(width, height, grid);
    }

    private static List<Neighbour> ExploreNeighbours(Coordinate point, DirectionOfWall[,] grid, int width, int height)
    {
        var neighborList = new List<Neighbour>();

        //leftDirection
        if (point.X > 0) 
        {
            if (!grid[point.X - 1, point.Y].HasFlag(DirectionOfWall.visitedNeighbor))
            {
                neighborList.Add(new Neighbour { Coordinate = new Coordinate { X = point.X - 1, Y = point.Y }, SharedWall = DirectionOfWall.leftDirection });
            }
        }

        // rightDirection
        if (point.X < width - 1) 
        {
            if (!grid[point.X + 1, point.Y].HasFlag(DirectionOfWall.visitedNeighbor))
            {
                neighborList.Add(new Neighbour { Coordinate = new Coordinate { X = point.X + 1, Y = point.Y }, SharedWall = DirectionOfWall.rightDirection });
            }
        }

        //bottomDirection
        if (point.Y > 0)
        {
            if (!grid[point.X, point.Y - 1].HasFlag(DirectionOfWall.visitedNeighbor))
            {
                neighborList.Add(new Neighbour { Coordinate = new Coordinate { X = point.X, Y = point.Y - 1 }, SharedWall = DirectionOfWall.bottomDirection });
            }
        }

        //topDirection
        if (point.Y < height - 1) 
        {
            if (!grid[point.X, point.Y + 1].HasFlag(DirectionOfWall.visitedNeighbor))
            {
                neighborList.Add(new Neighbour { Coordinate = new Coordinate { X = point.X, Y = point.Y + 1 }, SharedWall = DirectionOfWall.topDirection });
            }
        }

        return neighborList;
    }

    private static DirectionOfWall[,] DFS(int width, int height, DirectionOfWall[,] grid)
    {
        var rng = new System.Random();
        var positionStack = new Stack<Coordinate>();

        var X_position = rng.Next(0, width);
        var Y_position = rng.Next(0, height);

        var position = new Coordinate { X = X_position, Y = Y_position };

        grid[position.X, position.Y] |= DirectionOfWall.visitedNeighbor;

        positionStack.Push(position);

        while (positionStack.Count > 0)
        {
            var current = positionStack.Pop();
            var neighbours = ExploreNeighbours(current, grid, width, height);

            if (neighbours.Count > 0)
            {
                positionStack.Push(current);

                var randIndex = rng.Next(0, neighbours.Count);
                var randomNeighbour = neighbours[randIndex];

                var nPosition = randomNeighbour.Coordinate;
                grid[current.X, current.Y] &= ~randomNeighbour.SharedWall;
                grid[nPosition.X, nPosition.Y] &= ~OppWallDirection(randomNeighbour.SharedWall);

                grid[nPosition.X, nPosition.Y] |= DirectionOfWall.visitedNeighbor;

                positionStack.Push(nPosition);
            }
        }
        return grid;
    }

    private static DirectionOfWall OppWallDirection(DirectionOfWall wall)
    {
        switch (wall)
        {
            case DirectionOfWall.rightDirection: return DirectionOfWall.leftDirection;
            case DirectionOfWall.leftDirection: return DirectionOfWall.rightDirection;
            case DirectionOfWall.topDirection: return DirectionOfWall.bottomDirection;
            case DirectionOfWall.bottomDirection: return DirectionOfWall.topDirection;
            default: return DirectionOfWall.leftDirection;
        }
    }



}

public struct Coordinate
{
    public int X;
    public int Y;
}

public struct Neighbour
{
    public Coordinate Coordinate;
    public DirectionOfWall SharedWall;
}

[Flags]
public enum DirectionOfWall
{
    bottomDirection = 8, topDirection = 4, rightDirection = 2, leftDirection = 1, visitedNeighbor = 128,
}
