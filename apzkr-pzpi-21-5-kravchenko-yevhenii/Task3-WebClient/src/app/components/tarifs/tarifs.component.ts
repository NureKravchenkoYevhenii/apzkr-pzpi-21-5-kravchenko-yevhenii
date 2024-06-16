import { AfterViewInit, Component, OnInit } from '@angular/core';
import { TarifService } from '../../core/services/tarif.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable, of } from 'rxjs';
import { TarifModel } from '../../core/models/tarifs/tarif-model';
import { DayOfWeek } from '../../core/enums/day-of-week';
import { TimeUnitValue } from '../../core/enums/time-unit-value';
import { TarifEditComponent } from '../tarif-edit/tarif-edit.component';
import { L10nTranslationService } from 'angular-l10n';
import { L10nToastrService } from '../../core/services/l10n-toastr.service';
import { setActiveButton } from '../../core/helpers/helpers';
import { StorageService } from '../../core/services/storage.service';

@Component({
  selector: 'app-tarifs',
  templateUrl: './tarifs.component.html',
  styleUrl: './tarifs.component.scss'
})
export class TarifsComponent implements OnInit, AfterViewInit {

    tarifs$: Observable<TarifModel[]>;
    currencySign: string;

    constructor(
        private tarifService: TarifService,
        private toastr: L10nToastrService,
        private modalService: NgbModal,
        private translator: L10nTranslationService,
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
        this.updateTarifs()
    }

    ngAfterViewInit(): void {
        setActiveButton("#tarifBtn");
    }

    updateTarifs(): void {
        this.tarifService.getAll().subscribe({
            next: (result) => {
                this.tarifs$ = of(result);
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        });
    }

    onEditButtonClick(tarifId: number): void {
        var modalRef = this.modalService.open(TarifEditComponent);
        modalRef.componentInstance.tarifId = tarifId;
        
        modalRef.result.then((result) => {
            if (result) {
                this.updateTarifs();
            }
        }).catch(() => { });
    }

    onDeleteButtonClick(tarifId: number): void {
        var deleteResult$ = this.tarifService
            .delete(tarifId);

        deleteResult$.subscribe({
            next: () => {
                this.toastr.success(
                    'operationSuccessfull',
                    'success'
                );
                this.updateTarifs();
            },
            error: (error) => {
                this.toastr.error(
                    error.error['message'],
                    'error'
                );
            }
        });
    }

    onGetPaymentStatsButtonClick(): void {
        var dateFrom = (document.querySelector('#dateFrom') as HTMLInputElement).valueAsDate;
        var dateTo = (document.querySelector('#dateTo') as HTMLInputElement).valueAsDate;

        this.tarifService.getPaymentStatistics(dateFrom, dateTo);
    }

    displayActiveDays(values: DayOfWeek[]): string {
        var translations = this.translator.translate(values.map(x => DayOfWeek[x]));
        return values.map(x => translations[DayOfWeek[x]]).join(', ')
    }

    displayTimeUnit(value: TimeUnitValue): string {
        var translate = this.translator.translate(TimeUnitValue[value]);
        
        return translate;
    }

}
