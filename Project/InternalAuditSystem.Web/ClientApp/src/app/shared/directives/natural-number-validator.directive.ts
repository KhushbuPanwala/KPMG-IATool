import { Directive, ElementRef, AfterViewInit, HostListener } from '@angular/core';
import { StringConstants } from '../stringConstants';
import { NG_VALIDATORS, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Directive({
  selector: '[appValidateNaturalNumber]',
  providers: [{ provide: NG_VALIDATORS, useExisting: NaturalNumberValidatorDirective, multi: true }]
})

export class NaturalNumberValidatorDirective implements Validator {

  /**
   * This directive will validate all input data againt the provided return test result
   * @param control: Input form control
   */
  public validate(control: AbstractControl): ValidationErrors| null  {
    if (control.value !== null && control.value !== undefined) {

      const naturalNumberRegex = new RegExp(StringConstants.naturalNumberRegex);
      const valid = naturalNumberRegex.test(control.value);
      return control.value < 1 || valid ? null : { appValidateNaturalNumber: true };
    }
  }
}
