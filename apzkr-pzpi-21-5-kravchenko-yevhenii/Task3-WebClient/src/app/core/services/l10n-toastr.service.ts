import { Injectable } from '@angular/core';
import { L10nTranslationService } from 'angular-l10n';
import { ToastrService } from 'ngx-toastr';

@Injectable({
    providedIn: 'root'
})
export class L10nToastrService {

    constructor(
        private toastr: ToastrService,
        private translator: L10nTranslationService
    ) { }

    public success(message: string, title: string): void {
        var translations = this.translator.translate([message, title]);

        this.toastr.success(
            translations[message],
            translations[title]
        );
    }

    public error(message: string, title: string): void {
        var translations = this.translator.translate([message, title]);

        this.toastr.error(
            translations[message],
            translations[title]
        );
    }
}
