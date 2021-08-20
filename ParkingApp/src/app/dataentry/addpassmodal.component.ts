import { Component } from "@angular/core";

import { NgbActiveModal, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ParkingPassService } from '../services/parkingPass.service';
import { ParkingPassModel } from '../models/parkingPassModel';

@Component({
    selector:'ngbd-modal-content',
    templateUrl: './addpassmodal.component.html',
    styleUrls: ['./dataentry.component.css']
})
export class AddPassModal{
    passColor: string;
    passService: ParkingPassService;

    constructor(public activeModel:NgbActiveModal, private parkingPassService:ParkingPassService){
        this.passService = parkingPassService;
    }

    ngOnInit()
    {
        
    }
    
    cancel(){
        this.activeModel.close(false);
    }

    nextButton(){

        var pass:ParkingPassModel = {
            parkingPassCd1: this.passColor.toUpperCase(),
            shortDesc: this.passColor,
            displaySequence: 0,
            activeFlag: true}

        this.passService.addPass(pass).subscribe(
            x => 
                { console.log (x)},
            error => {
                alert("Pass color already exists");
            },
            () =>
                this.activeModel.close(true)
        )   
    }
}