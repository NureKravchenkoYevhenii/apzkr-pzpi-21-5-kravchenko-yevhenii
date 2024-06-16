import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ParkingPlaceModel } from '../models/parking-places/parking-place-model';
import { Observable } from 'rxjs';
import { API, extractData } from '../constants/api-constants';
import { ParkingPlaceListModel } from '../models/parking-places/parking-place-list-model';
import { downloadFile } from '../helpers/helpers';
import { L10nTranslationService } from 'angular-l10n';

@Injectable({
  providedIn: 'root'
})
export class ParkingPlaceService {

    constructor(
        private http: HttpClient,
        private translator: L10nTranslationService
    ) { }
    
    public create(parkingPlaceModel: ParkingPlaceModel): Observable<any> {
        var url = API.parkingPlaces.create;

        return this.http.post(
            url,
            parkingPlaceModel
        ).pipe(extractData());
    }

    public delete(parkingPlaceId: number): Observable<any> {
        var url = API.parkingPlaces.delete;
        var params = new HttpParams().set(
            "parkingPlaceId",
            parkingPlaceId
        );

        return this.http.delete(
            url, 
            { params: params }
        ).pipe(extractData());
    }

    public getAll(): Observable<ParkingPlaceListModel[]> {
        var url = API.parkingPlaces.getAll;

        return this.http.get(
            url
        ).pipe(extractData<ParkingPlaceListModel[]>());
    }

    public getById(parkingPlaceId: number): Observable<ParkingPlaceModel> {
        var url = API.parkingPlaces.get;
        var params = new HttpParams().set(
            "parkingPlaceId",
            parkingPlaceId
        );

        return this.http.get(
            url,
            { params: params }
        ).pipe(extractData<ParkingPlaceModel>())
    }

    public update(parkingPlaceModel: ParkingPlaceModel): Observable<any> {
        var url = API.parkingPlaces.update;
        
        return this.http.post(
            url,
            parkingPlaceModel
        ).pipe(extractData());
    }

    public getParkingStatistics(from: Date, to: Date): void {
        var url = API.parkingSessions.getParkingStatistics;
        var params = { from: from.toISOString(), to: to.toISOString() };
        var fileName = this.translator.translate('ParkingStatistics');

        this.http.get(url, {
            params: params,
            responseType: 'blob',
            reportProgress: true
        }).subscribe((result => {
            downloadFile(result, fileName);
        }))
    }
}
