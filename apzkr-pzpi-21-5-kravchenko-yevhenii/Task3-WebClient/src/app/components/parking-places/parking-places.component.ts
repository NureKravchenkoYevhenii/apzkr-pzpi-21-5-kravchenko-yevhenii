import { AfterViewInit, Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ParkingPlaceService } from '../../core/services/parking-place.service';
import { Observable, of } from 'rxjs';
import { ParkingPlaceListModel } from '../../core/models/parking-places/parking-place-list-model';
import { ParkingPlaceEditComponent } from '../parking-place-edit/parking-place-edit.component';
import { L10nToastrService } from '../../core/services/l10n-toastr.service';
import { setActiveButton } from '../../core/helpers/helpers';

@Component({
  selector: 'app-parking-places',
  templateUrl: './parking-places.component.html',
  styleUrl: './parking-places.component.scss'
})
export class ParkingPlacesComponent implements OnInit, AfterViewInit {

    parkingPlaces$: Observable<ParkingPlaceListModel[]>;

    constructor(
        private parkingPlaceService: ParkingPlaceService,
        private toastr: L10nToastrService,
        private modalService: NgbModal,
    ) { }

    ngOnInit(): void {
        this.updateParkingPlaces();
    }

    ngAfterViewInit(): void {
        setActiveButton('#parkingPlacesBtn');
    }

    updateParkingPlaces(): void {
        this.parkingPlaceService.getAll().subscribe({
            next: (result) => {
                this.parkingPlaces$ = of(result);
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        });
    }

    onEditButtonClick(parkingPlaceId: number): void {
        var modalRef = this.modalService.open(ParkingPlaceEditComponent);
        modalRef.componentInstance.parkingPlaceId = parkingPlaceId;
        
        modalRef.result.then((result) => {
            if (result) {
                this.updateParkingPlaces();
            }
        }).catch(() => { });
    }

    onDeleteButtonClick(parkingPlaceId: number): void {
        var deleteResult$ = this.parkingPlaceService
            .delete(parkingPlaceId);

        deleteResult$.subscribe({
            next: () => {
                this.toastr.success(
                    'operationSuccessfull',
                    'success'
                );
                this.updateParkingPlaces();
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        });
    }

    onGetParkingStatsButtonClick(): void {
        var dateFrom = (document.querySelector('#dateFrom') as HTMLInputElement).valueAsDate;
        var dateTo = (document.querySelector('#dateTo') as HTMLInputElement).valueAsDate;

        this.parkingPlaceService.getParkingStatistics(dateFrom, dateTo);
    }
}
