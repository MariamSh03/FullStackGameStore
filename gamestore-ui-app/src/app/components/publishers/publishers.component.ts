import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { PublisherService, Publisher } from '../../services/publisher.service';

@Component({
  selector: 'app-publishers',
  imports: [CommonModule, TranslateModule],
  templateUrl: './publishers.component.html',
  styleUrl: './publishers.component.css'
})
export class PublishersComponent implements OnInit {
  publishers: Publisher[] = [];
  loading = false;
  error: string | null = null;

  constructor(private publisherService: PublisherService) {}

  ngOnInit() {
    this.loadPublishers();
  }

  loadPublishers() {
    this.loading = true;
    this.error = null;

    this.publisherService.getAllPublishers().subscribe({
      next: (publishers) => {
        this.publishers = publishers;
        this.loading = false;
      },
      error: (error) => {
        console.error('Failed to load publishers:', error);
        this.error = 'Failed to load publishers from server. Please check your connection.';
        this.loading = false;
      }
    });
  }
}