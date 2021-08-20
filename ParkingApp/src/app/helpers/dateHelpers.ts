import {NgbDateStruct, NgbTimeStruct} from '@ng-bootstrap/ng-bootstrap';
​
export class DateHelpers {
    static getLogTime(processStartDate: Date, logDate: Date): string {
        var start = new Date(processStartDate);
        var end = new Date(logDate);
​
        var seconds = (end.getTime() - start.getTime()) / 1000;
        return `${~~(seconds / 60)}:${DateHelpers.padLeftWithZero(String(~~(seconds % 60)), 2)}`;
    }
​
    static padLeftWithZero(text: string, size: number): string {
        return (String('0').repeat(size) + text).substr((size * -1), size);
    }
​
    static ngDateStructToDate(date: NgbDateStruct): string {
        if (date) {
            if (!date.year || !date.month || !date.day) { return null; }
            return date.year + '-' + DateHelpers.padLeftWithZero(date.month.toString(), 2) + '-' + DateHelpers.padLeftWithZero(date.day.toString(), 2);
        }
        return null;
    }
​
    static dateToNgDateStruct(date: string): NgbDateStruct {
        if (date) {
            const parts = date.split(/[-/]+/);
            return {
                year: parseInt(parts[2]),
                month: parseInt(parts[0]),
                day: parseInt(parts[1])
            };
        }
        return null;
    }

    static ngTimeStructToTime(time: NgbTimeStruct): string {
        if (time) {
            // if (!time.hour || !time.minute || !time.second) { //     return null; }
            return time.hour + ':' + DateHelpers.padLeftWithZero(time.minute.toString(), 2) + ':' + DateHelpers.padLeftWithZero(time.second.toString(), 2);
        }
        return null;
    }

    static timeToNgTimeStruct(time: string): NgbTimeStruct {
        if(time) {
            const parts = time.split(":");
            return {
                hour: parseInt(parts[0]),
                minute: parseInt(parts[1]),
                second: parseInt(parts[2])
            };
        }
        return null;
    }
​
​
    static dateDiffInDays(a:Date, b: Date) {
        // Discard the time and time-zone information.
        var utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
        var utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());
​
        return Math.floor((utc2 - utc1) / (1000 * 60 * 60 * 24));
    }
​
​
    static padValue(value: any) {
        return (value < 10) ? "0" + value : value;
    }
​
    static formatDate(dateVal: string): string {
        var newDate = new Date(dateVal);
​
        var sMonth = newDate.getMonth() + 1;
        var sDay = newDate.getDate();
        var sYear = newDate.getFullYear();
        var sHour = newDate.getHours();
        var sMinute = this.padValue(newDate.getMinutes());
        var sAMPM = "AM";
​
        if (sHour === 12) {
            sAMPM = "PM";
        }
        else if (sHour > 12) {
            sAMPM = "PM";
            sHour = sHour - 12;
        }
        else if (sHour === 0) {
            sHour = 12;
        }
​
        sHour = this.padValue(sHour);
​
        return sMonth + "/" + sDay + "/" + sYear + " " + sHour + ":" + sMinute + " " + sAMPM + " EST";
    }

    static formatDateObj(dateVal: Date): string {
​
        var sMonth = dateVal.getMonth() + 1;
        var sDay = dateVal.getDate();
        var sYear = dateVal.getFullYear();
        var sHour = dateVal.getHours();
        var sMinute = this.padValue(dateVal.getMinutes());
        var sAMPM = "AM";
​
        if (sHour === 12) {
            sAMPM = "PM";
        }
        else if (sHour > 12) {
            sAMPM = "PM";
            sHour = sHour - 12;
        }
        else if (sHour === 0) {
            sHour = 12;
        }
​
        sHour = this.padValue(sHour);
​
        return sMonth + "/" + sDay + "/" + sYear + " " + sHour + ":" + sMinute + " " + sAMPM + " EST";
    }
​
​
    static formatDateNoTime(dateVal: string): string {
        if (dateVal) {
            const parts = dateVal.split(/[-/]+/);
            return parseInt(parts[1]) + "/" + parseInt(parts[2]) + "/" + parseInt(parts[0]);
        }
        return null;
    }
​
    static isValidDate(d: NgbDateStruct): boolean {
        return d && d.day !== 0 && d.month !== 0 && d.year !== 0;
    }
    static cloneDate(date: Date): Date {
        if (date) {
            return new Date(date.valueOf());
        }
        return null;
    }
​
    static hasValue(date: Date) {
        if (date) {
            return true;
        }
        return false;
    }
}