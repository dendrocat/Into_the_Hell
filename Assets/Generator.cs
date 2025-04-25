using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Generator : MonoBehaviour {
	[SerializeField] GameObject[] roomPrefabs;
	[SerializeField] GameObject[] halls;
	private void Start() {
		roomPrefabs = Resources.LoadAll<GameObject>("Rooms");
		halls = Resources.LoadAll<GameObject>("Halls");
		SimpleGenerate(25, roomPrefabs);
	}

	private void SimpleGenerate(int roomNumber, GameObject[] rooms, GameObject firstRoomPrefab=null, GameObject lastRoomPrefab=null){
		List<Vector2Int> possPos = new(){new(0, 0)};
		HashSet<Vector2Int> poses = new();
		System.Random random = new();
		List<Vector2> coridors = new();

		Vector2Int firstRoom = new(-1000, -1000), lastRoom = firstRoom;
		while(roomNumber -- > 0){ 
			int ind = random.Next(1000007);
			ind %= possPos.Count;
			Vector2Int pos = possPos[ind];
			possPos[ind] = possPos[possPos.Count-1];
			possPos.RemoveAt(possPos.Count-1);
			if(poses.Contains(pos)){
				++roomNumber;
			}else{ 
				poses.Add(pos);
				if(firstRoom.x == -1000) firstRoom = pos;
				lastRoom = pos;
				int num = 0;
				Vector2 coridor = new(0f, 0f);
				Array.ForEach(new Vector2Int[]{
					new(1, 0), new(-1, 0), new(0, 1), new(0, -1)  
				}, (shift)=>{
					if(poses.Contains(shift + pos)){ 
						if(UnityEngine.Random.value <= 1f / ++num || coridor == Vector2.zero)
							coridor = pos + 0.5f * (Vector2)shift;
					}else possPos.Add(pos + shift);
				});
				coridors.Add(coridor);
			}
		}
		Func<HashSet<Vector2Int>, GameObject[], Matrix<Room>> GetMatrix = (posses, rooms)=>{ 
			int minX=0, minY=0, maxX=0, maxY=0;
			foreach(var i in posses){
				if(i.x < minX) minX = i.x;
				if(i.y < minY) minY = i.y;
				if(i.x > maxX) maxX = i.x;
				if(i.y > maxY) maxY = i.y;
			}
			Vector2Int shift = new(minX, minY);
			Matrix<Room> mr = new(maxX-minX+1, maxY-minY+1);		
			foreach(var i in poses){
				if(i == lastRoom && lastRoomPrefab != null)
					mr[i - shift] = new Room(lastRoomPrefab);
				else if (i == firstRoom && firstRoomPrefab != null)
					mr[i - shift] = new Room(firstRoomPrefab);
				else
					mr[i - shift] = new Room(rooms[random.Next((int)1e9+7) % rooms.Length]);
			}

			foreach(var coridor_ in coridors){
				var coridor = coridor_ - shift;
				if(coridor.x == (int)coridor.x){
					mr[(int)coridor.x, (int)coridor.y].AddDoor(DoorDirection.Up);
					mr[(int)coridor.x, (int)coridor.y+1].AddDoor(DoorDirection.Down);
				}else if(coridor.y == (int)coridor.y){
					mr[(int)coridor.x, (int)coridor.y].AddDoor(DoorDirection.Right);
					mr[(int)coridor.x+1, (int)coridor.y].AddDoor(DoorDirection.Left);
				}else throw new Exception("Coridor is incorrect " + coridor + " " + coridor_);
			}

			return mr;
		};
		
		Func<Matrix<Room>,Matrix<Room>> NormilizeRooms = (mr)=>{
			int[] szw = new int[mr.w+1];
			int[] szh = new int[mr.h+1];
			for(int i=0;i<mr.w;++i)
				for(int j=0;j<mr.h;++j)
					if(mr[i, j] != null){
						if(szw[i+1] < mr[i, j].w) szw[i+1] = mr[i, j].w;
						if(szh[j+1] < mr[i, j].h) szh[j+1] = mr[i, j].h;
					}

			/*
			string debug = "";
			foreach(var i in szw) debug += i + " ";
			Debug.Log(debug);
			
			debug = "";
			foreach(var i in szh) debug += i + " ";	
			Debug.Log(debug);
			*/

			for(int i=0;i<szw.Length-1;++i) szw[i+1] += random.Next((int)1e+7) % 5 * 2 + 5;
			for(int i=0;i<szh.Length-1;++i) szh[i+1] += random.Next((int)1e+7) % 5 * 2 + 5;
			
			for(int i=0;i<szw.Length-1;++i) szw[i+1] += szw[i];
			for(int i=0;i<szh.Length-1;++i) szh[i+1] += szh[i];
			for(int i=0;i<mr.w;++i)
				for(int j=0;j<mr.h;++j)
					if(mr[i, j] != null)
						mr[i, j].coord = new Vector2(szw[i+1]*0.5f + szw[i]*0.5f, szh[j+1]*0.5f + szh[j]*0.5f);
			return mr;
		};

		var mr = NormilizeRooms(GetMatrix(poses, rooms));
		for(int i=0;i<mr.w;++i)
			for(int j=0;j<mr.h;++j) {
				if(mr[i, j] != null) {
					mr[i, j].SetUp();
					if(i > 0 && (mr[i, j].doors & (1<<(int) DoorDirection.Left)) != 0){
						Room.GenerateHall(
							mr[i-1,j].coord + Vector2.right * (mr[i-1,j].w+1)*0.5f,
							mr[i,j].coord + Vector2.left * (mr[i,j].w-1)*0.5f,
							halls[0]
							);
					}	
					if(j > 0 && (mr[i, j].doors & (1<<(int) DoorDirection.Down)) != 0){
						Room.GenerateHall(
							mr[i,j-1].coord + Vector2.up * (mr[i,j-1].h+1)*0.5f,
							mr[i,j].coord + Vector2.down * (mr[i,j].h-3)*0.5f,
							halls[1]
							);
					}	
				}
			}
	}

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

	public class Room{ 
		public int w {get; private set; } 	
		public int h {get; private set; } 	
		private GameObject room; 
		public Vector2 coord;
		public Room(GameObject prefab ){
			room = Instantiate(prefab);
			var col = room.GetComponent<ISizeGetable>().Size;
			w = col.x;
			h = col.y;
		}

		public int doors = 0;

		public void SetUp(){
			room.transform.position = (Vector3)coord;
			room.GetComponent<IRoomController>().DoorController.SetDoors(
				room.GetComponent<IRoomTilemapController>().DoorTilemap.ActiveDoors );
		}

        public void AddDoor(DoorDirection dir) {
			doors |= 1<<(int)dir;
			Debug.Log((room == null) + " " + (room?.GetComponent<IRoomController>() == null));
			room.GetComponent<IRoomTilemapController>().
					DoorTilemap.ActivateDoor(dir);
        }

        public static void GenerateHall(Vector2 start, Vector2 fin, GameObject prefab) {
			GameObject hall = Instantiate(prefab);
			hall.transform.position = (Vector3) start;
			hall.GetComponent<IHallTilemapController>().ExtendToLength((int)(fin-start).magnitude);
		}
    }

}
