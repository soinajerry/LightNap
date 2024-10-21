import { FormGroup } from "@angular/forms";

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
