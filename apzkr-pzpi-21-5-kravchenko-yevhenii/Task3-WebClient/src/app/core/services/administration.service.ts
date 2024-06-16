import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API, extractData } from '../constants/api-constants';
import { ParkingSettingsModel } from '../models/booking/parking-settings-model';

@Injectable({
  providedIn: 'root'
})
export class AdministrationService {

    constructor(
        private http: HttpClient
    ) { }

    public backupDatabase(savePath: string): Observable<any> {
        var url = API.administration.backupDatabase;

        return this.http.post(url, { savePathStr: savePath })
            .pipe(extractData());
    }

    public updateParkingSettings(settingsModel: ParkingSettingsModel): Observable<any> {
        var url = API.administration.updateParkingSettings;
        
        return this.http.post(
            url,
            settingsModel
        ).pipe(extractData());
    }

    public getParkingSettings(): Observable<ParkingSettingsModel> {
        var url = API.administration.getParkingSettings;

        return this.http.get(
            url
        ).pipe(extractData<ParkingSettingsModel>());
    }
}
