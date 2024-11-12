import {Component, OnInit} from '@angular/core';
import {MatCardModule} from '@angular/material/card';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {MatInputModule} from '@angular/material/input';
import {MatButtonModule} from '@angular/material/button';
import {Router, RouterLink} from '@angular/router';
import {MatFormFieldModule} from '@angular/material/form-field';
import {AuthService} from '../../../services/auth.service';
import {LoginRequest} from '../../../Interfaces/login-request';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;

  constructor(private fb: FormBuilder, private router: Router, private authService: AuthService) {

  }

  ngOnInit() {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    }, {
      validators: this.passwordMatchValidator
    });
  }

  passwordMatchValidator(form: FormGroup): null | { mismatch: boolean } {
    return form.get('password')?.value === form.get('confirmPassword')?.value
      ? null : {mismatch: true};
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      const loginRequestExample: LoginRequest = {
        email: this.registerForm.controls['email'].value,
        password: this.registerForm.controls['password'].value
      };
      console.log('Formularz rejestracji:', loginRequestExample);
      this.authService.register(loginRequestExample).subscribe(value => {
        this.router.navigate(['/login']).then(r => console.log('Przekierowanie na stronę logowania:', r));
      });
    }
  }

  get f() {
    return this.registerForm.controls;
  }
}
