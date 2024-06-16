import { Component, Input, OnInit, inject } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from '../../core/services/user.service';
import { L10nToastrService } from '../../core/services/l10n-toastr.service';
import { UserProfileModel } from '../../core/models/user/user-profile-model';
import { BlockMembershipModel } from '../../core/models/user-membership/block-membership-model';
import { BlockMembershipCommentComponent } from '../block-membership-comment/block-membership-comment.component';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrl: './user-details.component.scss'
})
export class UserDetailsComponent implements OnInit {
    
    activeModal = inject(NgbActiveModal);
    userProfile: UserProfileModel = {
        id: 0,
        firstName: '',
        lastName: '',
        profilePicture: '',
        address: '',
        phoneNumber: '',
        birthDate: null,
        birthDateString: '',
        email: '',
        login: '',
        registrationDate: null,
        registrationDateStr: '',
        role: 1,
        userMembershipModel: null
    };

    @Input() userId: number;

    constructor(
        private userService: UserService,
        private toastr: L10nToastrService,
        private modalService: NgbModal
    ) { }

    ngOnInit(): void {
        this.loadUserProfile();
    }

    onBlockMembershipBtnClick(userMembershipId: number): void {
        var modalRef = this.modalService.open(BlockMembershipCommentComponent);
        modalRef.componentInstance.userMembershipId = userMembershipId;
        
        modalRef.result.then((result) => {
            if (result.isSuccess) {
                this.userProfile.userMembershipModel.comment = result.comment;
                this.userProfile.userMembershipModel.isBlocked = true;
            }
        }).catch(() => { });
    }

    onUnBlockMembershipBtnClick(userMembershipId: number): void {
        var blockMembershipModel: BlockMembershipModel = {
            userMembershipId: userMembershipId,
            comment: '',
            isBlocked: false
        };

        this.userService.blockUserMembership(blockMembershipModel).subscribe({
            next: () => {
                this.userProfile.userMembershipModel.comment = '';
                this.userProfile.userMembershipModel.isBlocked = false;

                this.toastr.success(
                    'operationSuccessfull',
                    'success'
                );
            }
        });
    }

    private loadUserProfile(): void {
        var userProfile$ = this.userService.getUserProfileById(this.userId);
        userProfile$.subscribe({
            next: (userProfile) => {
                this.userProfile = userProfile;
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
