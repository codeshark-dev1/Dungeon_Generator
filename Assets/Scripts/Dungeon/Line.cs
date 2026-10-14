using UnityEngine;

public class Line
{
    private Orientation orientation;
    private Vector2Int coordinates;

    public Line(Orientation orientation, Vector2Int coordinates)
    {
        this.orientation = orientation;
        this.coordinates = coordinates;
    }
}

public enum Orientation
{
    Horizontal = 0,
    Vertical = 1
};