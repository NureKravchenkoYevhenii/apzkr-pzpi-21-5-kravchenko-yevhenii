import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreateBookingModel } from '../models/booking/create-booking-model';
import { Observable } from 'rxjs';
import { API, extractData } from '../constants/api-constants';
import { BookingModel } from '../models/booking/booking-model';

@Injectable({
  providedIn: 'root'
})
export class BookingService {

    constructor(
        private http: HttpClient
    ) { }

    public make(bookingModel: CreateBookingModel): Observable<any> {
        var url = API.bookings.makeBooking;

        return this.http.post(
            url,
            bookingModel
        ).pipe(extractData())
    }

    public cancel(bookingId: number): Observable<any> {
        var url = API.bookings.cancelBooking;
        var params = new HttpParams().set(
            "bookingId",
             bookingId
        );

        return this.http.delete(
            url,
            { params: params},
        ).pipe(extractData());
    }

    public getUserBookings(userId: number): Observable<any> {
        var url = API.bookings.getUserBookings;
        var params = new HttpParams().set(
            "userId",
            userId
        );

        return this.http.get(
            url,
            { params: params }
        ).pipe(extractData<BookingModel[]>())
    }
    
}
