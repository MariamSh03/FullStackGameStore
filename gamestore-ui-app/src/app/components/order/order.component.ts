import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { OrderService, Order, OrderGame } from '../../services/order.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-order',
  imports: [CommonModule, RouterLink],
  template: `
    <div class="page-container">
      <div class="header">
        <a routerLink="/orders" class="back-btn">← Back to Orders</a>
        <h1>Order Details</h1>
      </div>
      
      <!-- Loading state -->
      <div *ngIf="loading" class="loading">
        <p>Loading order details...</p>
      </div>
      
      <!-- Error state -->
      <div *ngIf="error" class="error">
        <p>Error loading order: {{ error }}</p>
        <button (click)="loadOrderDetails()" class="retry-btn">Retry</button>
      </div>
      
      <!-- Order details -->
      <div *ngIf="!loading && !error && order" class="order-details">
        <div class="order-info">
          <h2>Order #{{ order.id.slice(-8) }}</h2>
          <div class="info-grid">
            <div class="info-item">
              <label>Order ID:</label>
              <span>{{ order.id }}</span>
            </div>
            <div class="info-item">
              <label>Customer ID:</label>
              <span>{{ order.customerId }}</span>
            </div>
            <div class="info-item" *ngIf="order.date">
              <label>Order Date:</label>
              <span>{{ formatDate(order.date) }}</span>
            </div>
          </div>
        </div>
        
        <!-- Order items -->
        <div class="order-items">
          <h3>Order Items</h3>
          <div *ngIf="orderGames.length === 0" class="empty-items">
            <p>No items found in this order.</p>
          </div>
          <div *ngIf="orderGames.length > 0" class="items-list">
            <div *ngFor="let game of orderGames" class="item-card">
              <div class="item-info">
                <h4>{{ game.gameName }}</h4>
                <p class="game-id">Game ID: {{ game.gameId }}</p>
              </div>
              <div class="item-details">
                <div class="quantity">
                  <label>Quantity:</label>
                  <span>{{ game.quantity }}</span>
                </div>
                <div class="price">
                  <label>Price:</label>
                  <span>\${{ game.price.toFixed(2) }}</span>
                </div>
                <div class="total">
                  <label>Total:</label>
                  <span>\${{ (game.price * game.quantity).toFixed(2) }}</span>
                </div>
              </div>
            </div>
            
            <!-- Order total -->
            <div class="order-total">
              <h3>Order Total: \${{ calculateTotal().toFixed(2) }}</h3>
            </div>
          </div>
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
    
    .header { 
      margin-bottom: 30px; 
    }
    
    .back-btn { 
      color: #007bff; 
      text-decoration: none; 
      margin-bottom: 10px; 
      display: inline-block; 
    }
    
    .back-btn:hover { 
      text-decoration: underline; 
    }
    
    .loading, .error { 
      text-align: center; 
      padding: 40px 20px; 
    }
    
    .error { 
      color: #dc3545; 
    }
    
    .retry-btn { 
      background: #007bff; 
      color: white; 
      border: none; 
      padding: 10px 20px; 
      border-radius: 4px; 
      cursor: pointer; 
      margin-top: 10px; 
    }
    
    .retry-btn:hover { 
      background: #0056b3; 
    }
    
    .order-details { 
      background: white; 
      border-radius: 8px; 
      box-shadow: 0 4px 6px rgba(0,0,0,0.1); 
      overflow: hidden; 
    }
    
    .order-info { 
      padding: 30px; 
      border-bottom: 1px solid #eee; 
    }
    
    .order-info h2 { 
      margin-top: 0; 
      color: #333; 
    }
    
    .info-grid { 
      display: grid; 
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr)); 
      gap: 20px; 
      margin-top: 20px; 
    }
    
    .info-item { 
      display: flex; 
      flex-direction: column; 
    }
    
    .info-item label { 
      font-weight: bold; 
      color: #666; 
      margin-bottom: 5px; 
    }
    
    .info-item span { 
      color: #333; 
      word-break: break-all; 
    }
    
    .order-items { 
      padding: 30px; 
    }
    
    .order-items h3 { 
      margin-top: 0; 
      color: #333; 
    }
    
    .empty-items { 
      text-align: center; 
      color: #666; 
      padding: 40px 20px; 
    }
    
    .items-list { 
      margin-top: 20px; 
    }
    
    .item-card { 
      display: flex; 
      justify-content: space-between; 
      align-items: flex-start; 
      padding: 20px; 
      border: 1px solid #eee; 
      border-radius: 6px; 
      margin-bottom: 15px; 
    }
    
    .item-info h4 { 
      margin: 0 0 10px 0; 
      color: #333; 
    }
    
    .game-id { 
      color: #666; 
      font-size: 14px; 
      margin: 0; 
    }
    
    .item-details { 
      display: flex; 
      gap: 30px; 
      align-items: center; 
    }
    
    .quantity, .price, .total { 
      display: flex; 
      flex-direction: column; 
      align-items: center; 
      text-align: center; 
    }
    
    .quantity label, .price label, .total label { 
      font-size: 12px; 
      color: #666; 
      margin-bottom: 5px; 
    }
    
    .quantity span, .price span, .total span { 
      font-weight: bold; 
      color: #333; 
    }
    
    .total span { 
      color: #28a745; 
    }
    
    .order-total { 
      text-align: right; 
      margin-top: 30px; 
      padding-top: 20px; 
      border-top: 2px solid #eee; 
    }
    
    .order-total h3 { 
      margin: 0; 
      color: #28a745; 
    }
    
    @media (max-width: 768px) {
      .item-card { 
        flex-direction: column; 
        gap: 15px; 
      }
      
      .item-details { 
        justify-content: space-around; 
        width: 100%; 
      }
    }
  `]
})
export class OrderComponent implements OnInit {
  order: Order | null = null;
  orderGames: OrderGame[] = [];
  loading = false;
  error: string | null = null;
  orderId: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.orderId = params.get('id');
      if (this.orderId) {
        this.loadOrderDetails();
      } else {
        this.error = 'Order ID not provided';
      }
    });
  }

  loadOrderDetails() {
    if (!this.orderId) {
      this.error = 'Order ID not provided';
      return;
    }

    this.loading = true;
    this.error = null;
    
    // Load both order info and order details simultaneously
    forkJoin({
      order: this.orderService.getOrderById(this.orderId),
      orderGames: this.orderService.getOrderDetails(this.orderId)
    }).subscribe({
      next: (result) => {
        this.order = result.order;
        this.orderGames = result.orderGames;
        this.loading = false;
      },
      error: (err) => {
        this.error = err.message || 'Failed to load order details';
        this.loading = false;
        console.error('Error loading order details:', err);
      }
    });
  }

  formatDate(dateString: string): string {
    try {
      const date = new Date(dateString);
      return date.toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      });
    } catch {
      return dateString;
    }
  }

  calculateTotal(): number {
    return this.orderGames.reduce((total, game) => total + (game.price * game.quantity), 0);
  }
}
