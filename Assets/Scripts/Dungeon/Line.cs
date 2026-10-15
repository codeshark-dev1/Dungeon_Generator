using UnityEngine;

// A line that splits a space in two: its direction and its position.
public class Line
{
    private Orientation orientation;
    private Vector2Int coordinates;

    // Stores the direction and position of the line
    public Line(Orientation orientation, Vector2Int coordinates)
    {
        this.orientation = orientation;
        this.coordinates = coordinates;
    }

    // Direction of the line
    public Orientation Orientation { get => orientation; set => orientation = value; }
    // Position of the line: only y is used for horizontal lines, only x for vertical lines
    public Vector2Int Coordinates { get => coordinates; set => coordinates = value; }
}

// Direction of a split.
// Horizontal: the line runs left to right, at a y position.
// Vertical: the line runs bottom to top, at an x position.
public enum Orientation
{
    Horizontal = 0,
    Vertical = 1
};