import { Component, Input, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MembershipService } from '../../core/services/membership.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MembershipModel } from '../../core/models/membership/membership-model';
import { Observable } from 'rxjs';
import { L10nToastrService } from '../../core/services/l10n-toastr.service';

@Component({
  selector: 'app-membership-edit',
  templateUrl: './membership-edit.component.html',
  styleUrl: './membership-edit.component.scss'
})
export class MembershipEditComponent implements OnInit {

    activeModal = inject(NgbActiveModal);
    membershipForm!: FormGroup;

    @Input() membershipId: number;

    constructor(
        private formBuilder: FormBuilder,
        private membershipService: MembershipService,
        private toastr: L10nToastrService,
    ) { }

    ngOnInit(): void {
        this.initForm();
        if (this.membershipId > 0) {
            this.fillForm();
        }
    }

    onSubmit(): void {
        var operationResult$: Observable<any>;
        if (this.membershipId > 0) {
            operationResult$ = this.membershipService
                .update(this.membershipForm.value);
        } else {
            operationResult$ = this.membershipService
                .create(this.membershipForm.value);
        }

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
        this.membershipForm = this.formBuilder.group({
            'id': [this.membershipId, Validators.required],
            'name': ['', Validators.required],
            'durationInDays': [0, Validators.required],
            'price': [0, Validators.required]
        });
    }

    private fillForm(): void {
        var membership$ = this.getMembership();
        membership$.subscribe({
            next: (membership) => {
                this.membershipForm.patchValue({
                    id: membership.id,
                    name: membership.name,
                    durationInDays: membership.durationInDays,
                    price: membership.price
                });
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        });
    }

    private getMembership(): Observable<MembershipModel> {
        return this.membershipService.get(this.membershipId);
    }

}
