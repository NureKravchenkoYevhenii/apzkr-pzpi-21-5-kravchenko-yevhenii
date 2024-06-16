import { AfterViewInit, Component, OnInit } from '@angular/core';
import { setActiveButton } from '../../core/helpers/helpers';
import { AdministrationService } from '../../core/services/administration.service';
import { L10nToastrService } from '../../core/services/l10n-toastr.service';
import { StorageService } from '../../core/services/storage.service';
import { ParkingSettingsModel } from '../../core/models/booking/parking-settings-model';

@Component({
  selector: 'app-administration',
  templateUrl: './administration.component.html',
  styleUrl: './administration.component.scss'
})
export class AdministrationComponent implements OnInit, AfterViewInit {
    savePath: string = '';
    parkingSettings: ParkingSettingsModel = {
        id: 0,
        bookingDurationInMinutes: 0,
        bookingTimeAdvanceInMinutes: 0
    };

    constructor(
        private administrationService: AdministrationService,
        private toastr: L10nToastrService,
        private storageService: StorageService,
    ) { }

    ngOnInit(): void {
        if (this.isParkingAdmin()) {
            this.getParkingSettings();
        }
    }

    ngAfterViewInit(): void {
        setActiveButton('#administrationBtn');
    }

    onBackupDatabaseButtonClick(): void {
        if (this.savePath === '') {
            return;
        }

        this.administrationService.backupDatabase(this.savePath).subscribe({
            next: () => {
                this.toastr.success(
                    'operationSuccessfull',
                    'success'
                );
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        });
    }

    getParkingSettings(): void {
        this.administrationService.getParkingSettings().subscribe({
            next: (settings) => {
                this.parkingSettings = settings;
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        })
    }

    onSaveParkingSettings(): void {
        this.administrationService.updateParkingSettings(this.parkingSettings).subscribe({
            next: () => {
                this.toastr.success(
                    'operationSuccessfull',
                    'success'
                );
                this.getParkingSettings();
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        });
    }

    isAdmin(): boolean {
        return this.storageService.isAdmin();
    }

    isParkingAdmin(): boolean {
        return this.storageService.isParkingAdmin();
    }
}
