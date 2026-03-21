import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home';
import { MeasurementResultsComponent } from './pages/measurement-results/measurement-results';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'measurement-results', component: MeasurementResultsComponent }
];