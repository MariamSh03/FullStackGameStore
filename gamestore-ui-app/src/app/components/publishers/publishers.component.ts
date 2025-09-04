import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BehaviorSubject } from 'rxjs';
import { PublisherService, Publisher } from '../../services/publisher.service';

@Component({
  selector: 'app-publishers',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './publishers.component.html',
  styleUrls: ['./publishers.component.css']
})
export class PublishersComponent implements OnInit {
  publishers: Publisher[] = [];
  loading = new BehaviorSubject<boolean>(false);
  error: string | null = null;

  constructor(private publisherService: PublisherService) {}

  ngOnInit() {
    this.loadPublishers();
  }

  loadPublishers() {
    this.loading.next(true);
    this.error = null;

    this.publisherService.getAllPublishers().subscribe({
      next: (publishers) => {
        this.publishers = publishers;
        this.loading.next(false);
      },
      error: (error) => {
        console.error('Failed to load publishers:', error);
        this.error = 'Failed to load publishers from server. Please check your connection.';
        this.loading.next(false);
      }
    });
  }
}