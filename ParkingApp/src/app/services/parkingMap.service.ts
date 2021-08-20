import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ParkingMapModel } from '../models/parkingMapModel';

@Injectable({
  providedIn: 'root'
})
export class ParkingMapService {
  private apiUrl = 'https://parkappapi.azurewebsites.net/api/map';
  constructor(private http:HttpClient) { }
  getAllMaps():Observable<ParkingMapModel[]>{
    return this.http.get<ParkingMapModel[]>(this.apiUrl + "/getall");
  }
  addMap(map: ParkingMapModel){
    return this.http.post(this.apiUrl + "/add",map);
  }
  updateMap(map: ParkingMapModel){
    return this.http.post(this.apiUrl + "/update",map);
  }
  deleteMap(id:number){
    return this.http.post(this.apiUrl + "/delete?=" + id,null);
  }
  deleteMapByLotId(id:number){
    return this.http.post(this.apiUrl + "/deletebylotid?=" + id,null);
  }
  deleteMapByPassColor(cd:string){
    return this.http.post(this.apiUrl + "/deletebypasscolor?=" + cd,null);
  }
}