import {Directive, ElementRef, OnDestroy, OnInit, Renderer2} from '@angular/core';
import {AuthService} from '../services/auth.service';
import {Subscription} from 'rxjs';

@Directive({
  standalone: true,
  selector: '[appCheckRole]'
})
export class CheckRoleDirective implements OnInit, OnDestroy {
  private roleSubscription!: Subscription;

  constructor(
    private authService: AuthService,
    private renderer: Renderer2,
    private el: ElementRef
  ) {
  }

  ngOnInit(): void {
    this.roleSubscription = this.authService.checkRole().subscribe((role) => {
        if (role)
          this.renderer.setStyle(this.el.nativeElement, 'display', 'block');
        else
          this.renderer.setStyle(this.el.nativeElement, 'display', 'none');

      }
    );
  }

  ngOnDestroy():
    void {
    if (this.roleSubscription
    ) {
      this.roleSubscription.unsubscribe();
    }
  }
}
