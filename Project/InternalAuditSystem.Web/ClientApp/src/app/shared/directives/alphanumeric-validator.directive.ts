import { Directive, ElementRef, AfterViewInit, HostListener } from '@angular/core';
import { StringConstants } from '../stringConstants';
import { NG_VALIDATORS, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Directive({
  selector: '[appValidateAlphanumeric]',
  providers: [{ provide: NG_VALIDATORS, useExisting: AlphanumericValidatorDirective, multi: true }]
})

export class AlphanumericValidatorDirective implements Validator {

  /**
   * This directive will validate all input data againt the provided return test result
   * @param control : Input form control
   */
  public validate(control: AbstractControl): ValidationErrors | null  {
    if (control.value !== null && control.value !== undefined) {
      const alphanumericInputRegex = new RegExp(StringConstants.alphanumericRegex);
      const valid = alphanumericInputRegex.test(control.value.trim());
      return control.value < 1 || valid ? null : { appValidateAlphanumeric: true };
    }
  }
}
