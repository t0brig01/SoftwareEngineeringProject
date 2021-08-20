import { Component } from "@angular/core";

import { NgbActiveModal, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AdminService } from '../services/admin.service';
import { AdminModel } from '../models/adminModel';


@Component({
    selector:'ngbd-modal-content',
    templateUrl: './modal.component.html',
    styleUrls: ['./home.component.css']
})
export class Modal{
    username: string;
    password: string;
    admin: AdminModel;

    constructor(public activeModel:NgbActiveModal,private adminService:AdminService){
        this.admin = new AdminModel;
    }
    
    cancel(){
        this.activeModel.close(false);
    }
    nextButton(){
        this.admin.Id = 0;
        
        this.adminService.validateLogin(this.admin).subscribe(
            x => 
                { console.log (x)},
            error => {
                alert("Incorrect Username or Password")
                this.activeModel.close(error)
            },
            () =>
                this.activeModel.close(true)
        )
    }
}