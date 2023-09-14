import { Component, Input, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent {
  @Input() totalCount?: number;
  @Input() pageSize?: number;
  @Input() pageIndex?: number;
  @Output() pageChanged = new EventEmitter<number>();

  onPagerChanged(event: any){
    this.pageChanged.emit(event);
  }
}
