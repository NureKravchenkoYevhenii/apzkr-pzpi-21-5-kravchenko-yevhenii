import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MembershipModel } from '../models/membership/membership-model';
import { API, extractData } from '../constants/api-constants';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembershipService {

    constructor(
        private http: HttpClient
    ) { }

    public create(membershipModel: MembershipModel): Observable<any> {
        var url = API.memberships.create;

        return this.http.post(
            url,
            membershipModel
        ).pipe(extractData());
    }

    public delete(membershipId: number): Observable<any> {
        var url = API.memberships.delete;
        var params = new HttpParams().set(
            "membershipId",
            membershipId
        );

        return this.http.delete(
            url,
            { params: params }
        ).pipe(extractData())
    }

    public getAll(): Observable<MembershipModel[]> {
        var url = API.memberships.getAll;

        return this.http.get(url).pipe(extractData<MembershipModel[]>());
    }

    public get(membershipId: number): Observable<MembershipModel> {
        var url = API.memberships.get;
        var params = new HttpParams().set(
            'membershipId',
            membershipId
        );

        return this.http.get(
            url,
            { params: params }
        ).pipe(extractData());
    }

    public update(membershipModel: MembershipModel): Observable<any> {
        var url = API.memberships.update;
        
        return this.http.post(
            url,
            membershipModel
        ).pipe(extractData());
    }
}
