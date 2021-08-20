import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ParkingLotModel } from '../models/parkingLotModel';

@Injectable({
  providedIn: 'root'
})
export class ParkingLotService {
  private apiUrl = 'https://parkappapi.azurewebsites.net/api/lots';
  constructor(private http:HttpClient) { }
  getAllLots():Observable<ParkingLotModel[]>{
    return this.http.get<ParkingLotModel[]>(this.apiUrl + "/getall");
  }
  getLotsByPass(cd: string):Observable<ParkingLotModel[]>{
    return this.http.get<ParkingLotModel[]>(this.apiUrl + "/getbypass?="+cd);
  }
  getLot(id:number):Observable<ParkingLotModel>{
      return this.http.get<ParkingLotModel>(this.apiUrl + "/get?id="+id)
  }
  addLot(Lot: ParkingLotModel){
    return this.http.post(this.apiUrl + "/add",Lot);
  }
  updateLot(Lot: ParkingLotModel){
    return this.http.post(this.apiUrl + "/update",Lot);
  }
  deleteLot(id:number){
    return this.http.post(this.apiUrl + "/delete?=" + id,null);
  }
}