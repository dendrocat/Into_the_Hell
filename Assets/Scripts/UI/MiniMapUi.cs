using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MiniMapUi : MonoBehaviour {
    private List<GameObject> rooms = new();
    private RectTransform window;
    static MiniMapUi Instance = null;
    [SerializeField] static int roomSize = 50;
    [SerializeField] static int roomOffset = 10;
    [SerializeField] GameObject mapRoom;

    void Awake() {
        if(Instance){
            Destroy(gameObject);
            return;
        }
        window = GetComponent<RectTransform>();
        Instance = this;
        this.window.sizeDelta = Vector2.zero;
    }

    public void SetUpLevel(Level level){
        window.sizeDelta = new Vector2(level.rooms.w, level.rooms.h) 
                * (roomSize+roomOffset*2);
        DestroyUI();

        for(int i=0;i<level.rooms.w;++i)
        for(int j=0;j<level.rooms.h;++j){
            if(level.rooms[i, j] != null){
                var room = Instantiate(mapRoom, transform);
                rooms.Add(room);
                if(level.rooms[i, j]._isStartRoom){
                    room.GetComponent<RectTransform>().sizeDelta = Vector2.one * (roomSize - roomOffset);
                }else room.GetComponent<RectTransform>().sizeDelta = Vector2.one * roomSize;
                room.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(i+0.5f, j+0.5f) * (roomSize+2*roomOffset);
            }
        }
    }

    public void DestroyUI(){
        foreach(var room in rooms)Destroy(room);
        rooms.Clear();
    }

    void OnDestroy() { 
        DestroyUI();
        Instance = null; }
}
