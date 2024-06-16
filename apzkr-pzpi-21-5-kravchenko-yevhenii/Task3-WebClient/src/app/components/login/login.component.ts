import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';
import { StorageService } from '../../core/services/storage.service';
import { Router } from '@angular/router';
import { setActiveButton } from '../../core/helpers/helpers';
import { L10nToastrService } from '../../core/services/l10n-toastr.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit, AfterViewInit {

    loginForm!: FormGroup;

    constructor(
        private formBuilder: FormBuilder,
        private authService: AuthService,
        private storageService: StorageService,
        private router: Router,
        private toastr: L10nToastrService,
    ) { }

    ngOnInit(): void {
        this.loginForm = this.formBuilder.group({
            'login': new FormControl('', Validators.required),
            'password': new FormControl('', Validators.required),
        });

        if (this.storageService.isLoggedIn()) {
            this.router.navigate(['/books']);
        }
    }

    ngAfterViewInit(): void {
        setActiveButton('#loginBtn');
    }

    onSubmit(): void {
        this.authService.login(this.loginForm.value).subscribe({
            next: (token) => {
                this.storageService.saveToken(token);
                this.toastr.success(
                    'operationSuccessfull',
                    'success'
                );

                this.router.navigate(['/users']);
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
