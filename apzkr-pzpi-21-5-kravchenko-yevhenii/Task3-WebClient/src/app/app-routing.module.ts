import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { AdministrationComponent } from './components/administration/administration.component';
import { BookingComponent } from './components/booking/booking.component';
import { ParkingPlacesComponent } from './components/parking-places/parking-places.component';
import { TarifsComponent } from './components/tarifs/tarifs.component';
import { MembershipsComponent } from './components/memberships/memberships.component';
import { UsersComponent } from './components/users/users.component';

const routes: Routes = [
    {
        path: '',
        redirectTo: '/login',
        pathMatch: 'full',
    },
    {
        path: 'registration',
        component: RegistrationComponent,
    },
    {
        path: 'login',
        component: LoginComponent,
    },
    {
        path: 'administration',
        component: AdministrationComponent,
    },
    {
        path: 'bookings',
        component: BookingComponent
    },
    {
        path: 'parking-places',
        component: ParkingPlacesComponent
    },
    {
        path: 'tarifs',
        component: TarifsComponent
    },
    {
        path: 'memberships',
        component: MembershipsComponent
    }, 
    {
        path: 'users',
        component: UsersComponent
    }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
