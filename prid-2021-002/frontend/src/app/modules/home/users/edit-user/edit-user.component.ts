import { Component, Inject, OnDestroy } from "@angular/core";
import { AsyncValidatorFn, FormBuilder, FormControl, FormGroup, ValidationErrors, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { User } from "src/app/core/models/User";
import { AuthenticationService } from "src/app/core/services/authentication.service";
import { UserService } from "src/app/core/services/user.service";

@Component({
    selector: 'app-edit-user',
    templateUrl: './edit-user.component.html',
    styleUrls: ['./edit-user.component.scss']
})
export class EditUserComponent implements OnDestroy{
    public frm: FormGroup;
    public ctlPseudo: FormControl;
    public ctlPassword: FormControl;
    public ctlFirstName: FormControl;
    public ctlLastName: FormControl;
    public ctlEmail: FormControl;
    public ctlBirthDate: FormControl;
    public ctlRole: FormControl;
    public isNew: boolean;
    private tempPicturePath: string;
    private pictureChanged: boolean;
    hide = true;

    constructor(
        public dialogRef: MatDialogRef<EditUserComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { user: User; isNew: boolean},
        private fb: FormBuilder,
        private userService: UserService,
        private authenticationService: AuthenticationService
    )
    {
        this.ctlPseudo = this.fb.control('', [
            Validators.required,
            Validators.pattern('^[A-Za-z][A-Za-z0-9_]{2,9}$'),
            Validators.minLength(3),
            Validators.maxLength(10)
        ], [this.pseudoUsed()]);
        this.ctlPassword = this.fb.control('', data.isNew ? [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(10)] : []);
        this.ctlFirstName = this.fb.control('', [
            Validators.minLength(3),
            Validators.maxLength(50)]);
        this.ctlLastName = this.fb.control('', [
            Validators.minLength(3),
            Validators.maxLength(50)]);
        this.ctlEmail = this.fb.control('', [
            Validators.required,
            Validators.maxLength(50),
            Validators.email
        ], [this.emailUsed()]);
        this.ctlBirthDate = this.fb.control('', [
            this.validateBirthDate()]);
        this.ctlRole = this.fb.control('', []);
        this.frm = this.fb.group({
            pseudo : this.ctlPseudo,
            password : this.ctlPassword,
            email : this.ctlEmail,
            firstName : this.ctlFirstName,
            lastName : this.ctlLastName,
            birthDate : this.ctlBirthDate,
            role : this.ctlRole
        }, { validator: this.validateName});
        console.log(data);
        this.isNew = data.isNew;
        this.frm.patchValue(data.user);
        this.tempPicturePath = data.user.picturePath;
        this.pictureChanged = false;
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
                        this.authenticationService.pseudoUsed(pseudo).subscribe(user => {
                            resolve(!user ? { pseudoUser: true } : null);
                        })
                    }
                }, 300);
            });
        };
    }

    validateName(group: FormGroup) : ValidationErrors {
      if(!group.value) {return null;}
      let firstname: string = group.value.firstName;
      let lastname: string = group.value.lastName;
      if(lastname === "" && firstname !== "")
          return {lastNameRequired: true};

      if(lastname !== "" && firstname === "")
          return {firstNameRequired: true};

  }

    emailUsed(): AsyncValidatorFn {
        let timeout: NodeJS.Timer;
        return (ctl: FormControl) => {
            clearTimeout(timeout);
            const email= ctl.value;
            return new Promise(resolve => {
                timeout = setTimeout(() => {
                    if(ctl.pristine) {
                        resolve(null);
                    } else {
                        this.authenticationService.emailUsed(email).subscribe(user => {
                            resolve(user ? null : {emailUsed: true});
                        });
                    };
                }, 300);
            });
        }
    }

    validateBirthDate(): any {
        return (ctl: FormControl) => {
            const date = new Date(ctl.value);
            const diff = Date.now() - date.getTime();
            if(diff < 0)
                return {futureBorn: true}
            var age = new Date(diff).getFullYear() - 1970;
            if(age < 18)
                return { tooYoung: true };
            return null;
        };
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    update() {
        const data = this.frm.value
        data.picturePath = this.tempPicturePath;
        if(this.pictureChanged) {
            this.userService.confirmPicture(data.pseudo, this.tempPicturePath).subscribe();
            data.picturePath = 'uploads/' + data.pseudo + '.jpg';
            this.pictureChanged = false;
        }
        this.dialogRef.close(data);
    }

    cancel() {
        this.dialogRef.close();
    }

    cancelTempPicture() {
        const data = this.frm.value;
        if (this.pictureChanged) {
            this.userService.cancelPicture(this.tempPicturePath).subscribe();
        }
    }

    fileChange(event: any) {
        const fileList: FileList = event.target.files;
        if(fileList.length > 0) {
            const file = fileList[0];
            this.userService.uploadPicture(this.frm.value.pseudo || 'empty', file).subscribe(path => {
                console.log(path);
                this.cancelTempPicture();
                this.tempPicturePath = path;
                this.pictureChanged = true;
                this.frm.markAsDirty();
            });
        }
    }

    get picturePath(): string {
        return this.tempPicturePath && this.tempPicturePath !== '' ? this.tempPicturePath : 'uploads/unknown-user.jpg';
    }

    ngOnDestroy(): void {
        this.cancelTempPicture();
    }


}
