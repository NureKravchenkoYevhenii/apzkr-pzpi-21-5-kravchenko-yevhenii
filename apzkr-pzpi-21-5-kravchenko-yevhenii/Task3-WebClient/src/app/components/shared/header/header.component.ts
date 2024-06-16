import { Component, Inject, OnInit } from '@angular/core';
import { StorageService } from '../../../core/services/storage.service';
import { Router } from '@angular/router';
import { L10N_CONFIG, L10N_LOCALE, L10nConfig, L10nLocale, L10nTranslationService } from 'angular-l10n';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {

    selectedLocale: L10nLocale = this.config.schema[0].locale;

    constructor(
        private storageService: StorageService,
        private router: Router,
        @Inject(L10N_LOCALE) public locale: L10nLocale,
        @Inject(L10N_CONFIG) private config: L10nConfig,
        private translation: L10nTranslationService
    ) { }

    ngOnInit(): void {
        var cookieLocale = this.storageService.getLocale();
        if (cookieLocale == null || cookieLocale == undefined) {
            this.storageService.saveLocale(this.selectedLocale.language);
        } else {
            this.selectedLocale = this.config.schema.find(x => x.locale.language === cookieLocale)!.locale;
        }
    }

    onButtonClick(event: Event): void {
        var currentButton = <HTMLElement>event.currentTarget;
        if (currentButton.classList.contains('active')) {
            return;
        }

        this.disativateButtons();
        
        currentButton.classList.add('active');
    }

    disativateButtons(): void {
        var buttons = document.querySelectorAll('header ul button');

        for (let i = 0; i < buttons.length; i++) {
            buttons[i].classList.remove('active');
        }
    }

    isLoggedIn(): boolean {
        return this.storageService.isLoggedIn();
    }

    isAdmin(): boolean {
        return this.storageService.isAdmin();
    }

    isParkingAdmin(): boolean {
        return this.storageService.isParkingAdmin();
    }

    logout(): void {
        this.storageService.clear();
        this.router.navigate(['login']);
    }

    onSelectLocale(event: Event, locale: string): void {
        var eventTarget = <HTMLElement>event.currentTarget;
        if (eventTarget.classList.contains('active')) {
            return;
        }

        var ukLocale = <HTMLElement>document.querySelector('div.locale__ua');
        var enLocale = <HTMLElement>document.querySelector('div.locale__en');

        if (locale == 'en-US') {
            ukLocale.classList.remove('active');
            enLocale.classList.add('active');
        } else if (locale == 'uk-UA') {
            ukLocale.classList.add('active');
            enLocale.classList.remove('active');
        }
        
        this.selectedLocale = this.config.schema.find(x => x.locale.language === locale)!.locale;
        this.setLocale();
        window.location.reload();
    }

    setLocale(): void {
        this.translation.setLocale(this.selectedLocale);
        this.storageService.saveLocale(this.selectedLocale.language);
    }
}
