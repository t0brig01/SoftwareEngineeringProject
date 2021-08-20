import { Component, Input } from "@angular/core";

import { NgbActiveModal, NgbModal, NgbModalRef, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { ParkingMapService } from '../services/parkingMap.service';
import { ParkingLotService } from '../services/parkingLot.service';
import { ParkingPassModel } from '../models/parkingPassModel';
import { ParkingLotModel } from '../models/parkingLotModel';
import { DateHelpers } from '../helpers/dateHelpers';
import { ParkingMapModel } from '../models/parkingMapModel';

@Component({
    selector:'ngbd-modal-content',
    templateUrl: './addlotmodal.component.html',
    styleUrls: ['./dataentry.component.css']
})
export class AddLotModal{
    @Input() public passes: ParkingPassModel[];
    @Input() public existingLot: ParkingLotModel;
    passMaps: ParkingMapModel[];
    meridian: boolean = true;
    weekendEnforced: boolean = false;
    latitude: string;
    longitude: string;
    currentPass: string = "None";
    exPassStart: NgbTimeStruct;
    exPassEnd: NgbTimeStruct;
    anyPassStart: NgbTimeStruct;
    publicPassStart: NgbTimeStruct;
    lot: ParkingLotModel;
    map: ParkingMapModel;

    constructor(public activeModel:NgbActiveModal,private parkingMapService:ParkingMapService, private parkingLotService:ParkingLotService){
        this.lot = new ParkingLotModel;
    }

    ngOnInit()
    {
        if (this.existingLot) 
        {
            this.lot.id = this.existingLot.id;
            this.lot.shortDesc = this.existingLot.shortDesc;
            this.lot.address = this.existingLot.address;
            this.lot.latitude = this.existingLot.latitude;
            this.lot.longitude = this.existingLot.longitude;
            this.lot.weekendEnforcementFlag = this.existingLot.weekendEnforcementFlag;
            this.exPassStart = DateHelpers.timeToNgTimeStruct(this.existingLot.exclusivePassStart);
            this.exPassEnd = DateHelpers.timeToNgTimeStruct(this.existingLot.exclusivePassEnd);
            this.anyPassStart = DateHelpers.timeToNgTimeStruct(this.existingLot.anyPassStart);
            this.publicPassStart = DateHelpers.timeToNgTimeStruct(this.existingLot.allPassStart);

            this.parkingMapService.getAllMaps().subscribe(x => {
                this.passMaps = x;
                this.getAllPassMappingsForLot(this.existingLot.id);
            });            
        }
        else 
        {   
            this.lot.id = 0;
        }
    }

    changePass(pass: string)
    {
        this.currentPass = pass;
    }

    getAllPassMappingsForLot(lotId: number)
    {
        for(let map in this.passMaps)
        {
            if(this.passMaps[map].parkingLotId == lotId)
            {
                for(let pass in this.passes)
                {
                    if(this.passes[pass].parkingPassCd1 === this.passMaps[map].parkingPassCd)
                    {
                        this.currentPass = this.passes[pass].shortDesc;
                        this.map = this.passMaps[map];
                    }
                }
            }
        }
    }
    
    cancel(){
        this.activeModel.close(false);
    }

    nextButton(){
        console.log(this.lot);
        if(this.existingLot)
        {
        
            this.parkingLotService.updateLot(this.lot).subscribe(
                x => 
                    { console.log (x)},
                error => {
                    console.log("Failed to update lot");
                    this.activeModel.close(error)
                },
                () => {
                    this.addOrUpdateParkingPassMap();
                }
            )
        }
        else
        {
            this.lot.exclusivePassStart =  this.exPassStart.hour.toString() + ":" + DateHelpers.padLeftWithZero(this.exPassStart.minute.toString(), 2);
            this.lot.exclusivePassEnd =  this.exPassEnd.hour.toString() + ":" + DateHelpers.padLeftWithZero(this.exPassEnd.minute.toString(), 2);
            this.lot.anyPassStart =  this.anyPassStart.hour.toString() + ":" + DateHelpers.padLeftWithZero(this.anyPassStart.minute.toString(), 2);
            this.lot.allPassStart =  this.publicPassStart.hour.toString() + ":" + DateHelpers.padLeftWithZero(this.publicPassStart.minute.toString(), 2);
            this.parkingLotService.addLot(this.lot).subscribe(
                x => { 
                    console.log (x)
                    this.lot.id = Number(x)
                    },
                error => {
                    console.log("Failed to Add Lot");
                    this.activeModel.close(error);
                },
                () => {
                    this.addOrUpdateParkingPassMap();
                }
            )
        }
    }

    addOrUpdateParkingPassMap()
    {
        if(this.map)
        {
            // Update old map
            this.map.parkingPassCd = this.currentPass.toUpperCase();
            this.parkingMapService.updateMap(this.map).subscribe(
                x => 
                    { console.log (x)},
                error => {
                    console.log("Failed to Update Pass Map");
                    this.activeModel.close(error);
                },
                () => 
                    this.activeModel.close(true)
            )
        }
        else
        {
            // Create new map
            if(this.currentPass != "None")
            {
                var newMap: ParkingMapModel = {id: 0, parkingLotId: this.lot.id, parkingPassCd: this.currentPass.toUpperCase(), primaryFlag: true};
                this.parkingMapService.addMap(newMap).subscribe(
                    x => 
                        { console.log (x)},
                    error => {
                        console.log("Failed to Pass Map");
                        this.activeModel.close(error);
                    },
                    () => 
                        this.activeModel.close(true)
                )
            }
        }
    }

}