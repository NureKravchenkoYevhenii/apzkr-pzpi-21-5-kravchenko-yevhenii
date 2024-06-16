import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterUserModel } from '../models/user/register-user-model';
import { Observable } from 'rxjs';
import { API, extractData } from '../constants/api-constants';
import { UserProfileModel } from '../models/user/user-profile-model';
import { SetUserRoleModel } from '../models/user/set-user-role-model';
import { BlockMembershipModel } from '../models/user-membership/block-membership-model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

    constructor(
        private http: HttpClient
    ) { }

    public register(registerUserModel: RegisterUserModel): Observable<any> {
        var url = API.user.register;
        registerUserModel.profilePicture = '';

        return this.http.post(
            url, 
            registerUserModel
        ).pipe(extractData());
    }

    public getAll(searchQuery: string): Observable<UserProfileModel[]> {
        var url = API.user.getAll;
        var params = new HttpParams().set('searchQuery', searchQuery);

        return this.http.get(
            url, 
            { params: params }
        ).pipe(extractData<UserProfileModel[]>());
    }

    public getUserProfileById(userId: number): Observable<UserProfileModel> {
        var url = API.user.get;
        var params = new HttpParams().set('userId', userId);

        return this.http.get(
            url,
            { params: params }
        ).pipe(extractData<UserProfileModel>());
    }

    public setUserRole(setUserRoleModel: SetUserRoleModel): Observable<any> {
        var url = API.user.setUserRole;

        return this.http.post(
            url, 
            setUserRoleModel
        ).pipe(extractData());
    }

    public blockUserMembership(blockMembershipModel: BlockMembershipModel): Observable<any> {
        var url = API.user.blockMembership;

        return this.http.post(
            url,
            blockMembershipModel
        ).pipe(extractData());
    }

    public uploadUserData(file: File): Observable<any> {
        var url = API.user.uploadUserData;
        var formData = new FormData();
        formData.append('file', file);

        return this.http.post(
            url,
            formData
        ).pipe(extractData());
    }

}
