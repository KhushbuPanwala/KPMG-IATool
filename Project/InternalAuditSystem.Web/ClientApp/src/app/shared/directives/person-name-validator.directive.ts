import { Directive, ElementRef, AfterViewInit, HostListener } from '@angular/core';
import { NgControl, NgForm, NG_VALIDATORS, Validator, AbstractControl, ValidationErrors } from '@angular/forms';
import { StringConstants } from '../stringConstants';

@Directive({
  selector: '[appValidateName]',
  providers: [{ provide: NG_VALIDATORS, useExisting: PersonNameValidatorDirective, multi: true }]
})


export class PersonNameValidatorDirective implements Validator {

  /**
   * This directive will validate all input data againt the provided return test result
   * @param control: Input form control
   */
  public validate(control: AbstractControl): ValidationErrors | null  {
    if (control.value !== null && control.value !== undefined) {

      if (control.value.trim() !== '') {
        const nameRegex = new RegExp(StringConstants.nameRegex);
        const valid = nameRegex.test(control.value.trim());
        return control.value < 1 || valid ? null : { appValidateName: true };
      } else {
        return { appValidateName: true };
      }
    }
  }
}
