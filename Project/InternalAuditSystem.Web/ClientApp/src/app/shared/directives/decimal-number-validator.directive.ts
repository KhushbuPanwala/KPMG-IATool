import { Directive, ElementRef, AfterViewInit, HostListener } from '@angular/core';
import { StringConstants } from '../stringConstants';
import { NG_VALIDATORS, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Directive({
  selector: '[appValidateDecimalNumber]',
  providers: [{ provide: NG_VALIDATORS, useExisting: DecimalNumberValidatorDirective , multi: true }]
})

export class DecimalNumberValidatorDirective implements Validator {

  /**
   * This directive will validate all input data againt the provided return test result
   * @param control : Input form control
   */
  public validate(control: AbstractControl): ValidationErrors | null  {
    if (control.value !== null && control.value !== undefined) {
      const decimalNumberRegex = new RegExp(StringConstants.decimalNumberRegex);
      const valid = decimalNumberRegex.test(control.value);
      return control.value < 1 || valid ? null : { appValidateDecimalNumber: true };
    }
  }
}
