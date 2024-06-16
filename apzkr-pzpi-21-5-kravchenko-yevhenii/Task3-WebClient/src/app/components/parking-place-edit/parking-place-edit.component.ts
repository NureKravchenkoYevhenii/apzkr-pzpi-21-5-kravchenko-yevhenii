import { Component, Input, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ParkingPlaceService } from '../../core/services/parking-place.service';
import { Observable } from 'rxjs';
import { ParkingPlaceModel } from '../../core/models/parking-places/parking-place-model';
import { L10nToastrService } from '../../core/services/l10n-toastr.service';

@Component({
  selector: 'app-parking-place-edit',
  templateUrl: './parking-place-edit.component.html',
  styleUrl: './parking-place-edit.component.scss'
})
export class ParkingPlaceEditComponent implements OnInit {
    
    activeModal = inject(NgbActiveModal);
    parkingPlaceForm!: FormGroup;

    @Input() parkingPlaceId: number;

    constructor(
        private formBuilder: FormBuilder,
        private parkingPlaceService: ParkingPlaceService,
        private toastr: L10nToastrService,
    ) { }
    
    ngOnInit(): void {
        this.initForm();
        if (this.parkingPlaceId > 0) {
            this.fillForm();
        }
    }

    onSubmit(): void {
        var operationResult$: Observable<any>;
        if (this.parkingPlaceId > 0) {
            operationResult$ = this.parkingPlaceService
                .update(this.parkingPlaceForm.value);
        } else {
            operationResult$ = this.parkingPlaceService
                .create(this.parkingPlaceForm.value);
        }

        operationResult$.subscribe({
            next: () => {
                this.toastr.success(
                    'operationSuccessfull',
                    'success'
                );
                this.activeModal.close(true);
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        });
    }

    private initForm(): void {
        this.parkingPlaceForm = this.formBuilder.group({
            'id': [this.parkingPlaceId, Validators.required],
            'name': ['', Validators.required],
        });
    }

    private fillForm(): void {
        var parkingPlace$ = this.getParkingPlace();
        parkingPlace$.subscribe({
            next: (parkingPlace) => {
                this.parkingPlaceForm.patchValue({
                    id: parkingPlace.id,
                    name: parkingPlace.name,
                });
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        });
    }

    private getParkingPlace(): Observable<ParkingPlaceModel> {
        return this.parkingPlaceService.getById(this.parkingPlaceId);
    }

}
