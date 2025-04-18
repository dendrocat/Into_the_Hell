using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Generator : MonoBehaviour {
	[SerializeField] GameObject[] roomPrefabs;
	[SerializeField] GameObject testS;
	private void Start() {
		roomPrefabs = Resources.LoadAll<GameObject>("Rooms");
		SimpleGenerate(25, roomPrefabs);
	}

	private void SimpleGenerate(int roomNumber, GameObject[] rooms){
		List<Vector2Int> possPos = new(){new(0, 0)};
		HashSet<Vector2Int> poses = new();
		System.Random random = new();
		List<Vector2> coridors = new();
		
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
				int num = 0;
				Vector2 coridor = new(0f, 0f);
				Array.ForEach(new Vector2Int[]{
					new(1, 0), new(-1, 0), new(0, 1), new(0, -1)  
				}, (shift)=>{
					if(poses.Contains(shift + pos)){ 
						++num;
						if(UnityEngine.Random.value < 1f / num)
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
				mr[i - shift] = new Room(rooms[random.Next((int)1e9+7) % rooms.Length]);
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

			string debug = "";
			foreach(var i in szw) debug += i + " ";
			Debug.Log(debug);
			
			debug = "";
			foreach(var i in szh) debug += i + " ";	
			Debug.Log(debug);

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
		mr.ForEach((room)=>{
			room?.Instantiate();
		});

		foreach(var i in coridors){
			if(i.x == (int)i.x){
				
			}else{

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
		GameObject prefab; 
		GameObject room; 
		public Vector2 coord;
		public Room(GameObject prefab ){
			this.prefab = prefab;
			var col = prefab.GetComponent<BoxCollider2D>().size;
			w = ((int)col.x) + 4;
			h = ((int)col.y) + 4;
		}

		public byte doors = 0;

		public void Instantiate(){
			room = GameObject.Instantiate(prefab, (Vector3)coord, Quaternion.identity);
			// for(int i=1;i<16;i<<=1){
			// 	if((doors & i) != 0)	
			// 		room.GetComponent<IRoomTilemapController>().DoorTilemap.ActivateDoor(Enum.Parse(DoorDirection, i+"")));
			// }
		}
	}

}
