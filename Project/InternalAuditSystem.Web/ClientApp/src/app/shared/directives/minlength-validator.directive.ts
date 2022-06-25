import { Directive, ElementRef, HostListener, Input } from '@angular/core';
import { NgControl, NgForm, AbstractControl, Validator, NG_VALIDATORS, ValidationErrors } from '@angular/forms';
import { StringConstants } from '../stringConstants';

@Directive({
  selector: '[appValidateMinLength]',
  providers: [{ provide: NG_VALIDATORS, useExisting: MinLengthValidatorDirective, multi: true }]
})

export class MinLengthValidatorDirective implements Validator {
  @Input('appValidateMinLength') minlength: string;


  /**
   * This directive will validate all input data againt the provided return test result
   * @param control: Input form control
   */
  public validate(control: AbstractControl): ValidationErrors | null {
    if (control.value !== null && control.value !== undefined) {

      const value = control.value.toString();
      const valid = value.length >= this.minlength;
      return !valid ? { appValidateMinLength: true } : null;
    }
    return null;
  }
}
