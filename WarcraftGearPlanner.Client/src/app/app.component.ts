import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FooterComponent } from './layout/footer/footer.component';
import { HeaderComponent } from './layout/header/header.component';
import { ConfigService } from './shared/config/config.service';

@Component({
  standalone: true,
  imports: [RouterModule, HeaderComponent, FooterComponent],
  selector: 'warcraft-gear-planner-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  constructor(private _configService: ConfigService) {}

  ngOnInit(): void {
    this._configService.loadConfig();
  }
}
