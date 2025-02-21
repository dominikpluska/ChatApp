import { Component, output } from '@angular/core';

@Component({
  selector: 'app-sidebar-component',
  standalone: true,
  imports: [],
  templateUrl: './sidebar-component.component.html',
  styleUrl: './sidebar-component.component.css',
})
export class SidebarComponentComponent {
  select = output<string>();
  private sideBarArray = [
    'chats',
    'friends',
    'black-list',
    'requests',
    'search',
    '',
  ];

  onSelect(index: number) {
    this.select.emit(this.sideBarArray[index]);
  }
}
