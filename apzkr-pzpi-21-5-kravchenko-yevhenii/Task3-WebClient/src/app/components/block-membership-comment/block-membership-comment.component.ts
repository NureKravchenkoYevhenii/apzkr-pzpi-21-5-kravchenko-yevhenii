import { Component, Input, inject } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from '../../core/services/user.service';
import { L10nToastrService } from '../../core/services/l10n-toastr.service';
import { Observable } from 'rxjs';
import { BlockMembershipModel } from '../../core/models/user-membership/block-membership-model';

@Component({
  selector: 'app-block-membership-comment',
  templateUrl: './block-membership-comment.component.html',
  styleUrl: './block-membership-comment.component.scss'
})
export class BlockMembershipCommentComponent {

    activeModal = inject(NgbActiveModal);
    comment: string = '';

    @Input() userMembershipId: number;

    constructor(
        private userService: UserService,
        private toastr: L10nToastrService,
    ) { }

    onConfirmClick(): void {
        var operationResult$: Observable<any>;
        var blockMembershipModel: BlockMembershipModel = {
            userMembershipId: this.userMembershipId,
            comment: this.comment,
            isBlocked: true
        };

        operationResult$ = this.userService
            .blockUserMembership(blockMembershipModel);

        operationResult$.subscribe({
            next: () => {
                this.toastr.success(
                    'operationSuccessfull',
                    'success'
                );
                this.activeModal.close({
                    isSuccess: true,
                    comment: this.comment
                });
            },
            error: (error) => {
                console.log(error);
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        });
    }
}
