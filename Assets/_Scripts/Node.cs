using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node
{
    public Vector2 Position;
    public Node Parent;
    public int G;
    public float F;
    public Node(Vector2 position, Node parent = null)
    {
        Position = position;
        Parent = parent;
        G = parent == null ? 0 : parent.G + 1;
        F = 0;
    }

    public float CalculateF(Vector2 startPos, Vector2 targetPos)
    {
        // Use the Manhattan distance as the heuristic
        var h = Mathf.Abs(Position.x - targetPos.x) + Mathf.Abs(Position.y - targetPos.y);
        F = G + h;
        return F;
    }
}

