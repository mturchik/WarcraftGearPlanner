import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CollapseModule } from 'ngx-bootstrap/collapse';

@Component({
  selector: 'warcraft-gear-planner-header',
  standalone: true,
  imports: [CommonModule, CollapseModule, RouterLink, RouterLinkActive],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent {
  isCollapsed = true;
}
