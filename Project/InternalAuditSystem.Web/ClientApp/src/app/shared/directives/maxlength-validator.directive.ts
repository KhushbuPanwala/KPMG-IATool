import { Directive, ElementRef, HostListener, Input } from '@angular/core';
import { NgControl, NgForm, AbstractControl, Validator, NG_VALIDATORS, ValidationErrors } from '@angular/forms';
import { StringConstants } from '../stringConstants';

@Directive({
  selector: '[appValidateMaxLength]',
  providers: [{ provide: NG_VALIDATORS, useExisting: MaxLengthValidatorDirective, multi: true }]
})

export class MaxLengthValidatorDirective implements Validator {
  @Input('appValidateMaxLength') maxlength: string;


  /**
   * This directive will validate all input data againt the provided return test result
   * @param control: Input form control
   */
  public validate(control: AbstractControl): ValidationErrors | null {
    if (control.value !== null && control.value !== undefined) {
      const value = control.value.toString();
      const valid = value.length <= this.maxlength;
      return !valid ? { appValidateMaxLength: true } : null;
    }
    return null;
  }
}
