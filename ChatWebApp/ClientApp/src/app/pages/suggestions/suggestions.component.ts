import { Component } from '@angular/core';
import { map } from 'rxjs';
import { User, UserProfile } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { UserContactService } from 'src/app/_services/user-contact.service';

@Component({
  selector: 'app-suggestions',
  templateUrl: './suggestions.component.html',
  styleUrls: ['./suggestions.component.sass'],
})
export class SuggestionsComponent {
  user: User | null;
  users: UserProfile[] | null | undefined;
  selectedSuggestion: UserProfile | undefined;
  isAddingContact: boolean = false;
  constructor(
    private accountService: AccountService,
    private userContactService: UserContactService
  ) {
    this.user = this.accountService.userValue;
    this.getSuggestions();
  }

  getSuggestions() {
    this.accountService
      .getSuggestions()
      .pipe(
        map((response) => {
          if (response) {
            console.log(response);
            this.users = response;
          }
        })
      )
      .subscribe();
  }

  addContact(contactId: any) {
    this.isAddingContact = true;
    var userId;
    this.accountService.user?.subscribe((user) => {
      userId = user?.id;
    });
    var userContactForCreation = { userId: userId, contactId: contactId };
    this.userContactService
      .createUserContact(userContactForCreation)
      .subscribe((response) => {
        this.isAddingContact = false;
        this.selectedSuggestion = undefined;
        alert('Add contact successfully');
      });
  }
  selectSuggestion(user: UserProfile) {
    console.log(user);
    this.selectedSuggestion = user;
  }
}
