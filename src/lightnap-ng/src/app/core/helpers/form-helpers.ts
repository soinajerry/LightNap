import { FormGroup } from "@angular/forms";

/**
 * Validator function to check if two password fields match.
 *
 * @param firstPasswordName - The name of the first password form control.
 * @param secondPasswordName - The name of the second password form control.
 * @returns A validator function that takes a FormGroup and returns an error object if the passwords do not match, or null if they do.
 */
export function confirmPasswordValidator(firstPasswordName: string, secondPasswordName: string) {
  return (form: FormGroup) => {
    const newPassword = form.get(firstPasswordName);
    const confirmPassword = form.get(secondPasswordName);

    if (newPassword.value !== confirmPassword.value) {
      return { passwordsDoNotMatch: true };
    }

    return null;
  };
}
