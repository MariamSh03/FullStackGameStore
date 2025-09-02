import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, take, switchMap } from 'rxjs/operators';
import { ApiConfigService } from './api-config.service';

export interface Order {
  id: string;
  customerId: string;
  date?: string;
}

export interface OrderGame {
  gameId: string;
  gameKey?: string; // Optional game key for additional game information
  gameName: string;
  price: number;
  quantity: number;
}

export interface PaymentMethod {
  imageUrl: string;
  title: string;
  description: string;
}

export interface PaymentMethods {
  paymentMethods: PaymentMethod[];
}

export interface PaymentRequest {
  paymentMethod: string;
  // Add other payment-related fields as needed
}

export interface UpdateQuantityRequest {
  count: number;
}

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService
  ) {}

  // Get all orders
  getAllOrders(): Observable<Order[]> {
    return this.apiConfig.config$.pipe(
      filter(config => config !== null),
      take(1),
      switchMap(config => {
        const url = this.apiConfig.buildUrl(config!.ordersApiUrl);
        return this.http.get<Order[]>(url);
      })
    );
  }

  // Get specific order by ID
  getOrderById(orderId: string): Observable<Order> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.orderApiUrl, { id: orderId });
    return this.http.get<Order>(url);
  }

  // Get order details (games inside the order)
  getOrderDetails(orderId: string): Observable<OrderGame[]> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.orderDetailsApiUrl, { id: orderId });
    return this.http.get<OrderGame[]>(url);
  }

  // Get current user's cart
  getCart(): Observable<OrderGame[]> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.basketApiUrl);
    return this.http.get<OrderGame[]>(url);
  }

  // Add game to cart
  addGameToCart(gameId: string): Observable<{ message: string }> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.buyGameApiUrl, { key: gameId });
    return this.http.post<{ message: string }>(url, {});
  }

  // Remove game from cart
  removeGameFromCart(gameKey: string): Observable<void> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.cancelGameBuyApiUrl, { key: gameKey });
    return this.http.delete<void>(url);
  }

  // Get available payment methods
  getPaymentMethods(): Observable<PaymentMethods> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.makeOrderInfoApiUrl);
    return this.http.get<PaymentMethods>(url);
  }

  // Process payment
  processPayment(request: PaymentRequest): Observable<any> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.payApiUrl);
    return this.http.post(url, request);
  }

  // Update order detail quantity
  updateOrderDetailQuantity(detailId: string, request: UpdateQuantityRequest): Observable<{ message: string }> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.updateOrderDetailCountApiUrl, { id: detailId });
    return this.http.patch<{ message: string }>(url, request);
  }

  // Delete order detail
  deleteOrderDetail(detailId: string): Observable<{ message: string }> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.deleteOrderDetailApiUrl, { id: detailId });
    return this.http.delete<{ message: string }>(url);
  }

  // Ship order
  shipOrder(orderId: string): Observable<{ message: string }> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.shipOrderApiUrl, { id: orderId });
    return this.http.post<{ message: string }>(url, {});
  }

  // Add game to order
  addGameToOrder(orderId: string, gameKey: string): Observable<{ message: string }> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.addOrderDetailApiUrl, { id: orderId, key: gameKey });
    return this.http.post<{ message: string }>(url, {});
  }
}
