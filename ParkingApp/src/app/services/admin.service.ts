import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AdminModel } from '../models/adminModel';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = 'https://parkappapi.azurewebsites.net/api/admin';
  constructor(private http:HttpClient) { }
  getAllAdmins():Observable<AdminModel[]>{
    return this.http.get<AdminModel[]>(this.apiUrl + "/getall");
  }
  addAdmin(Admin: AdminModel){
    return this.http.post(this.apiUrl + "/add",Admin);
  }
  updateAdmin(Admin: AdminModel){
    return this.http.post(this.apiUrl + "/update",Admin);
  }
  deleteAdmin(username:string){
    return this.http.post(this.apiUrl + "/delete?=" + username,null);
  }
  validateLogin(admin:AdminModel){
    return this.http.post(this.apiUrl + "/ValidateLogin",admin);
  }
}