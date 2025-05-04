using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MiniMapUi : MonoBehaviour {
    private static Level level = null;
    private List<GameObject> rooms = new();
    private RectTransform window;
    static MiniMapUi Instance = null;
    [SerializeField] static int roomSize = 50;
    [SerializeField] static int roomOffset = 10;
    [SerializeField] GameObject mapRoom;

    void Awake() {
        window = GetComponent<RectTransform>();
        Instance = this;
        makeView();   
    }

    static void makeView(){
        if(Instance == null) return;
        Instance.window.sizeDelta = Vector2.zero;
        if(level == null) return;

        Instance.window.sizeDelta = new Vector2(level.rooms.w, level.rooms.h) * (roomSize+roomOffset*2);
        Instance.DestroyUI();

        for(int i=0;i<level.rooms.w;++i)
        for(int j=0;j<level.rooms.h;++j){
            if(level.rooms[i, j] != null){
                var room = Instantiate(Instance.mapRoom, Instance.transform);
                Instance.rooms.Add(room);
                if(level.rooms[i, j]._isStartRoom){
                    room.GetComponent<RectTransform>().sizeDelta = Vector2.one * (roomSize - roomOffset);
                }else room.GetComponent<RectTransform>().sizeDelta = Vector2.one * roomSize;
                room.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(i+0.5f, j+0.5f) * (roomSize+2*roomOffset);
            }
        }
    }

    public void DestroyUI(){
        foreach(var room in rooms)Destroy(room);
    }

    public static void SetUpLevel(Level _level){
        level = _level;
        makeView();
    }
}
