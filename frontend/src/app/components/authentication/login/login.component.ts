import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatCard, MatCardTitle} from '@angular/material/card';
import {MatAnchor, MatButton} from '@angular/material/button';
import {Router, RouterLink} from '@angular/router';
import {AuthService} from '../../../services/auth.service';
import {LoginRequest} from '../../../Interfaces/login-request';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, MatFormFieldModule, MatInputModule, ReactiveFormsModule, MatCardTitle, MatCard, MatButton, RouterLink, MatAnchor],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;


  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router) {
  }

  ngOnInit() {
    this.loginForm = this.fb.group({
      emailFormControl: ['', [Validators.required, Validators.email]],
      passwordFormControl: ['', [Validators.required, Validators.minLength(6)]]
    });
  }


  onSubmit() {
    const loginForm: LoginRequest = {
      email: this.loginForm.controls['emailFormControl'].value,
      password: this.loginForm.controls['passwordFormControl'].value
    }

    this.authService.login(loginForm).subscribe(value => {
      this.router.navigate(['/products']);

    });
  }

  get f() {
    return this.loginForm.controls;
  }
}
