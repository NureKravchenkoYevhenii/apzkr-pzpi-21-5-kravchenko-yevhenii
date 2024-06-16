import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { BookingService } from '../../core/services/booking.service';
import { StorageService } from '../../core/services/storage.service';
import { L10nToastrService } from '../../core/services/l10n-toastr.service';

@Component({
  selector: 'app-make-booking',
  templateUrl: './make-booking.component.html',
  styleUrl: './make-booking.component.scss'
})
export class MakeBookingComponent implements OnInit {
    activeModal = inject(NgbActiveModal);
    makeBookingForm!: FormGroup;
    
    constructor(
        private formBuilder: FormBuilder,
        private bookingService: BookingService,
        private toastr: L10nToastrService,
        private storageService: StorageService
    ) { }

    ngOnInit(): void {
        this.initForm();
    }

    onSubmit(): void {
        var operationResult$ = this.bookingService
            .make(this.makeBookingForm.value);

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
        var userId = this.storageService.getCurrentUserId();

        this.makeBookingForm = this.formBuilder.group({
            'id': [0, Validators.required],
            'bookDate': ['', Validators.required],
            'userId': [userId, Validators.required]
        });
    }
    
}
