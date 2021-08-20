import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { DataEntryComponent } from './dataentry/dataentry.component';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { Modal } from "./home/modal.component";
import { AddAdminModal } from "./dataentry/addadminmodal.component";
import { AddLotModal } from "./dataentry/addlotmodal.component";
import { AddPassModal } from "./dataentry/addpassmodal.component";
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    DataEntryComponent,
    Modal,
    AddAdminModal,
    AddLotModal,
    AddPassModal
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    FormsModule,
    HttpClientModule
  ],
  exports:[Modal, NgbModalModule, AddAdminModal, AddLotModal, AddPassModal],
  providers: [],
  bootstrap: [AppComponent, Modal, AddAdminModal, AddLotModal, AddPassModal, HomeComponent, DataEntryComponent],
  entryComponents: [Modal, AddAdminModal, AddLotModal, AddPassModal]
})
export class AppModule { }
