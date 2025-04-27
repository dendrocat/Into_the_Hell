using System;
using System.Collections.Generic;
using UnityEngine;

public class Generator {

	private List<GameObject> rooms=null;

	public static Generator New(){ return new(); }

	public Generator Rooms(List<GameObject> room_prefabs){
		rooms = room_prefabs;
		return this;
	} 

	private GameObject nhall=null, ehall=null;
	public Generator Halls(GameObject nhall_prefab, GameObject ehall_prefab){
		nhall = nhall_prefab; ehall = ehall_prefab;
		return this;
	} 

	private GameObject firstRoom = null;
	public Generator FirstRoom(GameObject firstRoom_prefab){
		firstRoom = firstRoom_prefab;
		return this;
	}

	private GameObject lastRoom = null;
	public Generator LastRoom(GameObject lastRoom_prefab){
		lastRoom = lastRoom_prefab;
		return this;
	}

	private void Exc(String message){
		throw new Exception("Generator: " + message);
	}

	public void Generate(int roomNumber, Level level){
		if(rooms == null) Exc("Rooms are not specified");
		if(nhall == null || ehall == null) Exc("Halls are not specified");
		if(roomNumber < 2 || roomNumber > 999) Exc("Bad room number");

		// Generating positions and coridors;
		List<Vector2Int> possPos = new(){new(0, 0)};
		HashSet<Vector2Int> poses = new();
		System.Random random = new();
		List<Vector2> coridors = new();

		Vector2Int firstRoom_coord = new(-1000, -1000), lastRoom_coord = firstRoom_coord;
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
				lastRoom_coord = pos;
				if(firstRoom_coord.x == -1000) firstRoom_coord = pos;
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
				if(pos != firstRoom_coord)
					coridors.Add(coridor);
			}
		}

		// Normilising coordinates in matrix;
		Func<HashSet<Vector2Int>, List<GameObject>, Matrix<Room>> 
						GetMatrix = (posses, rooms)=>{ 
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
				if(i == lastRoom_coord && lastRoom != null)
					mr[i - shift] = new Room(lastRoom, level);
				else if (i == firstRoom_coord && firstRoom != null)
					mr[i - shift] = new Room(firstRoom, level);
				else
					mr[i - shift] = new Room(rooms[random.Next((int)1e9+7) % rooms.Count], level);
			}
			lastRoom_coord -= shift;
			firstRoom_coord -= shift;

			foreach(var coridor_ in coridors){
				var coridor = coridor_ - shift;
				if(coridor.y != (int)coridor.y){
					mr[(int)coridor.x, (int)coridor.y].AddDoor(DoorDirection.Up);
					mr[(int)coridor.x, (int)coridor.y+1].AddDoor(DoorDirection.Down);
				}else if(coridor.x != (int)coridor.x){
					mr[(int)coridor.x, (int)coridor.y].AddDoor(DoorDirection.Right);
					mr[(int)coridor.x+1, (int)coridor.y].AddDoor(DoorDirection.Left);
				}else throw new Exception("Coridor is incorrect " + coridor + " " + coridor_);
			}

			return mr;
		};
		
		// Normilizing sizes and room positions
		Func<Matrix<Room>,Matrix<Room>> NormilizeRooms = (mr)=>{
			int[] szw = new int[mr.w+1];
			int[] szh = new int[mr.h+1];
			for(int i=0;i<mr.w;++i)
				for(int j=0;j<mr.h;++j)
					if(mr[i, j] != null){
						if(szw[i+1] < mr[i, j].w) szw[i+1] = mr[i, j].w;
						if(szh[j+1] < mr[i, j].h) szh[j+1] = mr[i, j].h;
					}

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

		// Getting main matrix
		var mr = NormilizeRooms(GetMatrix(poses, rooms));
		
		level.create(mr, new(coridors.Count));

		// Generating coridors
		for(int i=0;i<mr.w;++i)
			for(int j=0;j<mr.h;++j) {
				if(mr[i, j] != null) {
					if(i > 0 && (mr[i, j].doors & (1<<(int) DoorDirection.Left)) != 0){
						Room.GenerateHall(
							mr[i-1,j].coord + Vector2.right * (mr[i-1,j].w+1)*0.5f,
							mr[i,j].coord + Vector2.left * (mr[i,j].w-1)*0.5f,
							ehall, level
							);
					}	
					if(j > 0 && (mr[i, j].doors & (1<<(int) DoorDirection.Down)) != 0){
						Room.GenerateHall(
							mr[i,j-1].coord + Vector2.up * (mr[i,j-1].h+1)*0.5f,
							mr[i,j].coord + Vector2.down * (mr[i,j].h-3)*0.5f,
							nhall, level
							);
					}	
				}
			}
		// Normilizing rooms by world coord;
		Vector2 shift = -mr[firstRoom_coord].coord;
		mr.ForEach((room)=>{
			if(room != null){
				room.coord += shift;
				room.SetUp();
			}
		});
		level.halls.ForEach((hall)=>{
			hall.transform.position += (Vector3)shift;
		});
	}

	


}
