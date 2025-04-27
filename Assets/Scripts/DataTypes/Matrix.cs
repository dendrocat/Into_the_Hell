using System;
using System.Collections.Generic;
using UnityEngine;
public class Matrix<T> where T: class {
    public int w {get; private set;}	
    public int h {get; private set;}	
    private List<T> list;
    public Matrix(int w=0, int h=0){
        this.w = w;
        this.h = h;
        list = new List<T>(w*h);
        for(int i=w*h;i-->0;)list.Add(null);
    }

    public T this[int i, int j] {
        get=>list[to(i, j)];set=>list[to(i, j)]=value;}
    public T this[Vector2Int v] {
        get=>list[to(v.x, v.y)];set=>list[to(v.x, v.y)]=value;}

    private int to(int i, int j){
        if(i < 0 || j < 0 || i >= w || j >= h){ throw new ArgumentOutOfRangeException(); }
        return i*h+j;
    }
    
    public void ForEach(Action<T> f){
        list.ForEach(f);
    }
}