import { Component } from '@angular/core';
import { map } from 'rxjs';
import { User, UserProfile } from 'src/app/_models/user';
import { SignalRService } from 'src/app/_services/signalr.service';
import { UserContactService } from 'src/app/_services/user-contact.service';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.sass'],
})
export class ContactsComponent {
  userContacts!: UserProfile[];
  selectedContact!: UserProfile;
  constructor(
    private _userContactService: UserContactService,
    private _signalrService: SignalRService
  ) {
    this.getContacts();
  }
  ngOnInit() {
    this._signalrService.startConnection().then(() => {
      console.log('connected');
      this._signalrService.listenToUpdateProfile();
      this._signalrService.AllContactsObservable.subscribe((res: any) => {
        console.log(res);
        if (res.id == this.selectedContact.id) {
          this.selectedContact = res;
        }
      });
    });
  }
  getContacts() {
    this._userContactService.getContacts().subscribe((rs) => {
      this.userContacts = rs;
    });
  }
  selectContact(user: any) {
    console.log(user);
    this.selectedContact = user;
    console.log(user);
  }
}
