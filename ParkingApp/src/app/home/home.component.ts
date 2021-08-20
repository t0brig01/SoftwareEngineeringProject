import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { NgbDateStruct, NgbTimeStruct, NgbTimepicker}  from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { Modal } from "./modal.component";
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { DateHelpers } from '../helpers/dateHelpers';
import { ParkingLotModel } from '../models/parkingLotModel';
import { ParkingLotService } from '../services/parkingLot.service';
import { ParkingPassModel } from '../models/parkingPassModel';
import { ParkingPassService } from '../services/parkingPass.service';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit, AfterViewInit {
  currentPass: ParkingPassModel;
  passes: ParkingPassModel[];
  timeMeridian:boolean;
  filterTime:NgbTimeStruct;
  activeEditRef: NgbModalRef;
  parkingLots: ParkingLotModel[];
  filteredLots: ParkingLotModel[];
  lotService: ParkingLotService;
  time = "00:00:00" as unknown;
  lotList = new Array();
  inputDisabled: boolean = true;  

  @ViewChild("mapContainer", { static: false }) gmap: ElementRef;
  map: google.maps.Map;
  centerlat = 38.217472;
  centerlng = -85.758641;

  markers = [];

  //Coordinates to set the center of the map
  coordinates = new google.maps.LatLng(this.centerlat, this.centerlng);

  mapOptions: google.maps.MapOptions = {
    center: this.coordinates,
    zoom: 15,
    mapTypeId: "hybrid",
    styles: [
      {
        "featureType": "poi",
        "stylers": [
          { "visibility": "off" }
        ]
      }
    ]
  };

  constructor(private readonly modalService:NgbModal, private router:Router, lotService: ParkingLotService, passService: ParkingPassService) { 
    this.timeMeridian = true;
    this.filterTime = {hour: 0, minute: 0, second: 0};
    this.lotService = lotService;


    let dummy = new ParkingPassModel();
    dummy.shortDesc = "Any";
    if(this.currentPass === undefined)
    {
      this.currentPass = dummy;
    }

    passService.getAllPasses().subscribe((passData) => {
      this.passes = passData;
      console.log(this.passes);
    });

    lotService.getAllLots().subscribe((lotData) => {
      this.parkingLots = lotData;
      console.log(this.parkingLots);

      this.parkingLots.forEach(lot => {
        this.markers.push({
          position: new google.maps.LatLng(lot.latitude, lot.longitude),
          map: this.map,
          title: lot.shortDesc
        });
        this.lotList.push(lot.shortDesc);
      });

      this.loadAllMarkers();
    });
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    this.mapInitializer();
  }

  filter()
  {
    let temp = [];
    this.lotList = [];
    this.filteredLots.forEach(lot => {
      temp.push({
        position: new google.maps.LatLng(lot.latitude, lot.longitude),
        map: this.map,
        title: lot.shortDesc
      });
      this.lotList.push(lot.shortDesc);
    });

    this.parkingLots.forEach(lot => {
      if(Date.parse('01/01/2020 ' + this.time) > Date.parse('01/01/2020 ' + lot.anyPassStart.toString())
       || Date.parse('01/01/2020 ' + this.time) < Date.parse('01/01/2020 ' + lot.exclusivePassStart.toString()))
      {
        temp.push({
          position: new google.maps.LatLng(lot.latitude, lot.longitude),
          map: this.map,
          title: lot.shortDesc
        });
        if(!(this.lotList.indexOf(lot.shortDesc) > -1))
        {
          this.lotList.push(lot.shortDesc);
        }
      }
    });

    this.markers = temp;
    this.mapInitializer();
    this.inputDisabled = false;
  }

  onTimeChange(time) {
    this.time = DateHelpers.ngTimeStructToTime(time);
    this.filter();
  }

  changePass(chosenPass: ParkingPassModel) {
    this.currentPass = chosenPass;
    this.lotService.getLotsByPass(chosenPass.parkingPassCd1).subscribe((lotData) => {
      this.filteredLots = lotData
      this.filter();
    });
  }

  loginButton(){
    this.activeEditRef = this.modalService.open(Modal,{backdrop:"static"});
    this.activeEditRef.result.then(x => {
      if (x === true){
        this.router.navigateByUrl('/dataentry');
      }else{
      }
    });
  }

  mapInitializer(): void {
    this.map = new google.maps.Map(this.gmap.nativeElement, this.mapOptions);

    //Adding other markers
    this.loadAllMarkers();
  }

  loadAllMarkers(): void {
    this.markers.forEach(markerInfo => {
      //Creating a new marker object
      const marker = new google.maps.Marker({
        ...markerInfo
      });

      //creating a new info window with markers info
      const infoWindow = new google.maps.InfoWindow({
        content: marker.getTitle()
      });

      //Add click event to open info window on marker
      marker.addListener("click", () => {
        infoWindow.open(marker.getMap(), marker);
      });

      //Adding marker to google map
      marker.setMap(this.map);
    });
  }
}
