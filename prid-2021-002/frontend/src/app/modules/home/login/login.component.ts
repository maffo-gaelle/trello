import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl, AsyncValidatorFn } from '@angular/forms';
import { AuthenticationService } from 'src/app/core/services/authentication.service';


@Component({
    templateUrl: './login.component.html',
    styleUrls: ['login.component.scss']
})

export class LoginComponent implements OnInit {
    loginForm: FormGroup;
    loading = false;    
    submitted = false; 

    returnUrl: string;
    ctlPseudo: FormControl;
    ctlPassword: FormControl;
    hide = true;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService
    ) {
        if (this.authenticationService.currentUser) {
            this.router.navigate(['/boardlist']);
        }
      
    }

    ngOnInit() {

        this.ctlPseudo = this.formBuilder.control('', [
        Validators.required], 
        [this.pseudoUsed()]);
        this.ctlPassword = this.formBuilder.control('', Validators.required);
        this.loginForm = this.formBuilder.group({
            pseudo: this.ctlPseudo,
            password: this.ctlPassword
        });

        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/boardlist';
    }

    pseudoUsed() : AsyncValidatorFn {
        let timeout: NodeJS.Timer;
        return (ctl: FormControl) => {
            clearTimeout(timeout);
            const pseudo = ctl.value;
            return new Promise(resolve => {
                timeout = setTimeout(() => {
                    if(ctl.pristine) {
                        resolve(null);
                    } else {
                        this.authenticationService.pseudoUsed(pseudo).subscribe(user => {
                            resolve(!user ? null : {pseudoUsed: true});
                        });
                    }
                }, 300);
            });
        }; 
    }

    get f() { return this.loginForm.controls; }
    
    onSubmit() {
        this.submitted = true;

        if (this.loginForm.invalid) return;

        this.loading = true;
        this.authenticationService.login(this.f.pseudo.value, this.f.password.value)

            .subscribe(
                data => {
                    this.router.navigate([this.returnUrl]);
                },
                error => {
                    const errors = error.error.errors;
                    for (let field in errors) {
                        this.loginForm.get(field.toLowerCase()).setErrors({ custom: errors[field] })
                    }
                    this.loading = false;
                }
            );
    }
}