import { AfterViewInit, Component, OnInit } from '@angular/core';
import { UserProfileModel } from '../../core/models/user/user-profile-model';
import { Observable, of } from 'rxjs';
import { UserService } from '../../core/services/user.service';
import { StorageService } from '../../core/services/storage.service';
import { SetUserRoleModel } from '../../core/models/user/set-user-role-model';
import { Role } from '../../core/enums/role';
import { setActiveButton } from '../../core/helpers/helpers';
import { L10nTranslationService } from 'angular-l10n';
import { L10nToastrService } from '../../core/services/l10n-toastr.service';
import { UserDetailsComponent } from '../user-details/user-details.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss'
})
export class UsersComponent implements OnInit, AfterViewInit {

    searchQuery: string = '';
    users$: Observable<UserProfileModel[]>

    constructor(
        private userService: UserService,
        private toastr: L10nToastrService,
        private modalService: NgbModal,
        private storageService: StorageService,
        private translator: L10nTranslationService
    ) { }

    ngOnInit(): void {
        this.updateUsers(this.searchQuery);
    }
    
    ngAfterViewInit(): void {
        setActiveButton('#usersBtn');
    }

    updateUsers(searchQuery: string): void {
        this.userService.getAll(searchQuery).subscribe({
            next: (users) => {
                this.users$ = of(users);
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        });
    }

    isAdmin(): boolean {
        return this.storageService.isAdmin();
    }

    isParkingAdmin(): boolean {
        return this.storageService.isParkingAdmin();
    }

    getRoles(): any[] {
        var options = [];

        for (var key in Role) {
            if (isNaN(Number(key))) {
                options.push({
                    value: Role[key],
                    label: this.translator.translate(key)
                });
            }
        }

        return options;
    }

    onSearchClick(): void {
        var searchInput = <HTMLInputElement>document.querySelector("input#search");
        var searchQuery = searchInput.value;

        this.updateUsers(searchQuery);
    }

    onDetailsButtonClick(userId: number): void {
        var modalRef = this.modalService.open(UserDetailsComponent);
        modalRef.componentInstance.userId = userId;
        
        modalRef.result.then((result) => {
            if (result) {
                this.updateUsers(this.searchQuery);
            }
        }).catch(() => { });
    }

    onChange(event: Event, userId: number) {
        var newRole = +(<HTMLSelectElement>event.currentTarget).value;
        var setUserRoleModel: SetUserRoleModel = {
            userId: userId,
            role: newRole,
        };

        this.userService.setUserRole(setUserRoleModel).subscribe({
            next: () => {
                this.toastr.success(
                    'operationSuccessfull',
                    'success'
                );
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        });
    }

    identify(index: number, item: any) {
        return item.name;
    }

    onUploadClick(): void {
        var fileInput = document.querySelector('#fileUpload') as HTMLInputElement;

        if (fileInput.files.length <= 0) {
            return;
        }

        var file = fileInput.files[0];

        this.userService.uploadUserData(file).subscribe({
            next: () => {
                this.toastr.success(
                    'operationSuccessfull',
                    'success'
                );
                this.updateUsers(this.searchQuery);
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        })
    }
    
}
