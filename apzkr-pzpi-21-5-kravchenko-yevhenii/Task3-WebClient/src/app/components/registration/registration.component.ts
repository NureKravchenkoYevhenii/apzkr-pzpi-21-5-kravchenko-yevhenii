import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../core/services/user.service';
import { setActiveButton } from '../../core/helpers/helpers';
import { L10nToastrService } from '../../core/services/l10n-toastr.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.scss'
})
export class RegistrationComponent implements OnInit, AfterViewInit {
    registrationForm!: FormGroup

    constructor(
        private formBuilder: FormBuilder,
        private userService: UserService,
        private router: Router,
        private toastr: L10nToastrService
    ) { }

    ngOnInit(): void {
        this.registrationForm = this.formBuilder.group({
            'login': ['', [Validators.required, Validators.minLength(4), Validators.maxLength(32)]],
            'password': ['', [Validators.required, Validators.minLength(4), Validators.maxLength(32)]],
            'firstName': ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
            'lastName': ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
            'address': ['', [Validators.required, Validators.minLength(5), Validators.maxLength(100)]],
            'phoneNumber': ['', Validators.required],
            'birthDate': ['', Validators.required],
            'email': ['', [Validators.required, Validators.email]],
        });
    }

    ngAfterViewInit(): void {
        setActiveButton('#registrationBtn');
    }

    onSubmit(): void {
        this.userService.register(this.registrationForm.value).subscribe({
            next: () => {
                this.toastr.success(
                    'operationSuccessfull',
                    'success'
                );
                this.router.navigate(['login']);
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'operationError'
                );
            }
        });
    }
}
