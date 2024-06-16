import { DayOfWeek } from "../../enums/day-of-week";
import { TimeUnitValue } from "../../enums/time-unit-value";

export interface TarifModel {
    id: number,
    name: string,
    activeOnDaysOfWeek: DayOfWeek[],
    startTime: Date,
    endTime: Date,
    timeUnitValue: TimeUnitValue,
    pricePerTimeUnit: number
}
