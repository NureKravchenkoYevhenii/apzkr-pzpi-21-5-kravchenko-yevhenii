import { AfterViewInit, Component, OnInit } from '@angular/core';
import { BookingService } from '../../core/services/booking.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable, of } from 'rxjs';
import { BookingModel } from '../../core/models/booking/booking-model';
import { StorageService } from '../../core/services/storage.service';
import { MakeBookingComponent } from '../make-booking/make-booking.component';
import { L10nToastrService } from '../../core/services/l10n-toastr.service';
import { setActiveButton } from '../../core/helpers/helpers';

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrl: './booking.component.scss'
})
export class BookingComponent implements OnInit, AfterViewInit {

    bookings$: Observable<BookingModel[]>;

    constructor(
        private bookingService: BookingService,
        private toastr: L10nToastrService,
        private storageService: StorageService,
        private modalService: NgbModal,
    ) { }

    ngOnInit(): void {
        this.updateBookings();
    }

    ngAfterViewInit(): void {
        setActiveButton("#bookingsBtn");
    }

    updateBookings(): void {
        var userId = this.storageService.getCurrentUserId();

        this.bookingService.getUserBookings(userId).subscribe({
            next: (bookings) => {
                this.bookings$ = of(bookings);
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        });
    }

    onCancelButtonClick(bookingId: number) {
        var deleteResult$ = this.bookingService
            .cancel(bookingId);

        deleteResult$.subscribe({
            next: () => {
                this.toastr.success(
                    'operationSuccessfull',
                    'success'
                );
                this.updateBookings();
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        });
    }

    onAddButtonClick() {
        var modalRef = this.modalService.open(MakeBookingComponent);
        modalRef.result.then((result) => {
            if (result) {
                this.updateBookings();
            }
        }).catch(() => { });
    }
    
}
