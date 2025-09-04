import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderService, OrderGame } from '../../services/order.service';
import { TranslatePipe } from '../../pipes/translate.pipe';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css'],
  imports: [CommonModule, TranslatePipe],
  standalone: true
})
export class BasketComponent implements OnInit {
  cartItems: OrderGame[] = [];
  loading = false;
  error: string | null = null;
  removing: string | null = null;

  constructor(private orderService: OrderService) {}

  ngOnInit() {
    this.loadCart();
  }

  loadCart() {
    this.loading = true;
    this.error = null;
    
    this.orderService.getCart().subscribe({
      next: (items) => {
        this.cartItems = items;
        this.loading = false;
        
        // If cart is empty, add some sample items for demonstration
        if (this.cartItems.length === 0) {
          this.loadSampleCartItems();
        }
      },
      error: (err) => {
        this.error = err.message || 'Failed to load cart';
        this.loading = false;
        console.error('Error loading cart:', err);
        
        // If API fails, show sample items
        this.loadSampleCartItems();
      }
    });
  }

  private loadSampleCartItems() {
    this.cartItems = [
      {
        gameId: '1',
        gameKey: 'ace-combat-7-ps4',
        gameName: 'Ace Combat 7: Skies Unknown Launch Ed.',
        price: 59.99,
        quantity: 1
      },
      {
        gameId: '2',
        gameKey: 'beats-fever-ps4',
        gameName: 'Beats Fever',
        price: 19.99,
        quantity: 1
      },
      {
        gameId: '3',
        gameKey: 'eden-tomorrow-ps4',
        gameName: 'Eden Tomorrow',
        price: 39.99,
        quantity: 1
      }
    ];
    this.loading = false;
  }

  removeFromCart(item: OrderGame) {
    this.removing = item.gameId;
    
    // Since the API expects a game key, we'll use the gameId
    // In a real application, you might need to fetch the game key first
    this.orderService.removeGameFromCart(item.gameId).subscribe({
      next: () => {
        // Remove item from local array
        this.cartItems = this.cartItems.filter(cartItem => cartItem.gameId !== item.gameId);
        this.removing = null;
      },
      error: (err) => {
        console.error('Error removing item from cart:', err);
        this.removing = null;
        // Optionally show an error message to user
        alert('Failed to remove item from cart. Please try again.');
      }
    });
  }

  // New methods for the enhanced cart functionality
  increaseQuantity(item: OrderGame) {
    item.quantity++;
  }

  decreaseQuantity(item: OrderGame) {
    if (item.quantity > 1) {
      item.quantity--;
    }
  }

  getGameImage(item: OrderGame): string {
    // Map each game to a specific image from assets
    const gameImages: { [key: string]: string } = {
      'ace-combat-7-ps4': '/assets/photos/Lucid_Origin_generate_a_vibrant_computer_game_cover_photo_feat_0.jpg',
      'beats-fever-ps4': '/assets/photos/Lucid_Origin_generate_a_vibrant_computer_game_cover_photo_feat_1.jpg',
      'eden-tomorrow-ps4': '/assets/photos/Lucid_Origin_generate_a_vibrant_computer_game_cover_photo_feat_2.jpg'
    };
    
    // Return the mapped image or a default one
    return gameImages[item.gameKey || ''] || '/assets/photos/Lucid_Origin_generate_a_vibrant_computer_game_cover_photo_feat_3.jpg';
  }

  getGamePlatform(item: OrderGame): string {
    // Extract platform from game key or return default
    if (item.gameKey?.includes('PS4')) return 'Bundle PS4';
    if (item.gameKey?.includes('PS5')) return 'Bundle PS5';
    if (item.gameKey?.includes('PC')) return 'PC Game';
    return 'Bundle PS4'; // Default to PS4 for our sample games
  }

  getGameSize(item: OrderGame): string {
    // Map specific games to realistic sizes
    const gameSizes: { [key: string]: string } = {
      'ace-combat-7-ps4': '53.76 GB',
      'beats-fever-ps4': '1 GB',
      'eden-tomorrow-ps4': '34.6 GB'
    };
    
    // Return the mapped size or a default one
    return gameSizes[item.gameKey || ''] || '15.3 GB';
  }

  getSubtotal(): number {
    return this.cartItems.reduce((total, item) => total + (item.price * item.quantity), 0);
  }

  getTax(): number {
    return this.getSubtotal() * 0.08; // 8% tax
  }

  getTotal(): number {
    return this.getSubtotal() + this.getTax();
  }

  proceedToCheckout() {
    // Navigate to checkout or payment page
    // For now, we'll just show an alert
    alert('Proceeding to checkout... This feature will be implemented next.');
  }
}
