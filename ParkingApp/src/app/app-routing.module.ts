import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component'
import { DataEntryComponent } from './dataentry/dataentry.component';


const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'dataentry', component: DataEntryComponent},
  { path: '**', component: HomeComponent }
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
