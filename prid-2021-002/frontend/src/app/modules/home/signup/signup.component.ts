import { Component } from "@angular/core";
import { AsyncValidatorFn, FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { AuthenticationService } from "src/app/core/services/authentication.service";

@Component({
    selector: 'app-signup',
    templateUrl: './signup.component.html',
    styleUrls: ['./signup.component.scss']
})
export class SignupComponent {

    form: FormGroup;
    hide1 = true;
    hide2 = true;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        public formBuilder: FormBuilder,
        private authenticationService: AuthenticationService
    ) {
        this.createFormgroup();
    }


    createFormgroup() {
        this.form =  this.formBuilder.group({
            pseudo: [null, [
                Validators.required,
                Validators.pattern('^[A-Za-z][A-Za-z0-9_]{2,9}$'),
                Validators.minLength(3),
                Validators.maxLength(10),
            ], [this.pseudoUsed()]],
            password: [null, [
                Validators.required,
                Validators.minLength(3),
                Validators.maxLength(10)
            ], []],
            passwordConfirm: [null, [
                Validators.required
            ], [this.equalPassword()]],
            email: [null, [
                Validators.required,
                Validators.pattern('^[A-Za-z0-9](([_\\.\\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\\.\\-]?[a-zA-Z0-9]+)*)\\.([A-Za-z]{2,})$'),
            ], [this.emailUsed()]],
            firstName: [null, [
                Validators.required,
                Validators.minLength(3),
                Validators.maxLength(50)
            ], []],
            lastName: [null, [
                Validators.required,
                Validators.minLength(3),
                Validators.maxLength
            ], []],
            birthDate: [null, 
            [], [this.ageMinimum()]],
        })
    }

    pseudoUsed(): AsyncValidatorFn {
        let timeout: NodeJS.Timer;
        return (ctl: FormControl) => {
            clearTimeout(timeout);
            const pseudo = ctl.value;
            return new Promise(resolve => {
                timeout = setTimeout(() => {
                    if(ctl.pristine) {
                        resolve(null);
                    } else {
                        this.authenticationService.pseudoUsed(pseudo).subscribe( user => {
                            resolve(user ? null : {pseudoUsed: true});
                        });
                    }
                }, 300);
            })
        }
    }

    emailUsed(): AsyncValidatorFn {
        let timeout: NodeJS.Timer;
        return (ctl: FormControl) => {
            clearTimeout(timeout);
            const email = ctl.value;
            return new Promise(resolve => {
                timeout = setTimeout(() => {
                    if(ctl.pristine) {
                        resolve(null);
                    } else {
                        this.authenticationService.emailUsed(email).subscribe( user => {
                            resolve(user ? null : {emailUsed: true});
                        });
                    }
                }, 300);
            })
        }
    }

    ageMinimum(): any {
        let timeout: NodeJS.Timer;
        return (ctl: FormControl) => {
            clearTimeout(timeout);
            const date = new Date(ctl.value);
            const diff = Date.now() - date.getTime();
            const age = new Date(diff).getUTCFullYear() - 1970;
            return new Promise(resolve => {
                timeout = setTimeout(() => {
                    if(ctl.pristine) {
                        resolve(null);
                    } else if (diff < 0){
                        resolve(diff >= 0 ? null : {futureBorn: true});
                    } else {
                        resolve(age >= 18 ? null : {toYoung: true});
                    }
                }, 300);
            })
        }
    }
    
    equalPassword(): any {
        let timeout: NodeJS.Timer;
        return (ctl: FormControl) => {
            clearTimeout(timeout);
            const passwordConfirm = ctl.value;
            return new Promise(resolve => {
                timeout = setTimeout(() => {
                    if(ctl.pristine) {
                        resolve(null);
                    } else {
                        resolve(this.form.get("password").value == passwordConfirm ? null : { equalPassword : true});
                    }
                }, 300);
            })
        }
    }
    
    sauver() {
        const data = this.form.value;
     
        this.authenticationService.signup(data).subscribe( () => {
            if(this.authenticationService.currentUser) {
                this.router.navigate(['/home']);
            }
        });
    }

}