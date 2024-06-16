import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { L10nDateDirective, L10nIntlModule, L10nTranslationModule } from 'angular-l10n';
import { TranslationLoader, l10nConfig } from './l10n-config';
import { ToastrModule } from 'ngx-toastr';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';
import { httpInterceptorProviders } from './core/helpers/http-interceptor.service';
import { FooterComponent } from './components/shared/footer/footer.component';
import { HeaderComponent } from './components/shared/header/header.component';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { createTranslateLoader } from './core/helpers/helpers';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { AdministrationComponent } from './components/administration/administration.component';
import { BookingComponent } from './components/booking/booking.component';
import { MakeBookingComponent } from './components/make-booking/make-booking.component';
import { ParkingPlacesComponent } from './components/parking-places/parking-places.component';
import { ParkingPlaceEditComponent } from './components/parking-place-edit/parking-place-edit.component';
import { TarifsComponent } from './components/tarifs/tarifs.component';
import { TarifEditComponent } from './components/tarif-edit/tarif-edit.component';
import { MembershipsComponent } from './components/memberships/memberships.component';
import { MembershipEditComponent } from './components/membership-edit/membership-edit.component';
import { UsersComponent } from './components/users/users.component';
import { UserDetailsComponent } from './components/user-details/user-details.component';
import { BlockMembershipCommentComponent } from './components/block-membership-comment/block-membership-comment.component';

@NgModule({
    declarations: [
        AppComponent,
        FooterComponent,
        HeaderComponent,
        LoginComponent,
        RegistrationComponent,
        AdministrationComponent,
        BookingComponent,
        MakeBookingComponent,
        ParkingPlacesComponent,
        ParkingPlaceEditComponent,
        TarifsComponent,
        TarifEditComponent,
        MembershipsComponent,
        MembershipEditComponent,
        UsersComponent,
        UserDetailsComponent,
        BlockMembershipCommentComponent,
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        FormsModule,
        ReactiveFormsModule,
        BrowserAnimationsModule,
        ToastrModule.forRoot({
            positionClass: 'toast-bottom-right',
            closeButton: true,
            progressBar: true
        }),
        JwtModule.forRoot({
            config: {
                tokenGetter: () => localStorage.getItem('access_token'),
                allowedDomains: ['example.com'],
                disallowedRoutes: ['example.com/login'],
            },
        }),
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: createTranslateLoader,
                deps: [HttpClient]
            }
        }),
        L10nTranslationModule.forRoot(
            l10nConfig,
            {
                translationLoader: TranslationLoader
            }
        ),
        L10nIntlModule,
        L10nDateDirective,
        NgbModule
    ],
    exports: [
        L10nTranslationModule,
    ],
    providers: [
        httpInterceptorProviders,
        JwtHelperService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
