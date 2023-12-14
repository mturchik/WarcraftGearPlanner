import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FooterComponent } from './layout/footer/footer.component';

@Component({
  standalone: true,
  imports: [RouterModule, FooterComponent],
  selector: 'warcraft-gear-planner-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {}
