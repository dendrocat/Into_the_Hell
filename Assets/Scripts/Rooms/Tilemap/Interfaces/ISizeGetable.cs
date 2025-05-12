using UnityEngine;

/// <summary>
/// Интерфейс для объектов, предоставляющих размер в виде двумерного вектора целых чисел.
/// </summary>
public interface ISizeGetable
{
    /// <summary>
    /// Размер объекта в виде ширины (x) и высоты (y).
    /// </summary>
    public Vector2Int Size { get; }
}