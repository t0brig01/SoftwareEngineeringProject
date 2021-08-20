import { Component, Input } from "@angular/core";

import { NgbActiveModal, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AdminService } from '../services/admin.service';
import { AdminModel } from '../models/adminModel';

@Component({
    selector:'ngbd-modal-content',
    templateUrl: './addadminmodal.component.html',
    styleUrls: ['./dataentry.component.css']
})
export class AddAdminModal{
    @Input() public existingAdmin: any;
    admin: AdminModel;
    usernameDisabled: boolean = false;
    confirmPassword: string;
    oldPassword: string;
    newAdmin: boolean = true;

    constructor(public activeModel:NgbActiveModal,private adminService:AdminService){
        this.admin = new AdminModel;
    }

    ngOnInit()
    {
        if (this.existingAdmin) 
        {
            this.admin.Username = this.existingAdmin.username;
            this.admin.Id = this.existingAdmin.id;
            this.usernameDisabled = true;
            this.newAdmin = false;
        }
        else 
        {   
            this.admin.Id = 0;
        }
    }
    
    cancel(){
        this.activeModel.close(false);
    }

    nextButton(){
        // Require all fields to be filled and new password to be confirmed
        if(this.admin.Username != null && this.admin.Password != null && this.confirmPassword != null && this.admin.Password === this.confirmPassword)
        {
            this.activeModel.close(true);
        
            if(this.existingAdmin)
            {
                var oldAdmin: AdminModel = {
                    Id: this.admin.Id, 
                    Username: this.admin.Username, 
                    Password: this.oldPassword};

                this.adminService.validateLogin(oldAdmin).subscribe(
                    x => 
                        { console.log (x)},
                    error => {
                        console.log("Toast that the old password is wrong");
                    },
                    () => {
                        this.adminService.updateAdmin(this.admin).subscribe(
                            x => 
                                { console.log (x)},
                            error => {
                                this.activeModel.close(error);
                            },
                            () =>
                                this.activeModel.close(true)
                        )
                    }  
                )
            }
            else
            {
                this.adminService.addAdmin(this.admin).subscribe(
                    x => 
                        { console.log (x)},
                    error => {
                        alert("Username is already taken");
                    },
                    () =>
                        this.activeModel.close(true)
                )
            }
            
        }
        else
        {
            console.log("Toast that it be wrong");
        }
        
    }
}