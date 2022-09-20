import { Component } from '@angular/core';

@Component({
    selector: 'app-counter-component',
    templateUrl: './counter.component.html'
})
export class CounterComponent {
    public currentCount = 10;

    public incrementCounter() {
        this.currentCount++;
    }
}
