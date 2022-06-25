import { Directive, ElementRef, HostListener, Input } from '@angular/core';
import { NgControl, NgForm, AbstractControl, Validator, NG_VALIDATORS, ValidationErrors } from '@angular/forms';
import { StringConstants } from '../stringConstants';

@Directive({
  selector: '[appValidateEmail]',
  providers: [{ provide: NG_VALIDATORS, useExisting: EmailValidatorDirective, multi: true }]
})

export class EmailValidatorDirective implements Validator {

  /**
   * This directive will validate all input data againt the provided return test result
   * @param control: Input form control
   */
  public validate(control: AbstractControl): ValidationErrors  | null  {
    if (control.value !== null && control.value !== undefined) {
      if (control.value.trim() !== '') {
        const emailRegex = new RegExp(StringConstants.emailRegex);
        // test the input value
        const valid = emailRegex.test(control.value.trim());
        return control.value < 1 || valid ? null : { appValidateEmail: true };
      } else {
        return { appValidateEmail: true };
      }
    }
  }
}
