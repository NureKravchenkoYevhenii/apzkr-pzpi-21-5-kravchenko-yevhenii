import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TarifModel } from '../models/tarifs/tarif-model';
import { Observable } from 'rxjs';
import { API, extractData } from '../constants/api-constants';
import { downloadFile } from '../helpers/helpers';
import { L10nTranslationService } from 'angular-l10n';

@Injectable({
  providedIn: 'root'
})
export class TarifService {

    constructor(
        private http: HttpClient,
        private translator: L10nTranslationService
    ) { }

    public create(tarifModel: TarifModel): Observable<any> {
        var url = API.tarifs.create;

        return this.http.post(
            url,
            tarifModel
        ).pipe(extractData());
    }

    public delete(tarifId: number): Observable<any> {
        var url = API.tarifs.delete;
        var params = new HttpParams().set(
            "tarifId",
            tarifId
        );

        return this.http.delete(
            url,
            { params: params }
        ).pipe(extractData());
    }

    public getAll(): Observable<TarifModel[]> {
        var url = API.tarifs.getAll;

        return this.http.get(url)
            .pipe(extractData<TarifModel[]>());
    }

    public getById(tarifId: number): Observable<TarifModel> {
        var url = API.tarifs.get;
        var params = new HttpParams().set(
            'tarifId',
            tarifId
        );

        return this.http.get(
            url,
            { params : params}
        ).pipe(extractData<TarifModel>());
    }

    public update(tarifModel: TarifModel): Observable<any> {
        var url = API.tarifs.update;

        return this.http.post(
            url,
            tarifModel
        ).pipe(extractData());
    }

    public getPaymentStatistics(from: Date, to: Date): void {
        var url = API.payments.getPaymentStatistics;
        var params = { from: from.toISOString(), to: to.toISOString() };
        var fileName = this.translator.translate('PaymentStatistics');

        this.http.get(url, {
            params: params,
            responseType: 'blob',
            reportProgress: true
        }).subscribe((result => {
            downloadFile(result, fileName);
        }))
    }
}
