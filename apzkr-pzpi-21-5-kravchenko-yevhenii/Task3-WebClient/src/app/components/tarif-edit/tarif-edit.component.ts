import { Component, Input, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TarifService } from '../../core/services/tarif.service';
import { TarifModel } from '../../core/models/tarifs/tarif-model';
import { Observable } from 'rxjs';
import { DayOfWeek } from '../../core/enums/day-of-week';
import { L10nTranslationService } from 'angular-l10n';
import { TimeUnitValue } from '../../core/enums/time-unit-value';
import { L10nToastrService } from '../../core/services/l10n-toastr.service';

@Component({
  selector: 'app-tarif-edit',
  templateUrl: './tarif-edit.component.html',
  styleUrl: './tarif-edit.component.scss'
})
export class TarifEditComponent implements OnInit {

    activeModal = inject(NgbActiveModal);
    tarifForm!: FormGroup;

    @Input() tarifId: number;

    constructor(
        private formBuilder: FormBuilder,
        private tarifService: TarifService,
        private toastr: L10nToastrService,
        private translator: L10nTranslationService
    ) { }

    ngOnInit(): void {
        this.initForm();
        if (this.tarifId > 0) {
            this.fillForm();
        }
    }
    
    onSubmit(): void {
        var operationResult$: Observable<any>;
        this.tarifForm.patchValue({
            'startTime': this.toValidTimeOnlyFormat(this.tarifForm.get('startTime').value),
            'endTime': this.toValidTimeOnlyFormat(this.tarifForm.get('endTime').value),
            'timeUnitValue': +this.tarifForm.get('timeUnitValue').value
        });

        if (this.tarifId > 0) {
            operationResult$ = this.tarifService
                .update(this.tarifForm.value);
        } else {
            operationResult$ = this.tarifService
                .create(this.tarifForm.value);
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

    getDaysOfWeek(): any[] {
        var options = [];

        for (var key in DayOfWeek) {
            if (isNaN(Number(key))) {
                options.push({
                    value: DayOfWeek[key],
                    label: this.translator.translate(key)
                });
            }
        }

        return options;
    }

    getTimeUnitValues(): any[] {
        var options = [];

        for (var key in TimeUnitValue) {
            if (isNaN(Number(key))) {
                options.push({
                    value: TimeUnitValue[key],
                    label: this.translator.translate(key)
                });
            }
        }

        return options;
    }

    identify(index: number, item: any) {
        return item.name;
    }

    private initForm(): void {
        this.tarifForm = this.formBuilder.group({
            'id': [this.tarifId, Validators.required],
            'name': ['', Validators.required],
            'activeOnDaysOfWeek': [[], Validators.required],
            'startTime': ['', Validators.required],
            'endTime': ['', Validators.required],
            'timeUnitValue': [1, Validators.required],
            'pricePerTimeUnit': [0, Validators.required]
        });
    }

    private fillForm(): void {
        var tarif$ = this.getParkingPlace();
        tarif$.subscribe({
            next: (tarif) => {
                this.tarifForm.patchValue({
                    id: tarif.id,
                    name: tarif.name,
                    activeOnDaysOfWeek: tarif.activeOnDaysOfWeek,
                    startTime: tarif.startTime,
                    endTime: tarif.endTime,
                    timeUnitValue: tarif.timeUnitValue,
                    pricePerTimeUnit: tarif.pricePerTimeUnit
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

    private getParkingPlace(): Observable<TarifModel> {
        return this.tarifService.getById(this.tarifId);
    }

    private toValidTimeOnlyFormat(time: string): string {
        console.log(time);
        if (!time) {
            return time;
        }

        var timeParts = time.split(':');
        if (timeParts.length == 2) {
            return `${time}:00`;
        }

        return time;
    }
    
}
