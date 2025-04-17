using UnityEngine;

/// <summary>Interface for objects exposing size as a 2D integer vector</summary>
public interface ISizeGetable
{
    /// <summary>Object dimensions as width (x) and height (y)</summary>
    public Vector2Int Size { get; }
}