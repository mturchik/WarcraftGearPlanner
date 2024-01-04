import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'warcraft-gear-planner-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {
  constructor() {}
}
