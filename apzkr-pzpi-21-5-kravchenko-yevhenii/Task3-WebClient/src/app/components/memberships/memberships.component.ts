import { AfterViewInit, Component, OnInit } from '@angular/core';
import { MembershipService } from '../../core/services/membership.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable, of } from 'rxjs';
import { MembershipModel } from '../../core/models/membership/membership-model';
import { MembershipEditComponent } from '../membership-edit/membership-edit.component';
import { L10nToastrService } from '../../core/services/l10n-toastr.service';
import { setActiveButton } from '../../core/helpers/helpers';
import { StorageService } from '../../core/services/storage.service';

@Component({
  selector: 'app-memberships',
  templateUrl: './memberships.component.html',
  styleUrl: './memberships.component.scss'
})
export class MembershipsComponent implements OnInit, AfterViewInit {

    memberships$: Observable<MembershipModel[]>;
    currencySign: string = 'UAH';

    constructor(
        private membershipService: MembershipService,
        private toastr: L10nToastrService,
        private modalService: NgbModal,
        private storageService: StorageService
    ) { 
        var currencLocale = this.storageService.getLocale();
        if (currencLocale == 'uk-UA') {
            this.currencySign = 'UAH';
        } else {
            this.currencySign = 'USD'
        }
    }

    ngOnInit(): void {
        this.updateMemberships();
    }

    ngAfterViewInit(): void {
        setActiveButton('#membershipBtn')
    }

    updateMemberships(): void {
        this.membershipService.getAll().subscribe({
            next: (result) => {
                this.memberships$ = of(result);
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        });
    }

    onEditButtonClick(membershipId: number): void {
        var modalRef = this.modalService.open(MembershipEditComponent);
        modalRef.componentInstance.membershipId = membershipId;
        
        modalRef.result.then((result) => {
            if (result) {
                this.updateMemberships();
            }
        }).catch(() => { });
    }

    onDeleteButtonClick(membershipId: number): void {
        var deleteResult$ = this.membershipService
            .delete(membershipId);

        deleteResult$.subscribe({
            next: () => {
                this.toastr.success(
                    'operationSuccessfull',
                    'success'
                );
                this.updateMemberships();
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        });
    }
}
