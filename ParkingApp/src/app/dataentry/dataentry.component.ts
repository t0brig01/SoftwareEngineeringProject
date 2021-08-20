import { Component, OnInit } from '@angular/core';
import { ParkingLotModel } from '../models/parkingLotModel';
import { ParkingLotService } from '../services/parkingLot.service';
import { AdminModel } from '../models/adminModel';
import { AdminService } from '../services/admin.service';
import { ParkingPassModel } from '../models/parkingPassModel';
import { ParkingPassService } from '../services/parkingPass.service';
import { AddAdminModal } from "./addadminmodal.component";
import { AddLotModal } from "./addlotmodal.component";
import { AddPassModal } from "./addpassmodal.component";
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ParkingMapService } from '../services/parkingMap.service';

@Component({
  selector: 'dataentry',
  templateUrl: './dataentry.component.html',
  styleUrls: ['./dataentry.component.css']
})
export class DataEntryComponent implements OnInit {
  admins: AdminModel[];
  parkingLots: ParkingLotModel[];
  passes: ParkingPassModel[];
  activeEditRef: NgbModalRef;
  adminService: AdminService;
  lotService: ParkingLotService;
  parkingMapService: ParkingMapService;
  parkingPassService: ParkingPassService;

  constructor( 
    private readonly modalService:NgbModal, 
    lotService: ParkingLotService, 
    adminService: AdminService, 
    passService: ParkingPassService,
    parkingMapService: ParkingMapService)
  { 
    this.adminService = adminService;
    this.lotService = lotService;
    this.parkingMapService = parkingMapService;
    this.parkingPassService = passService;

    adminService.getAllAdmins().subscribe(x => {
      this.admins = x;
    });
    lotService.getAllLots().subscribe(x => {
      this.parkingLots = x;
    });
    passService.getAllPasses().subscribe(x => {
      this.passes = x;
    });
  }

  ngOnInit() {

  }

  addAdmin() {
    this.activeEditRef = this.modalService.open(AddAdminModal,{backdrop:"static"});
    this.activeEditRef.result.then(x => {
      if (x === true){
        console.log("New Admin to add");
        location.reload();
      }else{
        console.log("Cancelled");
      }
    });
  }

  deleteAdmin(admin: any) {
    this.adminService.deleteAdmin(admin.username).subscribe(
      x => { 
          console.log (x)
          },
      error => {
          console.log("Failed to Delete Admin");
          console.log(error);
      },
      () => {
        location.reload();
      }
    );
  }

  changePassword(admin: AdminModel) {
    this.activeEditRef = this.modalService.open(AddAdminModal,{backdrop:"static"});
    this.activeEditRef.componentInstance.existingAdmin = admin;
    this.activeEditRef.result.then(x => {
      if (x === true){
        location.reload();
      }else{
        console.log("Cancelled");
      }
    });
  }

  addLot() {
    this.activeEditRef = this.modalService.open(AddLotModal,{backdrop:"static"});
    this.activeEditRef.componentInstance.passes = this.passes;
    this.activeEditRef.result.then(x => {
      if (x === true){
        console.log("New Lot to add");
        location.reload();
      }else{
        console.log("Cancelled");
      }
    });
  }

  deleteLot(id: number) {
    this.parkingMapService.deleteMapByLotId(id).subscribe(
      x => { 
          console.log (x)
          },
      error => {
          this.lotService.deleteLot(id).subscribe(
            x => { 
                console.log (x)
                },
            error => {
                console.log("Failed to Delete Lot");
                console.log(error);
            },
            () => {
                console.log("Lot deleted");
                location.reload();
            }
          )
      },
      () => {
          console.log("Pass maps deleted");
          this.lotService.deleteLot(id).subscribe(
            x => { 
                console.log (x)
                },
            error => {
                console.log("Failed to Delete Lot");
                console.log(error);
            },
            () => {
                console.log("Lot deleted");
                location.reload();
            }
          )
      }
    )
  }

  changeLot(lot: ParkingLotModel) {
    this.activeEditRef = this.modalService.open(AddLotModal,{backdrop:"static"});
    this.activeEditRef.componentInstance.passes = this.passes;
    this.activeEditRef.componentInstance.existingLot = lot;
    this.activeEditRef.result.then(x => {
      if (x === true){
        console.log("New Lot to add");
        location.reload();
      }else{
        console.log("Cancelled");
      }
    });
  }

  addPass() {
    this.activeEditRef = this.modalService.open(AddPassModal,{backdrop:"static"});
    this.activeEditRef.result.then(x => {
      if (x === true){
        console.log("New Pass to add");
        location.reload();
      }else{
        console.log("Cancelled");
      }
    });
  }

  deletePass(pass: ParkingPassModel) {
    this.parkingMapService.deleteMapByPassColor(pass.parkingPassCd1).subscribe(
      x => { 
          console.log (x)
          },
      error => {
          this.parkingPassService.deletePass(pass.parkingPassCd1).subscribe(
            x => { 
                console.log (x)
                },
            error => {
                console.log("Failed to Delete Pass");
                console.log(error);
            },
            () => {
              location.reload();
            }
          )
      },
      () => {
        this.parkingPassService.deletePass(pass.parkingPassCd1).subscribe(
          x => { 
              console.log (x)
              },
          error => {
              console.log("Failed to Delete Pass");
              console.log(error);
          },
          () => {
            location.reload();
          }
        )
      }
    )
  }
}
