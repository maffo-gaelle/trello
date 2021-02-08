import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { NavigationEnd, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { local } from './i18n/en';
import { locale } from './i18n/fr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})

export class AppComponent {
  title = 'app';

  constructor(
    public translationService: TranslateService,
    public titleService: Title,
    public router: Router
  ){
    this.translationService.addLangs(["en", "fr"]);
    this.translationService.setTranslation("en", local);
    // cette langue sera utilisée comme solution de repli quand une traduction ne se trouve pas dans la langue actuelle
    this.translationService.setDefaultLang("en");
    // la langue à utiliser, si la langue n'est pas disponible, il utilisera le chargeur actuel pour les faire
    this.translationService.use("en");

    this.router.events.subscribe(event => {
      if(event  instanceof NavigationEnd) {
        this.titleService.setTitle(`
        ${this.translationService.instant('PAGE_TITLE.SITE_NAME')} |
        ${this.getTitle(router.routerState, router.routerState.root).join('-')}`)
      }
    })
  }

  private getTitle(state, parent) {
    const data = [];
    if(parent && parent.snapshot.data && parent.snapshot.data.title) {
        data.push(this.translationService.instant(parent.snapshot.data.title));
    }

    if(state && parent) {
        data.push(...this.getTitle(state, state.firstChild(parent)));
    }

    return data;
  }
}
