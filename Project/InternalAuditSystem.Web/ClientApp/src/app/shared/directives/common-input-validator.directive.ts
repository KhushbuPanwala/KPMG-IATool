import { Directive, ElementRef, AfterViewInit, HostListener } from '@angular/core';
import { StringConstants } from '../stringConstants';
import { NG_VALIDATORS, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Directive({
  selector: '[appValidateCommnInput]',
  providers: [{ provide: NG_VALIDATORS, useExisting: CommonInputValidatorDirective, multi: true }]
})

export class CommonInputValidatorDirective implements Validator {

  /**
   * This directive will validate all input data againt the provided return test result
   * @param control : Input form control
   */
  public validate(control: AbstractControl): ValidationErrors |  null {
    if (control.value !== null && control.value !== undefined) {
      if (control.value.trim() !== '') {
        const commonInputRegex = new RegExp(StringConstants.commonInputRegex);
        const valid = commonInputRegex.test(control.value.trim());
        return control.value < 1 || valid ? null : { appValidateCommnInput: true };
      } else {
        return { appValidateCommnInput: true };
      }
    }
  }
}
