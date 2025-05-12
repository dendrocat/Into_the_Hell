using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Класс, который используется для хранения двумерной матрицы объектов </summary>
public class Matrix<T> where T: class {
	
  /// <summary> Размер первого измерения матрицы </summary>
	public int w {get; private set;}
  /// <summary> Размер второго измерения матрицы </summary>
	public int h {get; private set;}
  /// <summary> Список для хранения объектов </summary>
	private List<T> list;

  /// <summary> Конструктор матрицы </summary>
  /// <param name="w"> Размер матрицы (первое измерение)</param>
  /// <param name="h"> Размер матрицы (второе измерение)</param>
	public Matrix(int w=0, int h=0){
		this.w = w;
		this.h = h;
		list = new List<T>(w*h);
		for(int i=w*h;i-->0;)list.Add(null);
	}

  /// <summary> Метод получения элемента матрицы </summary>
  /// <param name="i"> Первый индекс матрицы </summary> 
  /// <param name="j"> Второй индекс матрицы </summary> 
	public T this[int i, int j] {
		get=>list[to(i, j)];set=>list[to(i, j)]=value;}
  /// <summary> Метод получения элемента матрицы </summary>
  /// <param name="v"> Индексы матрицы </summary> 
	public T this[Vector2Int v] {
		get=>list[to(v.x, v.y)];set=>list[to(v.x, v.y)]=value;}

  /// <summary> Функция приведения координат матрицы к индексу списка </summary>
  /// <param name="i"> Первый индекс матрицы </summary> 
  /// <param name="j"> Второй индекс матрицы </summary> 
	private int to(int i, int j){
		if(i < 0 || j < 0 || i >= w || j >= h){ throw new ArgumentOutOfRangeException(); }
		return i*h+j;
	}
	
  /// <summary> Функция прохода по всем элементам матрицы </summary>
  /// <param name="f"> Функция, которая применяется к каждому элементу <see cref="Action"></param>
	public void ForEach(Action<T> f){
		list.ForEach(f);
	}
}

