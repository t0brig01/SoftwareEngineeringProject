import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ParkingPassModel } from '../models/parkingPassModel';

@Injectable({
  providedIn: 'root'
})
export class ParkingPassService {
  private apiUrl = 'https://parkappapi.azurewebsites.net/api/pass';
  constructor(private http:HttpClient) { }
  getAllPasses():Observable<ParkingPassModel[]>{
    return this.http.get<ParkingPassModel[]>(this.apiUrl + "/getall");
  }
  addPass(Lot: ParkingPassModel){
    return this.http.post(this.apiUrl + "/add",Lot);
  }
  updatePass(Lot: ParkingPassModel){
    return this.http.post(this.apiUrl + "/update",Lot);
  }
  deletePass(id: string){
    return this.http.post(this.apiUrl + "/delete?=" + id,null);
  }
}