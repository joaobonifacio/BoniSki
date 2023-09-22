import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
export class TextInputComponent implements ControlValueAccessor {
  @Input() type = 'text';
  @Input() label = '';

  constructor(@Self() public controlDir: NgControl){
    this.controlDir.valueAccessor = this;
    //console.log('cheguei ao text input component')
  }

  get control(): FormControl{
    //console.log(this.controlDir.control)
    return this.controlDir.control as FormControl
  }

  writeValue(obj: any): void {
  }
  registerOnChange(fn: any): void {
  }
  registerOnTouched(fn: any): void {
  } 
}
