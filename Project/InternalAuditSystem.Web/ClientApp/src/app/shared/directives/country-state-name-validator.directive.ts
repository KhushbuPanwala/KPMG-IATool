import { Directive, ElementRef, HostListener, Input } from '@angular/core';
import { NgControl, NgForm, AbstractControl, Validator, NG_VALIDATORS, ValidationErrors } from '@angular/forms';
import { StringConstants } from '../stringConstants';

@Directive({
  selector: '[appValidateCountryStateName]',
  providers: [{ provide: NG_VALIDATORS, useExisting: CountryStateNameValidatorDirective, multi: true }]
})

export class CountryStateNameValidatorDirective implements Validator {

  /**
   * This directive will validate all input data againt the provided return test result
   * @param control : Input form control
   */
  public validate(control: AbstractControl): ValidationErrors |null  {
    if (control.value !== null && control.value !== undefined) {
      const countryStateRegex = new RegExp(StringConstants.countryStateRegex);

      if (control.value.trim() !== '') {
        // test the input value
        const valid = countryStateRegex.test(control.value.trim());
        return control.value < 1 || valid ? null : { appValidateCountryStateName: true };
      } else {
        return { appValidateCountryStateName: true };
      }
    }
  }
}
