import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { OrderService, Order } from '../../services/order.service';

@Component({
  selector: 'app-orders',
  imports: [CommonModule, RouterLink],
  template: `
    <div class="page-container">
      <h1>Your Orders</h1>
      <p>View and manage your orders</p>
      
      <!-- Loading state -->
      <div *ngIf="loading" class="loading">
        <p>Loading orders...</p>
      </div>
      
      <!-- Error state -->
      <div *ngIf="error" class="error">
        <p>Error loading orders: {{ error }}</p>
        <button (click)="loadOrders()" class="retry-btn">Retry</button>
      </div>
      
      <!-- Empty state -->
      <div *ngIf="!loading && !error && orders.length === 0" class="empty-state">
        <p>You have no orders yet.</p>
        <a routerLink="/games" class="cta-btn">Browse Games</a>
      </div>
      
      <!-- Orders list -->
      <div *ngIf="!loading && !error && orders.length > 0" class="orders-list">
        <div *ngFor="let order of orders" class="order-card">
          <div class="order-header">
            <h3>Order #{{ order.id.slice(-8) }}</h3>
            <a [routerLink]="['/order', order.id]" class="view-details-btn">View Details</a>
          </div>
          <p *ngIf="order.date" class="order-date">Date: {{ formatDate(order.date) }}</p>
          <p *ngIf="!order.date" class="order-date">Date: Not specified</p>
          <p class="customer-id">Customer ID: {{ order.customerId.slice(-8) }}</p>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .page-container { 
      max-width: 1200px; 
      margin: 0 auto; 
      padding: 40px 20px; 
    }
    
    .loading, .error, .empty-state { 
      text-align: center; 
      padding: 40px 20px; 
    }
    
    .error { 
      color: #dc3545; 
    }
    
    .retry-btn, .cta-btn { 
      background: #007bff; 
      color: white; 
      border: none; 
      padding: 10px 20px; 
      border-radius: 4px; 
      cursor: pointer; 
      text-decoration: none; 
      display: inline-block; 
      margin-top: 10px; 
    }
    
    .retry-btn:hover, .cta-btn:hover { 
      background: #0056b3; 
    }
    
    .orders-list { 
      margin-top: 30px; 
    }
    
    .order-card { 
      background: white; 
      padding: 20px; 
      border-radius: 8px; 
      box-shadow: 0 4px 6px rgba(0,0,0,0.1); 
      margin-bottom: 15px; 
    }
    
    .order-header { 
      display: flex; 
      justify-content: space-between; 
      align-items: center; 
      margin-bottom: 10px; 
    }
    
    .order-header h3 { 
      margin: 0; 
      color: #333; 
    }
    
    .view-details-btn { 
      background: #28a745; 
      color: white; 
      padding: 8px 16px; 
      border-radius: 4px; 
      text-decoration: none; 
      font-size: 14px; 
    }
    
    .view-details-btn:hover { 
      background: #1e7e34; 
    }
    
    .order-date, .customer-id { 
      margin: 5px 0; 
      color: #666; 
    }
  `]
})
export class OrdersComponent implements OnInit {
  orders: Order[] = [];
  loading = false;
  error: string | null = null;

  constructor(private orderService: OrderService) {}

  ngOnInit() {
    this.loadOrders();
  }

  loadOrders() {
    this.loading = true;
    this.error = null;
    
    this.orderService.getAllOrders().subscribe({
      next: (orders) => {
        this.orders = orders;
        this.loading = false;
      },
      error: (err) => {
        this.error = err.message || 'Failed to load orders';
        this.loading = false;
        console.error('Error loading orders:', err);
      }
    });
  }

  formatDate(dateString: string): string {
    try {
      const date = new Date(dateString);
      return date.toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      });
    } catch {
      return dateString;
    }
  }
}
