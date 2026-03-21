import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home';
import { MeasurementResultsComponent } from './pages/measurement-results/measurement-results';
import { MeasurementResultCreateComponent } from './pages/measurement-result-create/measurement-result-create';
import { MeasurementResultDetailsComponent } from './pages/measurement-result-details/measurement-result-details';
import { MeasurementResultEditComponent } from './pages/measurement-result-edit/measurement-result-edit';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'measurement-results', component: MeasurementResultsComponent },
  { path: 'measurement-results/create', component: MeasurementResultCreateComponent },
  { path: 'measurement-results/:id', component: MeasurementResultDetailsComponent },
  { path: 'measurement-results/:id/edit', component: MeasurementResultEditComponent }
];