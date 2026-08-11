import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-auth-message',
    standalone: true,
    imports: [
        CommonModule
    ],
    templateUrl: './auth-message.html',
    styleUrls: ['./auth-message.scss']
})
export class AuthMessage {

    @Input()
    icon = '✔';

    @Input()
    title = '';

    @Input()
    message = '';

    @Input()
    buttonText = 'Voltar';

    @Output()
    buttonClick = new EventEmitter<void>();

    onButtonClick(): void {
        this.buttonClick.emit();
    }

}